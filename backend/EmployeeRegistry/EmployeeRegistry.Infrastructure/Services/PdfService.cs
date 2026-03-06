using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EmployeeRegistry.Application.Interfaces;
using EmployeeRegistry.Domain.Entities;
using EmployeeRegistry.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace EmployeeRegistry.Infrastructure.Services
{
    public class PdfService : IPdfService
    {
        private readonly ApplicationDbContext _context;

        public PdfService(ApplicationDbContext context)
        {
            _context = context;
            QuestPDF.Settings.License = LicenseType.Community;
        }

        // Generate PDF for a single employee profile
        public async Task<byte[]> GenerateEmployeePdfAsync(Guid employeeId)
        {
            var employee = await _context.Employees
                .Include(e => e.Spouse)
                .Include(e => e.Children)
                .FirstOrDefaultAsync(e => e.Id == employeeId);

            if (employee == null)
                throw new Exception("Employee not found");

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header()
                        .Text($"Employee Profile: {employee.Name}")
                        .Bold()
                        .FontSize(18);

                    page.Content().Column(col =>
                    {
                        col.Spacing(10);

                        col.Item().Text($"NID: {employee.NID}");
                        col.Item().Text($"Phone: {employee.Phone}");
                        col.Item().Text($"Department: {employee.Department}");
                        col.Item().Text($"Basic Salary: ৳{employee.BasicSalary}");

                        if (employee.Spouse != null)
                        {
                            col.Item().Text("Spouse:").Bold();
                            col.Item().Text($"Name: {employee.Spouse.Name}, NID: {employee.Spouse.NID}");
                        }

                        if (employee.Children.Any())
                        {
                            col.Item().Text("Children:").Bold();

                            foreach (var child in employee.Children)
                            {
                                col.Item().Text($"Name: {child.Name}, DOB: {child.DateOfBirth:yyyy-MM-dd}");
                            }
                        }
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Generated on ");
                        x.Span(DateTime.Now.ToString("yyyy-MM-dd HH:mm")).Italic();
                    });
                });
            });

            using var stream = new MemoryStream();
            document.GeneratePdf(stream);
            return stream.ToArray();
        }

        // Generate PDF table for employee list
        public async Task<byte[]> GenerateEmployeeListPdfAsync(IEnumerable<Employee> employees)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(1, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header()
                        .Text("Employee List")
                        .Bold()
                        .FontSize(16)
                        .AlignCenter();

                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(2); // Name
                            columns.RelativeColumn(2); // NID
                            columns.RelativeColumn(2); // Phone
                            columns.RelativeColumn(2); // Department
                            columns.RelativeColumn(2); // Salary
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("Name").Bold();
                            header.Cell().Text("NID").Bold();
                            header.Cell().Text("Phone").Bold();
                            header.Cell().Text("Department").Bold();
                            header.Cell().Text("Basic Salary").Bold();
                        });

                        foreach (var emp in employees)
                        {
                            table.Cell().Text(emp.Name);
                            table.Cell().Text(emp.NID);
                            table.Cell().Text(emp.Phone);
                            table.Cell().Text(emp.Department);
                            table.Cell().Text($"৳{emp.BasicSalary}");
                        }
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Generated on ");
                        x.Span(DateTime.Now.ToString("yyyy-MM-dd HH:mm")).Italic();
                    });
                });
            });

            using var stream = new MemoryStream();
            document.GeneratePdf(stream);
            return stream.ToArray();
        }
    }
}
