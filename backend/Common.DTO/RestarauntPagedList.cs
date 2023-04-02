using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO {
    public class RestarauntPagedList {
        public List<RestarauntDTO> Restaraunts { get; set; }
        public PageInfoDTO PageInfo { get; set; }
    }
}
