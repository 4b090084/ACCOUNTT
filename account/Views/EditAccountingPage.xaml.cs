namespace account.Views;
using account.Models;
using Firebase.Database;
using Firebase.Database.Query;

[QueryProperty(nameof(SelectedAccounting), "AccountingSelected")]

public partial class EditAccountingPage : ContentPage
{
    private readonly FirebaseClient _firebaseClient;
    public AddAccounting AccountingEdit;


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

    }

    private async void SaveClicked(object sender, EventArgs e)
    {
        
    }

    private void DeleteClicked(object sender, EventArgs e)
    {

    }
}