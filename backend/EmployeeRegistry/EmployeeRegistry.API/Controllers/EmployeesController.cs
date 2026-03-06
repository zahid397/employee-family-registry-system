using System;
using System.Threading.Tasks;
using EmployeeRegistry.Application.Commands;
using EmployeeRegistry.Application.Interfaces;
using EmployeeRegistry.Application.Validators;
using EmployeeRegistry.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace EmployeeRegistry.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Require role from middleware
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _repository;
        private readonly IPdfService _pdfService;

        public EmployeesController(IEmployeeRepository repository, IPdfService pdfService)
        {
            _repository = repository;
            _pdfService = pdfService;
        }

        // 🔍 Search employees (Viewer + Admin)
        [HttpGet("search")]
        [Authorize(Roles = "Viewer,Admin")]
        public async Task<IActionResult> Search([FromQuery] string q)
        {
            var employees = await _repository.SearchAsync(q);
            return Ok(employees);
        }

        // ➕ Create employee (Admin only)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateEmployeeCommand command)
        {
            var validator = new CreateEmployeeCommandValidator(_repository);
            var validationResult = await validator.ValidateAsync(command);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var employee = new Employee
            {
                Id = Guid.NewGuid(),
                Name = command.Name,
                NID = command.NID,
                Phone = command.Phone,
                Department = command.Department,
                BasicSalary = command.BasicSalary,

                Spouse = command.Spouse != null ? new Spouse
                {
                    Id = Guid.NewGuid(),
                    Name = command.Spouse.Name,
                    NID = command.Spouse.NID
                } : null,

                Children = command.Children?.ConvertAll(c => new Child
                {
                    Id = Guid.NewGuid(),
                    Name = c.Name,
                    DateOfBirth = c.DateOfBirth
                }) ?? new()
            };

            await _repository.AddAsync(employee);

            return Ok(employee.Id);
        }

        // ❌ Delete employee (Admin only)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }

        // 📄 Single employee PDF (Admin only)
        [HttpGet("{id}/pdf")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPdf(Guid id)
        {
            var pdfBytes = await _pdfService.GenerateEmployeePdfAsync(id);

            if (pdfBytes == null)
                return NotFound();

            return File(pdfBytes, "application/pdf", $"employee_{id}.pdf");
        }

        // 📊 Employee List PDF (Admin only)
        [HttpGet("pdf-list")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPdfList([FromQuery] string q = null)
        {
            var employees = await _repository.SearchAsync(q);

            var pdfBytes = await _pdfService.GenerateEmployeeListPdfAsync(employees);

            return File(
                pdfBytes,
                "application/pdf",
                $"employee_list_{DateTime.Now:yyyyMMddHHmm}.pdf"
            );
        }
    }
}
