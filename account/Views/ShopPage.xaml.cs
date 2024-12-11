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
            { "yellowhat.png", 1 }
        };
    private string UID;
    private int UScore;
    private Register currentUser;
    public ShopPage(FirebaseClient firebaseClient)
	{
        _firebaseClient = firebaseClient;
        InitializeComponent();
        SetupEventHandlers();
        UID = Preferences.Get("UID", "");
        UScore = Preferences.Get("UScore", 0);
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
            // �ˬd�O�_�������I��
            if (UScore >= price)
            {
                // ��������������
                UScore -= price;
                // ��s���a�s�x������
                Preferences.Set("UScore", UScore);
                // ��s Firebase �Τ���
                if (currentUser != null)
                {
                    currentUser.UScore = UScore;
                    await _firebaseClient
                        .Child("Users")
                        .Child(currentUser.UID)
                        .PutAsync(currentUser);
                }

                // �K�[�����޿�
                AccessoryManager.Instance.AddAccessory(itemName);

                // �x�s�w�ʶR���t��
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
                // �ɯ�^�e�@��
                await Shell.Current.GoToAsync("..");

                // ��ܧI�����\�T��
                await DisplayAlert("�I�����\", $"�z�w���\�I�� {itemName}\n", "�T�w");
            }
            else
            {
                // ����I�Ƥ����T��
                await DisplayAlert("���Ƥ���", "�z�����Ƥ����H�I�����ӫ~", "�T�w");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("���~", $"�I������: {ex.Message}", "�T�w");
        }
    }
}

 //if (ScoreManager.Instance.Score >= price)
        //{
        //    bool answer = await DisplayAlert("�ʶR�T�{",
        //        $"�O�_�n�ϥ� {price} �n���ʶR�o�󪫫~�H", "�O", "�_");

        //    if (answer)
        //    {
        //        ScoreManager.Instance.DeductScore(price);
        //        AccessoryManager.Instance.AddAccessory(itemName);
        //        await DisplayAlert("���\", "�ʶR���\�I", "�T�w");
        //    }
        //}
        //else
        //{
        //    await DisplayAlert("�l�B����",
        //        $"�ݭn {price} �n���~���ʶR�o�󪫫~", "�T�w");
        //}
