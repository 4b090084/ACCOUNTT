//using Windows.UI.ApplicationSettings;
namespace account.Views;
using System;
using Firebase.Database;
using Firebase.Database.Query;

public partial class HomePage : ContentPage
{
    private readonly FirebaseClient _firebaseClient;
    public HomePage(FirebaseClient firebaseClient)
	{
		InitializeComponent();
        _firebaseClient = firebaseClient;
        string UName = Preferences.Get("UName", "沒有登入姓名");
        txtUName.Text = UName;

    }

    private void MonthlyExpenseClicked(object sender, EventArgs e)
    {
        DisplayChart("月支出");
    }

    private void MonthlyIncomeClicked(object sender, EventArgs e)
    {
        DisplayChart("月收入");
    }

    private void DisplayChart(string chartType)
    {
        // 清除當前圖表
        ChartContainer.Content = null;

        // 這裡應該根據 chartType 創建並顯示相應的圖表
        // 由於 .NET MAUI 沒有內置的圖表控件,您可能需要使用第三方庫或自定義視圖
        // 以下僅為示例,實際應用中應替換為真實的圖表
        ChartContainer.Content = new Label
        {
            Text = $"這裡顯示{chartType}圖表",
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };
    }

    private async void AddAccounting_Clicked(object sender, EventArgs e)
    {
        // 跳轉到記帳頁面的邏輯
        await Shell.Current.GoToAsync("AddAccountingPage");
    }

    private async void AllAccounting_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("AllAccountingPage");
    }

    private async void LooutClicked(object sender, EventArgs e)
    {
        Preferences.Remove("UID");
        await Shell.Current.GoToAsync("LoginPage");
    }
}