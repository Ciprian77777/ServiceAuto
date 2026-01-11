using ServiceAuto.Services;

namespace ServiceAuto;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new AppShell();

        Device.StartTimer(TimeSpan.FromMilliseconds(100), () =>
        {
            Shell.Current.GoToAsync("//LoginPage");
            return false;
        });

        StartSimpleNotifications();
    }

    private async void StartSimpleNotifications()
    {
        await Task.Delay(5000);

        try
        {
            var request = new Plugin.LocalNotification.NotificationRequest
            {
                NotificationId = 1000,
                Title = "🚗 ServiceAuto",
                Description = "Bine ai venit!",
                Schedule = new Plugin.LocalNotification.NotificationRequestSchedule
                {
                    NotifyTime = DateTime.Now.AddSeconds(3)
                }
            };

            await Plugin.LocalNotification.LocalNotificationCenter.Current.Show(request);
        }
        catch
        {
            // Ignoră erorile
        }
    }
}