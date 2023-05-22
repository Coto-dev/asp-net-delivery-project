using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Backend.DAL.Data;
using Backend.DAL.Data.Entities;
using Common.BackendInterfaces;
using Common.DTO;
using Common.Exceptions;
using CoomonThings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Backend.BL.Services {
	public class RestarauntService : IRestarauntService {
		private readonly ILogger<RestarauntService> _logger;
		private readonly BackendDbContext _context;
		private readonly IMapper _mapper;
		public RestarauntService(ILogger<RestarauntService> logger, BackendDbContext context, IMapper mapper) {
			_logger = logger;
			_context = context;
			_mapper = mapper;
		}
		public async Task<RestarauntPagedList> GetAllRestaraunts(string? NameFilter, int Page = 1) {
			if (Page <= 0) throw new BadRequestException("Неверно указана страница");
			var restaraunts = await _context.Restaraunts
				   .Where(x => 
				   (string.IsNullOrEmpty(NameFilter) || x.Name.Contains(NameFilter)))
				   .ToListAsync();

			var totalItems =  restaraunts.Count();
			var totalPages = (int)Math.Ceiling((double)totalItems / AppConstants.PageSize);
			if (totalPages < Page && totalItems != 0) throw new BadRequestException("Неверно указана текущая страница или список ресторанов пуст");
			restaraunts = restaraunts
				   .Skip((Page - 1) * AppConstants.PageSize)
				   .Take(AppConstants.PageSize)
				   .ToList();

			var response = new RestarauntPagedList {
				Restaraunts = restaraunts
				.Select(x=> _mapper.Map<RestarauntDTO>(x)).ToList(),
				PageInfo = new PageInfoDTO {
					Count = totalPages,
					Current = Page,
					Size = AppConstants.PageSize
				}
			};
			return response;
		}

	}
}
