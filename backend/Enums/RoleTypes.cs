using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Common.Enums {
    public enum RoleType {
        [Display(Name = ApplicationRoleNames.Customer)]
        Customer,
        [Display(Name = ApplicationRoleNames.Manager)]
        Manager,
        [Display(Name = ApplicationRoleNames.Cook)]
        Cook,
        [Display(Name = ApplicationRoleNames.Courier)]
        Courier,
        [Display(Name = ApplicationRoleNames.Administrator)]
        Administrator
    }

    public class ApplicationRoleNames {
        public const string Administrator = "Administrator";
        public const string Manager = "Manager";
        public const string Cook = "Cook";
        public const string Courier = "Courier";
        public const string Customer = "Customer";
    }
}
