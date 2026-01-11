using ServiceAuto.Models;
using ServiceAuto.Services;

namespace ServiceAuto.Views;

public partial class InregistrarePage : ContentPage
{
    private readonly DatabaseService _dbService;

    private Entry _numeEntry;
    private Entry _emailEntry;
    private Entry _telefonEntry;
    private Entry _parolaEntry;
    private Entry _confirmareParolaEntry;

    public InregistrarePage()
    {
        _dbService = new DatabaseService();

       

        CreateUI();
    }

    private void CreateUI()
    {
        _numeEntry = new Entry { Placeholder = "Introdu numele complet..." };
        _emailEntry = new Entry { Placeholder = "ex: nume@email.com", Keyboard = Keyboard.Email };
        _telefonEntry = new Entry { Placeholder = "ex: 0722 123 456", Keyboard = Keyboard.Telephone };
        _parolaEntry = new Entry { Placeholder = "Alege o parolă...", IsPassword = true };
        _confirmareParolaEntry = new Entry { Placeholder = "Introdu parola din nou...", IsPassword = true };

        var registerButton = new Button
        {
            Text = "✅ Creează cont",
            BackgroundColor = Color.FromArgb("#4CAF50"),
            TextColor = Colors.White,
            CornerRadius = 10,
            HeightRequest = 50
        };
        registerButton.Clicked += OnInregistrareClicked; 

        var loginButton = new Button
        {
            Text = "← Am deja cont",
            BackgroundColor = Colors.Transparent,
            TextColor = Color.FromArgb("#2196F3")
        };
        loginButton.Clicked += OnAmDejaContClicked; 

        var stackLayout = new StackLayout
        {
            Padding = 30,
            Spacing = 20,
            Children =
            {
                new Label
                {
                    Text = "📝 Creare cont nou",
                    FontSize = 24,
                    FontAttributes = FontAttributes.Bold,
                    HorizontalOptions = LayoutOptions.Center
                },

                new Frame
                {
                    CornerRadius = 10,
                    Padding = 20,
                    Content = new StackLayout
                    {
                        Spacing = 15,
                        Children =
                        {
                            CreateField("Nume complet *", _numeEntry),
                            CreateField("Email *", _emailEntry),
                            CreateField("Telefon", _telefonEntry),
                            CreateField("Parolă *", _parolaEntry),
                            CreateField("Confirmă parola *", _confirmareParolaEntry),

                            registerButton,
                            loginButton
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

    private async void OnInregistrareClicked(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(_numeEntry.Text))
            {
                await DisplayAlert("Eroare", "Completează numele!", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(_emailEntry.Text) || !_emailEntry.Text.Contains("@"))
            {
                await DisplayAlert("Eroare", "Introdu un email valid!", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(_parolaEntry.Text) || _parolaEntry.Text.Length < 3)
            {
                await DisplayAlert("Eroare", "Parola trebuie să aibă minim 3 caractere!", "OK");
                return;
            }

            if (_parolaEntry.Text != _confirmareParolaEntry.Text)
            {
                await DisplayAlert("Eroare", "Parolele nu coincid!", "OK");
                return;
            }

            var existingMecanic = await _dbService.GetMecanicByEmailAsync(_emailEntry.Text.Trim());
            if (existingMecanic != null)
            {
                await DisplayAlert("Eroare", "Acest email este deja înregistrat!", "OK");
                return;
            }

            var nouMecanic = new Mecanic
            {
                Nume = _numeEntry.Text.Trim(),
                Email = _emailEntry.Text.Trim(),
                Telefon = _telefonEntry.Text?.Trim(),
                ParolaHash = _parolaEntry.Text.Trim(),
                DataAngajare = DateTime.Now,
                EsteAdmin = false,
                EsteActiv = true
            };

            await _dbService.SaveMecanicAsync(nouMecanic);
            await DisplayAlert("Succes", "Cont creat cu succes!", "OK");
            await Shell.Current.GoToAsync("//LoginPage");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Eroare", $"Nu s-a putut crea contul: {ex.Message}", "OK");
        }
    }

    private async void OnAmDejaContClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//LoginPage");
    }
}