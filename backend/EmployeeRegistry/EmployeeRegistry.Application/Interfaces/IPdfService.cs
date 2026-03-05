using System.Threading.Tasks;
using EmployeeRegistry.Domain.Entities;

namespace EmployeeRegistry.Application.Interfaces
{
    public interface IPdfService
    {
        Task<byte[]> GenerateEmployeePdfAsync(Employee employee);
    }
}
