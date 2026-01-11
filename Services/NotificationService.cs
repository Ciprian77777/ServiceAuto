using Plugin.LocalNotification;

namespace ServiceAuto.Services;

public class NotificationService
{

    public async Task Notifica(string titlu, string mesaj)
    {
        try
        {
            var notificare = new NotificationRequest
            {
                NotificationId = new Random().Next(1, 1000),
                Title = titlu,
                Description = mesaj,
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = DateTime.Now.AddSeconds(1)
                }
            };

            await LocalNotificationCenter.Current.Show(notificare);
        }
        catch
        {
            // Ignoră erorile 
        }
    }

    public async Task NotificaMaine(string ceva)
    {
        var notificare = new NotificationRequest
        {
            NotificationId = 101,
            Title = "Mâine la service",
            Description = $"Nu uita: {ceva}",
            Schedule = new NotificationRequestSchedule
            {
                NotifyTime = DateTime.Today.AddDays(1).AddHours(8)
            }
        };

        await LocalNotificationCenter.Current.Show(notificare);
    }
}