using MediatR;
using MiniLMS.Domain.States;
using System.ComponentModel.DataAnnotations;

namespace MiniLMS.Domain.Entities;
public class BaseEntity:INotification
{
    public int Id { get; set; }
    public string? FullName { get; set; }
    public DateOnly BirthDate { get; set; }
    public Gender Gender { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Login { get; set; }
    public string? Password { get; set; }

  /*  public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        int date = DateTime.Now.Year;
        if (date - BirthDate.Year < 18)
        {
            yield return new ValidationResult("Siz hali yoshsiz!");
        }
    }*/
}
