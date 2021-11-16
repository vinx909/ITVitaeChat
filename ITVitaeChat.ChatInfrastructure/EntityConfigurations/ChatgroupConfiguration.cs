using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITVitaeChat.ChatCore.Entities;
using ITVitaeChat.ChatCore.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ITVitaeChat.ChatInfrastructure.EntityConfigurations
{
    class ChatgroupConfiguration : IEntityTypeConfiguration<Chatgroup>
    {
        private const string chatgroupTableName = "Chatgroups";
        private const int nameMaxLength = 255;
        private const int passwordSaltMaxLength = 255;
        private const int passwordMaxLength = 255;
        private const int maxUsersDefaultValue = 0;
        private const ChatgroupVisibility visibilityDefaultValue = ChatgroupVisibility.Public;

        public void Configure(EntityTypeBuilder<Chatgroup> builder)
        {
            builder.ToTable(chatgroupTableName);
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name).HasMaxLength(nameMaxLength).IsRequired();
            builder.Property(c => c.MaxUsers).IsRequired(false).HasDefaultValue(maxUsersDefaultValue);
            builder.Property(c => c.Visibility).IsRequired().HasDefaultValue(visibilityDefaultValue);
            builder.Property(u => u.PasswordSalt).HasMaxLength(passwordSaltMaxLength).IsRequired(false);
            builder.Property(c => c.Password).HasMaxLength(passwordMaxLength).IsRequired(false);
            builder.HasOne(c => c.Moderator).WithMany().HasForeignKey(c => c.ModeratorId).IsRequired();
            builder.Property(c => c.OneToOne).IsRequired();
        }
    }
}
