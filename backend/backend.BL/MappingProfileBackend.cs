using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Backend.DAL.Data.Entities;
using Common.DTO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Backend.BL {
	public class MappingProfileBackend : Profile {
		public MappingProfileBackend() {
			CreateMap<Menu, MenuDTO>().ForMember(dest => dest.DishDetails,
				source => source.MapFrom(source => source.Dishes.Select(x => x).ToList()));

			CreateMap<Menu, MenuShortDTO>();

			CreateMap<DishInCart, DishBasketDTO>().ForMember(dest => dest.TotalPrice,
				source => source.MapFrom(source => source.Dish.Price * source.Count))
				.ForMember(dest => dest.Price,
				source => source.MapFrom(source => source.Dish.Price))
				.ForMember(dest => dest.Name,
				source => source.MapFrom(source => source.Dish.Name))
				.ForMember(dest => dest.PhotoUrl,
				source => source.MapFrom(source => source.Dish.PhotoUrl));

			CreateMap<DishModelDTO, Dish>();
			CreateMap<Dish, DishDetailsDTO>().ForMember(dest => dest.Rating,
				source => source.MapFrom(source => source.Ratings.Count != 0 ? source.Ratings.Average(x => x.Value) : 0));

			CreateMap<Restaraunt, RestarauntDTO>();

		}
	}
}
