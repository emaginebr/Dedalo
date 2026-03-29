using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dedalo.Infra.Interfaces.Repository
{
    public interface IContentRepository<TModel> where TModel : class
    {
        Task<TModel> GetByIdAsync(long id);
        Task<IEnumerable<TModel>> ListByPageAsync(long pageId);
        Task<IEnumerable<TModel>> ListByPageSlugAsync(string pageSlug, string websiteSlug, string domain);
        Task<IEnumerable<TModel>> ListBySlugAsync(long pageId, string contentSlug);
        Task<TModel> InsertAsync(TModel model);
        Task InsertBatchAsync(IEnumerable<TModel> models);
        Task UpdateBatchAsync(IEnumerable<TModel> models);
        Task DeleteBatchAsync(IEnumerable<long> ids);
        Task<TModel> UpdateAsync(TModel model);
        Task DeleteAsync(long id);
    }
}
