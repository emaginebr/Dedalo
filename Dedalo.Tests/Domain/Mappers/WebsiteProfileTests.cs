using AutoMapper;
using Dedalo.Domain.Models;
using Dedalo.DTO.Website;
using Dedalo.Infra.Context;
using Dedalo.Infra.Mappers;
using Xunit;

namespace Dedalo.Tests.Domain.Mappers
{
    public class WebsiteProfileTests
    {
        private readonly IMapper _mapper;

        public WebsiteProfileTests()
        {
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<WebsiteProfile>()).CreateMapper();
        }

        [Fact]
        public void Entity_To_Model_MapsEnumsCorrectly()
        {
            var entity = new Website
            {
                WebsiteId = 1, UserId = 10, WebsiteSlug = "slug", Name = "Site",
                DomainType = 2, Status = 3, CustomDomain = "site.com"
            };

            var model = _mapper.Map<WebsiteModel>(entity);

            Assert.Equal(1, model.WebsiteId);
            Assert.Equal(DomainTypeEnum.Folder, model.DomainType);
            Assert.Equal(WebsiteStatusEnum.Inactive, model.Status);
            Assert.Equal("site.com", model.CustomDomain);
        }

        [Fact]
        public void Model_To_Entity_MapsEnumsToInt()
        {
            var model = new WebsiteModel
            {
                WebsiteId = 1, UserId = 10, Name = "Site",
                DomainType = DomainTypeEnum.CustomDomain, Status = WebsiteStatusEnum.Blocked
            };

            var entity = _mapper.Map<Website>(model);

            Assert.Equal(3, entity.DomainType);
            Assert.Equal(2, entity.Status);
        }

        [Fact]
        public void InsertInfo_To_Model_IgnoresIdAndTimestamps()
        {
            var dto = new WebsiteInsertInfo
            {
                Name = "Site",
                DomainType = DomainTypeEnum.Subdomain, CustomDomain = "site.com"
            };

            var model = _mapper.Map<WebsiteModel>(dto);

            Assert.Equal(0, model.WebsiteId);
            Assert.Equal(0, model.UserId);
            Assert.Null(model.WebsiteSlug);
            Assert.Equal(DomainTypeEnum.Subdomain, model.DomainType);
        }

        [Fact]
        public void UpdateInfo_To_Model_IgnoresUserIdAndTimestamps()
        {
            var dto = new WebsiteUpdateInfo
            {
                WebsiteId = 1, WebsiteSlug = "new-slug", Name = "New",
                DomainType = DomainTypeEnum.Folder, Status = WebsiteStatusEnum.Active
            };

            var model = _mapper.Map<WebsiteModel>(dto);

            Assert.Equal(1, model.WebsiteId);
            Assert.Equal(0, model.UserId);
            Assert.Equal("new-slug", model.WebsiteSlug);
        }

        [Fact]
        public void Model_To_Info_MapsAllFields()
        {
            var model = new WebsiteModel
            {
                WebsiteId = 1, UserId = 10, WebsiteSlug = "slug", Name = "Site",
                DomainType = DomainTypeEnum.CustomDomain, Status = WebsiteStatusEnum.Active,
                CustomDomain = "site.com"
            };

            var info = _mapper.Map<WebsiteInfo>(model);

            Assert.Equal(1, info.WebsiteId);
            Assert.Equal(10, info.UserId);
            Assert.Equal("slug", info.WebsiteSlug);
            Assert.Equal(DomainTypeEnum.CustomDomain, info.DomainType);
            Assert.Equal(WebsiteStatusEnum.Active, info.Status);
        }

        [Fact]
        public void Configuration_IsValid()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<WebsiteProfile>());
            config.AssertConfigurationIsValid();
        }
    }
}
