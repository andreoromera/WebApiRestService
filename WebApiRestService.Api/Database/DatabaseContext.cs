using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace WebApiRestService.Api.Database
{
    public partial class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("name=DefaultConnection")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
    
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Restaurant> Restaurants { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }
    }
}
