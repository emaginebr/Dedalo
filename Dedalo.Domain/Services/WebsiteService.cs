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
        private readonly IMapper _mapper;

        public WebsiteService(
            IWebsiteRepository<WebsiteModel> websiteRepository,
            IMapper mapper
        )
        {
            _websiteRepository = websiteRepository;
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

        public async Task<WebsiteModel> GetBySlugAsync(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
                throw new Exception("Slug is required");

            var model = await _websiteRepository.GetBySlugAsync(slug);
            if (model == null)
                throw new Exception("Website not found for the provided slug");

            return model;
        }

        public async Task<WebsiteModel> GetByDomainAsync(string domain)
        {
            if (string.IsNullOrWhiteSpace(domain))
                throw new Exception("Domain is required");

            var model = await _websiteRepository.GetByDomainAsync(domain);
            if (model == null)
                throw new Exception("Website not found for the provided domain");

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
            model.Status = WebsiteStatusEnum.Active;
            model.MarkCreated();

            return await _websiteRepository.InsertAsync(model);
        }

        public async Task<WebsiteModel> UpdateAsync(WebsiteUpdateInfo website, long userId)
        {
            var existing = await _websiteRepository.GetByIdAsync(website.WebsiteId);
            if (existing == null)
                throw new Exception("Website not found");

            existing.ValidateOwnership(userId);
            _mapper.Map(website, existing);
            existing.MarkUpdated();

            return await _websiteRepository.UpdateAsync(existing);
        }

        public async Task UpdateLogoAsync(long websiteId, string logoUrl, long userId)
        {
            var existing = await _websiteRepository.GetByIdAsync(websiteId);
            if (existing == null)
                throw new Exception("Website not found");

            existing.ValidateOwnership(userId);
            await _websiteRepository.UpdateLogoAsync(websiteId, logoUrl);
        }

        public async Task DeleteAsync(long websiteId, long userId)
        {
            var existing = await _websiteRepository.GetByIdAsync(websiteId);
            if (existing == null)
                throw new Exception("Website not found");

            existing.ValidateOwnership(userId);
            await _websiteRepository.DeleteAsync(websiteId);
        }
    }
}
