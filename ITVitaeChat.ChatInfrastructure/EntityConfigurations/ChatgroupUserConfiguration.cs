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
    class ChatgroupUserConfiguration : IEntityTypeConfiguration<ChatgroupUser>
    {
        private const string chatgroupUserTableName = "ChatgroupUsers";

        public void Configure(EntityTypeBuilder<ChatgroupUser> builder)
        {
            builder.ToTable(chatgroupUserTableName);
            builder.HasOne(c => c.User).WithMany().HasForeignKey(c => c.UserId);
            builder.HasOne(c => c.Chatgroup).WithMany().HasForeignKey(c => c.ChatgroupId);
            builder.HasKey(c => new { c.UserId, c.ChatgroupId });
            builder.Property(c => c.OneToOne).IsRequired();
        }
    }
}
