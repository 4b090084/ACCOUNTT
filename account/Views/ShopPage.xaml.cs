namespace account.Views;
using System;
using System.Collections.ObjectModel;
using account.Models;
using Microsoft.Maui.Controls;

public partial class ShopPage : ContentPage
{
    private Dictionary<string, int> itemPrices = new Dictionary<string, int>
        {
            { "hat.png", 100 },
            { "greenhat.png", 150 },
            { "witchhat.png", 200 },
            { "flower.png", 300 },
            { "stick.png", 400 },
            { "yellowhat.png", 450 }
        };
    public ShopPage()
	{
		InitializeComponent();
        SetupEventHandlers();
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

        if (ScoreManager.Instance.Score >= price)
        {
            bool answer = await DisplayAlert("購買確認",
                $"是否要使用 {price} 積分購買這件物品？", "是", "否");

            if (answer)
            {
                ScoreManager.Instance.DeductScore(price);
                AccessoryManager.Instance.AddAccessory(itemName);
                await DisplayAlert("成功", "購買成功！", "確定");
            }
        }
        else
        {
            await DisplayAlert("餘額不足",
                $"需要 {price} 積分才能購買這件物品", "確定");
        }
    }
}


