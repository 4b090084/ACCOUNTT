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
    private string userId;
    public PetMainPage(FirebaseClient firebaseClient)
	{
        InitializeComponent();
        _firebaseClient = firebaseClient;
        InitializeUserData();
        UpdateAccessoryDisplay();
        UpdatePetImage();
    }

    private async Task InitializeUserData()
    {
        // 從 LoginPage 獲取用戶 ID
        userId = await SecureStorage.GetAsync("userId");
        if (string.IsNullOrEmpty(userId))
        {
            // 如果沒有用戶 ID,可能需要處理登入邏輯
            return;
        }
        await LoadUserDataFromFirebase();
    }

    private async Task LoadUserDataFromFirebase()
    {
        try
        {
            var userData = await _firebaseClient
                .Child("users")
                .Child(userId)
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
                .Child("users")
                .Child(userId)
                .PutAsync(userData);
        }
        catch (Exception ex)
        {
            await DisplayAlert("錯誤", "保存數據失敗: " + ex.Message, "確定");
        }
    }

    protected override  async void OnAppearing()
    {
        base.OnAppearing();
        await LoadUserDataFromFirebase();
        UpdateAccessoryDisplay();
        UpdateScoreDisplay();
        UpdatePetImage(); // 每次頁面出現時更新寵物圖片
    }

    // 根據積分更新寵物圖片的方法
    private  async void UpdatePetImage()
    {
        int currentScore = ScoreManager.Instance.Score;

        // 根據積分決定顯示的圖片
        if (currentScore >= 1000)
        {
            PetImage.Source = "chicken.png";
            PetImage.WidthRequest = 150;  // 可以根據需要調整大小
            PetImage.HeightRequest = 150;
        }
        else if (currentScore >= 700)
        {
            PetImage.Source = "hat.png";
            PetImage.WidthRequest = 150;
            PetImage.HeightRequest = 150;
        }
        else if (currentScore >= 400)
        {
            PetImage.Source = "key.png";
            PetImage.WidthRequest = 150;
            PetImage.HeightRequest = 150;
        }
        else if (currentScore >= 200)
        {
            PetImage.Source = "yellowhat.png";
            PetImage.WidthRequest = 150;
            PetImage.HeightRequest = 150;
        }
        else
        {
            PetImage.Source = "egg.png";
            PetImage.WidthRequest = 150;
            PetImage.HeightRequest = 150;
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

    //protected override void OnAppearing()
    //{
    //    base.OnAppearing();
    //    UpdateScoreDisplay();
    //}

    private async void UpdateScoreDisplay()
    {
        ConditionEntry.Text = $"Score: {ScoreManager.Instance.Score}";
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