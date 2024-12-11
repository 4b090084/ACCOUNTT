namespace account.Views;
using Firebase.Database;
using Firebase.Database.Query;
using account.Models;
using System.Collections.ObjectModel;

public partial class AllAccountingPage : ContentPage
{
    //�s��Ҧ��O�ƪ���ƶ�
    public ObservableCollection<AddAccounting> AccountingList { get; set; } = new ObservableCollection<AddAccounting>();
    string UID = Preferences.Get("UID", "");


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
        var result = await _firebaseClient.Child("AEvents/" + UID).OnceAsync<AddAccounting>();

        //�v���N�U�����O�ƥ[�J�O�Ƹ�ƶ�
        foreach (var item in result)
        {
            AccountingList.Add(item.Object);
            AccountingList.Last().Key = item.Key.ToString();//�]�w�O�b��ƪ�KEY��
        }
    
    }
    private async void AddAccounting_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("AddAccountingPage");
    }

    //���I��O�b��ƮɡA�ɦV��O�b�קﭶ��
    private async void Accounting_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count != 0)
        {
            // ���o��ܪ��O��
            AddAccounting accounting = (AddAccounting)e.CurrentSelection[0];

            //�]�w�ǻ����Ѽ�
            var navigationParameter = new Dictionary<string, object> { { "AccountingSelected", accounting } };

            //�}�ҽs��O�ƭ����öǻ��Ѽ�
            await Shell.Current.GoToAsync($"EditAccountingPage", navigationParameter);

        }
    }
}