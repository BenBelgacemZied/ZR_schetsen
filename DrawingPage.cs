namespace ZR_schetsen;

public class DrawingPage : ContentPage
{
    readonly DrawingView drawing = new();
    readonly TutorialView tutorial;
    readonly Label stepLabel = new();
    readonly Button playPause = new();
    bool isPlaying;
    int tutorialStep;

    public DrawingPage(string challenge)
    {
        Title = challenge;
        BackgroundColor = Color.FromArgb("#FFFDF6");
        tutorial = new TutorialView(challenge);

        var heading = new Label
        {
            Text = $"✏️ Teken: {challenge}",
            FontSize = 24,
            FontAttributes = FontAttributes.Bold,
            HorizontalTextAlignment = TextAlignment.Center
        };

        var videoTitle = new Label
        {
            Text = "🎬 Stap-voor-stap",
            FontSize = 18,
            FontAttributes = FontAttributes.Bold,
            HorizontalTextAlignment = TextAlignment.Center
        };

        tutorial.HeightRequest = 210;
        tutorial.BackgroundColor = Color.FromArgb("#F3F7FB");

        stepLabel.Text = "Stap 1 van 6";
        stepLabel.FontSize = 15;
        stepLabel.HorizontalTextAlignment = TextAlignment.Center;

        playPause.Text = "▶️ Afspelen";
        playPause.CornerRadius = 16;
        var restart = new Button { Text = "🔄 Opnieuw", CornerRadius = 16 };
        var previous = new Button { Text = "⏮️ Vorige", CornerRadius = 16 };
        var next = new Button { Text = "⏭️ Volgende", CornerRadius = 16 };

        playPause.Clicked += (_, _) => TogglePlayback();
        restart.Clicked += (_, _) => RestartTutorial();
        previous.Clicked += (_, _) => SetTutorialStep(Math.Max(0, tutorialStep - 1));
        next.Clicked += (_, _) => SetTutorialStep(Math.Min(5, tutorialStep + 1));

        var drawTitle = new Label
        {
            Text = "✏️ Teken hieronder mee",
            FontSize = 18,
            FontAttributes = FontAttributes.Bold,
            HorizontalTextAlignment = TextAlignment.Center
        };

        drawing.HeightRequest = 360;
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

        Content = new ScrollView
        {
            Content = new VerticalStackLayout
            {
                Padding = 14,
                Spacing = 10,
                Children =
                {
                    heading,
                    videoTitle,
                    tutorial,
                    stepLabel,
                    new HorizontalStackLayout
                    {
                        Spacing = 6,
                        HorizontalOptions = LayoutOptions.Center,
                        Children = { previous, playPause, next, restart }
                    },
                    drawTitle,
                    drawing,
                    new HorizontalStackLayout
                    {
                        Spacing = 10,
                        HorizontalOptions = LayoutOptions.Center,
                        Children = { back, clear, done }
                    }
                }
            }
        };

        SetTutorialStep(0);
    }

    void TogglePlayback()
    {
        isPlaying = !isPlaying;
        playPause.Text = isPlaying ? "⏸️ Pauze" : "▶️ Afspelen";

        if (!isPlaying) return;

        Dispatcher.StartTimer(TimeSpan.FromMilliseconds(900), () =>
        {
            if (!isPlaying) return false;

            if (tutorialStep >= 5)
            {
                isPlaying = false;
                playPause.Text = "▶️ Afspelen";
                return false;
            }

            SetTutorialStep(tutorialStep + 1);
            return true;
        });
    }

    void RestartTutorial()
    {
        isPlaying = false;
        playPause.Text = "▶️ Afspelen";
        SetTutorialStep(0);
    }

    void SetTutorialStep(int step)
    {
        tutorialStep = step;
        tutorial.Step = tutorialStep;
        tutorial.Invalidate();
        stepLabel.Text = $"Stap {tutorialStep + 1} van 6";
    }
}

public class TutorialView : GraphicsView, IDrawable
{
    readonly string challenge;
    public int Step { get; set; }

    public TutorialView(string challenge)
    {
        this.challenge = challenge;
        Drawable = this;
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.FillColor = Color.FromArgb("#F3F7FB");
        canvas.FillRectangle(dirtyRect);
        canvas.StrokeColor = Colors.Black;
        canvas.StrokeSize = 4;
        canvas.StrokeLineCap = LineCap.Round;

        if (challenge.Contains("Atomium", StringComparison.OrdinalIgnoreCase))
            DrawAtomium(canvas, dirtyRect);
        else
            DrawEiffel(canvas, dirtyRect);
    }

    void DrawEiffel(ICanvas canvas, RectF r)
    {
        float cx = r.Center.X;
        float top = 20;
        float bottom = r.Height - 18;
        float left = cx - r.Width * .28f;
        float right = cx + r.Width * .28f;

        if (Step >= 0) canvas.DrawLine(cx, top, cx - 18, top + 45);
        if (Step >= 1) canvas.DrawLine(cx, top, cx + 18, top + 45);
        if (Step >= 2) { canvas.DrawLine(cx - 18, top + 45, left, bottom); canvas.DrawLine(cx + 18, top + 45, right, bottom); }
        if (Step >= 3) { canvas.DrawLine(cx - 40, top + 95, cx + 40, top + 95); canvas.DrawLine(cx - 65, top + 135, cx + 65, top + 135); }
        if (Step >= 4) { canvas.DrawLine(left + 25, bottom - 35, right - 25, bottom - 35); canvas.DrawArc(cx - 38, bottom - 60, 76, 48, 180, 180, false, false); }
        if (Step >= 5) { canvas.DrawLine(cx - 8, top + 45, cx - 28, bottom - 40); canvas.DrawLine(cx + 8, top + 45, cx + 28, bottom - 40); }
    }

    void DrawAtomium(ICanvas canvas, RectF r)
    {
        float cx = r.Center.X;
        float cy = r.Center.Y;
        float d = Math.Min(r.Width, r.Height) * .22f;
        var p = new[]
        {
            new PointF(cx, cy),
            new PointF(cx - d, cy - d),
            new PointF(cx + d, cy - d),
            new PointF(cx - d, cy + d),
            new PointF(cx + d, cy + d),
            new PointF(cx, cy - d * 1.65f),
            new PointF(cx, cy + d * 1.65f)
        };

        if (Step >= 0) DrawBall(canvas, p[0], 16);
        if (Step >= 1) { canvas.DrawLine(p[0], p[1]); canvas.DrawLine(p[0], p[2]); DrawBall(canvas, p[1], 14); DrawBall(canvas, p[2], 14); }
        if (Step >= 2) { canvas.DrawLine(p[0], p[3]); canvas.DrawLine(p[0], p[4]); DrawBall(canvas, p[3], 14); DrawBall(canvas, p[4], 14); }
        if (Step >= 3) { canvas.DrawLine(p[1], p[5]); canvas.DrawLine(p[2], p[5]); DrawBall(canvas, p[5], 13); }
        if (Step >= 4) { canvas.DrawLine(p[3], p[6]); canvas.DrawLine(p[4], p[6]); DrawBall(canvas, p[6], 13); }
        if (Step >= 5) { canvas.DrawLine(p[1], p[3]); canvas.DrawLine(p[2], p[4]); canvas.DrawLine(p[5], p[0]); canvas.DrawLine(p[0], p[6]); }
    }

    static void DrawBall(ICanvas canvas, PointF p, float radius)
    {
        canvas.FillColor = Colors.White;
        canvas.FillCircle(p.X, p.Y, radius);
        canvas.DrawCircle(p.X, p.Y, radius);
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
