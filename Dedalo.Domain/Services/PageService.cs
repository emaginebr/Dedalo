using AutoMapper;
using Dedalo.Infra.Interfaces.Repository;
using Dedalo.Domain.Models;
using Dedalo.Domain.Interfaces;
using Dedalo.DTO.Page;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dedalo.Domain.Services
{
    public class PageService : IPageService
    {
        private readonly IPageRepository<PageModel> _pageRepository;
        private readonly IWebsiteRepository<WebsiteModel> _websiteRepository;
        private readonly IMapper _mapper;

        public PageService(
            IPageRepository<PageModel> pageRepository,
            IWebsiteRepository<WebsiteModel> websiteRepository,
            IMapper mapper
        )
        {
            _pageRepository = pageRepository;
            _websiteRepository = websiteRepository;
            _mapper = mapper;
        }

        public async Task<PageModel> GetByIdAsync(long pageId)
        {
            return await _pageRepository.GetByIdAsync(pageId);
        }

        public async Task<PageModel> GetBySlugAsync(string pageSlug, string websiteSlug, string domain)
        {
            if (string.IsNullOrWhiteSpace(pageSlug))
                throw new Exception("Page slug is required");

            if (string.IsNullOrWhiteSpace(websiteSlug) && string.IsNullOrWhiteSpace(domain))
                throw new Exception("Website slug or domain is required");

            var model = await _pageRepository.GetBySlugAsync(pageSlug, websiteSlug, domain);
            if (model == null)
                throw new Exception("Page not found");

            return model;
        }

        public async Task<IEnumerable<PageModel>> ListByWebsiteAsync(long websiteId)
        {
            return await _pageRepository.ListByWebsiteAsync(websiteId);
        }

        public async Task<IEnumerable<PageModel>> ListPublicAsync(string websiteSlug, string domain)
        {
            if (string.IsNullOrWhiteSpace(websiteSlug) && string.IsNullOrWhiteSpace(domain))
                throw new Exception("Website slug or domain is required");

            return await _pageRepository.ListByWebsiteSlugOrDomainAsync(websiteSlug, domain);
        }

        public async Task<PageModel> InsertAsync(PageInsertInfo page, long userId)
        {
            var website = await _websiteRepository.GetByIdAsync(page.WebsiteId);
            if (website == null)
                throw new Exception("Website not found");
            website.ValidateOwnership(userId);

            var model = _mapper.Map<PageModel>(page);
            model.MarkCreated();

            return await _pageRepository.InsertAsync(model);
        }

        public async Task<PageModel> UpdateAsync(PageUpdateInfo page, long userId)
        {
            var existing = await _pageRepository.GetByIdAsync(page.PageId);
            if (existing == null)
                throw new Exception("Page not found");

            var website = await _websiteRepository.GetByIdAsync(existing.WebsiteId);
            if (website == null)
                throw new Exception("Website not found");
            website.ValidateOwnership(userId);

            _mapper.Map(page, existing);
            existing.MarkUpdated();

            return await _pageRepository.UpdateAsync(existing);
        }

        public async Task DeleteAsync(long pageId, long userId)
        {
            var existing = await _pageRepository.GetByIdAsync(pageId);
            if (existing == null)
                throw new Exception("Page not found");

            var website = await _websiteRepository.GetByIdAsync(existing.WebsiteId);
            if (website == null)
                throw new Exception("Website not found");
            website.ValidateOwnership(userId);

            await _pageRepository.DeleteAsync(pageId);
        }
    }
}
