using AutoMapper;
using DocumentManager.Model;

namespace DocumentManager.Dto;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<DocumentDto, Document>();
        CreateMap<Document, DocumentDto>();
    }
}