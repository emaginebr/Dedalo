using AutoMapper;
using Dedalo.Domain.Models;
using Dedalo.Domain.Services;
using Dedalo.Domain.Validators;
using Dedalo.DTO.Menu;
using Dedalo.Infra.Interfaces.Repository;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Dedalo.Tests.Domain.Services
{
    public class MenuServiceTests
    {
        private readonly Mock<IMenuRepository<MenuModel>> _menuRepository;
        private readonly Mock<IWebsiteRepository<WebsiteModel>> _websiteRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly MenuService _service;

        public MenuServiceTests()
        {
            _menuRepository = new Mock<IMenuRepository<MenuModel>>();
            _websiteRepository = new Mock<IWebsiteRepository<WebsiteModel>>();
            _mapper = new Mock<IMapper>();
            _service = new MenuService(
                _menuRepository.Object,
                _websiteRepository.Object,
                new MenuInsertValidator(),
                new MenuUpdateValidator(),
                _mapper.Object
            );
        }

        // -- GetByIdAsync --

        [Fact]
        public async Task GetByIdAsync_ReturnsModel()
        {
            var model = new MenuModel { MenuId = 1 };
            _menuRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(model);

            var result = await _service.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.MenuId);
        }

        // -- ListByWebsiteAsync --

        [Fact]
        public async Task ListByWebsiteAsync_ReturnsListFromRepository()
        {
            var list = new List<MenuModel> { new MenuModel { MenuId = 1 } };
            _menuRepository.Setup(r => r.ListByWebsiteAsync(1)).ReturnsAsync(list);

            var result = await _service.ListByWebsiteAsync(1);

            Assert.Single(result);
        }

        // -- ListPublicAsync --

        [Fact]
        public async Task ListPublicAsync_ReturnsList_WithWebsiteSlug()
        {
            var list = new List<MenuModel> { new MenuModel { MenuId = 1 } };
            _menuRepository.Setup(r => r.ListByWebsiteSlugOrDomainAsync("my-site", null)).ReturnsAsync(list);

            var result = await _service.ListPublicAsync("my-site", null);

            Assert.Single(result);
        }

        [Fact]
        public async Task ListPublicAsync_ReturnsList_WithDomain()
        {
            var list = new List<MenuModel> { new MenuModel { MenuId = 1 } };
            _menuRepository.Setup(r => r.ListByWebsiteSlugOrDomainAsync(null, "site.com")).ReturnsAsync(list);

            var result = await _service.ListPublicAsync(null, "site.com");

            Assert.Single(result);
        }

        [Fact]
        public async Task ListPublicAsync_ReturnsEmpty_WhenBothEmpty()
        {
            var result = await _service.ListPublicAsync("", "");

            Assert.Empty(result);
        }

        // -- InsertAsync --

        [Fact]
        public async Task InsertAsync_CreatesMenu_WhenOwnerMatches()
        {
            var website = new WebsiteModel { WebsiteId = 1, UserId = 10 };
            var dto = new MenuInsertInfo { WebsiteId = 1, Name = "Home", LinkType = LinkTypeEnum.None };
            var model = new MenuModel { WebsiteId = 1, Name = "Home" };

            _websiteRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(website);
            _mapper.Setup(m => m.Map<MenuModel>(dto)).Returns(model);
            _menuRepository.Setup(r => r.InsertAsync(It.IsAny<MenuModel>()))
                .ReturnsAsync((MenuModel m) => { m.MenuId = 1; return m; });

            var result = await _service.InsertAsync(dto, 10);

            Assert.Equal(1, result.MenuId);
        }

        [Fact]
        public async Task InsertAsync_Throws_WhenWebsiteNotFound()
        {
            _websiteRepository.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((WebsiteModel)null);

            await Assert.ThrowsAsync<Exception>(() =>
                _service.InsertAsync(new MenuInsertInfo { WebsiteId = 99, Name = "Test", LinkType = LinkTypeEnum.None }, 10));
        }

        [Fact]
        public async Task InsertAsync_ThrowsUnauthorized_WhenOwnerMismatch()
        {
            var website = new WebsiteModel { WebsiteId = 1, UserId = 10 };
            _websiteRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(website);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _service.InsertAsync(new MenuInsertInfo { WebsiteId = 1, Name = "Test", LinkType = LinkTypeEnum.None }, 999));
        }

        // -- UpdateAsync --

        [Fact]
        public async Task UpdateAsync_Updates_WhenOwnerMatches()
        {
            var existing = new MenuModel { MenuId = 1, WebsiteId = 1 };
            var website = new WebsiteModel { WebsiteId = 1, UserId = 10 };
            var dto = new MenuUpdateInfo { MenuId = 1, Name = "New", LinkType = LinkTypeEnum.None };

            _menuRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
            _websiteRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(website);
            _mapper.Setup(m => m.Map(dto, existing));
            _menuRepository.Setup(r => r.UpdateAsync(existing)).ReturnsAsync(existing);

            var result = await _service.UpdateAsync(dto, 10);

            Assert.NotNull(result);
            _menuRepository.Verify(r => r.UpdateAsync(existing), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_Throws_WhenMenuNotFound()
        {
            _menuRepository.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((MenuModel)null);

            await Assert.ThrowsAsync<Exception>(() =>
                _service.UpdateAsync(new MenuUpdateInfo { MenuId = 99, Name = "Test", LinkType = LinkTypeEnum.None }, 10));
        }

        // -- DeleteAsync --

        [Fact]
        public async Task DeleteAsync_Deletes_WhenOwnerMatches()
        {
            var existing = new MenuModel { MenuId = 1, WebsiteId = 1 };
            var website = new WebsiteModel { WebsiteId = 1, UserId = 10 };

            _menuRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
            _websiteRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(website);

            await _service.DeleteAsync(1, 10);

            _menuRepository.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_Throws_WhenMenuNotFound()
        {
            _menuRepository.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((MenuModel)null);

            await Assert.ThrowsAsync<Exception>(() => _service.DeleteAsync(99, 10));
        }

        [Fact]
        public async Task DeleteAsync_ThrowsUnauthorized_WhenOwnerMismatch()
        {
            var existing = new MenuModel { MenuId = 1, WebsiteId = 1 };
            var website = new WebsiteModel { WebsiteId = 1, UserId = 10 };

            _menuRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
            _websiteRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(website);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _service.DeleteAsync(1, 999));
        }
    }
}
