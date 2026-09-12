namespace ZR_schetsen;

public class DrawingPage : ContentPage
{
    readonly DrawingView drawing = new();

    public DrawingPage(string challenge)
    {
        Title = challenge;
        BackgroundColor = Color.FromArgb("#FFFDF6");

        var heading = new Label { Text = $"✏️ Teken: {challenge}", FontSize = 26, FontAttributes = FontAttributes.Bold, HorizontalTextAlignment = TextAlignment.Center };
        var hint = new Label { Text = "Teken met je vinger. Maak je eigen schets!", FontSize = 17, HorizontalTextAlignment = TextAlignment.Center };
        drawing.HeightRequest = 430;
        drawing.BackgroundColor = Colors.White;

        var clear = new Button { Text = "🧽 Wissen", CornerRadius = 18 };
        var done = new Button { Text = "⭐ Klaar!", CornerRadius = 18 };
        var back = new Button { Text = "⬅️ Terug", CornerRadius = 18 };

        clear.Clicked += (_, _) => drawing.Clear();
        back.Clicked += async (_, _) => await Navigation.PopAsync();
        done.Clicked += async (_, _) =>
        {
            bool home = await DisplayAlert(
                "Goed gedaan! 🎉",
                "Je krijgt 10 XP voor het oefenen! ⭐\n\nWil je terug naar het startscherm?",
                "🏠 Startscherm",
                "✏️ Verder tekenen");

            if (home)
                await Navigation.PopToRootAsync();
        };

        Content = new VerticalStackLayout
        {
            Padding = 18,
            Spacing = 14,
            Children =
            {
                heading,
                hint,
                drawing,
                new HorizontalStackLayout
                {
                    Spacing = 10,
                    HorizontalOptions = LayoutOptions.Center,
                    Children = { back, clear, done }
                }
            }
        };
    }
}

public class DrawingView : GraphicsView, IDrawable
{
    readonly List<List<PointF>> strokes = new();
    List<PointF>? current;

    public DrawingView()
    {
        Drawable = this;
        StartInteraction += (_, e) => { current = new List<PointF> { e.Touches[0] }; strokes.Add(current); Invalidate(); };
        DragInteraction += (_, e) => { if (current != null) { current.Add(e.Touches[0]); Invalidate(); } };
        EndInteraction += (_, _) => current = null;
    }

    public void Clear() { strokes.Clear(); Invalidate(); }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.StrokeColor = Colors.Black;
        canvas.StrokeSize = 5;
        canvas.StrokeLineCap = LineCap.Round;
        foreach (var stroke in strokes)
            for (int i = 1; i < stroke.Count; i++)
                canvas.DrawLine(stroke[i - 1], stroke[i]);
    }
}
