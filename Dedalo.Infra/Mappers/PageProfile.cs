using System.Linq;
using AutoMapper;
using Dedalo.Domain.Models;
using Dedalo.DTO.Content;
using Dedalo.DTO.Page;
using Dedalo.Infra.Context;

namespace Dedalo.Infra.Mappers
{
    public class PageProfile : Profile
    {
        public PageProfile()
        {
            // Entity <-> Model
            CreateMap<Page, PageModel>()
                .ForMember(d => d.Contents, opt => opt.MapFrom(s => s.Contents));
            CreateMap<PageModel, Page>()
                .ForMember(d => d.Website, opt => opt.Ignore())
                .ForMember(d => d.Menus, opt => opt.Ignore())
                .ForMember(d => d.Contents, opt => opt.Ignore());

            // DTO -> Model
            CreateMap<PageInsertInfo, PageModel>()
                .ForMember(d => d.PageId, opt => opt.Ignore())
                .ForMember(d => d.PageSlug, opt => opt.Ignore())
                .ForMember(d => d.Contents, opt => opt.Ignore())
                .ForMember(d => d.CreatedAt, opt => opt.Ignore())
                .ForMember(d => d.UpdatedAt, opt => opt.Ignore());
            CreateMap<PageUpdateInfo, PageModel>()
                .ForMember(d => d.WebsiteId, opt => opt.Ignore())
                .ForMember(d => d.Contents, opt => opt.Ignore())
                .ForMember(d => d.CreatedAt, opt => opt.Ignore())
                .ForMember(d => d.UpdatedAt, opt => opt.Ignore());

            // Model -> DTO
            CreateMap<PageModel, PageInfo>();
            CreateMap<PageModel, PagePublicInfo>()
                .ForMember(d => d.Contents, opt => opt.MapFrom((src, dest, _, ctx) =>
                    src.Contents
                        .OrderBy(c => c.Index)
                        .GroupBy(c => c.ContentSlug ?? "")
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(c => ctx.Mapper.Map<ContentInfo>(c)).ToList()
                        )
                ));
        }
    }
}
