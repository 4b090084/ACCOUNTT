﻿using account.Models;
namespace account.Views;
using System;
using Microsoft.Maui.Controls;
using Firebase.Database;
using Firebase.Database.Query;

public partial class PetMainPage : ContentPage
{
    private readonly FirebaseClient _firebaseClient;
    public static object Instance { get;  set; }
    private string Key, UID, UName, UPwd;
    private int UScore, UPoint, ULevel;
    private Register currentUser;

    public PetMainPage(FirebaseClient firebaseClient)
	{
        InitializeComponent();

        _firebaseClient = firebaseClient;
        Key = Preferences.Get("Key", "");
        UID = Preferences.Get("UID", "");
        UName = Preferences.Get("UName", "");
        UPwd = Preferences.Get("UPwd", "");
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
            Register currentUser = new Register();
            string key = Preferences.Get("Key", "");
            currentUser.Key = key;
            currentUser.UID = UID;
            currentUser.UName = UName;
            currentUser.UPwd = UPwd;
            currentUser.UScore = UScore;
            currentUser.UPoint = UPoint;
            currentUser.ULevel = ULevel;
            await _firebaseClient
                  .Child("Users")
                  .Child(currentUser.Key)
                  .PutAsync(currentUser);
        }
        catch (Exception ex)
        {
            await DisplayAlert("錯誤", "載入數據失敗: " + ex.Message, "確定");
        }
    }

    protected override  async void OnAppearing()
    {
        Key = Preferences.Get("Key", "");
        UID = Preferences.Get("UID", "");
        UName = Preferences.Get("UName", "");
        UPwd = Preferences.Get("UPwd", "");
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
        // 獲取當前分數
        int UScore = Preferences.Get("UScore", 0);

        // 檢查分數門檻並更新等級和圖片
        if (UScore >= 400)
        {
            ULevel = 5;
            UScore -= 400;  // 扣除400分
            PetImage.Source = "chicken.png";
        }
        else if (UScore >= 300)
        {
            ULevel = 4;
            UScore -= 300;  // 扣除300分
            PetImage.Source = "cba.png";
        }
        else if (UScore >= 200)
        {
            ULevel = 3;
            UScore -= 200;  // 扣除200分
            PetImage.Source = "ch.png";
        }
        else if (UScore >= 100)
        {
            ULevel = 2;
            UScore -= 100;  // 扣除100分
            PetImage.Source = "ec.png";
        }
        else
        {
            ULevel = 1;
            PetImage.Source = "egg.png";
        }

        // 更新界面顯示和保存設置
        PetImage.WidthRequest = 200;
        PetImage.HeightRequest = 200;

        Preferences.Set("ULevel", ULevel);
        Preferences.Set("UScore", UScore);
        txtLevel.Text = "等级:" + ULevel.ToString();
        txtScore.Text = "分數:" + UScore.ToString();

        // 同步到Firebase數據庫
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

    private async void OnAccessoryTapped(object sender, TappedEventArgs e)
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