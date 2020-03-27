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
        [DataType(DataType.Date)]
        public DateTime Created { get; set; }
        [MaxLength(450)]
        [Display(Name = "Created by")]
        public string CreatedById { get; set; }
        [DataType(DataType.Date)]
        public DateTime? LastUpdate { get; set; }
        [MaxLength(450)]
        [Display(Name = "Updated by")]
        public string LastUpdateById { get; set; }
        [DataType(DataType.Date)]
        public DateTime? Deleted { get; set; }
        [MaxLength(450)]
        [Display(Name = "Deleted by")]
        public string DeletedById { get; set; }

        public virtual ApplicationUser CreatedBy { get; set; }
    }

}
