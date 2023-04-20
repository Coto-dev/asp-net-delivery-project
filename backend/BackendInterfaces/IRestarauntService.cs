using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DTO;

namespace Common.BackendInterfaces {
	public interface IRestarauntService {
		Task<RestarauntPagedList> GetAllRestaraunts(int Page, string NameFilter);
	}
}
