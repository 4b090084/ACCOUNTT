namespace account.Views;
using account.Models;
using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.Maui;

public partial class AddAccountingPage : ContentPage
{
    private readonly FirebaseClient _firebaseClient;
    private readonly List<string> expenseCategories = new List<string> { "食", "衣", "住", "行", "育", "樂" };
    private readonly List<string> incomeCategories = new List<string> { "薪水", "父母", "獎金" };

    int UPoint = 0;
    public AddAccountingPage(FirebaseClient firebaseClient)
	{
		InitializeComponent();
        DatePicker.Date = DateTime.Now;  // 設置默認日期為今天
        _firebaseClient = firebaseClient;

        UPoint = Preferences.Get("UPoint", 0);
    }

    private async void SaveClicked(object sender, EventArgs e)
    {
        // 獲取用戶輸入
        string type = TypePicker.SelectedItem?.ToString();
        decimal amount;
        if (!decimal.TryParse(AmountEntry.Text, out amount))
        {
            await DisplayAlert("錯誤", "請輸入有效的金額", "確定");
            return;
        }

        DateTime date = DatePicker.Date;
        string category = CategoryPicker.SelectedItem?.ToString();
        string note = NoteEditor.Text;

        // 創建 AddAccounting 對象
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
            // 將記帳事件保存到 Firebase
            await _firebaseClient
                .Child("AEvents")
                .PostAsync(accountingEvent);
            await DisplayAlert("成功", "記帳事件已添加到 Firebase", "確定");

            //點數增加
            UPoint += 1;
            //將點數讀回資料庫
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

            // 返回上一頁
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await DisplayAlert("錯誤", $"無法保存到 Firebase: {ex.Message}", "確定");
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

        if (TypePicker.SelectedItem.ToString() == "支出")
        {
            CategoryPicker.ItemsSource = expenseCategories;
        }
        else if (TypePicker.SelectedItem.ToString() == "收入")
        {
            CategoryPicker.ItemsSource = incomeCategories;
        }

        CategoryPicker.SelectedIndex = -1;
    }
}