using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CARAPI.Data
{
    public class ApplicationUser 
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

        // Identity User
        public string Id { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneNumberConfirmed { get; set; }

    }
}
