using HitachiBE.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace HitachiBE.DB
{
    public class ConectionDbContext : DbContext
    {
        public ConectionDbContext(DbContextOptions options) : base(options) { }

        public DbSet<DayDataModel> DaysDB { get; set; }
    }
}
