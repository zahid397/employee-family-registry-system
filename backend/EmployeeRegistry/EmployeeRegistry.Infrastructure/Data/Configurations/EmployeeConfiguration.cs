using EmployeeRegistry.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeRegistry.Infrastructure.Data.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(e => e.NID)
                .IsUnique();

            builder.Property(e => e.NID)
                .IsRequired()
                .HasMaxLength(17);

            builder.Property(e => e.Phone)
                .IsRequired()
                .HasMaxLength(15);

            builder.Property(e => e.Department)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.BasicSalary)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.HasOne(e => e.Spouse)
                .WithOne(s => s.Employee)
                .HasForeignKey<Spouse>(s => s.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
