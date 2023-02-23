using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetShelter.DataAccessLayer.Models;
namespace PetShelter.DataAccessLayer.Configuration
{
    internal class FundraiserConfiguration : IEntityTypeConfiguration<Fundraiser>
    {
        public void Configure(EntityTypeBuilder<Fundraiser> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name).IsRequired().HasMaxLength(255);
            builder.Property(p => p.Description).HasMaxLength(1000).IsRequired();

            builder.Property(p => p.Target).IsRequired(true).HasDefaultValue(0);
            builder.Property(p => p.Total).IsRequired(true).HasDefaultValue(0);
        }
    }
}
