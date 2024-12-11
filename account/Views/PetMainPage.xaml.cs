using account.Models;
namespace account.Views;
using System;
using Microsoft.Maui.Controls;
using Firebase.Database;
using Firebase.Database.Query;

public partial class PetMainPage : ContentPage
{
    private readonly FirebaseClient _firebaseClient;
    public static object Instance { get;  set; }
    private string UID;
    private int UScore, UPoint, ULevel;

    public PetMainPage(FirebaseClient firebaseClient)
	{
        InitializeComponent();

        _firebaseClient = firebaseClient;
        UID = Preferences.Get("UID", "");
        UScore = Preferences.Get("UScore", 0);
        UPoint = Preferences.Get("UPoint", 0);
        ULevel = Preferences.Get("ULevel", 0);

        txtLevel.Text = "等级:"+ ULevel.ToString();
        txtScore.Text = "分數:" + UScore.ToString();
        txtPoint.Text = "點數:" + UPoint.ToString();
        UpdateAccessoryDisplay();
        UpdatePetImage();
    }


    private async Task LoadUserDataFromFirebase()
    {
        try
        {
            var userData = await _firebaseClient
                .Child("Users")
                .Child(UID)
                .OnceSingleAsync<UserData>();

            if (userData != null)
            {
                // 更新本地數據
                ScoreManager.Instance.Score = userData.Score;
                AccessoryManager.Instance.CurrentAccessory = userData.CurrentAccessory;
                if (userData.OwnedAccessories != null)
                {
                    foreach (var accessory in userData.OwnedAccessories)
                    {
                        AccessoryManager.Instance.AddAccessory(accessory);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("錯誤", "載入數據失敗: " + ex.Message, "確定");
        }
    }

    private async Task SaveUserDataToFirebase()
    {
        try
        {
            var userData = new UserData
            {
                Score = ScoreManager.Instance.Score,
                CurrentAccessory = AccessoryManager.Instance.CurrentAccessory,
                OwnedAccessories = AccessoryManager.Instance.GetOwnedAccessories()
            };

            await _firebaseClient
                .Child("Users")
                .Child(UID)
                .PutAsync(userData);
        }
        catch (Exception ex)
        {
            await DisplayAlert("錯誤", "保存數據失敗: " + ex.Message, "確定");
        }
    }

    protected override  async void OnAppearing()
    {
        UID = Preferences.Get("UID", "");
        UScore = Preferences.Get("UScore", 0);
        UPoint = Preferences.Get("UPoint", 0);
        ULevel = Preferences.Get("ULevel", 0);

        txtLevel.Text = "等级:" + ULevel.ToString();
        txtScore.Text = "分數:" + UScore.ToString();
        txtPoint.Text = "點數:" + UPoint.ToString();
        //base.OnAppearing();
        //await LoadUserDataFromFirebase();
        UpdateAccessoryDisplay();
        //UpdateScoreDisplay();
        UpdatePetImage(); // 每次頁面出現時更新寵物圖片
    }

    // 根據積分更新寵物圖片的方法
    private  async void UpdatePetImage()
    {
        int currentScore = ScoreManager.Instance.Score;

        // 根據積分決定顯示的圖片和升級
        if (currentScore >= 200)
        {
            // 當分數達到200時升級
            if (ULevel < 5)  // 限制最大等級為5
            {
                ULevel++;
                Preferences.Set("ULevel", ULevel);
                txtLevel.Text = "等级:" + ULevel.ToString();
            }
            PetImage.Source = "chicken.png";
            PetImage.WidthRequest = 200;
            PetImage.HeightRequest = 200;
        }
        else if (currentScore >= 180)
        {
            // 當分數達到180時升級
            if (ULevel < 4)  // 限制最大等級為4
            {
                ULevel++;
                Preferences.Set("ULevel", ULevel);
                txtLevel.Text = "等级:" + ULevel.ToString();
            }
            PetImage.Source = "eba.png";
            PetImage.WidthRequest = 200;
            PetImage.HeightRequest = 200;
        }
        else if (currentScore >= 150)
        {
            // 當分數達到150時升級
            if (ULevel < 3)  // 限制最大等級為3
            {
                ULevel++;
                Preferences.Set("ULevel", ULevel);
                txtLevel.Text = "等级:" + ULevel.ToString();
            }
            PetImage.Source = "ch.png";
            PetImage.WidthRequest = 200;
            PetImage.HeightRequest = 200;
        }
        else if (currentScore >= 100)
        {
            // 當分數達到100時升級
            if (ULevel < 2)  // 限制最大等級為2
            {
                ULevel++;
                Preferences.Set("ULevel", ULevel);
                txtLevel.Text = "等级:" + ULevel.ToString();
            }
            PetImage.Source = "ec.png";
            PetImage.WidthRequest = 200;
            PetImage.HeightRequest = 200;
        }
        else
        {
            // 初始等級
            if (ULevel < 1)  // 限制最大等級為1
            {
                ULevel = 1;
                Preferences.Set("ULevel", ULevel);
                txtLevel.Text = "等级:" + ULevel.ToString();
            }
            PetImage.Source = "egg.png";
            PetImage.WidthRequest = 200;
            PetImage.HeightRequest = 200;
        }

        // 每次更新寵物圖片時同步到 Firebase
        await SaveUserDataToFirebase();
    }


    private void UpdateAccessoryDisplay()
    {
        string currentAccessory = AccessoryManager.Instance.CurrentAccessory;
        if (!string.IsNullOrEmpty(currentAccessory))
        {
            AccessoryImage.Source = currentAccessory;
            AccessoryImage.IsVisible = true;
            // 根據寵物大小調整裝飾品大小
            AccessoryImage.WidthRequest = PetImage.WidthRequest;
            AccessoryImage.HeightRequest = PetImage.HeightRequest;
        }
        else
        {
            AccessoryImage.IsVisible = false;
        }
    }

    private async void OnAccessoryTapped(object sender, EventArgs e)
    {
        var accessories = AccessoryManager.Instance.GetOwnedAccessories();
        if (accessories.Count == 0)
        {
            await DisplayAlert("提示", "您還沒有購買任何裝飾品", "確定");
            return;
        }

        var options = new List<string>(accessories);
        options.Add("移除裝飾品");

        string selected = await DisplayActionSheet("選擇裝飾品", "取消", null, options.ToArray());

        if (selected == "移除裝飾品")
        {
            AccessoryManager.Instance.RemoveCurrentAccessory();
            AccessoryImage.IsVisible = false;
        }
        else if (selected != null && selected != "取消")
        {
            AccessoryManager.Instance.EquipAccessory(selected);
            UpdateAccessoryDisplay();
        }

        // 保存裝飾品更改到 Firebase
        await SaveUserDataToFirebase();
    }

    private async void UpdateScoreDisplay()
    {
        txtScore.Text = $"Score: {ScoreManager.Instance.Score}";
        // 每次更新分數時同步到 Firebase
        await SaveUserDataToFirebase();
    }

    private async void SettingsButton_Click(object sender, TappedEventArgs e)
    {
        // 跳轉到設置頁面的邏輯
        await Shell.Current.GoToAsync("SetPage");
    }

    private async void HelpButton_Click(object sender, TappedEventArgs e)
    {
        // 跳轉到帮助頁面的邏輯
        await Shell.Current.GoToAsync("HelpPage");
    }

    private async void HomeButton_Click(object sender, TappedEventArgs e)
    {
        // 跳轉到主頁的邏輯
        await Shell.Current.GoToAsync("HomePage");
    }

    private async void FeedButton_Click(object sender, TappedEventArgs e)
    {
        // 跳轉餵養頁面的邏輯
        await Shell.Current.GoToAsync("FoodPage");
        UpdatePetImage(); // 餵食後更新寵物圖片
        UpdateScoreDisplay(); // 更新積分顯示
    }

    private async void ShopButton_Click(object sender, TappedEventArgs e)
    {
        // 跳轉商店頁面的邏輯
        await Shell.Current.GoToAsync("ShopPage");
    }

    internal void AddItemToChicken(string v)
    {
        throw new NotImplementedException();
    }
}