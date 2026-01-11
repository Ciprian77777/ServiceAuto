using ServiceAuto.Views;

namespace ServiceAuto;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        FlyoutBehavior = FlyoutBehavior.Disabled;

        RegisterRoutes();

        Navigating += OnShellNavigating;
    }

    private void RegisterRoutes()
    {
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        Routing.RegisterRoute(nameof(InregistrarePage), typeof(InregistrarePage));
        Routing.RegisterRoute(nameof(ContPage), typeof(ContPage));
        Routing.RegisterRoute(nameof(DashboardPage), typeof(DashboardPage));
        Routing.RegisterRoute(nameof(InterventiiPage), typeof(InterventiiPage));
        Routing.RegisterRoute(nameof(ClientiPage), typeof(ClientiPage));
        Routing.RegisterRoute(nameof(MasiniPage), typeof(MasiniPage));
        Routing.RegisterRoute(nameof(PiesePage), typeof(PiesePage));
        Routing.RegisterRoute(nameof(AdaugaInterventiePage), typeof(AdaugaInterventiePage));
        Routing.RegisterRoute(nameof(AdaugaClientPage), typeof(AdaugaClientPage));
        Routing.RegisterRoute(nameof(DetaliiClientPage), typeof(DetaliiClientPage));
        Routing.RegisterRoute(nameof(InterventiiClientPage), typeof(InterventiiClientPage));
        Routing.RegisterRoute(nameof(MasiniClientPage), typeof(MasiniClientPage));
        Routing.RegisterRoute(nameof(AdaugaMasinaPage), typeof(AdaugaMasinaPage));
        Routing.RegisterRoute(nameof(AdaugaPiesaPage), typeof(AdaugaPiesaPage));
    }

    private void OnShellNavigating(object sender, ShellNavigatingEventArgs e)
    {
        if (e.Current?.Location?.OriginalString?.Contains("LoginPage") == true &&
            e.Target?.Location?.OriginalString?.Contains("DashboardPage") == true)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                FlyoutBehavior = FlyoutBehavior.Flyout;
                Console.WriteLine("✅ Flyout activat după login");
            });
        }

        if ((e.Current?.Location?.OriginalString?.Contains("DashboardPage") == true ||
             e.Current?.Location?.OriginalString?.Contains("ContPage") == true) &&
            e.Target?.Location?.OriginalString?.Contains("LoginPage") == true)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                FlyoutBehavior = FlyoutBehavior.Disabled;
                Console.WriteLine("✅ Flyout dezactivat pentru logout");
            });
        }
    }

    public void EnableFlyout()
    {
        Device.BeginInvokeOnMainThread(() =>
        {
            FlyoutBehavior = FlyoutBehavior.Flyout;
            Console.WriteLine("✅ Flyout activat manual");
        });
    }
}