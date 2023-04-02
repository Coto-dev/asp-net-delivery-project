using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO {
    /// <summary>
    /// information about page
    /// </summary>
    public class PageInfoDTO {
        /// <summary>
        /// current number of page
        /// </summary>
        public int Current;
        /// <summary>
        /// number of max size elements on page
        /// </summary>
        public int Size = 6;
        /// <summary>
        /// total number of pages
        /// </summary>
        public int Count;
    }
}
