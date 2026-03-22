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

    [HttpPost]
    public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationDto dto)
    {
        var id = await _notificationService.CreateNotificationAsync(dto);
        // We return OK since the prompt only requested POST for controllers, but we still support GetById internally.
        return Ok(new { Id = id });
    }
}
