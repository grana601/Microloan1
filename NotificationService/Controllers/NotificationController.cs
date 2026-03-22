using Microsoft.AspNetCore.Mvc;
using NotificationService.Interfaces;
using NotificationService.Models.DTOs;

namespace NotificationService.Controllers;

[ApiController]
[Route("api/notifications")]
public class NotificationController : ControllerBase
{
    private readonly INotificationAppService _notificationService;

    public NotificationController(INotificationAppService notificationService)
    {
        _notificationService = notificationService;
    }

   
}
