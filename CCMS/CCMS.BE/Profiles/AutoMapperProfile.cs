using AutoMapper;
using CCMS.BE.Data.Models;
using CCMS.Common.Dto;

namespace CCMS.BE.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<AppSetting, AppSettingDto>();
            CreateMap<UserSettingDto, UserSetting>().ReverseMap();

        }
    }
}
