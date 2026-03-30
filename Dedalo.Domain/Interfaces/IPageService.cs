using Dedalo.Domain.Models;
using Dedalo.DTO.Page;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dedalo.Domain.Interfaces
{
    public interface IPageService
    {
        Task<PageModel> GetByIdAsync(long pageId);
        Task<PageModel?> GetBySlugAsync(string pageSlug, string websiteSlug, string domain);
        Task<IEnumerable<PageModel>> ListByWebsiteAsync(long websiteId);
        Task<IEnumerable<PageModel>> ListPublicAsync(string websiteSlug, string domain);
        Task<PageModel> InsertAsync(PageInsertInfo page, long userId);
        Task<PageModel> UpdateAsync(PageUpdateInfo page, long userId);
        Task DeleteAsync(long pageId, long userId);
    }
}
