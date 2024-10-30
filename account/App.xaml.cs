using account.Views;

namespace account
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        public static object Database { get; internal set; }
    }
}
