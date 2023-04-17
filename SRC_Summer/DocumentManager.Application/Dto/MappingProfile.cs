using AutoMapper;
using DocumentManager.Model;

namespace DocumentManager.Dto;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<DocumentDto, Document>();
        CreateMap<Document, DocumentDto>();
        CreateMap<FolderDto, Folder>();
        CreateMap<Folder, FolderDto>();
        CreateMap<TagDto, Tag>();
        CreateMap<Tag, TagDto>();
    }
}