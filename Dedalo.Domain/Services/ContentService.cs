using AutoMapper;
using Dedalo.Infra.Interfaces.Repository;
using Dedalo.Domain.Models;
using Dedalo.Domain.Interfaces;
using Dedalo.DTO.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dedalo.Domain.Services
{
    public class ContentService : IContentService
    {
        private readonly IContentRepository<ContentModel> _contentRepository;
        private readonly IWebsiteRepository<WebsiteModel> _websiteRepository;
        private readonly IMapper _mapper;

        public ContentService(
            IContentRepository<ContentModel> contentRepository,
            IWebsiteRepository<WebsiteModel> websiteRepository,
            IMapper mapper
        )
        {
            _contentRepository = contentRepository;
            _websiteRepository = websiteRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ContentModel>> SaveAreaAsync(ContentAreaInfo area, long userId)
        {
            if (string.IsNullOrWhiteSpace(area.ContentSlug))
                throw new Exception("ContentSlug is required");

            var website = await _websiteRepository.GetByIdAsync(area.WebsiteId);
            if (website == null)
                throw new Exception("Website not found");
            website.ValidateOwnership(userId);

            var existing = (await _contentRepository.ListBySlugAsync(area.PageId, area.ContentSlug)).ToList();
            var existingIds = existing.Select(e => e.ContentId).ToHashSet();
            var incomingIds = area.Items.Where(i => i.ContentId > 0).Select(i => i.ContentId).ToHashSet();

            // Delete: items that exist in DB but not in incoming list
            var toDeleteIds = existingIds.Except(incomingIds).ToList();
            if (toDeleteIds.Any())
                await _contentRepository.DeleteBatchAsync(toDeleteIds);

            // Update: items that exist in both
            var toUpdate = new List<ContentModel>();
            foreach (var item in area.Items.Where(i => i.ContentId > 0 && existingIds.Contains(i.ContentId)))
            {
                var model = existing.First(e => e.ContentId == item.ContentId);
                model.ContentType = item.ContentType;
                model.Index = item.Index;
                model.ContentValue = item.ContentValue;
                model.MarkUpdated();
                toUpdate.Add(model);
            }
            if (toUpdate.Any())
                await _contentRepository.UpdateBatchAsync(toUpdate);

            // Insert: items with ContentId == 0
            var toInsert = new List<ContentModel>();
            foreach (var item in area.Items.Where(i => i.ContentId == 0))
            {
                var model = new ContentModel
                {
                    WebsiteId = area.WebsiteId,
                    PageId = area.PageId,
                    ContentSlug = area.ContentSlug,
                    ContentType = item.ContentType,
                    Index = item.Index,
                    ContentValue = item.ContentValue
                };
                model.MarkCreated();
                toInsert.Add(model);
            }
            if (toInsert.Any())
                await _contentRepository.InsertBatchAsync(toInsert);

            return await _contentRepository.ListBySlugAsync(area.PageId, area.ContentSlug);
        }

        public async Task<ContentModel> GetByIdAsync(long contentId)
        {
            return await _contentRepository.GetByIdAsync(contentId);
        }

        public async Task<IEnumerable<ContentModel>> ListByPageAsync(long pageId)
        {
            return await _contentRepository.ListByPageAsync(pageId);
        }

        public async Task<IEnumerable<ContentModel>> ListPublicAsync(string pageSlug, string websiteSlug, string domain)
        {
            if (string.IsNullOrWhiteSpace(pageSlug))
                throw new Exception("Page slug is required");

            if (string.IsNullOrWhiteSpace(websiteSlug) && string.IsNullOrWhiteSpace(domain))
                throw new Exception("Website slug or domain is required");

            return await _contentRepository.ListByPageSlugAsync(pageSlug, websiteSlug, domain);
        }

        public async Task<ContentModel> InsertAsync(ContentInsertInfo content, long userId)
        {
            var website = await _websiteRepository.GetByIdAsync(content.WebsiteId);
            if (website == null)
                throw new Exception("Website not found");
            website.ValidateOwnership(userId);

            var model = _mapper.Map<ContentModel>(content);
            model.MarkCreated();

            return await _contentRepository.InsertAsync(model);
        }

        public async Task<ContentModel> UpdateAsync(ContentUpdateInfo content, long userId)
        {
            var existing = await _contentRepository.GetByIdAsync(content.ContentId);
            if (existing == null)
                throw new Exception("Content not found");

            var website = await _websiteRepository.GetByIdAsync(existing.WebsiteId);
            if (website == null)
                throw new Exception("Website not found");
            website.ValidateOwnership(userId);

            _mapper.Map(content, existing);
            existing.MarkUpdated();

            return await _contentRepository.UpdateAsync(existing);
        }

        public async Task DeleteAsync(long contentId, long userId)
        {
            var existing = await _contentRepository.GetByIdAsync(contentId);
            if (existing == null)
                throw new Exception("Content not found");

            var website = await _websiteRepository.GetByIdAsync(existing.WebsiteId);
            if (website == null)
                throw new Exception("Website not found");
            website.ValidateOwnership(userId);

            await _contentRepository.DeleteAsync(contentId);
        }
    }
}
