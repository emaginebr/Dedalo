using AutoMapper;
using Dedalo.Domain.Models;
using Dedalo.Domain.Services;
using Dedalo.DTO.Website;
using Dedalo.Infra.Interfaces.Repository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Dedalo.Tests.Domain.Services
{
    public class WebsiteServiceTests
    {
        private readonly Mock<IWebsiteRepository<WebsiteModel>> _websiteRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly WebsiteService _service;

        public WebsiteServiceTests()
        {
            _websiteRepository = new Mock<IWebsiteRepository<WebsiteModel>>();
            _mapper = new Mock<IMapper>();
            _service = new WebsiteService(_websiteRepository.Object, _mapper.Object);
        }

        // -- GetByIdAsync --

        [Fact]
        public async Task GetByIdAsync_ReturnsModel_WhenOwnerMatches()
        {
            var model = new WebsiteModel { WebsiteId = 1, UserId = 10 };
            _websiteRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(model);

            var result = await _service.GetByIdAsync(1, 10);

            Assert.NotNull(result);
            Assert.Equal(1, result.WebsiteId);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
        {
            _websiteRepository.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((WebsiteModel)null);

            var result = await _service.GetByIdAsync(99, 10);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetByIdAsync_ThrowsUnauthorized_WhenOwnerMismatch()
        {
            var model = new WebsiteModel { WebsiteId = 1, UserId = 10 };
            _websiteRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(model);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _service.GetByIdAsync(1, 999));
        }

        // -- GetBySlugAsync --

        [Fact]
        public async Task GetBySlugAsync_ReturnsModel_WhenFound()
        {
            var model = new WebsiteModel { WebsiteId = 1, WebsiteSlug = "my-site" };
            _websiteRepository.Setup(r => r.GetBySlugAsync("my-site")).ReturnsAsync(model);

            var result = await _service.GetBySlugAsync("my-site");

            Assert.Equal("my-site", result.WebsiteSlug);
        }

        [Fact]
        public async Task GetBySlugAsync_Throws_WhenSlugEmpty()
        {
            await Assert.ThrowsAsync<Exception>(() => _service.GetBySlugAsync(""));
        }

        [Fact]
        public async Task GetBySlugAsync_Throws_WhenNotFound()
        {
            _websiteRepository.Setup(r => r.GetBySlugAsync("unknown")).ReturnsAsync((WebsiteModel)null);

            await Assert.ThrowsAsync<Exception>(() => _service.GetBySlugAsync("unknown"));
        }

        // -- GetByDomainAsync --

        [Fact]
        public async Task GetByDomainAsync_ReturnsModel_WhenFound()
        {
            var model = new WebsiteModel { WebsiteId = 1, CustomDomain = "site.com" };
            _websiteRepository.Setup(r => r.GetByDomainAsync("site.com")).ReturnsAsync(model);

            var result = await _service.GetByDomainAsync("site.com");

            Assert.Equal("site.com", result.CustomDomain);
        }

        [Fact]
        public async Task GetByDomainAsync_Throws_WhenDomainEmpty()
        {
            await Assert.ThrowsAsync<Exception>(() => _service.GetByDomainAsync(""));
        }

        [Fact]
        public async Task GetByDomainAsync_Throws_WhenNotFound()
        {
            _websiteRepository.Setup(r => r.GetByDomainAsync("unknown.com")).ReturnsAsync((WebsiteModel)null);

            await Assert.ThrowsAsync<Exception>(() => _service.GetByDomainAsync("unknown.com"));
        }

        // -- ListByUserAsync --

        [Fact]
        public async Task ListByUserAsync_ReturnsListFromRepository()
        {
            var list = new List<WebsiteModel> { new WebsiteModel { WebsiteId = 1 } };
            _websiteRepository.Setup(r => r.ListByUserAsync(10)).ReturnsAsync(list);

            var result = await _service.ListByUserAsync(10);

            Assert.Single(result);
        }

        // -- InsertAsync --

        [Fact]
        public async Task InsertAsync_SetsOwnerAndStatus_ReturnsModel()
        {
            var dto = new WebsiteInsertInfo { Name = "My Site" };
            var model = new WebsiteModel { Name = "My Site" };

            _mapper.Setup(m => m.Map<WebsiteModel>(dto)).Returns(model);
            _websiteRepository.Setup(r => r.InsertAsync(It.IsAny<WebsiteModel>()))
                .ReturnsAsync((WebsiteModel m) => { m.WebsiteId = 1; return m; });

            var result = await _service.InsertAsync(dto, 10);

            Assert.Equal(10, result.UserId);
            Assert.Equal(WebsiteStatusEnum.Active, result.Status);
            Assert.Equal(1, result.WebsiteId);
            Assert.Equal("my-site", result.WebsiteSlug);
        }

        // -- UpdateAsync --

        [Fact]
        public async Task UpdateAsync_UpdatesExisting_WhenOwnerMatches()
        {
            var existing = new WebsiteModel { WebsiteId = 1, UserId = 10, Name = "Old" };
            var dto = new WebsiteUpdateInfo { WebsiteId = 1, Name = "New" };

            _websiteRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
            _mapper.Setup(m => m.Map(dto, existing));
            _websiteRepository.Setup(r => r.UpdateAsync(existing)).ReturnsAsync(existing);

            var result = await _service.UpdateAsync(dto, 10);

            Assert.NotNull(result);
            _websiteRepository.Verify(r => r.UpdateAsync(existing), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_Throws_WhenNotFound()
        {
            _websiteRepository.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((WebsiteModel)null);

            await Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(new WebsiteUpdateInfo { WebsiteId = 99 }, 10));
        }

        [Fact]
        public async Task UpdateAsync_ThrowsUnauthorized_WhenOwnerMismatch()
        {
            var existing = new WebsiteModel { WebsiteId = 1, UserId = 10 };
            _websiteRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _service.UpdateAsync(new WebsiteUpdateInfo { WebsiteId = 1 }, 999));
        }

        // -- Slug Uniqueness --

        [Fact]
        public async Task InsertAsync_Throws_WhenSlugAlreadyExists()
        {
            var dto = new WebsiteInsertInfo { Name = "My Site" };
            var model = new WebsiteModel { Name = "My Site" };
            var existing = new WebsiteModel { WebsiteId = 99, WebsiteSlug = "my-site" };

            _mapper.Setup(m => m.Map<WebsiteModel>(dto)).Returns(model);
            _websiteRepository.Setup(r => r.GetBySlugAsync("my-site")).ReturnsAsync(existing);

            var ex = await Assert.ThrowsAsync<Exception>(() => _service.InsertAsync(dto, 10));
            Assert.Contains("slug", ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task UpdateAsync_Throws_WhenSlugTakenByAnother()
        {
            var current = new WebsiteModel { WebsiteId = 1, UserId = 10, Name = "Old", WebsiteSlug = "old" };
            var another = new WebsiteModel { WebsiteId = 2, WebsiteSlug = "taken-slug" };
            var dto = new WebsiteUpdateInfo { WebsiteId = 1, WebsiteSlug = "taken-slug", Name = "Old" };

            _websiteRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(current);
            _mapper.Setup(m => m.Map(dto, current)).Callback(() => current.WebsiteSlug = "taken-slug");
            _websiteRepository.Setup(r => r.GetBySlugAsync("taken-slug")).ReturnsAsync(another);

            var ex = await Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(dto, 10));
            Assert.Contains("slug", ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task UpdateAsync_AllowsSameSlugForSameWebsite()
        {
            var current = new WebsiteModel { WebsiteId = 1, UserId = 10, Name = "Site", WebsiteSlug = "my-slug" };
            var dto = new WebsiteUpdateInfo { WebsiteId = 1, WebsiteSlug = "my-slug", Name = "Site" };

            _websiteRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(current);
            _mapper.Setup(m => m.Map(dto, current));
            _websiteRepository.Setup(r => r.GetBySlugAsync("my-slug")).ReturnsAsync(current);
            _websiteRepository.Setup(r => r.UpdateAsync(current)).ReturnsAsync(current);

            var result = await _service.UpdateAsync(dto, 10);

            Assert.NotNull(result);
        }

        // -- Domain Uniqueness --

        [Fact]
        public async Task InsertAsync_Throws_WhenDomainAlreadyExists()
        {
            var dto = new WebsiteInsertInfo { Name = "My Site", CustomDomain = "taken.com" };
            var model = new WebsiteModel { Name = "My Site", CustomDomain = "taken.com" };
            var existing = new WebsiteModel { WebsiteId = 99, CustomDomain = "taken.com" };

            _mapper.Setup(m => m.Map<WebsiteModel>(dto)).Returns(model);
            _websiteRepository.Setup(r => r.GetByDomainAsync("taken.com")).ReturnsAsync(existing);

            var ex = await Assert.ThrowsAsync<Exception>(() => _service.InsertAsync(dto, 10));
            Assert.Contains("domain", ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task InsertAsync_AllowsEmptyDomain()
        {
            var dto = new WebsiteInsertInfo { Name = "My Site" };
            var model = new WebsiteModel { Name = "My Site" };

            _mapper.Setup(m => m.Map<WebsiteModel>(dto)).Returns(model);
            _websiteRepository.Setup(r => r.InsertAsync(It.IsAny<WebsiteModel>()))
                .ReturnsAsync((WebsiteModel m) => { m.WebsiteId = 1; return m; });

            var result = await _service.InsertAsync(dto, 10);

            Assert.Equal(1, result.WebsiteId);
        }
    }
}
