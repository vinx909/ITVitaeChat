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
    class ChatgroupUserConfiguration : IEntityTypeConfiguration<ChatGroupUser>
    {
        private const string chatgroupUserTableName = "ChatgroupUsers";

        public void Configure(EntityTypeBuilder<ChatGroupUser> builder)
        {
            builder.ToTable(chatgroupUserTableName);
            builder.HasOne(c => c.User).WithMany().HasForeignKey(c => c.UserId);
            builder.HasOne(c => c.Chatgroup).WithMany().HasForeignKey(c => c.ChatgroupId);
            builder.HasKey(c => new { c.UserId, c.ChatgroupId });
        }
    }
}
