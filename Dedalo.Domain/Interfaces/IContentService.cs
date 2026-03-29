using Dedalo.Domain.Models;
using Dedalo.DTO.Content;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dedalo.Domain.Interfaces
{
    public interface IContentService
    {
        Task<IEnumerable<ContentModel>> SaveAreaAsync(ContentAreaInfo area, long userId);
        Task<ContentModel> GetByIdAsync(long contentId);
        Task<IEnumerable<ContentModel>> ListByPageAsync(long pageId);
        Task<IEnumerable<ContentModel>> ListPublicAsync(string pageSlug, string websiteSlug, string domain);
        Task<ContentModel> InsertAsync(ContentInsertInfo content, long userId);
        Task<ContentModel> UpdateAsync(ContentUpdateInfo content, long userId);
        Task DeleteAsync(long contentId, long userId);
    }
}
