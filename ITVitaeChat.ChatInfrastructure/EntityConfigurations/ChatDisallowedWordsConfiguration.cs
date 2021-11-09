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
    class ChatDisallowedWordsConfiguration : IEntityTypeConfiguration<ChatDisallowedWord>
    {
        private const string chatDisallowedWordTableName = "ChatDisallowedWords";
        private const int contentMaxLength = 255;

        public void Configure(EntityTypeBuilder<ChatDisallowedWord> builder)
        {
            builder.ToTable(chatDisallowedWordTableName);
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Content).HasMaxLength(contentMaxLength).IsRequired();
        }
    }
}
