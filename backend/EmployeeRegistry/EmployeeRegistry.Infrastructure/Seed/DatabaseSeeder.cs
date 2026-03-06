using System; // Fixed capital 'U'
using System.Collections.Generic;
using System.Linq;
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
            if (await context.Employees.AnyAsync()) return;

            var employees = new List<Employee>
            {
                new Employee
                {
                    Id = Guid.NewGuid(),
                    Name = "Md. Rahim Uddin",
                    NID = "1234567890",
                    Phone = "01712345678",
                    Department = "IT",
                    BasicSalary = 55000m,
                    Spouse = new Spouse
                    {
                        Id = Guid.NewGuid(),
                        Name = "Salma Begum",
                        NID = "9876543210"
                    },
                    Children = new List<Child>
                    {
                        new Child { Id = Guid.NewGuid(), Name = "Rafi", DateOfBirth = DateTime.SpecifyKind(new DateTime(2015,5,10), DateTimeKind.Utc) },
                        new Child { Id = Guid.NewGuid(), Name = "Rima", DateOfBirth = DateTime.SpecifyKind(new DateTime(2018,8,22), DateTimeKind.Utc) }
                    }
                },
                new Employee
                {
                    Id = Guid.NewGuid(),
                    Name = "Karima Khan",
                    NID = "98765432109876543",
                    Phone = "+8801712345679",
                    Department = "HR",
                    BasicSalary = 48000m,
                    Spouse = null,
                    Children = new List<Child>()
                },
                new Employee
                {
                    Id = Guid.NewGuid(),
                    Name = "Tanvir Hasan",
                    NID = "1234567891",
                    Phone = "01812345678",
                    Department = "Finance",
                    BasicSalary = 60000m,
                    Spouse = new Spouse
                    {
                        Id = Guid.NewGuid(),
                        Name = "Moushumi Akter",
                        NID = "1122334455"
                    },
                    Children = new List<Child>
                    {
                        new Child { Id = Guid.NewGuid(), Name = "Tanisha", DateOfBirth = DateTime.SpecifyKind(new DateTime(2019,2,14), DateTimeKind.Utc) }
                    }
                },
                new Employee
                {
                    Id = Guid.NewGuid(),
                    Name = "Shahidul Alam",
                    NID = "2233445566",
                    Phone = "01912345678",
                    Department = "Marketing",
                    BasicSalary = 52000m
                },
                new Employee
                {
                    Id = Guid.NewGuid(),
                    Name = "Nusrat Jahan",
                    NID = "33445566778812345",
                    Phone = "01512345678",
                    Department = "Admin",
                    BasicSalary = 45000m,
                    Spouse = new Spouse
                    {
                        Id = Guid.NewGuid(),
                        Name = "Kamrul Islam",
                        NID = "9988776655"
                    }
                },
                new Employee
                {
                    Id = Guid.NewGuid(),
                    Name = "Delwar Hossain",
                    NID = "4455667788",
                    Phone = "01612345678",
                    Department = "IT",
                    BasicSalary = 62000m,
                    Children = new List<Child>
                    {
                        new Child { Id = Guid.NewGuid(), Name = "Dihan", DateOfBirth = DateTime.SpecifyKind(new DateTime(2016,11,3), DateTimeKind.Utc) },
                        new Child { Id = Guid.NewGuid(), Name = "Disha", DateOfBirth = DateTime.SpecifyKind(new DateTime(2019,7,19), DateTimeKind.Utc) }
                    }
                },
                new Employee
                {
                    Id = Guid.NewGuid(),
                    Name = "Farida Yasmin",
                    NID = "5566778899",
                    Phone = "+8801712345680",
                    Department = "HR",
                    BasicSalary = 47000m
                },
                new Employee
                {
                    Id = Guid.NewGuid(),
                    Name = "Jahangir Kabir",
                    NID = "6677889900",
                    Phone = "01712345681",
                    Department = "Finance",
                    BasicSalary = 59000m,
                    Spouse = new Spouse
                    {
                        Id = Guid.NewGuid(),
                        Name = "Shamima Nasrin",
                        NID = "1231231230"
                    }
                },
                new Employee
                {
                    Id = Guid.NewGuid(),
                    Name = "Maksuda Begum",
                    NID = "7788990011",
                    Phone = "01812345682",
                    Department = "Marketing",
                    BasicSalary = 51000m,
                    Children = new List<Child>
                    {
                        new Child { Id = Guid.NewGuid(), Name = "Mahir", DateOfBirth = DateTime.SpecifyKind(new DateTime(2017,4,25), DateTimeKind.Utc) }
                    }
                },
                new Employee
                {
                    Id = Guid.NewGuid(),
                    Name = "Anwar Hossain",
                    NID = "88990011223344556",
                    Phone = "01912345683",
                    Department = "Admin",
                    BasicSalary = 53000m
                }
            };

            await context.Employees.AddRangeAsync(employees);
            await context.SaveChangesAsync();
        }
    }
}
