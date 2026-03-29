using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dedalo.Infra.Interfaces.Repository
{
    public interface IWebsiteRepository<TModel> where TModel : class
    {
        Task<TModel> GetByIdAsync(long id);
        Task<TModel> GetBySlugAsync(string slug);
        Task<TModel> GetByDomainAsync(string domain);
        Task<IEnumerable<TModel>> ListByUserAsync(long userId);
        Task<TModel> InsertAsync(TModel model);
        Task<TModel> UpdateAsync(TModel model);
        Task UpdateLogoAsync(long id, string logoUrl);
        Task DeleteAsync(long id);
    }
}
