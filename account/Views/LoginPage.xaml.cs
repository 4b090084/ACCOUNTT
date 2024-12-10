namespace account.Views;
using Firebase.Database;
using Firebase.Database.Query;
using account.Models;
using Microsoft.Maui;
using Microsoft.Maui.Storage;

public partial class LoginPage : ContentPage
{
    private readonly FirebaseClient _firebaseClient;
    public LoginPage(FirebaseClient firebaseClient)
	{
		InitializeComponent();
        _firebaseClient = firebaseClient;
        checkLoin();
    }
    private async void checkLoin()
    {
        if (Preferences.ContainsKey("UID"))
        {
            await Shell.Current.GoToAsync("HomePage");
        }
    }
    //跳轉至註冊頁面
    private async void Register_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("RegisterPage");

    }
    //登入按鈕
    private async void Login_Clicked(object sender, EventArgs e)
    {
        string UID = UIDEntry.Text; 
        string UPwd = UPwdEntry.Text;
        if (string.IsNullOrEmpty(UID) || string.IsNullOrEmpty(UPwd))
        {
            await DisplayAlert("錯誤", "請輸入帳號和密碼", "確定");
            return;
        }
        try
        {
            var user = await _firebaseClient
                .Child("Users")
                .OrderBy("UID")
                .EqualTo(UID)
                .OnceAsync<Register>();

            bool success = false;
            Register loginUser = null;
            if (user != null)
            {
                loginUser = user.First().Object;
                if (loginUser.UPwd == UPwd)
                {
                    success = true;
                }
            }



            if (success)
            {
                Preferences.Set("Key", user.First().Key);
                Preferences.Set("UID", UID);
                Preferences.Set("UPwd", loginUser.UPwd);
                Preferences.Set("UName", loginUser.UName);
                Preferences.Set("UScore", loginUser.UScore);
                Preferences.Set("UPoint", loginUser.UPoint);
                Preferences.Set("ULevel", loginUser.ULevel);
                await DisplayAlert("成功", "登入成功", "確定");
                await Shell.Current.GoToAsync("HomePage");
                // 這裡可以導航到主頁面
            }
            else
            {
                await DisplayAlert("錯誤", "帳號或密碼錯誤", "確定");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("錯誤", $"登入失敗: {ex.Message}", "確定");
        }
    }
}