using ServiceAuto.Services;

namespace ServiceAuto.Views;

public partial class InterventiiPage : ContentPage
{
    private readonly DatabaseService _dbService;

    public InterventiiPage()
    {
        InitializeComponent();
        _dbService = new DatabaseService();
    }

    private async void OnAdaugaInterventieClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AdaugaInterventiePage());
    }

    private async void OnInterventiiAziClicked(object sender, EventArgs e)
    {
        try
        {
            var interventiiAzi = await _dbService.GetInterventiiAziAsync();
            StatusLabel.Text = $"Azi sunt {interventiiAzi.Count} intervenții programate.";
        }
        catch (Exception ex)
        {
            await DisplayAlert("Eroare", $"Eroare: {ex.Message}", "OK");
        }
    }
}