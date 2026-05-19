using PRN232.LMS.Services.Common;

namespace PRN232.LMS.Services.Services;

public interface ICrudService<TResponse, TCreate, TUpdate>
{
    Task<PagedResult<object>> GetAllAsync(QueryParameters param);
    Task<TResponse?> GetByIdAsync(int id);
    Task<TResponse> CreateAsync(TCreate request);
    Task<bool> UpdateAsync(int id, TUpdate request);
    Task<bool> DeleteAsync(int id);
}