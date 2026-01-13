using ServiceAuto.Models;
using ServiceAuto.Services;

namespace ServiceAuto.Views;

public partial class DetaliiInterventiePage : ContentPage
{
    private readonly DatabaseService _dbService;
    private Interventie _interventie;
    private List<InterventiePiesa> _pieseInterventie;

    public Interventie Interventie
    {
        get => _interventie;
        set
        {
            _interventie = value;
            OnPropertyChanged(nameof(Interventie));
            OnPropertyChanged(nameof(StatusColor));
            OnPropertyChanged(nameof(PoateFiFinalizata));
        }
    }

    public Color StatusColor => Interventie?.StatusColor ?? Colors.Gray;
    public bool PoateFiFinalizata => Interventie?.Status == "Programata" || Interventie?.Status == "InCurs";

    public decimal CostManopera => Interventie?.CostTotal ?? 0;
    public decimal CostPiese => _pieseInterventie?.Sum(ip => ip.Subtotal) ?? 0;
    public decimal CostTotal => CostManopera + CostPiese;

    public DetaliiInterventiePage(int interventieId)
    {
        InitializeComponent();
        _dbService = new DatabaseService();

        BindingContext = this;
        LoadInterventie(interventieId);
    }

    private async void LoadInterventie(int interventieId)
    {
        try
        {
            Interventie = await _dbService.GetInterventieByIdAsync(interventieId);

            if (Interventie == null)
            {
                await DisplayAlert("Eroare", "Interventia nu a fost gasita!", "OK");
                await Navigation.PopAsync();
                return;
            }

            Title = $"Interventie #{Interventie.Id}";

            await LoadClientSiMasina();

            await LoadPieseInterventie();

            OnPropertyChanged(nameof(CostManopera));
            OnPropertyChanged(nameof(CostPiese));
            OnPropertyChanged(nameof(CostTotal));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Eroare", $"Eroare la încarcarea interventiei: {ex.Message}", "OK");
        }
    }

    private async Task LoadClientSiMasina()
    {
        try
        {
            var client = await _dbService.GetClientByIdAsync(Interventie.ClientId);
            var masina = await _dbService.GetMasinaByIdAsync(Interventie.MasinaId);

            ClientLabel.Text = $"?? {client?.Nume ?? "Necunoscut"}";
            MasinaLabel.Text = $"?? {masina?.DisplayText ?? "Necunoscut?"}";
        }
        catch
        {
            // Ignora
        }
    }

    private async Task LoadPieseInterventie()
    {
        try
        {
            _pieseInterventie = await _dbService.GetPiesePentruInterventieAsync(Interventie.Id);
            PieseCountLabel.Text = $"({_pieseInterventie.Count})";

            PieseCollectionView.ItemsSource = _pieseInterventie;
            PieseCollectionView.ItemTemplate = new DataTemplate(() =>
            {
                var grid = new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = GridLength.Star },
                        new ColumnDefinition { Width = GridLength.Auto },
                        new ColumnDefinition { Width = GridLength.Auto }
                    },
                    Padding = new Thickness(10, 5),
                    HeightRequest = 40
                };

                var nameLabel = new Label
                {
                    VerticalOptions = LayoutOptions.Center,
                    FontSize = 14
                };
                nameLabel.SetBinding(Label.TextProperty, "DenumirePiesa");

                var qtyLabel = new Label
                {
                    VerticalOptions = LayoutOptions.Center,
                    FontSize = 12,
                    TextColor = Colors.Gray
                };
                qtyLabel.SetBinding(Label.TextProperty, "Cantitate", stringFormat: "{0} buc");

                var priceLabel = new Label
                {
                    VerticalOptions = LayoutOptions.Center,
                    FontSize = 12,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Colors.Green
                };
                priceLabel.SetBinding(Label.TextProperty, "Subtotal", stringFormat: "{0:F2} RON");

                grid.Add(nameLabel, 0, 0);
                grid.Add(qtyLabel, 1, 0);
                grid.Add(priceLabel, 2, 0);

                return grid;
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Eroare la încarcarea pieselor: {ex.Message}");
        }
    }

    private async void OnPieseClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Info", "Gestiunea pieselor pentru interventie va fi implementata!", "OK");
    }

    private async void OnFinalizeazaClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Finalizare",
            "Esti sigur ca vrei sa finalizezi aceasta interventie?",
            "Da, finalizeaza", "Anuleaza");

        if (confirm)
        {
            try
            {
                await _dbService.UpdateInterventieStatusAsync(Interventie.Id, "Finalizata");
                await DisplayAlert("Succes", "Interventia a fost finalizata!", "OK");
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Eroare", $"Eroare la finalizare: {ex.Message}", "OK");
            }
        }
    }

    private async void OnAdaugaPiesaClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AdaugaPiesaPage));
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (Interventie != null)
        {
            LoadPieseInterventie();
        }
    }
}