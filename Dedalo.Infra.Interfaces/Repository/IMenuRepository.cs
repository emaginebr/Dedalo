using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dedalo.Infra.Interfaces.Repository
{
    public interface IMenuRepository<TModel> where TModel : class
    {
        Task<TModel> GetByIdAsync(long id);
        Task<IEnumerable<TModel>> ListByWebsiteAsync(long websiteId);
        Task<IEnumerable<TModel>> ListByWebsiteSlugOrDomainAsync(string websiteSlug, string domain);
        Task<TModel> InsertAsync(TModel model);
        Task<TModel> UpdateAsync(TModel model);
        Task DeleteAsync(long id);
    }
}
