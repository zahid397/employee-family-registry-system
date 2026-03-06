using EmployeeRegistry.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeRegistry.Infrastructure.Data.Configurations
{
public class ChildConfiguration : IEntityTypeConfiguration<Child>
{
public void Configure(EntityTypeBuilder<Child> builder)
{
builder.HasKey(c => c.Id);
builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
builder.Property(c => c.DateOfBirth).IsRequired();

builder.HasOne(c => c.Employee)  
            .WithMany(e => e.Children)  
            .HasForeignKey(c => c.EmployeeId)  
            .OnDelete(DeleteBehavior.Cascade);  
    }  
}

}
