using System;

namespace EmployeeRegistry.Domain.Entities
{
    public class Child
    {
        public Guid Id { get; set; }

        public Guid EmployeeId { get; set; }

        public string Name { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public Employee? Employee { get; set; }
    }
}
