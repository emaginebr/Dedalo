using Dedalo.Domain.Models;
using Dedalo.DTO.Website;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dedalo.Domain.Interfaces
{
    public interface IWebsiteService
    {
        Task<WebsiteModel> GetByIdAsync(long websiteId, long userId);
        Task<WebsiteModel> GetBySlugAsync(string slug);
        Task<WebsiteModel> GetByDomainAsync(string domain);
        Task<IEnumerable<WebsiteModel>> ListByUserAsync(long userId);
        Task<WebsiteModel> InsertAsync(WebsiteInsertInfo website, long userId);
        Task<WebsiteModel> UpdateAsync(WebsiteUpdateInfo website, long userId);
        Task UpdateLogoAsync(long websiteId, string logoUrl, long userId);
        Task DeleteAsync(long websiteId, long userId);
    }
}
