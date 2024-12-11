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
        string issue = await DisplayPromptAsync("�^�����D", "�дy�z�z�J�쪺���D�G");
        if (!string.IsNullOrEmpty(issue))
        {
            // �N���D�O�s��ƾڮw
            await DisplayAlert("����", "�z�����D�w�^��", "�T�w");
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