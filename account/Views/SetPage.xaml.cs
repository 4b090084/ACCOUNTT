namespace account.Views;
using account.Models;
using Microsoft.Maui.Controls;
public partial class SetPage : ContentPage
{
	public SetPage()
	{
		InitializeComponent();
	}

  

    private async void ReportIssue_Clicked(object sender, EventArgs e)
    {
        string issue = await DisplayPromptAsync("回報問題", "請描述您遇到的問題：");
        if (!string.IsNullOrEmpty(issue))
        {
            // 將問題保存到數據庫
            await DisplayAlert("謝謝", "您的問題已回報", "確定");
        }
    }

    private  async void FAQ_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new FAQPage());
    }

    private  async void Logout_Clicked(object sender, EventArgs e)
    {
        Preferences.Remove("UID");
        await Shell.Current.GoToAsync("LoginPage");
    }
}