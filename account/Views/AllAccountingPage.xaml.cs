namespace account.Views;
using Firebase.Database;
using Firebase.Database.Query;
using account.Models;
using System.Collections.ObjectModel;

public partial class AllAccountingPage : ContentPage
{
    //存放所有記事的資料集
    public ObservableCollection<AddAccounting> AccountingList { get; set; } = new ObservableCollection<AddAccounting>();
    string UID = Preferences.Get("UID", "");


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
        var result = await _firebaseClient.Child("AEvents/" + UID).OnceAsync<AddAccounting>();

        //逐筆將下載的記事加入記事資料集
        foreach (var item in result)
        {
            AccountingList.Add(item.Object);
            AccountingList.Last().Key = item.Key.ToString();//設定記帳資料的KEY值
        }
    
    }
    private async void AddAccounting_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("AddAccountingPage");
    }

    //當點選記帳資料時，導向到記帳修改頁面
    private async void Accounting_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count != 0)
        {
            // 取得選擇的記事
            AddAccounting accounting = (AddAccounting)e.CurrentSelection[0];

            //設定傳遞的參數
            var navigationParameter = new Dictionary<string, object> { { "AccountingSelected", accounting } };

            //開啟編輯記事頁面並傳遞參數
            await Shell.Current.GoToAsync($"EditAccountingPage", navigationParameter);

        }
    }
}