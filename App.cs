namespace ZR_schetsen;

public class App : Application
{
    public App()
    {
        MainPage = new NavigationPage(new MainPage());
    }
}
