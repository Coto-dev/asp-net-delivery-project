using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auth.DAL.Data.Entities;
using AutoMapper;
using Common.DTO;

namespace Auth.BL {
    public class MappingProfile : Profile {
        public MappingProfile() {
            CreateMap<User, RegisterModelDTO>();
            CreateMap<User, RegisterModelDTO>().ReverseMap();

        }
    }
}
