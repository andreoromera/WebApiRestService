using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApiRestService.Api.Database
{
    public partial class Review
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string ReviewerName { get; set; }

        public Nullable<int> RatingId { get; set; }

        public virtual Rating Rating { get; set; }
        
        public int RestaurantId { get; set; }
    }
}
