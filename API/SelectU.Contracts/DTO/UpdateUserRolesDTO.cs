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
        public required string UserId { get; set; }
        public List<string>? RemoveRoles { get; set; }
        public List<string>? AddRoles { get; set;}
    }
}
