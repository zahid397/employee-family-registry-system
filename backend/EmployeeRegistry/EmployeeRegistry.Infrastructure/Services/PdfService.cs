using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EmployeeRegistry.Application.Interfaces;
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
    }
}
