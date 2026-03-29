using AutoMapper;
using Dedalo.Domain.Models;
using Dedalo.DTO.Website;
using Dedalo.Infra.Context;

namespace Dedalo.Infra.Mappers
{
    public class WebsiteProfile : Profile
    {
        public WebsiteProfile()
        {
            // Entity <-> Model
            CreateMap<Website, WebsiteModel>()
                .ForMember(d => d.DomainType, opt => opt.MapFrom(s => (DomainTypeEnum)s.DomainType))
                .ForMember(d => d.Status, opt => opt.MapFrom(s => (WebsiteStatusEnum)s.Status));
            CreateMap<WebsiteModel, Website>()
                .ForMember(d => d.DomainType, opt => opt.MapFrom(s => (int)s.DomainType))
                .ForMember(d => d.Status, opt => opt.MapFrom(s => (int)s.Status))
                .ForMember(d => d.Pages, opt => opt.Ignore())
                .ForMember(d => d.Menus, opt => opt.Ignore())
                .ForMember(d => d.Contents, opt => opt.Ignore());

            // DTO -> Model
            CreateMap<WebsiteInsertInfo, WebsiteModel>()
                .ForMember(d => d.WebsiteId, opt => opt.Ignore())
                .ForMember(d => d.UserId, opt => opt.Ignore())
                .ForMember(d => d.LogoUrl, opt => opt.Ignore())
                .ForMember(d => d.Status, opt => opt.Ignore())
                .ForMember(d => d.CreatedAt, opt => opt.Ignore())
                .ForMember(d => d.UpdatedAt, opt => opt.Ignore());
            CreateMap<WebsiteUpdateInfo, WebsiteModel>()
                .ForMember(d => d.UserId, opt => opt.Ignore())
                .ForMember(d => d.LogoUrl, opt => opt.Ignore())
                .ForMember(d => d.CreatedAt, opt => opt.Ignore())
                .ForMember(d => d.UpdatedAt, opt => opt.Ignore());

            // Model -> DTO
            CreateMap<WebsiteModel, WebsiteInfo>();
        }
    }
}
