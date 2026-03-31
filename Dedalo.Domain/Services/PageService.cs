using AutoMapper;
using Dedalo.Infra.Interfaces.Repository;
using Dedalo.Domain.Models;
using Dedalo.Domain.Interfaces;
using Dedalo.Domain.Validators;
using Dedalo.DTO.Page;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dedalo.Domain.Services
{
    public class PageService : IPageService
    {
        private readonly IPageRepository<PageModel> _pageRepository;
        private readonly IWebsiteRepository<WebsiteModel> _websiteRepository;
        private readonly IValidator<PageInsertInfo> _insertValidator;
        private readonly IValidator<PageUpdateInfo> _updateValidator;
        private readonly IMapper _mapper;

        public PageService(
            IPageRepository<PageModel> pageRepository,
            IWebsiteRepository<WebsiteModel> websiteRepository,
            IValidator<PageInsertInfo> insertValidator,
            IValidator<PageUpdateInfo> updateValidator,
            IMapper mapper
        )
        {
            _pageRepository = pageRepository;
            _websiteRepository = websiteRepository;
            _insertValidator = insertValidator;
            _updateValidator = updateValidator;
            _mapper = mapper;
        }

        public async Task<PageModel> GetByIdAsync(long pageId)
        {
            return await _pageRepository.GetByIdAsync(pageId);
        }

        public async Task<PageModel?> GetBySlugAsync(string pageSlug, string websiteSlug, string domain)
        {
            if (string.IsNullOrWhiteSpace(websiteSlug) && string.IsNullOrWhiteSpace(domain))
                return null;

            if (string.IsNullOrWhiteSpace(pageSlug))
                pageSlug = "home";

            return await _pageRepository.GetBySlugAsync(pageSlug, websiteSlug, domain);
        }

        public async Task<IEnumerable<PageModel>> ListByWebsiteAsync(long websiteId)
        {
            return await _pageRepository.ListByWebsiteAsync(websiteId);
        }

        public async Task<IEnumerable<PageModel>> ListPublicAsync(string websiteSlug, string domain)
        {
            if (string.IsNullOrWhiteSpace(websiteSlug) && string.IsNullOrWhiteSpace(domain))
                return Enumerable.Empty<PageModel>();

            return await _pageRepository.ListByWebsiteSlugOrDomainAsync(websiteSlug, domain);
        }

        public async Task<PageModel> InsertAsync(PageInsertInfo page, long userId)
        {
            ValidationHelper.Validate(_insertValidator, page);
            var website = await _websiteRepository.GetByIdAsync(page.WebsiteId);
            if (website == null)
                throw new Exception("Website not found");
            website.ValidateOwnership(userId);

            var model = _mapper.Map<PageModel>(page);
            model.GenerateSlug(page.Name);
            model.MarkCreated();

            return await _pageRepository.InsertAsync(model);
        }

        public async Task<PageModel> UpdateAsync(PageUpdateInfo page, long userId)
        {
            ValidationHelper.Validate(_updateValidator, page);
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
