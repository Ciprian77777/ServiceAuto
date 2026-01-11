using ServiceAuto.Models;
using ServiceAuto.Services;

namespace ServiceAuto.Views;

public partial class DetaliiClientPage : ContentPage
{
    private readonly DatabaseService _dbService;
    private Client _client;

    public Client Client
    {
        get => _client;
        set
        {
            _client = value;
            OnPropertyChanged(nameof(Client));
        }
    }

    public DetaliiClientPage(Client client)
    {
        InitializeComponent();
        _dbService = new DatabaseService();
        Client = client;

        BindingContext = this;
        LoadStatistics();
    }

    private async void LoadStatistics()
    {
        try
        {
            var masini = await _dbService.GetMasiniByClientAsync(Client.Id);
            MasiniCountLabel.Text = masini.Count.ToString();

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Eroare la încărcarea statisticilor: {ex.Message}");
        }
    }

    private async void OnMasiniClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MasiniClientPage(Client.Id));
    }

    private async void OnInterventiiClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new InterventiiClientPage(Client.Id, Client.Nume));
    }


}