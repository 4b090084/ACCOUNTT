namespace account.Views;
using System;
using account.Models;
using Microsoft.Maui.Controls;
using Firebase.Database;
using Firebase.Database.Query;

public partial class FoodPage : ContentPage
{
    private readonly FirebaseClient _firebaseClient;

    private int broccoliPoints = 1;
    private int carrotsPoints = 2;
    private int chestnutPoints = 3;
    private int chipsPoints = 4;
    private int kiwiPoints = 5;
    private string UID;
    private int UPoint,UScore ;
    private Register currentUser;
    public FoodPage(FirebaseClient firebaseClient)
	{ 
        _firebaseClient = firebaseClient;
		InitializeComponent();
        SetupEventHandlers();
        LoadCurrentUserData();   
        UID = Preferences.Get("UID", "");
        UPoint = Preferences.Get("UPoint", 0);   
        UScore = Preferences.Get("UScore", 0);
    }

    private void SetupEventHandlers()
    {
        Brocoli.GestureRecognizers.Add(CreateTapGestureRecognizer(BrocoliTapped));
        Carrots.GestureRecognizers.Add(CreateTapGestureRecognizer(CarrotsTapped));
        Chestnut.GestureRecognizers.Add(CreateTapGestureRecognizer(ChestnutTapped));
        Chips.GestureRecognizers.Add(CreateTapGestureRecognizer(ChipsTapped));
        Kiwi.GestureRecognizers.Add(CreateTapGestureRecognizer(KiwiTapped));
    }

    private TapGestureRecognizer CreateTapGestureRecognizer(EventHandler handler)
    {
        return new TapGestureRecognizer { Command = new Command(() => handler(this, EventArgs.Empty)) };
    }

       private async void LoadCurrentUserData()
      {
        try
        {
            string key = Preferences.Get("Key", "");

            var userData = await _firebaseClient
                .Child("Users")
                .Child(key)
                .OnceSingleAsync<UserData>();

            if (userData != null)
            {
                // 更新本地數據
                ScoreManager.Instance.Score = userData.Score;
                Preferences.Set("UPoint", UPoint);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("錯誤", "載入數據失敗: " + ex.Message, "確定");
        }
       }

    private  async void BrocoliTapped(object sender, EventArgs e)
    {
        await RedeemFoodItem("Brocoli", broccoliPoints);
    }

    private async void CarrotsTapped(object sender, EventArgs e)
    {
        await RedeemFoodItem("Carrots", carrotsPoints);
    }

    private async void ChestnutTapped(object sender, EventArgs e)
    {
        await RedeemFoodItem("Carrots", carrotsPoints);
    }

    private async void ChipsTapped(object sender, EventArgs e)
    {
        await RedeemFoodItem("Chips", chipsPoints);
    }

    private async void KiwiTapped(object sender, EventArgs e)
    {
        await RedeemFoodItem("Kiwi", kiwiPoints);
    }

    private async void UpdateScore()
    {
        await Navigation.PopAsync();
    }

    private async Task RedeemFoodItem(string itemName, int pointCost)
    {
        try
        {
            // 檢查是否有足夠點數
            if (UPoint >= pointCost)
            {
                // 扣除點數
                UPoint -= pointCost;

                // 增加對應的分數
                UScore += pointCost;

                // 更新本地存儲的點數和分數
                Preferences.Set("UPoint", UPoint);
                Preferences.Set("UScore", UScore);

                // 更新 Firebase 用戶資料
                if (currentUser != null)
                {
                    currentUser.UPoint = UPoint;
                    currentUser.UScore = UScore;
                    await Shell.Current.GoToAsync("..");
                    //await FirebaseManager.UpdateUserData(currentUser);
                }



                // 顯示兌換成功訊息
                await DisplayAlert("兌換成功", $"您已成功兌換 {itemName}\n獲得 {pointCost} 分", "確定");
            }
            else
            {
                // 顯示點數不足訊息
                await DisplayAlert("點數不足", "您的點數不足以兌換此商品", "確定");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("錯誤", $"兌換失敗: {ex.Message}", "確定");
        }
    }
}
   