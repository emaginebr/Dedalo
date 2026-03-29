using AutoMapper;
using Dedalo.Infra.Interfaces.Repository;
using Dedalo.Domain.Models;
using Dedalo.Domain.Interfaces;
using Dedalo.DTO.Menu;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dedalo.Domain.Services
{
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository<MenuModel> _menuRepository;
        private readonly IWebsiteRepository<WebsiteModel> _websiteRepository;
        private readonly IMapper _mapper;

        public MenuService(
            IMenuRepository<MenuModel> menuRepository,
            IWebsiteRepository<WebsiteModel> websiteRepository,
            IMapper mapper
        )
        {
            _menuRepository = menuRepository;
            _websiteRepository = websiteRepository;
            _mapper = mapper;
        }

        public async Task<MenuModel> GetByIdAsync(long menuId)
        {
            return await _menuRepository.GetByIdAsync(menuId);
        }

        public async Task<IEnumerable<MenuModel>> ListByWebsiteAsync(long websiteId)
        {
            return await _menuRepository.ListByWebsiteAsync(websiteId);
        }

        public async Task<IEnumerable<MenuModel>> ListPublicAsync(string websiteSlug, string domain)
        {
            if (string.IsNullOrWhiteSpace(websiteSlug) && string.IsNullOrWhiteSpace(domain))
                throw new Exception("Website slug or domain is required");

            return await _menuRepository.ListByWebsiteSlugOrDomainAsync(websiteSlug, domain);
        }

        public async Task<MenuModel> InsertAsync(MenuInsertInfo menu, long userId)
        {
            var website = await _websiteRepository.GetByIdAsync(menu.WebsiteId);
            if (website == null)
                throw new Exception("Website not found");
            website.ValidateOwnership(userId);

            var model = _mapper.Map<MenuModel>(menu);
            model.MarkCreated();

            return await _menuRepository.InsertAsync(model);
        }

        public async Task<MenuModel> UpdateAsync(MenuUpdateInfo menu, long userId)
        {
            var existing = await _menuRepository.GetByIdAsync(menu.MenuId);
            if (existing == null)
                throw new Exception("Menu not found");

            var website = await _websiteRepository.GetByIdAsync(existing.WebsiteId);
            if (website == null)
                throw new Exception("Website not found");
            website.ValidateOwnership(userId);

            _mapper.Map(menu, existing);
            existing.MarkUpdated();

            return await _menuRepository.UpdateAsync(existing);
        }

        public async Task DeleteAsync(long menuId, long userId)
        {
            var existing = await _menuRepository.GetByIdAsync(menuId);
            if (existing == null)
                throw new Exception("Menu not found");

            var website = await _websiteRepository.GetByIdAsync(existing.WebsiteId);
            if (website == null)
                throw new Exception("Website not found");
            website.ValidateOwnership(userId);

            await _menuRepository.DeleteAsync(menuId);
        }
    }
}
