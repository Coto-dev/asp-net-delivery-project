using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Backend.DAL.Data.Entities;
using Common.DTO;

namespace AdmipPanel.BL {
    public class MappingProfile : Profile {
        public MappingProfile() {
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
