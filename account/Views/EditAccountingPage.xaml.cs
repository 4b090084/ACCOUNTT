namespace account.Views;
using account.Models;

[QueryProperty(nameof(SelectedAccounting), "AccountingSelected")]

public partial class EditAccountingPage : ContentPage
{
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
    public EditAccountingPage()
	{
		InitializeComponent();
	}

    private void TypeChanged(object sender, EventArgs e)
    {

    }

    private void SaveClicked(object sender, EventArgs e)
    {

    }

    private void DeleteClicked(object sender, EventArgs e)
    {

    }
}