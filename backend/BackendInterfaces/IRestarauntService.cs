﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DTO;

namespace Common.AuthInterfaces {
    public interface IRestarauntService {
        Task<RestarauntPagedList> GetAllRestaraunts(int Page, string NameFilter);
    }
}
