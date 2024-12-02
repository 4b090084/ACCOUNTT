namespace account.Views;
using Firebase.Database;
using Firebase.Database.Query;
using account.Models;
using System.Collections.ObjectModel;


public partial class AllAccountingPage : ContentPage
{
    //�s��Ҧ��O�ƪ���ƶ�
    public ObservableCollection<AddAccounting> AccountingList { get; set; } = new ObservableCollection<AddAccounting>();

    //FIREBASE��Ʈw�s�u���ܼ�
    private readonly FirebaseClient _firebaseClient;

    public AllAccountingPage(FirebaseClient firebaseClient)
	{
		InitializeComponent();
        BindingContext = this;  //�N�ثe�����]�����ô�����ӷ�
        _firebaseClient = firebaseClient;

    }
    //�����X�{�ɡA���s���J�Ҧ��O��
    protected override async void OnAppearing()
    {
        await queryDB();

    }
        public async Task queryDB()
    {
        //�M�ťثe���O�ƶ��X
        AccountingList.Clear();

        //�qFirebase�U���Ҧ��M��
        var result = await _firebaseClient.Child("AEvents").OnceAsync<AddAccounting>();

        //�v���N�U�����O�ƥ[�J�O�Ƹ�ƶ�
        foreach (var item in result)
        {
            AccountingList.Add(item.Object);
        }
    
    }
    private async void AddAccounting_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("AddAccountingPage");
    }
}