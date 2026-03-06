using System;
using System.Threading.Tasks;
using EmployeeRegistry.Application.Commands;
using EmployeeRegistry.Application.Interfaces;
using EmployeeRegistry.Application.Validators;
using EmployeeRegistry.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeRegistry.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _repository;
        private readonly IPdfService _pdfService;

        public EmployeesController(IEmployeeRepository repository, IPdfService pdfService)
        {
            _repository = repository;
            _pdfService = pdfService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string q)
        {
            var employees = await _repository.SearchAsync(q);
            return Ok(employees);
        }

        [HttpPost]
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("{id}/pdf")]
        public async Task<IActionResult> GetPdf(Guid id)
        {
            var pdfBytes = await _pdfService.GenerateEmployeePdfAsync(id);

            if (pdfBytes == null)
                return NotFound();

            return File(pdfBytes, "application/pdf", $"employee_{id}.pdf");
        }
    }
}
