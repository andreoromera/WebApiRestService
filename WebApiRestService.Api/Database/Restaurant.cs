using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApiRestService.Api.Database
{
    public partial class Restaurant
    {
        public Restaurant()
        {
        }
    
        public int Id { get; set; }

        [Required]
        [MaxLength(60)]
        public string Name { get; set; }
        
        [Required]
        [MaxLength(500)]
        public string Address { get; set; }
        
        public Nullable<int> CountryId { get; set; }
    
        public virtual Country Country { get; set; }
        
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
