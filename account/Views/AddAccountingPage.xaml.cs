namespace account.Views;
using account.Models;
using Firebase.Database;
using Firebase.Database.Query;

public partial class AddAccountingPage : ContentPage
{
    private readonly FirebaseClient _firebaseClient;
    private readonly List<string> expenseCategories = new List<string> { "��", "��", "��", "��", "�|", "��" };
    private readonly List<string> incomeCategories = new List<string> { "�~��", "����", "����" };
    public AddAccountingPage(FirebaseClient firebaseClient)
	{
		InitializeComponent();
        DatePicker.Date = DateTime.Now;  // �]�m�q�{���������
        _firebaseClient = firebaseClient;
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