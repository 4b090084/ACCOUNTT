namespace account.Views;
using Firebase.Database;
using Firebase.Database.Query;
using account.Models;
using System.Collections.ObjectModel;

public partial class RegisterPage : ContentPage
{
    //private readonly FirebaseClient _firebaseClient;
    private readonly FirebaseClient _firebaseClient = new FirebaseClient("https://chicken-7bab7-default-rtdb.firebaseio.com/");
    //public RegisterPage(FirebaseClient firebaseClient)
    public RegisterPage()
    {

		InitializeComponent();

        //_firebaseClient = firebaseClient;
    }
    // ���U���s
    private async void Register_Clicked(object sender, EventArgs e)
    {
        string UID = UIDEntry.Text;
        string UName = UNameEntry.Text;
        string UPwd = UPwdEntry.Text;


        if (string.IsNullOrEmpty(UID) || string.IsNullOrEmpty(UName) || string.IsNullOrEmpty(UPwd))
        {
            await DisplayAlert("���~", "�ж�g�Ҧ����", "�T�w");
            return;
        }

        try
        {
            var existingUser = await _firebaseClient
                .Child("Users")
                .OrderBy("UID")
                .EqualTo(UID)
                .OnceAsync<Register>();

            if (existingUser.Any())
            {
                await DisplayAlert("���~", "�ӱb���w�Q�ϥ�", "�T�w");
                return;
            }

            var newnote = _firebaseClient.Child("Users").PostAsync(new Register
            {
                UID = UIDEntry.Text,
                UName = UNameEntry.Text,
                UPwd = UPwdEntry.Text,
                UScore = 0,
                UPoint = 0,
                ULevel = 0,
            });

            await DisplayAlert("���\", "���U���\", "�T�w");
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await DisplayAlert("���~", $"���U����: {ex.Message}", "�T�w");
        }


    }
  
}