using AutoMapper;
using Dedalo.Infra.Interfaces.Repository;
using Dedalo.Domain.Models;
using Dedalo.Domain.Interfaces;
using Dedalo.DTO.Website;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dedalo.Domain.Services
{
    public class WebsiteService : IWebsiteService
    {
        private readonly IWebsiteRepository<WebsiteModel> _websiteRepository;
        private readonly IPageRepository<PageModel> _pageRepository;
        private readonly IMapper _mapper;

        public WebsiteService(
            IWebsiteRepository<WebsiteModel> websiteRepository,
            IPageRepository<PageModel> pageRepository,
            IMapper mapper
        )
        {
            _websiteRepository = websiteRepository;
            _pageRepository = pageRepository;
            _mapper = mapper;
        }

        public async Task<WebsiteModel> GetByIdAsync(long websiteId, long userId)
        {
            var model = await _websiteRepository.GetByIdAsync(websiteId);
            if (model == null)
                return null;

            model.ValidateOwnership(userId);
            return model;
        }

        public async Task<WebsiteModel?> GetBySlugAsync(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
                return null;

            return await _websiteRepository.GetBySlugAsync(slug);
        }

        public async Task<WebsiteModel?> GetByDomainAsync(string domain)
        {
            if (string.IsNullOrWhiteSpace(domain))
                return null;

            var model = await _websiteRepository.GetByDomainAsync(domain);
            if (model == null || model.DomainType != DomainTypeEnum.CustomDomain)
                return null;

            return model;
        }

        public async Task<IEnumerable<WebsiteModel>> ListByUserAsync(long userId)
        {
            return await _websiteRepository.ListByUserAsync(userId);
        }

        public async Task<WebsiteModel> InsertAsync(WebsiteInsertInfo website, long userId)
        {
            var model = _mapper.Map<WebsiteModel>(website);
            model.SetOwner(userId);
            model.GenerateSlug(website.Name);
            model.Status = WebsiteStatusEnum.Active;
            model.MarkCreated();

            await ValidateSlugUniqueAsync(model.WebsiteSlug);
            await ValidateDomainUniqueAsync(model.CustomDomain);

            var created = await _websiteRepository.InsertAsync(model);

            var homePage = new PageModel
            {
                WebsiteId = created.WebsiteId,
                PageSlug = "home",
                Name = "Home"
            };
            homePage.MarkCreated();
            await _pageRepository.InsertAsync(homePage);

            return created;
        }

        public async Task<WebsiteModel> UpdateAsync(WebsiteUpdateInfo website, long userId)
        {
            var existing = await _websiteRepository.GetByIdAsync(website.WebsiteId);
            if (existing == null)
                throw new Exception("Website not found");

            existing.ValidateOwnership(userId);
            _mapper.Map(website, existing);
            existing.EnsureSlug();
            existing.MarkUpdated();

            await ValidateSlugUniqueAsync(existing.WebsiteSlug, existing.WebsiteId);
            await ValidateDomainUniqueAsync(existing.CustomDomain, existing.WebsiteId);

            return await _websiteRepository.UpdateAsync(existing);
        }

        private async Task ValidateSlugUniqueAsync(string slug, long? excludeId = null)
        {
            if (string.IsNullOrWhiteSpace(slug))
                return;

            var found = await _websiteRepository.GetBySlugAsync(slug);
            if (found != null && found.WebsiteId != excludeId)
                throw new Exception("A website with this slug already exists");
        }

        private async Task ValidateDomainUniqueAsync(string domain, long? excludeId = null)
        {
            if (string.IsNullOrWhiteSpace(domain))
                return;

            var found = await _websiteRepository.GetByDomainAsync(domain);
            if (found != null && found.WebsiteId != excludeId)
                throw new Exception("A website with this domain already exists");
        }

        public async Task UpdateLogoAsync(long websiteId, string logoUrl, long userId)
        {
            var existing = await _websiteRepository.GetByIdAsync(websiteId);
            if (existing == null)
                throw new Exception("Website not found");

            existing.ValidateOwnership(userId);
            await _websiteRepository.UpdateLogoAsync(websiteId, logoUrl);
        }

    }
}
