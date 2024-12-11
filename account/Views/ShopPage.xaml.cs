namespace account.Views;
using System;
using System.Collections.ObjectModel;
using account.Models;
using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.Maui.Controls;

public partial class ShopPage : ContentPage
{
    private readonly FirebaseClient _firebaseClient;
    private Dictionary<string, int> itemPrices = new Dictionary<string, int>
        {
            { "hat.png", 10 },
            { "greenhat.png", 50 },
            { "witchhat.png", 20 },
            { "flower.png", 30 },
            { "stick.png", 40 },
            { "yellowhat.png", 60 }
        };
    private string Key, UID, UName, UPwd;
    private int UScore, UPoint, ULevel;//拿出全部資料food頁面一樣更改
    
    public ShopPage(FirebaseClient firebaseClient)
	{
        _firebaseClient = firebaseClient;
        InitializeComponent();
        SetupEventHandlers();
        Key = Preferences.Get("Key", "");
        UID = Preferences.Get("UID", "");
        UName = Preferences.Get("UName", "");
        UPwd = Preferences.Get("UPwd", "");
        UScore = Preferences.Get("UScore", 0);
        UPoint = Preferences.Get("UPoint", 0);
        ULevel = Preferences.Get("ULevel", 0);
    }

    private void SetupEventHandlers()
    {
        var images = new[] { hatImage, flowerImage, greenHatImage,
                               stickImage, witchHatImage, wreathImage };

        foreach (var image in images)
        {
            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += async (s, e) => await OnItemTapped(image);
            image.GestureRecognizers.Add(tapGesture);
        }
    }

    private async Task OnItemTapped(Image selectedImage)
    {
        string itemName = selectedImage.Source.ToString().Replace("File: ", "");
        int price = itemPrices[itemName];
        try
        {
            // 檢查是否有足夠點數
            if (UScore >= price)
            {
                // 扣除對應的分數
                UScore -= price;
                // 更新本地存儲的分數
                Preferences.Set("UScore", UScore);
                // 更新 Firebase 用戶資料
                Register currentUser = new Register();
                string key = Preferences.Get("Key", "");
                currentUser.Key = key;
                currentUser.UID = UID;
                currentUser.UName = UName;
                currentUser.UPwd = UPwd;     
                currentUser.UScore = UScore;
                currentUser.UPoint  = UPoint;
                currentUser.ULevel = ULevel;
           
                //拿出全部資料food頁面一樣更改
                await _firebaseClient
                    .Child("Users")
                    .Child(currentUser.Key)
                    .PutAsync(currentUser);  //改成修改資料庫


                // 添加換裝邏輯
                AccessoryManager.Instance.AddAccessory(itemName);

                // 儲存已購買的配件
                var purchasedAccessories = Preferences.Get("PurchasedAccessories", string.Empty);
                if (string.IsNullOrEmpty(purchasedAccessories))
                {
                    purchasedAccessories = itemName;
                }
                else
                {
                    purchasedAccessories += $",{itemName}";
                }
                Preferences.Set("PurchasedAccessories", purchasedAccessories);
                // 導航回前一頁
                await Shell.Current.GoToAsync("..");

                // 顯示兌換成功訊息
                await DisplayAlert("兌換成功", $"您已成功兌換 {itemName}\n", "確定");
            }
            else
            {
                // 顯示點數不足訊息
                await DisplayAlert("分數不足", "您的分數不足以兌換此商品", "確定");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("錯誤", $"兌換失敗: {ex.Message}", "確定");
        }
    }
}

 //if (ScoreManager.Instance.Score >= price)
        //{
        //    bool answer = await DisplayAlert("購買確認",
        //        $"是否要使用 {price} 積分購買這件物品？", "是", "否");

        //    if (answer)
        //    {
        //        ScoreManager.Instance.DeductScore(price);
        //        AccessoryManager.Instance.AddAccessory(itemName);
        //        await DisplayAlert("成功", "購買成功！", "確定");
        //    }
        //}
        //else
        //{
        //    await DisplayAlert("餘額不足",
        //        $"需要 {price} 積分才能購買這件物品", "確定");
        //}
