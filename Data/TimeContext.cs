using Microsoft.EntityFrameworkCore;
using TimeApi.Models;

namespace TimeApi.Data
{
    public class TimeContext : DbContext
    {
        public TimeContext(DbContextOptions<TimeContext> options) : base(options) { }

        public DbSet<TimeEntry> TimeEntries { get; set; }
    }
}
