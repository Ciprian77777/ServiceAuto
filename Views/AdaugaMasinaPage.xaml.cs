using ServiceAuto.Models;
using ServiceAuto.Services;

namespace ServiceAuto.Views;

public partial class AdaugaMasinaPage : ContentPage
{
    private readonly DatabaseService _dbService;
    private Masina _masina;
    private bool _isEditMode;
    private List<Client> _clienti;

    public string Title => _isEditMode ? "Editare mașină" : "Adaugă mașină nouă";
    public string SaveButtonText => _isEditMode ? "Salvează modificări" : "Adaugă mașină";

    public List<string> TipuriCombustibil { get; } = new List<string>
    {
        "Benzina",
        "Diesel",
        "Electric",
        "Hibrid",
        "GPL",
        "Altul"
    };

    public Masina Masina
    {
        get => _masina;
        set
        {
            _masina = value;
            OnPropertyChanged(nameof(Masina));
        }
    }

    public AdaugaMasinaPage(Masina masina = null)
    {
        InitializeComponent();
        _dbService = new DatabaseService();

        _isEditMode = masina != null;
        Masina = masina ?? new Masina();

        BindingContext = this;
        LoadClienti();
    }

    private async void LoadClienti()
    {
        try
        {
            _clienti = await _dbService.GetClientiAsync();
            ClientPicker.ItemsSource = _clienti;

            if (Masina.ClientId > 0)
            {
                var client = _clienti.FirstOrDefault(c => c.Id == Masina.ClientId);
                if (client != null)
                {
                    ClientPicker.SelectedItem = client;
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Eroare", $"Eroare la încărcarea clienților: {ex.Message}", "OK");
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (ClientPicker.SelectedItem == null)
        {
            ShowError("Selectează un client!");
            return;
        }

        if (string.IsNullOrWhiteSpace(Masina.Marca))
        {
            ShowError("Marca este obligatorie!");
            return;
        }

        if (string.IsNullOrWhiteSpace(Masina.Model))
        {
            ShowError("Modelul este obligatoriu!");
            return;
        }

        if (string.IsNullOrWhiteSpace(Masina.NrInmatriculare))
        {
            ShowError("Numărul de înmatriculare este obligatoriu!");
            return;
        }

        var selectedClient = (Client)ClientPicker.SelectedItem;
        Masina.ClientId = selectedClient.Id;

        ErrorMessage.IsVisible = false;

        try
        {
            await _dbService.SaveMasinaAsync(Masina);

            string message = _isEditMode ? "Mașina a fost actualizată cu succes!"
                                         : "Mașina a fost adăugată cu succes!";

            await DisplayAlert("Succes", message, "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            ShowError($"Eroare la salvarea mașinii: {ex.Message}");
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