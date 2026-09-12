namespace ZR_schetsen;

public class MainPage : ContentPage
{
    public MainPage()
    {
        Title = "ZR schetsen";
        BackgroundColor = Color.FromArgb("#FFF7DF");

        var title = new Label { Text = "✏️ ZR schetsen", FontSize = 34, FontAttributes = FontAttributes.Bold, HorizontalTextAlignment = TextAlignment.Center };
        var subtitle = new Label { Text = "Kies je tekenavontuur!", FontSize = 20, HorizontalTextAlignment = TextAlignment.Center };

        var paris = MakeButton("🗼  Zayd → Parijs", "#E7B96A");
        var brussels = MakeButton("⚛️  Razan → Brussel", "#76B89C");
        var avatars = MakeButton("👦👧👩👨  Kies je avatar", "#AFC9E8");
        var duel = MakeButton("⚔️  Tegen elkaar", "#E99B8E");

        paris.Clicked += async (_, _) => await Navigation.PushAsync(new DrawingPage("Eiffeltoren 🗼"));
        brussels.Clicked += async (_, _) => await Navigation.PushAsync(new DrawingPage("Atomium ⚛️"));
        avatars.Clicked += async (_, _) => await DisplayAlert("Kies je avatar", "Zayd 👦   Razan 👧   Mama 👩   Papa 👨", "Leuk!");
        duel.Clicked += async (_, _) => await DisplayAlert("Tegen elkaar", "Iedereen krijgt dezelfde tekening. Wie wordt de tekenkampioen? 🏆", "Start binnenkort");

        var level = new Label { Text = "⭐ Level 1 — Beginner\n0 XP  •  Oefen en stijg in level!", FontSize = 18, HorizontalTextAlignment = TextAlignment.Center };
        var footer = new Label { Text = "💚 Gratis  •  🚫 Geen advertenties", FontSize = 16, FontAttributes = FontAttributes.Bold, HorizontalTextAlignment = TextAlignment.Center };

        Content = new ScrollView { Content = new VerticalStackLayout { Padding = 24, Spacing = 18, Children = { title, subtitle, paris, brussels, avatars, duel, level, footer } } };
    }

    static Button MakeButton(string text, string color) => new()
    {
        Text = text, FontSize = 22, FontAttributes = FontAttributes.Bold,
        HeightRequest = 72, CornerRadius = 22, BackgroundColor = Color.FromArgb(color), TextColor = Colors.Black
    };
}
