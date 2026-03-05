using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeRegistry.Domain.Entities;

namespace EmployeeRegistry.Application.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> SearchAsync(string? query);

        Task<Employee?> GetByIdAsync(Guid id);

        Task<bool> IsNidExistsAsync(string nid);

        Task AddAsync(Employee employee);

        Task DeleteAsync(Guid id);
    }
}
