using ServiceAuto.Models;
using ServiceAuto.Services;

namespace ServiceAuto.Views;

public partial class MasiniPage : ContentPage
{
    private readonly DatabaseService _dbService;
    private List<Masina> _allMasini;

    public MasiniPage()
    {
        InitializeComponent();
        _dbService = new DatabaseService();

        LoadMasini();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadMasini();
    }

    private async void LoadMasini()
    {
        try
        {
            _allMasini = await _dbService.GetMasiniAsync();
            MasiniCollectionView.ItemsSource = _allMasini;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Eroare", $"Eroare la incarcarea masinilor: {ex.Message}", "OK");
        }
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            MasiniCollectionView.ItemsSource = _allMasini;
        }
        else
        {
            var searchText = e.NewTextValue.ToLower();
            var filtered = _allMasini.Where(m =>
                m.Marca.ToLower().Contains(searchText) ||
                m.Model.ToLower().Contains(searchText) ||
                m.NrInmatriculare.ToLower().Contains(searchText) ||
                m.Culoare.ToLower().Contains(searchText)
            ).ToList();

            MasiniCollectionView.ItemsSource = filtered;
        }
    }

    private async void OnAdaugaMasinaClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AdaugaMasinaPage());
    }

    private async void OnEditMasinaClicked(object sender, EventArgs e)
    {
        if (MasiniCollectionView.SelectedItem is Masina masina)
        {
            await Navigation.PushAsync(new AdaugaMasinaPage(masina));
        }
        else
        {
            await DisplayAlert("Selectare", "Selecteaza o masina pentru editare", "OK");
        }
    }
}