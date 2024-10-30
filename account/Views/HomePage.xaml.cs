//using Windows.UI.ApplicationSettings;
namespace account.Views;
using System;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();

        string UName = Preferences.Get("UName", "�S���n�J�m�W");
        txtUName.Text = UName;

    }

    private void MonthlyExpenseClicked(object sender, EventArgs e)
    {
        DisplayChart("���X");
    }

    private void MonthlyIncomeClicked(object sender, EventArgs e)
    {
        DisplayChart("�리�J");
    }

    private void DisplayChart(string chartType)
    {
        // �M����e�Ϫ�
        ChartContainer.Content = null;

        // �o�����Ӯھ� chartType �Ыب���ܬ������Ϫ�
        // �ѩ� .NET MAUI �S�����m���Ϫ���,�z�i��ݭn�ϥβĤT��w�Φ۩w�q����
        // �H�U�Ȭ��ܨ�,������Τ����������u�ꪺ�Ϫ�
        ChartContainer.Content = new Label
        {
            Text = $"�o�����{chartType}�Ϫ�",
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };
    }

    private async void AddAccounting_Clicked(object sender, EventArgs e)
    {
        // �����O�b�������޿�
        await Shell.Current.GoToAsync("AddAccountingPage");
    }
}