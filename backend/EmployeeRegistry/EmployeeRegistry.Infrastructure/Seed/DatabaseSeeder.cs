using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeRegistry.Domain.Entities;
using EmployeeRegistry.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeeRegistry.Infrastructure.Seed
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (await context.Employees.AnyAsync())
                return;

            var employees = new List<Employee>
            {
                new Employee
                {
                    Id = Guid.NewGuid(),
                    Name = "Rahim Uddin",
                    NID = "1234567890",
                    Phone = "+8801712345678",
                    Department = "IT",
                    BasicSalary = 50000,

                    Spouse = new Spouse
                    {
                        Id = Guid.NewGuid(),
                        Name = "Salma Begum",
                        NID = "9876543210"
                    },

                    Children = new List<Child>
                    {
                        new Child
                        {
                            Id = Guid.NewGuid(),
                            Name = "Rafi",
                            DateOfBirth = new DateTime(2015, 5, 10)
                        },
                        new Child
                        {
                            Id = Guid.NewGuid(),
                            Name = "Rima",
                            DateOfBirth = new DateTime(2018, 8, 22)
                        }
                    }
                },

                new Employee
                {
                    Id = Guid.NewGuid(),
                    Name = "Karima Khan",
                    NID = "98765432109876543",
                    Phone = "+8801712345679",
                    Department = "HR",
                    BasicSalary = 45000
                }
            };

            await context.Employees.AddRangeAsync(employees);
            await context.SaveChangesAsync();
        }
    }
}
