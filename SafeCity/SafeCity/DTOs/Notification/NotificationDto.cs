using System;

namespace SafeCity.DTOs.Notification;

public class NotificationDto
{
    public string Event { get; set; }        // e.g. "DispatchAssigned", "PatrolUpdated"
    public string TargetGroup { get; set; }  // e.g. "unit_Fire Unit AN-03", "patrol_P01"
    public object Payload { get; set; }      // any DTO — DispatchResponseDto, PatrolDto, etc.
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
}
