using ServiceAuto.Models;
using ServiceAuto.Services;

namespace ServiceAuto.Views;

public partial class PiesePage : ContentPage
{
    private readonly DatabaseService _dbService;
    private List<PiesaDeSchimb> _allPiese;

    public int TotalPiese { get; set; }
    public decimal ValoareTotala { get; set; }

    public PiesePage()
    {
        InitializeComponent();
        _dbService = new DatabaseService();
        BindingContext = this;

        LoadPiese();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadPiese();
    }

    private async void LoadPiese()
    {
        try
        {
            RefreshView.IsRefreshing = true;

            _allPiese = await _dbService.GetPieseDeSchimbAsync();
            PieseCollectionView.ItemsSource = _allPiese;

            TotalPiese = _allPiese.Count;
            ValoareTotala = _allPiese.Sum(p => p.Pret * p.Stoc);

            OnPropertyChanged(nameof(TotalPiese));
            OnPropertyChanged(nameof(ValoareTotala));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Eroare", $"Eroare la incarcarea pieselor: {ex.Message}", "OK");
        }
        finally
        {
            RefreshView.IsRefreshing = false;
        }
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            PieseCollectionView.ItemsSource = _allPiese;
        }
        else
        {
            var searchText = e.NewTextValue.ToLower();
            var filtered = _allPiese.Where(p =>
                p.Denumire.ToLower().Contains(searchText) ||
                p.Cod.ToLower().Contains(searchText) ||
                p.Producator.ToLower().Contains(searchText) ||
                p.Categorie.ToLower().Contains(searchText)
            ).ToList();

            PieseCollectionView.ItemsSource = filtered;
        }
    }

    private async void OnAdaugaPiesaClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AdaugaPiesaPage());
    }

    private async void OnEditPiesaClicked(object sender, EventArgs e)
    {
        var swipeItem = sender as SwipeItem;
        var piesa = swipeItem?.BindingContext as PiesaDeSchimb;

        if (piesa != null)
        {
            await Navigation.PushAsync(new AdaugaPiesaPage(piesa));
        }
    }

    private async void OnDeletePiesaClicked(object sender, EventArgs e)
    {
        var swipeItem = sender as SwipeItem;
        var piesa = swipeItem?.BindingContext as PiesaDeSchimb;

        if (piesa != null)
        {
            bool confirm = await DisplayAlert("Stergere piesa",
                $"Esti sigur ca vrei sa stergi piesa '{piesa.Denumire}'?",
                "Da, sterge", "Anulează");

            if (confirm)
            {
                await _dbService.DeletePiesaAsync(piesa);
                LoadPiese();
                await DisplayAlert("Succes", "Piesa a fost stearsa!", "OK");
            }
        }
    }
}