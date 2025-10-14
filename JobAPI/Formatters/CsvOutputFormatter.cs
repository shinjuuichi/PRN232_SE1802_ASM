using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using SharedLibrary.DTOs;
using System.Text;

namespace JobAPI.Formatters
{
    public class CsvOutputFormatter : TextOutputFormatter
    {
        public CsvOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/csv"));
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        protected override bool CanWriteType(Type? type)
        {
            if (type == null)
                return false;

            return typeof(IEnumerable<JobReadDto>).IsAssignableFrom(type) ||
                   typeof(JobReadDto).IsAssignableFrom(type);
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var response = context.HttpContext.Response;
            var buffer = new StringBuilder();

            if (context.Object is IEnumerable<JobReadDto> students)
            {
                buffer.AppendLine("Id,Title,Company,Location,Salary,Experience,Description,CreatedAt");

                foreach (var student in students)
                {
                    buffer.AppendLine($"{student.Id}," +
                        $"\"{EscapeCsvField(student.Title)}\"," +
                        $"\"{EscapeCsvField(student.Company)}\"," +
                        $"\"{EscapeCsvField(student.Location)}\"," +
                        $"\"{EscapeCsvField(student.Salary.ToString())}\"," +
                        $"\"{EscapeCsvField(student.Experience.ToString())}\"," +
                        $"\"{EscapeCsvField(student.Description)}\"," +
                         $"\"{EscapeCsvField(student.CreatedAt.ToString())}\"");
                }
            }
            else if (context.Object is JobReadDto student)
            {
                buffer.AppendLine("Id,Title,Company,Location,Salary,Experience,Description");

                buffer.AppendLine($"{student.Id}," +
                     $"\"{EscapeCsvField(student.Title)}\"," +
                     $"\"{EscapeCsvField(student.Company)}\"," +
                     $"\"{EscapeCsvField(student.Location)}\"," +
                     $"\"{EscapeCsvField(student.Salary.ToString())}\"," +
                     $"\"{EscapeCsvField(student.Experience.ToString())}\"," +
                     $"\"{EscapeCsvField(student.Description)}\"," +
                     $"\"{EscapeCsvField(student.CreatedAt.ToString())}\"");
            }

            await response.WriteAsync(buffer.ToString(), selectedEncoding);
        }

        private static string EscapeCsvField(string? field)
        {
            if (string.IsNullOrEmpty(field))
                return string.Empty;

            return field.Replace("\"", "\"\"");
        }
    }
}