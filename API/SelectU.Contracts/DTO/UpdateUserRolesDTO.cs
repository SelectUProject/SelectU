using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelectU.Contracts.DTO
{
    public class UpdateUserRolesDTO
    {
        public required string userId { get; set; }
        public ICollection<string>? RemoveRoles { get; set; }
        public ICollection<string>? AddRoles { get; set;}
    }
}
