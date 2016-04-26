using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApiRestService.Api.Database
{
    public partial class Country
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
