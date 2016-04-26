using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApiRestService.Api.Database
{
    public partial class Rating
    {
        public int Id { get; set; }

        [Required]
        public int Stars { get; set; }

        [Required]
        [MaxLength(30)]
        public string Description { get; set; }
    }
}