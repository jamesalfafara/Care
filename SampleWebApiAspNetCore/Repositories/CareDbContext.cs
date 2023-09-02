using Microsoft.EntityFrameworkCore;
using SampleWebApiAspNetCore.Entities;

namespace SampleWebApiAspNetCore.Repositories
{
    public class CareDbContext : DbContext
    {
        public CareDbContext(DbContextOptions<CareDbContext> options)
            : base(options)
        {
        }

        public DbSet<CareEntity> CareItems { get; set; } = null!;
    }
}
