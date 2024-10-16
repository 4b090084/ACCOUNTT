using account.Models;
using Microsoft.Extensions.Logging;
using Firebase.Database;
using account.Views;

namespace account
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            //設定Firebase資料庫網址並連線
            builder.Services.AddSingleton(new FirebaseClient("https://chicken-7bab7-default-rtdb.firebaseio.com/"));
            //設定需要使用資料庫連線的頁面
            builder.Services.AddSingleton<RegisterPage>();
            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddSingleton<AddAccountingPage>();
            builder.Services.AddSingleton<PetMainPage>();
            builder.Services.AddSingleton<ShopPage>();
            builder.Services.AddSingleton<FoodPage>();
            //builder.Services.AddSingleton<EditNotePage>();

            return builder.Build();
        }
    }
}
