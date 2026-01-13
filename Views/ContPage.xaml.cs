using System;
using System.Linq;
using Microsoft.Maui.Controls;
using ServiceAuto.Models;
using ServiceAuto.Services;
using System.Threading.Tasks;

namespace ServiceAuto.Views
{
    public partial class ContPage : ContentPage
    {
        private readonly DatabaseService _dbService;
        private Mecanic _currentMecanic;

        private Entry _numeEntry;
        private Entry _emailEntry;
        private Entry _telefonEntry;
        private Entry _parolaEntry;

        public ContPage()
        {
            InitializeComponent();

            _dbService = new DatabaseService();

            CreateUI();

            LoadCurrentUser();
        }

        private void CreateUI()
        {
            _numeEntry = new Entry { Placeholder = "Introdu numele..." };
            _emailEntry = new Entry { Placeholder = "ex: nume@email.com", Keyboard = Keyboard.Email };
            _telefonEntry = new Entry { Placeholder = "ex: 0722 123 456", Keyboard = Keyboard.Telephone };
            _parolaEntry = new Entry { Placeholder = "Introdu parola nouă...", IsPassword = true };

            var saveButton = new Button
            {
                Text = "💾 Salvează modificările",
                BackgroundColor = Color.FromArgb("#2196F3"),
                TextColor = Colors.White,
                CornerRadius = 10,
                HeightRequest = 50
            };
            saveButton.Clicked += OnSalveazaClicked;

            var logoutButton = new Button
            {
                Text = "🚪 Deconectare",
                BackgroundColor = Colors.LightGray,
                TextColor = Colors.Black,
                CornerRadius = 10
            };
            logoutButton.Clicked += OnLogoutClicked;

            var deleteButton = new Button
            {
                Text = "🗑️ Șterge contul",
                BackgroundColor = Color.FromArgb("#F44336"),
                TextColor = Colors.White,
                CornerRadius = 10
            };
            deleteButton.Clicked += OnStergeContClicked;

            var stackLayout = new StackLayout
            {
                Padding = 20,
                Spacing = 20,
                Children =
                {
                    new Label { Text = "👤 Profilul meu", FontSize = 24, FontAttributes = FontAttributes.Bold, HorizontalOptions = LayoutOptions.Center },

                    new Frame
                    {
                        CornerRadius = 10,
                        Padding = 15,
                        Content = new StackLayout
                        {
                            Spacing = 15,
                            Children =
                            {
                                new Label { Text = "Date personale", FontSize = 18, FontAttributes = FontAttributes.Bold },

                                CreateField("Nume complet", _numeEntry),
                                CreateField("Email", _emailEntry),
                                CreateField("Telefon", _telefonEntry),
                                CreateField("Parolă nouă (lasă gol pentru a păstra vechea)", _parolaEntry),

                                saveButton
                            }
                        }
                    },

                    new Frame
                    {
                        CornerRadius = 10,
                        Padding = 15,
                        Content = new StackLayout
                        {
                            Spacing = 15,
                            Children =
                            {
                                new Label { Text = "Setări cont", FontSize = 18, FontAttributes = FontAttributes.Bold },
                                logoutButton,
                                deleteButton
                            }
                        }
                    }
                }
            };

            Content = new ScrollView { Content = stackLayout };
        }

        private StackLayout CreateField(string labelText, Entry entry)
        {
            return new StackLayout
            {
                Children =
                {
                    new Label { Text = labelText, FontSize = 14, TextColor = Colors.Gray },
                    entry
                }
            };
        }

        private async void LoadCurrentUser()
        {
            try
            {
                var mecanici = await _dbService.GetMecaniciAsync();
                _currentMecanic = mecanici.FirstOrDefault();

                if (_currentMecanic != null)
                {
                    _numeEntry.Text = _currentMecanic.Nume;
                    _emailEntry.Text = _currentMecanic.Email;
                    _telefonEntry.Text = _currentMecanic.Telefon;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Eroare", $"Nu s-au putut incarca datele: {ex.Message}", "OK");
            }
        }

        private async void OnSalveazaClicked(object sender, EventArgs e)
        {
            try
            {
                if (_currentMecanic == null) return;

                _currentMecanic.Nume = _numeEntry.Text?.Trim();
                _currentMecanic.Email = _emailEntry.Text?.Trim();
                _currentMecanic.Telefon = _telefonEntry.Text?.Trim();

                if (!string.IsNullOrWhiteSpace(_parolaEntry.Text))
                {
                    _currentMecanic.ParolaHash = _parolaEntry.Text.Trim();
                }

                await DisplayAlert("Succes", "Datele au fost actualizate!", "OK");
                _parolaEntry.Text = string.Empty;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Eroare", $"Nu s-au putut salva datele: {ex.Message}", "OK");
            }
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Deconectare", "Sigur doriti sa va deconectati?", "Da", "Nu");
            if (confirm) await Shell.Current.GoToAsync("//LoginPage");
        }

        private async void OnStergeContClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Stergere cont", "Sigur doriti sa stergeti contul?", "Da, sterge", "Anuleaza");
            if (confirm) await DisplayAlert("Info", "Functionalitate demo", "OK");
        }
    }
}
