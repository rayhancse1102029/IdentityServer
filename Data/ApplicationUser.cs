using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace IdentiyServer4.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string SubjectId { get; set; }
        public string FullName { get; set; }
        public string EmployeeCode { get; set; }
        public string RoleName { get; set; }
        public bool IsVerified { get; set; }
        public bool IsActive { get; set; }

        [DefaultValue(0)]
        public bool? IsDelete { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        [MaxLength(250)]
        public string CreatedBy { get; set; }
        [MaxLength(250)]
        public string UpdatedBy { get; set; }
        public string ImgUrl { get; set; }

    }
}
