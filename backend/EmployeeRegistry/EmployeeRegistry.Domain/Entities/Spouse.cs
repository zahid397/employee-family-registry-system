using System;

namespace EmployeeRegistry.Domain.Entities
{
    public class Spouse
    {
        public Guid Id { get; set; }

        public Guid EmployeeId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string NID { get; set; } = string.Empty;

        public Employee? Employee { get; set; }
    }
}
