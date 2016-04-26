namespace WebApiRestService.Api.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using WebApiRestService.Api.Database;

    internal sealed class Configuration : DbMigrationsConfiguration<DatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DatabaseContext context)
        {
            context.Ratings.AddOrUpdate(rating => rating.Stars,
                new Rating() { Stars = 1, Description = "Ruim" },
                new Rating() { Stars = 2, Description = "Razoável" },
                new Rating() { Stars = 3, Description = "Bom" },
                new Rating() { Stars = 4, Description = "Muito Bom" },
                new Rating() { Stars = 5, Description = "Excelente" }
            );

            context.Countries.AddOrUpdate(country => country.Name,
                new Country() { Name = "Brasil" },
                new Country() { Name = "Estados Unidos" },
                new Country() { Name = "França" },
                new Country() { Name = "Inglaterra" }
            );

            context.Restaurants.AddOrUpdate(rest => rest.Name, GetRestaurants());

            context.SaveChanges();
        }

        private Restaurant[] GetRestaurants()
        {
            Restaurant[] rests = new Restaurant[100];

            for (int i = 1; i <= 100; i++)
            {
                int countryId = 1;

                if ((i & 2) == 0) countryId = 2;
                if ((i & 3) == 0) countryId = 3;
                if ((i & 4) == 0) countryId = 4;

                rests[i - 1] = new Restaurant()
                {
                    Name = "Restaurant " + i,
                    Address = "Rua " + i + ", Bairro, Estado",
                    CountryId = countryId
                };
            }

            return rests;
        }
    }
}
