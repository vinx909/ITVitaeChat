using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITVitaeChat.ChatInfrastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace ITVitaeChat.ChatInfrastructure
{
    public class ITVitaeChatDbContext : DbContext
    {
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ITVitaeChat;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder
                .ApplyConfiguration(new AdministratorConfiguration())
                .ApplyConfiguration(new ChatDisallowedWordsConfiguration())
                .ApplyConfiguration(new ChatgroupConfiguration())
                .ApplyConfiguration(new ChatgroupUserConfiguration())
                .ApplyConfiguration(new ChatMessageConfiguration())
                .ApplyConfiguration(new UserConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }
}
