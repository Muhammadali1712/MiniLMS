using MediatR;
using MiniLMS.Domain.Entities;

namespace MiniLMS.Application.Notifacation
{
    public class StudentAddNotifacation : INotification
    {
        public Student Student { get; set; }
    }
}
