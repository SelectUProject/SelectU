using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelectU.Contracts.Entities
{
    public class ListObject<T>
    {
        [Key]
        public Guid Id { get; set; }
        public T Value { get; set; }
    }
}
