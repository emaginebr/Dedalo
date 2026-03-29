using Dedalo.Domain.Models;
using Dedalo.DTO.Menu;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dedalo.Domain.Interfaces
{
    public interface IMenuService
    {
        Task<MenuModel> GetByIdAsync(long menuId);
        Task<IEnumerable<MenuModel>> ListByWebsiteAsync(long websiteId);
        Task<IEnumerable<MenuModel>> ListPublicAsync(string websiteSlug, string domain);
        Task<MenuModel> InsertAsync(MenuInsertInfo menu, long userId);
        Task<MenuModel> UpdateAsync(MenuUpdateInfo menu, long userId);
        Task DeleteAsync(long menuId, long userId);
    }
}
