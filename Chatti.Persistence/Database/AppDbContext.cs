using Chatti.Entities;
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
        public DbSet<Client> Clients { get; init; }
        public DbSet<ChatRoom> ChatRooms { get; init; }
        public DbSet<Message> Messages { get; init; }
        public DbSet<Attachment> Attachments { get; init; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>();
            modelBuilder.Entity<Client>();
            modelBuilder.Entity<ChatRoom>();
            modelBuilder.Entity<Message>();
            modelBuilder.Entity<Attachment>();
        }
    }

}
