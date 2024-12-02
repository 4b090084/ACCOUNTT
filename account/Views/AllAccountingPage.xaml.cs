namespace account.Views;
using Firebase.Database;
using Firebase.Database.Query;
using account.Models;
using System.Collections.ObjectModel;


public partial class AllAccountingPage : ContentPage
{
    //存放所有記事的資料集
    public ObservableCollection<AddAccounting> AccountingList { get; set; } = new ObservableCollection<AddAccounting>();

    //FIREBASE資料庫連線的變數
    private readonly FirebaseClient _firebaseClient;

    public AllAccountingPage(FirebaseClient firebaseClient)
	{
		InitializeComponent();
        BindingContext = this;  //將目前頁面設為資料繫結的來源
        _firebaseClient = firebaseClient;

    }
    //當頁面出現時，重新載入所有記事
    protected override async void OnAppearing()
    {
        await queryDB();

    }
        public async Task queryDB()
    {
        //清空目前的記事集合
        AccountingList.Clear();

        //從Firebase下載所有清單
        var result = await _firebaseClient.Child("AEvents").OnceAsync<AddAccounting>();

        //逐筆將下載的記事加入記事資料集
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