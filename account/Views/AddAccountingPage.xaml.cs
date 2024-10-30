namespace account.Views;
using account.Models;
using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.Maui;

public partial class AddAccountingPage : ContentPage
{
    private readonly FirebaseClient _firebaseClient;
    private readonly List<string> expenseCategories = new List<string> { "��", "��", "��", "��", "�|", "��" };
    private readonly List<string> incomeCategories = new List<string> { "�~��", "����", "����" };

    int UPoint = 0;
    public AddAccountingPage(FirebaseClient firebaseClient)
	{
		InitializeComponent();
        DatePicker.Date = DateTime.Now;  // �]�m�q�{���������
        _firebaseClient = firebaseClient;

        UPoint = Preferences.Get("UPoint", 0);
    }

    private async void SaveClicked(object sender, EventArgs e)
    {
        // ����Τ��J
        string type = TypePicker.SelectedItem?.ToString();
        decimal amount;
        if (!decimal.TryParse(AmountEntry.Text, out amount))
        {
            await DisplayAlert("���~", "�п�J���Ī����B", "�T�w");
            return;
        }

        DateTime date = DatePicker.Date;
        string category = CategoryPicker.SelectedItem?.ToString();
        string note = NoteEditor.Text;

        // �Ы� AddAccounting ��H
        var accountingEvent = new AddAccounting
        {
            Type = type,
            Amount = amount,
            Date = date,
            Category = category,
            Note = note
        };

        try
        {
            // �N�O�b�ƥ�O�s�� Firebase
            await _firebaseClient
                .Child("AEvents")
                .PostAsync(accountingEvent);
            await DisplayAlert("���\", "�O�b�ƥ�w�K�[�� Firebase", "�T�w");

            //�I�ƼW�[
            UPoint += 1;
            //�N�I��Ū�^��Ʈw
            Preferences.Set("UPoint", UPoint);

            string Key = Preferences.Get("Key", "");
            await _firebaseClient
              .Child("Users")
              .Child(Key)
              .PutAsync(new Register
              {
                  UID = Preferences.Get("UID",""),
                  UName = Preferences.Get("UName", ""),
                  UPwd = Preferences.Get("UPwd", ""),
                  UScore = Preferences.Get("UScore", 0),
                  UPoint = Preferences.Get("UPoint", 0), 
                  ULevel = Preferences.Get("ULevel",0),
              });

            // ��^�W�@��
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await DisplayAlert("���~", $"�L�k�O�s�� Firebase: {ex.Message}", "�T�w");
        }
    }

    private async void TypeChanged(object sender, EventArgs e)
    {
        if (TypePicker.SelectedIndex == -1)
        {
            CategoryPicker.IsEnabled = false;
            CategoryPicker.ItemsSource = null;
            return;
        }

        CategoryPicker.IsEnabled = true;

        if (TypePicker.SelectedItem.ToString() == "��X")
        {
            CategoryPicker.ItemsSource = expenseCategories;
        }
        else if (TypePicker.SelectedItem.ToString() == "���J")
        {
            CategoryPicker.ItemsSource = incomeCategories;
        }

        CategoryPicker.SelectedIndex = -1;
    }
}