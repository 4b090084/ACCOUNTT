using account.Views;

namespace account
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
           InitializeComponent();
            //定義頁面路徑
            Routing.RegisterRoute(nameof(Views.AddAccountingPage), typeof(Views.AddAccountingPage));
            Routing.RegisterRoute(nameof(Views.AllAccountingPage), typeof(Views.AllAccountingPage));
            Routing.RegisterRoute(nameof(Views.AppSetPage), typeof(Views.AppSetPage));
            Routing.RegisterRoute(nameof(Views.CalculatorPage), typeof(Views.CalculatorPage));
            Routing.RegisterRoute(nameof(Views.DiAccountPage), typeof(Views.DiAccountPage));
            Routing.RegisterRoute(nameof(Views.EditAccountingPage), typeof(Views.EditAccountingPage));
            Routing.RegisterRoute(nameof(Views.FAQPage), typeof(Views.FAQPage));
            Routing.RegisterRoute(nameof(Views.FinancePage), typeof(Views.FinancePage));
            Routing.RegisterRoute(nameof(Views.FoodPage), typeof(Views.FoodPage));
            Routing.RegisterRoute(nameof(Views.HelpPage), typeof(Views.HelpPage));
            Routing.RegisterRoute(nameof(Views.HomePage), typeof(Views.HomePage));
            Routing.RegisterRoute(nameof(Views.IncreasePage), typeof(Views.IncreasePage));
            Routing.RegisterRoute(nameof(Views.LoginPage), typeof(Views.LoginPage));
            Routing.RegisterRoute(nameof(Views.PetMainPage), typeof(Views.PetMainPage));
            Routing.RegisterRoute(nameof(Views.RegisterPage), typeof(Views.RegisterPage));
            Routing.RegisterRoute(nameof(Views.SetPage), typeof(Views.SetPage));
            Routing.RegisterRoute(nameof(Views.ShopPage), typeof(Views.ShopPage));
            Routing.RegisterRoute(nameof(Views.SortPage), typeof(Views.SortPage));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
        }
    }
}
