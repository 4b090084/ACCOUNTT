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
    private int UScore, UPoint, ULevel;//���X�������food�����@�˧��
    
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
            // �ˬd�O�_�������I��
            if (UScore >= price)
            {
                // ��������������
                UScore -= price;
                // ��s���a�s�x������
                Preferences.Set("UScore", UScore);
                // ��s Firebase �Τ���
                Register currentUser = new Register();
                string key = Preferences.Get("Key", "");
                currentUser.Key = key;
                currentUser.UID = UID;
                currentUser.UName = UName;
                currentUser.UPwd = UPwd;     
                currentUser.UScore = UScore;
                currentUser.UPoint  = UPoint;
                currentUser.ULevel = ULevel;
           
                //���X�������food�����@�˧��
                await _firebaseClient
                    .Child("Users")
                    .Child(currentUser.Key)
                    .PutAsync(currentUser);  //�令�ק��Ʈw


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
