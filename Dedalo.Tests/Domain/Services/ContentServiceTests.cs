using AutoMapper;
using Dedalo.Domain.Models;
using Dedalo.Domain.Services;
using Dedalo.Domain.Validators;
using Dedalo.DTO.Content;
using Dedalo.Infra.Interfaces.Repository;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Dedalo.Tests.Domain.Services
{
    public class ContentServiceTests
    {
        private readonly Mock<IContentRepository<ContentModel>> _contentRepository;
        private readonly Mock<IWebsiteRepository<WebsiteModel>> _websiteRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly ContentService _service;

        public ContentServiceTests()
        {
            _contentRepository = new Mock<IContentRepository<ContentModel>>();
            _websiteRepository = new Mock<IWebsiteRepository<WebsiteModel>>();
            _mapper = new Mock<IMapper>();
            _service = new ContentService(
                _contentRepository.Object,
                _websiteRepository.Object,
                new ContentInsertValidator(),
                new ContentUpdateValidator(),
                new ContentAreaValidator(),
                _mapper.Object
            );
        }

        // -- GetByIdAsync --

        [Fact]
        public async Task GetByIdAsync_ReturnsModel()
        {
            var model = new ContentModel { ContentId = 1 };
            _contentRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(model);

            var result = await _service.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.ContentId);
        }

        // -- ListByPageAsync --

        [Fact]
        public async Task ListByPageAsync_ReturnsListFromRepository()
        {
            var list = new List<ContentModel> { new ContentModel { ContentId = 1 } };
            _contentRepository.Setup(r => r.ListByPageAsync(1)).ReturnsAsync(list);

            var result = await _service.ListByPageAsync(1);

            Assert.Single(result);
        }

        // -- ListPublicAsync --

        [Fact]
        public async Task ListPublicAsync_ReturnsList_WithWebsiteSlug()
        {
            var list = new List<ContentModel> { new ContentModel { ContentId = 1 } };
            _contentRepository.Setup(r => r.ListByPageSlugAsync("about", "my-site", null)).ReturnsAsync(list);

            var result = await _service.ListPublicAsync("about", "my-site", null);

            Assert.Single(result);
        }

        [Fact]
        public async Task ListPublicAsync_ReturnsList_WithDomain()
        {
            var list = new List<ContentModel> { new ContentModel { ContentId = 1 } };
            _contentRepository.Setup(r => r.ListByPageSlugAsync("about", null, "site.com")).ReturnsAsync(list);

            var result = await _service.ListPublicAsync("about", null, "site.com");

            Assert.Single(result);
        }

        [Fact]
        public async Task ListPublicAsync_ReturnsEmpty_WhenPageSlugEmpty()
        {
            var result = await _service.ListPublicAsync("", "my-site", null);

            Assert.Empty(result);
        }

        [Fact]
        public async Task ListPublicAsync_ReturnsEmpty_WhenBothWebsiteSlugAndDomainEmpty()
        {
            var result = await _service.ListPublicAsync("about", "", "");

            Assert.Empty(result);
        }

        // -- InsertAsync --

        [Fact]
        public async Task InsertAsync_CreatesContent_WhenOwnerMatches()
        {
            var website = new WebsiteModel { WebsiteId = 1, UserId = 10 };
            var dto = new ContentInsertInfo { WebsiteId = 1, PageId = 1, ContentType = "text", ContentSlug = "hero" };
            var model = new ContentModel { WebsiteId = 1, PageId = 1, ContentSlug = "hero" };

            _websiteRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(website);
            _mapper.Setup(m => m.Map<ContentModel>(dto)).Returns(model);
            _contentRepository.Setup(r => r.InsertAsync(It.IsAny<ContentModel>()))
                .ReturnsAsync((ContentModel m) => { m.ContentId = 1; return m; });

            var result = await _service.InsertAsync(dto, 10);

            Assert.Equal(1, result.ContentId);
        }

        [Fact]
        public async Task InsertAsync_Throws_WhenWebsiteNotFound()
        {
            _websiteRepository.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((WebsiteModel)null);

            await Assert.ThrowsAsync<Exception>(() =>
                _service.InsertAsync(new ContentInsertInfo { WebsiteId = 99, ContentType = "text" }, 10));
        }

        [Fact]
        public async Task InsertAsync_ThrowsUnauthorized_WhenOwnerMismatch()
        {
            var website = new WebsiteModel { WebsiteId = 1, UserId = 10 };
            _websiteRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(website);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _service.InsertAsync(new ContentInsertInfo { WebsiteId = 1, ContentType = "text" }, 999));
        }

        // -- UpdateAsync --

        [Fact]
        public async Task UpdateAsync_Updates_WhenOwnerMatches()
        {
            var existing = new ContentModel { ContentId = 1, WebsiteId = 1 };
            var website = new WebsiteModel { WebsiteId = 1, UserId = 10 };
            var dto = new ContentUpdateInfo { ContentId = 1, ContentType = "text", ContentValue = "New Value" };

            _contentRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
            _websiteRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(website);
            _mapper.Setup(m => m.Map(dto, existing));
            _contentRepository.Setup(r => r.UpdateAsync(existing)).ReturnsAsync(existing);

            var result = await _service.UpdateAsync(dto, 10);

            Assert.NotNull(result);
            _contentRepository.Verify(r => r.UpdateAsync(existing), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_Throws_WhenContentNotFound()
        {
            _contentRepository.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((ContentModel)null);

            await Assert.ThrowsAsync<Exception>(() =>
                _service.UpdateAsync(new ContentUpdateInfo { ContentId = 99, ContentType = "text" }, 10));
        }

        // -- DeleteAsync --

        [Fact]
        public async Task DeleteAsync_Deletes_WhenOwnerMatches()
        {
            var existing = new ContentModel { ContentId = 1, WebsiteId = 1 };
            var website = new WebsiteModel { WebsiteId = 1, UserId = 10 };

            _contentRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
            _websiteRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(website);

            await _service.DeleteAsync(1, 10);

            _contentRepository.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_Throws_WhenContentNotFound()
        {
            _contentRepository.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((ContentModel)null);

            await Assert.ThrowsAsync<Exception>(() => _service.DeleteAsync(99, 10));
        }

        [Fact]
        public async Task DeleteAsync_ThrowsUnauthorized_WhenOwnerMismatch()
        {
            var existing = new ContentModel { ContentId = 1, WebsiteId = 1 };
            var website = new WebsiteModel { WebsiteId = 1, UserId = 10 };

            _contentRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
            _websiteRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(website);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _service.DeleteAsync(1, 999));
        }
    }
}
