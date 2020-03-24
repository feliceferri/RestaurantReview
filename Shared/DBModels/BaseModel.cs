using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.DBModels
{
    public class BaseModel
    {
        public Guid Id { get; set; }

        public DateTime Created { get; set; }
        [MaxLength(450)]
        public string CreatedById { get; set; }
        public DateTime? LastUpdate { get; set; }
        [MaxLength(450)]
        public string LastUpdateById { get; set; }
        public DateTime? Deleted { get; set; }
        [MaxLength(450)]
        public string DeletedById { get; set; }

        public virtual ApplicationUser CreatedBy { get; set; }
    }

}
