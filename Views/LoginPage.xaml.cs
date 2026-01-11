using System;
using Microsoft.Maui.Controls;
using ServiceAuto.Services;
using System.Threading.Tasks;

namespace ServiceAuto.Views
{
    public partial class LoginPage : ContentPage
    {
        private Entry _emailEntry;
        private Entry _passwordEntry;
        private Label _errorMessage;

        public LoginPage()
        {
            BuildUI();
        }

        private void BuildUI()
        {
            _emailEntry = new Entry { Placeholder = "Email", Text = "mecanic@service.ro" };
            _passwordEntry = new Entry { Placeholder = "Parolă", IsPassword = true, Text = "123456" };
            _errorMessage = new Label { TextColor = Colors.Red, IsVisible = false };

            var loginButton = new Button
            {
                Text = "👉 Login",
                BackgroundColor = Color.FromArgb("#2196F3"),
                TextColor = Colors.White
            };
            loginButton.Clicked += LoginButton_Clicked;

            var registerButton = new Button
            {
                Text = "📝 Creează cont nou",
                BackgroundColor = Colors.Transparent,
                TextColor = Color.FromArgb("#2196F3")
            };
            registerButton.Clicked += OnInregistrareClicked; 

            Content = new StackLayout
            {
                Padding = 30,
                Spacing = 20,
                Children =
                {
                    new Label { Text = "🚗 ServiceAuto", FontSize = 32, HorizontalOptions = LayoutOptions.Center },

                    new Frame
                    {
                        CornerRadius = 10,
                        Padding = 20,
                        Content = new StackLayout
                        {
                            Spacing = 15,
                            Children =
                            {
                                new Label { Text = "Autentificare", FontSize = 20, FontAttributes = FontAttributes.Bold, HorizontalOptions = LayoutOptions.Center },
                                _emailEntry,
                                _passwordEntry,
                                _errorMessage,
                                loginButton,
                                registerButton
                            }
                        }
                    }
                }
            };
        }

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            var dbService = new DatabaseService();
            var isAuthenticated = await dbService.VerificaAutentificareAsync(_emailEntry.Text, _passwordEntry.Text);

            if (isAuthenticated)
            {
                await Shell.Current.GoToAsync("//DashboardPage");
                if (Shell.Current is AppShell appShell) appShell.EnableFlyout();
            }
            else
            {
                _errorMessage.Text = "Email sau parolă incorectă!";
                _errorMessage.IsVisible = true;
            }
        }

        private async void OnInregistrareClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(InregistrarePage));
        }
    }
}
