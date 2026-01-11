using ServiceAuto.Models;
using ServiceAuto.Services;

namespace ServiceAuto.Views;

public partial class AdaugaClientPage : ContentPage
{
    private readonly DatabaseService _dbService;
    private Client _client;
    private bool _isEditMode;

    public string Title => _isEditMode ? "Editare client" : "Adaugă client nou";
    public string SaveButtonText => _isEditMode ? "Salvează modificări" : "Adaugă client";

    public Client Client
    {
        get => _client;
        set
        {
            _client = value;
            OnPropertyChanged(nameof(Client));
        }
    }

    public AdaugaClientPage(Client client = null)
    {
        InitializeComponent();
        _dbService = new DatabaseService();

        _isEditMode = client != null;
        Client = client ?? new Client();

        BindingContext = this;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Client.Nume))
        {
            ShowError("Numele este obligatoriu!");
            return;
        }

        if (string.IsNullOrWhiteSpace(Client.Telefon))
        {
            ShowError("Telefonul este obligatoriu!");
            return;
        }

        ErrorMessage.IsVisible = false;

        try
        {
            await _dbService.SaveClientAsync(Client);

            string message = _isEditMode ? "Clientul a fost actualizat cu succes!"
                                         : "Clientul a fost adăugat cu succes!";

            await DisplayAlert("Succes", message, "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            ShowError($"Eroare la salvarea clientului: {ex.Message}");
        }
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private void ShowError(string message)
    {
        ErrorMessage.Text = message;
        ErrorMessage.IsVisible = true;
    }
}