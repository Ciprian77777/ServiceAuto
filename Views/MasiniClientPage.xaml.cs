using ServiceAuto.Models;
using ServiceAuto.Services;

namespace ServiceAuto.Views;

public partial class MasiniClientPage : ContentPage
{
    private readonly DatabaseService _dbService;
    private readonly int _clientId;
    private Client _client;

    public MasiniClientPage(int clientId)
    {
        InitializeComponent();
        _dbService = new DatabaseService();
        _clientId = clientId;

        LoadClientAndMasini();
    }

    private async void LoadClientAndMasini()
    {
        try
        {
            _client = await _dbService.GetClientByIdAsync(_clientId);
            Title = $"Mașinile lui {_client.Nume}";

            var masini = await _dbService.GetMasiniByClientAsync(_clientId);
            MasiniCollectionView.ItemsSource = masini;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Eroare", $"Eroare la încărcarea datelor: {ex.Message}", "OK");
        }
    }

    private async void OnAdaugaMasinaClicked(object sender, EventArgs e)
    {
        var masinaNoua = new Masina { ClientId = _clientId };
        await Navigation.PushAsync(new AdaugaMasinaPage(masinaNoua));
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadClientAndMasini();
    }
}