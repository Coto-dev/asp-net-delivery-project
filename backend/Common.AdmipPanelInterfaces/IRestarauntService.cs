﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DTO;

namespace Common.AdminPanelInterfaces {
    public interface IRestarauntService {
        public List<RestarauntDTO> GetRestarauntList();
        public Task CreateRestaraunt(RestarauntViewModel model);
        public Task<RestarauntViewModel> GetDetails(Guid id);
        public Task AddCook(string email , Guid restarauntId);
		public Task AddManager(string email, Guid restarauntId);
        public Task DeleteCook(AddUserViewModel model);
		public Task DeleteManager(AddUserViewModel model);
		public Task Delete(Guid id);
		public Task<ViewRestaraunt> GetRestaraunt(Guid id);
		public Task RecoverRest(Guid id);
		public Task Edit(EditRestarauntVIew model);
		public Task<EditRestarauntVIew> GetForEdit(Guid id);


	}
}
