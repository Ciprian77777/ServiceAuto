using ServiceAuto.Models;
using ServiceAuto.Services;

namespace ServiceAuto.Views;

public partial class ClientiPage : ContentPage
{
    private readonly DatabaseService _dbService;
    private List<Client> _allClienti;

    public ClientiPage()
    {
        InitializeComponent();
        _dbService = new DatabaseService();

        LoadClienti();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadClienti();
    }

    private async void LoadClienti()
    {
        try
        {
            _allClienti = await _dbService.GetClientiAsync();
            ClientiCollectionView.ItemsSource = _allClienti;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Eroare", $"Eroare la încarcarea clientilor: {ex.Message}", "OK");
        }
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            ClientiCollectionView.ItemsSource = _allClienti;
        }
        else
        {
            var searchText = e.NewTextValue.ToLower();
            var filtered = _allClienti.Where(c =>
                c.Nume.ToLower().Contains(searchText) ||
                c.Email.ToLower().Contains(searchText) ||
                c.Telefon.Contains(searchText)
            ).ToList();

            ClientiCollectionView.ItemsSource = filtered;
        }
    }

    private async void OnAdaugaClientClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AdaugaClientPage());
    }

    private async void OnVeziClientClicked(object sender, EventArgs e)
    {
        if (ClientiCollectionView.SelectedItem is Client client)
        {
            await Navigation.PushAsync(new DetaliiClientPage(client));
        }
        else
        {
            await DisplayAlert("Selectare", "Selecteaza un client", "OK");
        }
    }

    private async void OnEditClientClicked(object sender, EventArgs e)
    {
        if (ClientiCollectionView.SelectedItem is Client client)
        {
            await Navigation.PushAsync(new AdaugaClientPage(client));
        }
        else
        {
            await DisplayAlert("Selectare", "Selecteaza un client pentru editare", "OK");
        }
    }

    private async void OnDeleteClientClicked(object sender, EventArgs e)
    {
        if (ClientiCollectionView.SelectedItem is Client client)
        {
            bool confirm = await DisplayAlert("Stergere",
                $"Esti sigur ca vrei sa stergi clientul '{client.Nume}'?",
                "Da, sterge", "Anuleaza");

            if (confirm)
            {
                await _dbService.DeleteClientAsync(client);
                LoadClienti();
                await DisplayAlert("Succes", "Clientul a fost sters!", "OK");
            }
        }
        else
        {
            await DisplayAlert("Selectare", "Selecteaza un client pentru stergere", "OK");
        }
    }
}