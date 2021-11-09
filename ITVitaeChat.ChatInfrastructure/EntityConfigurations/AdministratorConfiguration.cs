using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITVitaeChat.ChatCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ITVitaeChat.ChatInfrastructure.EntityConfigurations
{
    class AdministratorConfiguration : IEntityTypeConfiguration<Administrator>
    {
        private const string administratorsTableName = "Administrators";

        public void Configure(EntityTypeBuilder<Administrator> builder)
        {
            builder.ToTable(administratorsTableName);
            builder.HasOne(a => a.User).WithMany().HasForeignKey(a => a.UserId).IsRequired();
            builder.HasKey(a => a.UserId);
        }
    }
}
