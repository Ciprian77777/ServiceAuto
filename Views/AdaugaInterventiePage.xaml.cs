using ServiceAuto.Models;
using ServiceAuto.Services;

namespace ServiceAuto.Views;

public partial class AdaugaInterventiePage : ContentPage
{
    private readonly DatabaseService _dbService;

    public AdaugaInterventiePage()
    {
        InitializeComponent();
        _dbService = new DatabaseService();
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        ErrorMessage.IsVisible = false;

        if (string.IsNullOrWhiteSpace(DescriereEntry.Text))
        {
            ShowError("Descrierea este obligatorie!");
            return;
        }

        if (!int.TryParse(ClientIdEntry.Text, out int clientId) || clientId <= 0)
        {
            ShowError("ID Client invalid!");
            return;
        }

        if (!int.TryParse(MasinaIdEntry.Text, out int masinaId) || masinaId <= 0)
        {
            ShowError("ID Mașină invalid!");
            return;
        }

        var interventie = new Interventie
        {
            Descriere = DescriereEntry.Text,
            ClientId = clientId,
            MasinaId = masinaId,
            MecanicId = 1, 
            DataProgramare = DateTime.Now.AddDays(1), 
            Status = "Programata",
            TipInterventie = "Reparație",
            CostTotal = 0,
            OreManopera = 0
        };

        try
        {
            await _dbService.SaveInterventieAsync(interventie);
            await DisplayAlert("Succes", "Intervenția a fost adăugată!", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            ShowError($"Eroare: {ex.Message}");
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