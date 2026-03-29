using AutoMapper;
using Dedalo.Domain.Models;
using Dedalo.DTO.Page;
using Dedalo.Infra.Context;
using Dedalo.DTO.Content;
using Dedalo.Infra.Mappers;
using Xunit;

namespace Dedalo.Tests.Domain.Mappers
{
    public class PageProfileTests
    {
        private readonly IMapper _mapper;

        public PageProfileTests()
        {
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<PageProfile>();
                cfg.AddProfile<ContentProfile>();
            }).CreateMapper();
        }

        [Fact]
        public void Entity_To_Model_MapsAllFields()
        {
            var entity = new Page
            {
                PageId = 1, WebsiteId = 2, PageSlug = "about",
                TemplatePageSlug = "tpl-about", Name = "About"
            };

            var model = _mapper.Map<PageModel>(entity);

            Assert.Equal(1, model.PageId);
            Assert.Equal(2, model.WebsiteId);
            Assert.Equal("about", model.PageSlug);
            Assert.Equal("tpl-about", model.TemplatePageSlug);
        }

        [Fact]
        public void Model_To_Entity_IgnoresNavigations()
        {
            var model = new PageModel
            {
                PageId = 1, WebsiteId = 2, PageSlug = "about", Name = "About"
            };

            var entity = _mapper.Map<Page>(model);

            Assert.Equal(1, entity.PageId);
            Assert.Null(entity.Website);
            Assert.Empty(entity.Menus);
            Assert.Empty(entity.Contents);
        }

        [Fact]
        public void InsertInfo_To_Model_IgnoresIdAndTimestamps()
        {
            var dto = new PageInsertInfo
            {
                WebsiteId = 2, PageSlug = "contact", Name = "Contact"
            };

            var model = _mapper.Map<PageModel>(dto);

            Assert.Equal(0, model.PageId);
            Assert.Equal(2, model.WebsiteId);
            Assert.Equal("contact", model.PageSlug);
        }

        [Fact]
        public void Model_To_Info_MapsAllFields()
        {
            var model = new PageModel
            {
                PageId = 1, WebsiteId = 2, PageSlug = "about", Name = "About"
            };

            var info = _mapper.Map<PageInfo>(model);

            Assert.Equal(1, info.PageId);
            Assert.Equal(2, info.WebsiteId);
            Assert.Equal("about", info.PageSlug);
        }

        [Fact]
        public void Model_To_PublicInfo_GroupsContentsBySlug()
        {
            var model = new PageModel
            {
                PageId = 1, WebsiteId = 2, PageSlug = "about", Name = "About",
                Contents = new System.Collections.Generic.List<ContentModel>
                {
                    new ContentModel { ContentId = 10, ContentSlug = "hero", Index = 0, ContentValue = "Title" },
                    new ContentModel { ContentId = 11, ContentSlug = "hero", Index = 1, ContentValue = "Subtitle" },
                    new ContentModel { ContentId = 12, ContentSlug = "body", Index = 0, ContentValue = "Text" }
                }
            };

            var info = _mapper.Map<PagePublicInfo>(model);

            Assert.Equal(1, info.PageId);
            Assert.Equal(2, info.Contents.Count);
            Assert.True(info.Contents.ContainsKey("hero"));
            Assert.True(info.Contents.ContainsKey("body"));
            Assert.Equal(2, info.Contents["hero"].Count);
            Assert.Single(info.Contents["body"]);
            Assert.Equal("Title", info.Contents["hero"][0].ContentValue);
            Assert.Equal("Subtitle", info.Contents["hero"][1].ContentValue);
        }

        [Fact]
        public void Configuration_IsValid()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<PageProfile>();
                cfg.AddProfile<ContentProfile>();
            });
            config.AssertConfigurationIsValid();
        }
    }
}
