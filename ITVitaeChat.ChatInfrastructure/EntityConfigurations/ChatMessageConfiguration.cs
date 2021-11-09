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
    class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
    {
        private const string chatMessageTableName = "ChatMessages";

        public void Configure(EntityTypeBuilder<ChatMessage> builder)
        {
            builder.ToTable(chatMessageTableName);
            builder.HasKey(c => c.Id);
            builder.HasOne(c => c.User).WithMany().HasForeignKey(c => c.UserId);
            builder.HasOne(c => c.Chatgroup).WithMany().HasForeignKey(c => c.ChatgroupId);
            builder.Property(c => c.SendTime).IsRequired();
            builder.Property(c => c.Content).IsRequired();
        }
    }
}
