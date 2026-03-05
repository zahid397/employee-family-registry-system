using System;
using System.Collections.Generic;

namespace EmployeeRegistry.Application.DTOs
{
    public class EmployeeDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string NID { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Department { get; set; } = string.Empty;

        public decimal BasicSalary { get; set; }

        public SpouseDto? Spouse { get; set; }

        public List<ChildDto> Children { get; set; } = new();
    }
}
