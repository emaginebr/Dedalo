using AutoMapper;
using Dedalo.Domain.Models;
using Dedalo.DTO.Content;
using Dedalo.Infra.Context;
using Dedalo.Infra.Mappers;
using Xunit;

namespace Dedalo.Tests.Domain.Mappers
{
    public class ContentProfileTests
    {
        private readonly IMapper _mapper;

        public ContentProfileTests()
        {
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<ContentProfile>()).CreateMapper();
        }

        [Fact]
        public void Entity_To_Model_MapsAllFields()
        {
            var entity = new Content
            {
                ContentId = 1, WebsiteId = 2, PageId = 3,
                ContentType = "text-block", Index = 2, ContentSlug = "hero",
                ContentValue = "<h1>Hello</h1>"
            };

            var model = _mapper.Map<ContentModel>(entity);

            Assert.Equal(1, model.ContentId);
            Assert.Equal(2, model.WebsiteId);
            Assert.Equal(3, model.PageId);
            Assert.Equal("text-block", model.ContentType);
            Assert.Equal(2, model.Index);
            Assert.Equal("hero", model.ContentSlug);
            Assert.Equal("<h1>Hello</h1>", model.ContentValue);
        }

        [Fact]
        public void Model_To_Entity_IgnoresNavigations()
        {
            var model = new ContentModel
            {
                ContentId = 1, WebsiteId = 2, PageId = 3,
                ContentSlug = "hero", ContentValue = "text"
            };

            var entity = _mapper.Map<Content>(model);

            Assert.Equal(1, entity.ContentId);
            Assert.Null(entity.Website);
            Assert.Null(entity.Page);
        }

        [Fact]
        public void InsertInfo_To_Model_IgnoresIdAndTimestamps()
        {
            var dto = new ContentInsertInfo
            {
                WebsiteId = 2, PageId = 3, ContentType = "image",
                Index = 0, ContentSlug = "banner", ContentValue = "Welcome"
            };

            var model = _mapper.Map<ContentModel>(dto);

            Assert.Equal(0, model.ContentId);
            Assert.Equal(2, model.WebsiteId);
            Assert.Equal(3, model.PageId);
            Assert.Equal("banner", model.ContentSlug);
        }

        [Fact]
        public void UpdateInfo_To_Model_IgnoresWebsiteIdAndPageId()
        {
            var dto = new ContentUpdateInfo
            {
                ContentId = 1, ContentType = "video", Index = 3,
                ContentSlug = "new-slug", ContentValue = "New Value"
            };

            var model = _mapper.Map<ContentModel>(dto);

            Assert.Equal(1, model.ContentId);
            Assert.Equal(0, model.WebsiteId);
            Assert.Equal(0, model.PageId);
            Assert.Equal("new-slug", model.ContentSlug);
        }

        [Fact]
        public void Model_To_Info_MapsAllFields()
        {
            var model = new ContentModel
            {
                ContentId = 1, WebsiteId = 2, PageId = 3,
                ContentType = "heading", Index = 0, ContentSlug = "hero",
                ContentValue = "text"
            };

            var info = _mapper.Map<ContentInfo>(model);

            Assert.Equal(1, info.ContentId);
            Assert.Equal(2, info.WebsiteId);
            Assert.Equal(3, info.PageId);
            Assert.Equal("hero", info.ContentSlug);
        }

        [Fact]
        public void Configuration_IsValid()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<ContentProfile>());
            config.AssertConfigurationIsValid();
        }
    }
}
