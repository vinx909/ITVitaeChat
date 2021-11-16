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
    class UserConfiguration : IEntityTypeConfiguration<User>
    {
        private const string usersTableName = "Users";
        private const int nameMaxLength = 255;
        private const int displayNameMaxLength = 255;
        private const int emailadresMaxLength = 255;
        private const int passwordSaltMaxLength = 255;
        private const int passwordMaxLength = 255;

        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(usersTableName);
            builder.HasKey(u => u.Id);
            builder.HasIndex(u => u.IdentityId).IsUnique().IsUnique(false);
            builder.Property(u => u.Name).HasMaxLength(nameMaxLength).IsRequired();
            builder.Property(u => u.DisplayName).HasMaxLength(displayNameMaxLength).IsRequired();
            builder.HasIndex(u => u.DisplayName).IsUnique();
            builder.Property(u => u.Emailadres).HasMaxLength(emailadresMaxLength).IsRequired();
            builder.HasIndex(u => u.Emailadres).IsUnique();
            builder.Property(u => u.PasswordSalt).HasMaxLength(passwordSaltMaxLength).IsRequired();
            builder.Property(u => u.Password).HasMaxLength(passwordMaxLength).IsRequired();
            builder.Property(u => u.Validated).IsRequired();
        }
    }
}
