using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Common.BackendInterfaces {
	public interface IRestarauntService {
		public Task<RestarauntPagedList> GetAllRestaraunts(string? NameFilter, int Page = 1);

		}
	}
