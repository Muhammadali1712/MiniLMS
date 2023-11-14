using MiniLMS.Domain.Entities;
using MiniLMS.Domain.States;
using System.Text.Json.Serialization;

namespace MiniLMS.Domain.Models.StudentDTO;
public class StudentBaseDTO
{
    public string? FullName { get; set; }
    public DateOnly BirthDate { get; set; }
    public Gender Gender { get; set; }
    public string? PhoneNumber { get; set; }
    public string Major { get; set; }
    public IEnumerable<int> Teachersid { get; set; }

}
