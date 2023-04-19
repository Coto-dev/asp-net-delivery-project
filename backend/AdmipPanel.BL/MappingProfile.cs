using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auth.DAL.Data.Entities;
using AutoMapper;
using Backend.DAL.Data.Entities;
using Common.DTO;
using Common.Enums;

namespace AdmipPanel.BL {
    public class MappingProfile : Profile {
        public MappingProfile() {
            CreateMap<UsersViewModel, User>();
            CreateMap<User, UsersViewModel>()
                .ForMember(dest => dest.Roles,
                source => source.MapFrom(source => source.Roles.Select(x => x.Role).Select(r => r.Name.ToString()).ToList()))
                .ForMember(dest => dest.Address,
                source => source.MapFrom(source => source.Customer.Address));
			CreateMap<RestarauntDTO, Restaraunt>();
            CreateMap<Restaraunt, RestarauntDTO>();
            CreateMap<RestarauntViewModel, Restaraunt>();
            CreateMap<Restaraunt, RestarauntViewModel>().ForMember(dest => dest.ManagerEmails,
                source => source.MapFrom(source => source.Managers.Select(x => x.Id.ToString()).ToList()))
				.ForMember(dest => dest.CookEmails,
				source => source.MapFrom(source => source.Cooks.Select(x => x.Id.ToString()).ToList()));

		}
    }
}
