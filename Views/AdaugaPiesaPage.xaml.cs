using ServiceAuto.Models;
using ServiceAuto.Services;

namespace ServiceAuto.Views;

public partial class AdaugaPiesaPage : ContentPage
{
    private readonly DatabaseService _dbService;
    private PiesaDeSchimb _piesa;
    private bool _isEditMode;

    public string Title => _isEditMode ? "Editare piesă" : "Adaugă piesă nouă";
    public string SaveButtonText => _isEditMode ? "Salvează modificări" : "Adaugă piesă";

    public List<string> Categorii { get; } = new List<string>
    {
        "Motor",
        "Transmisie",
        "Frâne",
        "Suspensie",
        "Electric",
        "Caroserie",
        "Filtre",
        "Uleiuri",
        "Lichide",
        "General"
    };

    public PiesaDeSchimb Piesa
    {
        get => _piesa;
        set
        {
            _piesa = value;
            OnPropertyChanged(nameof(Piesa));
        }
    }

    public AdaugaPiesaPage(PiesaDeSchimb piesa = null)
    {
        InitializeComponent();
        _dbService = new DatabaseService();

        _isEditMode = piesa != null;
        Piesa = piesa ?? new PiesaDeSchimb();

        BindingContext = this;

        if (!string.IsNullOrEmpty(Piesa.Categorie) && Categorii.Contains(Piesa.Categorie))
        {
            CategoriePicker.SelectedItem = Piesa.Categorie;
        }
        else
        {
            CategoriePicker.SelectedIndex = Categorii.Count - 1; 
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Piesa.Denumire))
        {
            ShowError("Denumirea piesei este obligatorie!");
            return;
        }

        if (Piesa.Pret <= 0)
        {
            ShowError("Prețul trebuie să fie mai mare decât 0!");
            return;
        }

        if (Piesa.Stoc < 0)
        {
            ShowError("Stocul nu poate fi negativ!");
            return;
        }

        ErrorMessage.IsVisible = false;

        try
        {
            await _dbService.SavePiesaAsync(Piesa);

            string message = _isEditMode ? "Piesa a fost actualizată cu succes!"
                                         : "Piesa a fost adăugată cu succes!";

            await DisplayAlert("Succes", message, "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            ShowError($"Eroare la salvarea piesei: {ex.Message}");
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