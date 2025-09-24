using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitFlex.Application.DTO_s
{
    public class BaseEntity
    {
        public bool IsDelete { get; set; } = false;
        public DateTime CreatedOn { get; set; } =DateTime.UtcNow;
        public int CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; } = null!;
        public int? ModifiedBy { get; set; } = null!;
        public DateTime? DeletedOn { get; set; } = null!;
        public int? DeletedBy { get; set; } = null!;

    }
}
