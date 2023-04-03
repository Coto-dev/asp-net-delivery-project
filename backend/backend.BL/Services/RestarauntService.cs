using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.AuthInterfaces;
using Common.DTO;

namespace Backend.BL.Services {
    public class RestarauntService : IRestarauntService {
        public Task<RestarauntPagedList> GetAllRestaraunts(int Page, string NameFilter) {
            throw new NotImplementedException();
        }
    }
}
