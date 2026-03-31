using AutoMapper;
using Dedalo.Domain.Models;
using Dedalo.Domain.Services;
using Dedalo.Domain.Validators;
using Dedalo.DTO.Page;
using Dedalo.Infra.Interfaces.Repository;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Dedalo.Tests.Domain.Services
{
    public class PageServiceTests
    {
        private readonly Mock<IPageRepository<PageModel>> _pageRepository;
        private readonly Mock<IWebsiteRepository<WebsiteModel>> _websiteRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly PageService _service;

        public PageServiceTests()
        {
            _pageRepository = new Mock<IPageRepository<PageModel>>();
            _websiteRepository = new Mock<IWebsiteRepository<WebsiteModel>>();
            _mapper = new Mock<IMapper>();
            _service = new PageService(
                _pageRepository.Object,
                _websiteRepository.Object,
                new PageInsertValidator(),
                new PageUpdateValidator(),
                _mapper.Object
            );
        }

        // -- GetByIdAsync --

        [Fact]
        public async Task GetByIdAsync_ReturnsModel()
        {
            var model = new PageModel { PageId = 1 };
            _pageRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(model);

            var result = await _service.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.PageId);
        }

        // -- GetBySlugAsync --

        [Fact]
        public async Task GetBySlugAsync_ReturnsModel_WithWebsiteSlug()
        {
            var model = new PageModel { PageId = 1, PageSlug = "about" };
            _pageRepository.Setup(r => r.GetBySlugAsync("about", "my-site", null)).ReturnsAsync(model);

            var result = await _service.GetBySlugAsync("about", "my-site", null);

            Assert.Equal("about", result.PageSlug);
        }

        [Fact]
        public async Task GetBySlugAsync_ReturnsModel_WithDomain()
        {
            var model = new PageModel { PageId = 1, PageSlug = "about" };
            _pageRepository.Setup(r => r.GetBySlugAsync("about", null, "site.com")).ReturnsAsync(model);

            var result = await _service.GetBySlugAsync("about", null, "site.com");

            Assert.Equal("about", result.PageSlug);
        }

        [Fact]
        public async Task GetBySlugAsync_FallsBackToHome_WhenPageSlugEmpty()
        {
            var model = new PageModel { PageId = 1, PageSlug = "home", WebsiteId = 1 };
            _pageRepository.Setup(r => r.GetBySlugAsync("home", "my-site", null)).ReturnsAsync(model);

            var result = await _service.GetBySlugAsync("", "my-site", null);

            Assert.NotNull(result);
            Assert.Equal("home", result.PageSlug);
        }

        [Fact]
        public async Task GetBySlugAsync_ReturnsNull_WhenBothWebsiteSlugAndDomainEmpty()
        {
            var result = await _service.GetBySlugAsync("about", "", "");

            Assert.Null(result);
        }

        [Fact]
        public async Task GetBySlugAsync_ReturnsNull_WhenNotFound()
        {
            _pageRepository.Setup(r => r.GetBySlugAsync("unknown", "my-site", null)).ReturnsAsync((PageModel)null);

            var result = await _service.GetBySlugAsync("unknown", "my-site", null);

            Assert.Null(result);
        }

        // -- ListByWebsiteAsync --

        [Fact]
        public async Task ListByWebsiteAsync_ReturnsListFromRepository()
        {
            var list = new List<PageModel> { new PageModel { PageId = 1 } };
            _pageRepository.Setup(r => r.ListByWebsiteAsync(1)).ReturnsAsync(list);

            var result = await _service.ListByWebsiteAsync(1);

            Assert.Single(result);
        }

        // -- InsertAsync --

        [Fact]
        public async Task InsertAsync_CreatesPage_WhenOwnerMatches()
        {
            var website = new WebsiteModel { WebsiteId = 1, UserId = 10 };
            var dto = new PageInsertInfo { WebsiteId = 1, Name = "About" };
            var model = new PageModel { WebsiteId = 1, Name = "About" };

            _websiteRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(website);
            _mapper.Setup(m => m.Map<PageModel>(dto)).Returns(model);
            _pageRepository.Setup(r => r.InsertAsync(It.IsAny<PageModel>()))
                .ReturnsAsync((PageModel m) => { m.PageId = 1; return m; });

            var result = await _service.InsertAsync(dto, 10);

            Assert.Equal(1, result.PageId);
        }

        [Fact]
        public async Task InsertAsync_Throws_WhenWebsiteNotFound()
        {
            _websiteRepository.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((WebsiteModel)null);

            await Assert.ThrowsAsync<Exception>(() =>
                _service.InsertAsync(new PageInsertInfo { WebsiteId = 99, Name = "Test" }, 10));
        }

        [Fact]
        public async Task InsertAsync_ThrowsUnauthorized_WhenOwnerMismatch()
        {
            var website = new WebsiteModel { WebsiteId = 1, UserId = 10 };
            _websiteRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(website);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _service.InsertAsync(new PageInsertInfo { WebsiteId = 1, Name = "Test" }, 999));
        }

        // -- UpdateAsync --

        [Fact]
        public async Task UpdateAsync_Updates_WhenOwnerMatches()
        {
            var existing = new PageModel { PageId = 1, WebsiteId = 1 };
            var website = new WebsiteModel { WebsiteId = 1, UserId = 10 };
            var dto = new PageUpdateInfo { PageId = 1, Name = "New Name", PageSlug = "new-name" };

            _pageRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
            _websiteRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(website);
            _mapper.Setup(m => m.Map(dto, existing));
            _pageRepository.Setup(r => r.UpdateAsync(existing)).ReturnsAsync(existing);

            var result = await _service.UpdateAsync(dto, 10);

            Assert.NotNull(result);
            _pageRepository.Verify(r => r.UpdateAsync(existing), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_Throws_WhenPageNotFound()
        {
            _pageRepository.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((PageModel)null);

            await Assert.ThrowsAsync<Exception>(() =>
                _service.UpdateAsync(new PageUpdateInfo { PageId = 99, Name = "Test", PageSlug = "test" }, 10));
        }

        // -- DeleteAsync --

        [Fact]
        public async Task DeleteAsync_Deletes_WhenOwnerMatches()
        {
            var existing = new PageModel { PageId = 1, WebsiteId = 1 };
            var website = new WebsiteModel { WebsiteId = 1, UserId = 10 };

            _pageRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
            _websiteRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(website);

            await _service.DeleteAsync(1, 10);

            _pageRepository.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_Throws_WhenPageNotFound()
        {
            _pageRepository.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((PageModel)null);

            await Assert.ThrowsAsync<Exception>(() => _service.DeleteAsync(99, 10));
        }

        [Fact]
        public async Task DeleteAsync_ThrowsUnauthorized_WhenOwnerMismatch()
        {
            var existing = new PageModel { PageId = 1, WebsiteId = 1 };
            var website = new WebsiteModel { WebsiteId = 1, UserId = 10 };

            _pageRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
            _websiteRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(website);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _service.DeleteAsync(1, 999));
        }
    }
}
