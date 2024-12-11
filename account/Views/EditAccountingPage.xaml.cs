namespace account.Views;
using account.Models;
using Firebase.Database;
using Firebase.Database.Query;

[QueryProperty(nameof(SelectedAccounting), "AccountingSelected")]

public partial class EditAccountingPage : ContentPage
{
    private readonly FirebaseClient _firebaseClient;
    private readonly List<string> expenseCategories = new List<string> { "食", "衣", "住", "行", "育", "樂" };
    private readonly List<string> incomeCategories = new List<string> { "薪水", "父母", "獎金" };
    public AddAccounting AccountingEdit;
    string UID = Preferences.Get("UID", "");


    public AddAccounting SelectedAccounting
    {
        set
        {
            AccountingEdit = value;

            //TypePicker.SelectedItem = AccountingEdit.Type;
            //DatePicker.Date = AccountingEdit.Date;
            //CategoryPicker.SelectedItem = AccountingEdit.Category;
            //String Amount = AccountingEdit.Amount.ToString();
            //NoteEditor.Text = AccountingEdit.Note;

            this.BindingContext = AccountingEdit;

        }
    }
    public EditAccountingPage(FirebaseClient firebaseClient)
	{
		InitializeComponent();
        _firebaseClient = firebaseClient;
    }

    private void TypeChanged(object sender, EventArgs e)
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

    private async void SaveClicked(object sender, EventArgs e)
    {
        try
        {
            await _firebaseClient
                .Child("AEvents/" + UID)
                .Child(AccountingEdit.Key)
                .PutAsync(AccountingEdit);
            await DisplayAlert("修改成功", "記帳事件已修改到 Firebase", "確定");
        }
        catch (Exception ex)
        {
            await DisplayAlert("錯誤", $"無法保存到 Firebase: {ex.Message}", "確定");

           
        }
    }

    private async void DeleteClicked(object sender, EventArgs e)
    {
            bool answer = await DisplayAlert("警告", "確定要刪除嗎?", "確定", "取消");
            if (answer)
            {
                await _firebaseClient
                  .Child("AEvents/" + UID)
                  .Child(AccountingEdit.Key)
                  .DeleteAsync();

            //回到記帳列表頁面
            await Shell.Current.GoToAsync("AllAccountingPage");
            }
    }
}