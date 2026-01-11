using ServiceAuto.Services;

namespace ServiceAuto.Views;

public partial class DashboardPage : ContentPage
{
    private readonly DatabaseService _dbService;

    public DashboardPage()
    {
        InitializeComponent();
        _dbService = new DatabaseService();

        LoadDashboardData();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadDashboardData();
    }

    private async void LoadDashboardData()
    {
        try
        {
            WelcomeLabel.Text = "Bun venit, Mecanic!";
            DateLabel.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy");

            var pieseCount = await _dbService.GetPieseCountAsync();
            var clientiCount = await _dbService.GetClientiCountAsync();

            PieseCountLabel.Text = pieseCount.ToString();
            ClientiCountLabel.Text = clientiCount.ToString();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Eroare: {ex.Message}");
        }
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Deconectare",
            "Ești sigur că vrei să te deconectezi?",
            "Da", "Nu");

        if (confirm)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }

    private async void OnPieseClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PiesePage());
    }

    private async void OnInterventiiClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new InterventiiPage());
    }

    private async void OnClientiClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Info", "Pagina pentru Clienți va fi implementată în curând!", "OK");
    }

    private async void OnAdaugaInterventieClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AdaugaInterventiePage));
    }

    private async void OnClientClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ClientiPage());
    }
}