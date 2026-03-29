using AutoMapper;
using Dedalo.Domain.Models;
using Dedalo.DTO.Menu;
using Dedalo.Infra.Context;
using Dedalo.Infra.Mappers;
using Xunit;

namespace Dedalo.Tests.Domain.Mappers
{
    public class MenuProfileTests
    {
        private readonly IMapper _mapper;

        public MenuProfileTests()
        {
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MenuProfile>()).CreateMapper();
        }

        [Fact]
        public void Entity_To_Model_MapsEnumCorrectly()
        {
            var entity = new Menu
            {
                MenuId = 1, WebsiteId = 2, Name = "Home",
                LinkType = 3, PageId = 5, ParentId = null
            };

            var model = _mapper.Map<MenuModel>(entity);

            Assert.Equal(1, model.MenuId);
            Assert.Equal(LinkTypeEnum.InternalPage, model.LinkType);
            Assert.Equal(5, model.PageId);
            Assert.Null(model.ParentId);
        }

        [Fact]
        public void Model_To_Entity_MapsEnumToInt()
        {
            var model = new MenuModel
            {
                MenuId = 1, WebsiteId = 2, Name = "Home",
                LinkType = LinkTypeEnum.External, ExternalLink = "https://example.com"
            };

            var entity = _mapper.Map<Menu>(model);

            Assert.Equal(2, entity.LinkType);
            Assert.Equal("https://example.com", entity.ExternalLink);
            Assert.Null(entity.Website);
            Assert.Null(entity.Page);
            Assert.Null(entity.Parent);
            Assert.Empty(entity.Children);
        }

        [Fact]
        public void InsertInfo_To_Model_IgnoresIdAndTimestamps()
        {
            var dto = new MenuInsertInfo
            {
                WebsiteId = 2, Name = "Products",
                LinkType = LinkTypeEnum.None, ParentId = 1
            };

            var model = _mapper.Map<MenuModel>(dto);

            Assert.Equal(0, model.MenuId);
            Assert.Equal(2, model.WebsiteId);
            Assert.Equal(1, model.ParentId);
            Assert.Equal(LinkTypeEnum.None, model.LinkType);
        }

        [Fact]
        public void Model_To_Info_MapsAllFields()
        {
            var model = new MenuModel
            {
                MenuId = 1, WebsiteId = 2, Name = "Home",
                LinkType = LinkTypeEnum.InternalPage, PageId = 5, ParentId = null
            };

            var info = _mapper.Map<MenuInfo>(model);

            Assert.Equal(1, info.MenuId);
            Assert.Equal(2, info.WebsiteId);
            Assert.Equal(LinkTypeEnum.InternalPage, info.LinkType);
            Assert.Equal(5, info.PageId);
        }

        [Fact]
        public void Configuration_IsValid()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MenuProfile>());
            config.AssertConfigurationIsValid();
        }
    }
}
