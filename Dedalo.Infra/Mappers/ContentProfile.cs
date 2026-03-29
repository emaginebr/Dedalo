using AutoMapper;
using Dedalo.Domain.Models;
using Dedalo.DTO.Content;
using Dedalo.Infra.Context;

namespace Dedalo.Infra.Mappers
{
    public class ContentProfile : Profile
    {
        public ContentProfile()
        {
            // Entity <-> Model
            CreateMap<Content, ContentModel>();
            CreateMap<ContentModel, Content>()
                .ForMember(d => d.Website, opt => opt.Ignore())
                .ForMember(d => d.Page, opt => opt.Ignore());

            // DTO -> Model
            CreateMap<ContentInsertInfo, ContentModel>()
                .ForMember(d => d.ContentId, opt => opt.Ignore())
                .ForMember(d => d.CreatedAt, opt => opt.Ignore())
                .ForMember(d => d.UpdatedAt, opt => opt.Ignore());
            CreateMap<ContentUpdateInfo, ContentModel>()
                .ForMember(d => d.WebsiteId, opt => opt.Ignore())
                .ForMember(d => d.PageId, opt => opt.Ignore())
                .ForMember(d => d.CreatedAt, opt => opt.Ignore())
                .ForMember(d => d.UpdatedAt, opt => opt.Ignore());

            // Model -> DTO
            CreateMap<ContentModel, ContentInfo>();
        }
    }
}
