﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApiRestService.Dto
{
    public class Restaurant
    {
		public int Id { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
        public int? CountryId { get; set; }
        public Country Country { get; set; }
    }
}
