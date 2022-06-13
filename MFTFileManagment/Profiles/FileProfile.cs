using AutoMapper;
using MFTFileManagment.ViewModels;

namespace MFTFileManagment.Profiles
{
    public class FileProfile : Profile
    {
        public override string ProfileName
        {
            get
            {
                return "FileProfile";
            }
        }
        private void ConfigureMappings()
        {
            CreateMap<Documents.Data.File, FileViewModel>()
                            .ForMember(dest =>
                                        dest.Id,
                                        opt => opt.MapFrom(src => src.Id))
                            .ForMember(dest =>
                                        dest.Name,
                                        opt => opt.MapFrom(src => src.Name))
                            .ForMember(dest =>
                                        dest.Path,
                                        opt => opt.MapFrom(src => src.Path))
                            .ForMember(dest =>
                                        dest.Extension,
                                        opt => opt.MapFrom(src => src.Extension))
                            .ForMember(dest =>
                                        dest.MakeBy,
                                        opt => opt.MapFrom(src => src.MakeBy))
                            .ForMember(dest =>
                                        dest.MakeDate,
                                        opt => opt.MapFrom(src => src.MakeDate))
                            .ForMember(dest =>
                                        dest.Remarks,
                                        opt => opt.MapFrom(src => src.Remarks))
                            .ForMember(dest =>
                                        dest.CreationTime,
                                        opt => opt.MapFrom(src => src.CreationTime.HasValue ? src.CreationTime.Value.ToUniversalTime() : (DateTime?)null))
                            .ReverseMap()
                            ;
        }
        public FileProfile()
        {
            ConfigureMappings();
        }
    }
}
