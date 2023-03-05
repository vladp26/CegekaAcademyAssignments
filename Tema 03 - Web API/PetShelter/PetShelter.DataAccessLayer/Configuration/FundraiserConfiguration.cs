using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetShelter.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShelter.DataAccessLayer.Configuration
{
    public class FundraiserConfiguration : IEntityTypeConfiguration<Fundraiser>
    {
        public ICollection<Person>? Donors { get; set; }
        public void Configure(EntityTypeBuilder<Fundraiser> builder)
        {
            builder.HasKey(p => p.Id);

            //Columns mapping and constraints
            builder.Property(p => p.Name).IsRequired().HasMaxLength(255);
            
            builder.Property(p => p.GoalValue).IsRequired().HasDefaultValue(0);
            builder.Property(p => p.DueDate).IsRequired().HasDefaultValue(DateTime.Now.AddDays(30));
            builder.Property(p => p.Status).IsRequired().HasDefaultValue("Active");
            builder.Property(p => p.CreationDate).IsRequired().HasDefaultValue(DateTime.Now);
            builder.Property(p => p.DonationAmount).IsRequired().HasDefaultValue(0);

            //Relationships
            builder.HasOne(p => p.Owner).WithMany(p => p.OwnedFundraisers).HasForeignKey(p => p.OwnerId).IsRequired();

        }
    }
}
