namespace account.Views;
using account.Models;
using Firebase.Database;
using Firebase.Database.Query;

[QueryProperty(nameof(SelectedAccounting), "AccountingSelected")]

public partial class EditAccountingPage : ContentPage
{
    private readonly FirebaseClient _firebaseClient;
    private readonly List<string> expenseCategories = new List<string> { "��", "��", "��", "��", "�|", "��" };
    private readonly List<string> incomeCategories = new List<string> { "�~��", "����", "����" };
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

    private async void SaveClicked(object sender, EventArgs e)
    {
        try
        {
            await _firebaseClient
                .Child("AEvents/" + UID)
                .Child(AccountingEdit.Key)
                .PutAsync(AccountingEdit);
            await DisplayAlert("�ק令�\", "�O�b�ƥ�w�ק�� Firebase", "�T�w");
        }
        catch (Exception ex)
        {
            await DisplayAlert("���~", $"�L�k�O�s�� Firebase: {ex.Message}", "�T�w");

           
        }
    }

    private async void DeleteClicked(object sender, EventArgs e)
    {
            bool answer = await DisplayAlert("ĵ�i", "�T�w�n�R����?", "�T�w", "����");
            if (answer)
            {
                await _firebaseClient
                  .Child("AEvents/" + UID)
                  .Child(AccountingEdit.Key)
                  .DeleteAsync();

            //�^��O�b�C����
            await Shell.Current.GoToAsync("AllAccountingPage");
            }
    }
}