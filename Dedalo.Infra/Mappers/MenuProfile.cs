using AutoMapper;
using Dedalo.Domain.Models;
using Dedalo.DTO.Menu;
using Dedalo.Infra.Context;

namespace Dedalo.Infra.Mappers
{
    public class MenuProfile : Profile
    {
        public MenuProfile()
        {
            // Entity <-> Model
            CreateMap<Menu, MenuModel>()
                .ForMember(d => d.LinkType, opt => opt.MapFrom(s => (LinkTypeEnum)s.LinkType));
            CreateMap<MenuModel, Menu>()
                .ForMember(d => d.LinkType, opt => opt.MapFrom(s => (int)s.LinkType))
                .ForMember(d => d.Website, opt => opt.Ignore())
                .ForMember(d => d.Page, opt => opt.Ignore())
                .ForMember(d => d.Parent, opt => opt.Ignore())
                .ForMember(d => d.Children, opt => opt.Ignore());

            // DTO -> Model
            CreateMap<MenuInsertInfo, MenuModel>()
                .ForMember(d => d.MenuId, opt => opt.Ignore())
                .ForMember(d => d.CreatedAt, opt => opt.Ignore())
                .ForMember(d => d.UpdatedAt, opt => opt.Ignore());
            CreateMap<MenuUpdateInfo, MenuModel>()
                .ForMember(d => d.WebsiteId, opt => opt.Ignore())
                .ForMember(d => d.CreatedAt, opt => opt.Ignore())
                .ForMember(d => d.UpdatedAt, opt => opt.Ignore());

            // Model -> DTO
            CreateMap<MenuModel, MenuInfo>();
        }
    }
}
