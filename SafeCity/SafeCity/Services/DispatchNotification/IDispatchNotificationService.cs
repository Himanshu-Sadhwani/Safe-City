using SafeCity.DTOs;

namespace SafeCity.Services.DispatchNotification
{
    public interface IDispatchNotificationService
    {
        Task NotifyUnitAssignedAsync(string unitName, DispatchResponseDto response);
    }
}