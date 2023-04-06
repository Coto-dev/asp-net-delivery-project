using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auth.BL.Data.Entities;
using AutoMapper;
using Common.DTO;

namespace Auth.BL {
    public class MappingProfile : Profile {
        public MappingProfile() {
            CreateMap<User, RegisterModelDTO>();
            CreateMap<User, RegisterModelDTO>().ReverseMap().ForMember(dest => dest.UserName, 
                source => source.MapFrom(source => source.Email))
                .ForMember(dest => dest.Email,
                source => source.MapFrom(source => source.Email));
            CreateMap<User, ProfileDTO>()
            .ForMember(dest => dest.Roles,
                source => source.MapFrom(source => source.Roles.Select(x => x.Role).Select(r => r.Name.ToString()).ToList()))
            .ForMember(dest => dest.Address,
                source => source.MapFrom(source => source.Customer.Address));
              CreateMap<ProfileDTO, User>();
            CreateMap<User, EditProfileDTO>();
            CreateMap<User, EditProfileDTO>().ReverseMap();
            /*CreateMap<Customer, RegisterModelDTO>();
            CreateMap<Customer, RegisterModelDTO>().ReverseMap().ForMember(dest => dest.UserName,
                source => source.MapFrom(source => source.Email))
                .ForMember(dest => dest.Email,
                source => source.MapFrom(source => source.Email));*/

        }
    }
}
