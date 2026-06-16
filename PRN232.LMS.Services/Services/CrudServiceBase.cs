using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories;
using PRN232.LMS.Services.Common;
using PRN232.LMS.Services.Extensions;

namespace PRN232.LMS.Services.Services;

public abstract class CrudServiceBase<TEntity, TBusiness, TResponse, TCreate, TUpdate>(GenericRepository<TEntity> repo, IMapper mapper)
    where TEntity : class
{
    protected readonly GenericRepository<TEntity> _repo = repo;
    protected readonly IMapper _mapper = mapper;

    protected abstract Expression<Func<TEntity, bool>> KeyPredicate(int id);

    protected virtual IReadOnlyDictionary<string, Func<IQueryable<TEntity>, IQueryable<TEntity>>> ExpandMap
        => new Dictionary<string, Func<IQueryable<TEntity>, IQueryable<TEntity>>>();

    protected virtual IQueryable<TEntity> BuildQuery(QueryParameters param)
        => _repo.Query()
                .ApplySearch(param.Search)
                .ApplySort(param.Sort)
                .ApplyExpand(param.Expand, ExpandMap);

    protected virtual IQueryable<TEntity> BuildIdQuery() => _repo.Query();

    public virtual async Task<PagedResult<object>> GetAllAsync(QueryParameters param)
    {
        var query = BuildQuery(param);

        var total = await query.CountAsync();
        var data = await query.ApplyPaging(param.Page, param.Size).ToListAsync();
        var businesses = _mapper.Map<List<TBusiness>>(data);
        var responses = _mapper.Map<List<TResponse>>(businesses);

        return responses.ToPagedResult(total, param);
    }

    public virtual async Task<TResponse?> GetByIdAsync(int id)
    {
        var data = await BuildIdQuery().FirstOrDefaultAsync(KeyPredicate(id));
        if (data is null) return default;
        var business = _mapper.Map<TBusiness>(data);
        return _mapper.Map<TResponse>(business);
    }

    public virtual async Task<TResponse> CreateAsync(TCreate request)
    {
        var business = _mapper.Map<TBusiness>(request);
        var entity = _mapper.Map<TEntity>(business);
        await _repo.AddAsync(entity);
        var resultBusiness = _mapper.Map<TBusiness>(entity);
        return _mapper.Map<TResponse>(resultBusiness);
    }

    public virtual async Task<bool> PatchAsync(int id, TUpdate request)
    {
        var entity = await _repo.FirstOrDefaultAsync(KeyPredicate(id));
        if (entity is null) return false;

        var business = _mapper.Map<TBusiness>(entity);
        _mapper.Map(request, business);
        _mapper.Map(business, entity);

        await _repo.UpdateAsync(entity);
        return true;
    }

    public virtual async Task<bool> DeleteAsync(int id)
    {
        var data = await _repo.FirstOrDefaultAsync(KeyPredicate(id));
        if (data is null) return false;

        await _repo.RemoveAsync(data);
        return true;
    }
}