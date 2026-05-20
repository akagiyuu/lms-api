using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories;
using PRN232.LMS.Services.Common;
using PRN232.LMS.Services.Extensions;

namespace PRN232.LMS.Services.Services;

public abstract class CrudServiceBase<TEntity, TResponse, TCreate, TUpdate>(GenericRepository<TEntity> repo, IMapper mapper)
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
        var responses = _mapper.Map<List<TResponse>>(data);

        return responses.ToPagedResult(total, param);
    }

    public virtual async Task<TResponse?> GetByIdAsync(int id)
    {
        var data = await BuildIdQuery().FirstOrDefaultAsync(KeyPredicate(id));
        return data is null ? default : _mapper.Map<TResponse>(data);
    }

    public virtual async Task<TResponse> CreateAsync(TCreate request)
    {
        var data = _mapper.Map<TEntity>(request);
        await _repo.AddAsync(data);
        return _mapper.Map<TResponse>(data);
    }

    public virtual async Task<bool> PatchAsync(int id, TUpdate request)
    {
        var data = await _repo.FirstOrDefaultAsync(KeyPredicate(id));
        if (data is null) return false;

        _mapper.Map(request, data);
        await _repo.UpdateAsync(data);
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