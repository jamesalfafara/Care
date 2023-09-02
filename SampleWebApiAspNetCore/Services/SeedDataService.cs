using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Repositories;

namespace SampleWebApiAspNetCore.Services
{
    public class SeedDataService : ISeedDataService
    {
        public void Initialize(CareDbContext context)
        {
            context.CareItems.Add(new CareEntity() { Effectiveness = 5, Product = "Cleanser", Name = "Master", Created = DateTime.Now });
            context.CareItems.Add(new CareEntity() { Effectiveness = 5, Product = "Acne Toner", Name = "Luxxe Organix", Created = DateTime.Now });
            context.CareItems.Add(new CareEntity() { Effectiveness = 5, Product = "Sunscreen", Name = "Belo", Created = DateTime.Now });
            context.CareItems.Add(new CareEntity() { Effectiveness = 5, Product = "Benzoyl Peroxide", Name = "Derma", Created = DateTime.Now });

            context.SaveChanges();
        }
    }
}
