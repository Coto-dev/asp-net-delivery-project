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
			if (Page <= 0 || Page == null) throw new BadRequestException("Неверно указана страница");

			var totalItems = await _context.Restaraunts.CountAsync();
			var totalPages = (int)Math.Ceiling((double)totalItems / AppConstants.PageSize);

			if (totalPages < Page) throw new BadRequestException("Неверно указана текущая страница или список ресторанов пуст");

			var restaraunts = new List<Restaraunt>();
			if (string.IsNullOrEmpty(NameFilter)) {
				restaraunts = await _context.Restaraunts
				   .Skip((Page - 1) * AppConstants.PageSize)
				   .Take(AppConstants.PageSize)
				   .ToListAsync();
			}
			else {
				restaraunts = await _context.Restaraunts
				   .Where(x=>x.Name.Contains(NameFilter))
				   .Skip((Page - 1) * AppConstants.PageSize)
				   .Take(AppConstants.PageSize)
				   .ToListAsync();
			}
			

			
			var response = new RestarauntPagedList {
				Restaraunts = restaraunts.Select(x=> _mapper.Map<RestarauntDTO>(x)).ToList(),
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
