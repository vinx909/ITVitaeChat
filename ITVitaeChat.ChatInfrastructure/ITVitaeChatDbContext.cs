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
    }
}
