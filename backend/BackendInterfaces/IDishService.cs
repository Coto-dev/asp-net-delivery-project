﻿using Backend.Common.Enums;
using Common.DTO;
using Common.Enums;

namespace Common.AuthInterfaces {
   public interface IDishService {
        Task<DishDetailsDTO> GetDishById(Guid id);
        Task<DishDetailsDTO> GetPagedListDishes(Categories Category,bool Vegetarian,DishSorting Sorting,int page,Guid RestarauntId);
        Task<Response> AddRatingToDish(Guid DishId, double value);
    }
}