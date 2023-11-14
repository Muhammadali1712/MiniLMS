using MediatR;
using Microsoft.Extensions.Logging;
using MiniLMS.Application.Notifacation;

namespace MiniLMS.Application.NotifacationHandlars;

public class EmailHandler : INotificationHandler<StudentAddNotifacation>
{
    private readonly ILogger _logger;

    public EmailHandler(ILogger logger)
    {
        _logger = logger;
    }

    public Task Handle(StudentAddNotifacation notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($@"/n{notification.Student.FullName} send email/n/n/n/n/n");
        return Task.CompletedTask;
    }
}
