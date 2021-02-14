using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace player_log_api.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Npc> Npcs { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Quest> Quests { get; set; }
        public DbSet<Army> Armies { get; set; }
        public DbSet<Character> Characters { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
