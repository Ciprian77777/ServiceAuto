using System.Collections.ObjectModel;
using System.Windows.Input;
using ServiceAuto.Models;
using ServiceAuto.Services;

namespace ServiceAuto.Views;

public partial class InterventiiClientPage : ContentPage
{
    private readonly DatabaseService _dbService;
    private readonly int _clientId;
    private string _clientNume;

    public ICommand BackCommand { get; }
    public ICommand LoadInterventiiCommand { get; }
    public ICommand ViewDetailsCommand { get; }

    private ObservableCollection<Interventie> _interventii;
    public ObservableCollection<Interventie> Interventii
    {
        get => _interventii;
        set
        {
            _interventii = value;
            OnPropertyChanged(nameof(Interventii));
        }
    }

    private bool _isRefreshing;
    public bool IsRefreshing
    {
        get => _isRefreshing;
        set
        {
            _isRefreshing = value;
            OnPropertyChanged(nameof(IsRefreshing));
        }
    }

    public string ClientInfo => $"👤 {_clientNume}";

    public InterventiiClientPage(int clientId, string clientNume = "")
    {
        InitializeComponent();

        _dbService = new DatabaseService();
        _clientId = clientId;
        _clientNume = clientNume;

        BackCommand = new Command(async () => await Navigation.PopAsync());
        LoadInterventiiCommand = new Command(async () => await LoadInterventiiAsync());
        ViewDetailsCommand = new Command<Interventie>(async (interventie) => await ViewInterventieDetailsAsync(interventie));

        Shell.SetBackButtonBehavior(this, new BackButtonBehavior
        {
            Command = BackCommand
        });

        Shell.SetNavBarIsVisible(this, true);

        BindingContext = this;

        LoadInterventiiAsync();
    }

    private async Task LoadInterventiiAsync()
    {
        try
        {
            IsRefreshing = true;

            if (string.IsNullOrEmpty(_clientNume))
            {
                var client = await _dbService.GetClientByIdAsync(_clientId);
                _clientNume = client?.Nume ?? $"Client #{_clientId}";
                OnPropertyChanged(nameof(ClientInfo));
            }

           
            var allInterventii = await _dbService.GetInterventiiAsync();
            var clientInterventii = allInterventii.Where(i => i.ClientId == _clientId).ToList();

            Interventii = new ObservableCollection<Interventie>();

            foreach (var interventie in clientInterventii)
            {
                var masina = await _dbService.GetMasinaByIdAsync(interventie.MasinaId);
                if (masina != null)
                {
                    interventie.MasinaInfo = $"{masina.Marca} {masina.Model} ({masina.NrInmatriculare})";
                }

                Interventii.Add(interventie);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Eroare", $"Nu s-au putut încărca intervențiile: {ex.Message}", "OK");
        }
        finally
        {
            IsRefreshing = false;
        }
    }

    private async Task ViewInterventieDetailsAsync(Interventie interventie)
    {
        if (interventie != null)
        {
            await DisplayAlert("Detalii Intervenție",
                $"Descriere: {interventie.Descriere}\n" +
                $"Data: {interventie.DataProgramare:dd.MM.yyyy HH:mm}\n" +
                $"Status: {interventie.Status}\n" +
                $"Mașina: {interventie.MasinaInfo}",
                "OK");
        }
    }

    /*protected override bool OnBackButtonPressed()
    {
        Device.BeginInvokeOnMainThread(async () =>
        {
            await Navigation.PopAsync();
        });
        return true;
    }*/
}