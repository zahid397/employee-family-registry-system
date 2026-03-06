using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeRegistry.Domain.Entities;

namespace EmployeeRegistry.Application.Interfaces
{
    public interface IPdfService
    {
        // Generate PDF for a single employee profile
        Task<byte[]> GenerateEmployeePdfAsync(Guid employeeId);

        // Generate PDF table for employee list
        Task<byte[]> GenerateEmployeeListPdfAsync(IEnumerable<Employee> employees);
    }
}
