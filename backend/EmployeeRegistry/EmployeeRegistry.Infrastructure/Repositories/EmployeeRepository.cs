using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeRegistry.Application.Interfaces;
using EmployeeRegistry.Domain.Entities;
using EmployeeRegistry.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeeRegistry.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> SearchAsync(string? query)
        {
            var employees = _context.Employees
                .Include(e => e.Spouse)
                .Include(e => e.Children)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                employees = employees.Where(e =>
                    e.Name.Contains(query) ||
                    e.NID.Contains(query) ||
                    e.Department.Contains(query));
            }

            return await employees.ToListAsync();
        }

        public async Task<Employee?> GetByIdAsync(Guid id)
        {
            return await _context.Employees
                .Include(e => e.Spouse)
                .Include(e => e.Children)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<bool> IsNidExistsAsync(string nid)
        {
            return await _context.Employees.AnyAsync(e => e.NID == nid);
        }

        public async Task AddAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee != null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }
        }
    }
}
