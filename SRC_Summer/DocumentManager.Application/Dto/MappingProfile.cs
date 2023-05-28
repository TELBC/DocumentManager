using System;
using System.Linq;
using AutoMapper;
using DocumentManager.Model;
using Microsoft.EntityFrameworkCore;

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