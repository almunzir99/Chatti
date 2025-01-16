using Chatti.Entities.ChatRooms;
using Chatti.Entities.Messages;
using Chatti.Entities.Tenants;
using Chatti.Entities.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatti.Persistence.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<User> Users { get; init; }
        public DbSet<Tenant> Tenants { get; init; }
        public DbSet<ChatRoom> ChatRooms { get; init; }
        public DbSet<Message> Messages { get; init; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>();
            modelBuilder.Entity<Tenant>();
            modelBuilder.Entity<ChatRoom>();
            modelBuilder.Entity<Message>();
        }
    }

}
