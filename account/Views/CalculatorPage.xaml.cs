namespace account.Views;

public partial class CalculatorPage : ContentPage
{
    int currentState = 1;
    string mathOperator;
    double firstNumber, secondNumber;
    public CalculatorPage()
	{
		InitializeComponent();
        OnClearClicked(this, null);
    }
    //數字按鈕
    private void OnNumberClicked(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        string pressed = button.Text;

        if (resultText.Text == "0" || currentState < 0)
        {
            resultText.Text = "";
            if (currentState < 0)
                currentState *= -1;
        }

        resultText.Text += pressed;

        double number;
        if (double.TryParse(resultText.Text, out number))
        {
            resultText.Text = number.ToString("N0");
            if (currentState == 1)
            {
                firstNumber = number;
            }
            else
            {
                secondNumber = number;
            }
        }
    }
    //操作按鈕
    private void OnOperatorClicked(object sender, EventArgs e)
    {
        currentState = -2;
        Button button = (Button)sender;
        string pressed = button.Text;
        mathOperator = pressed;
    }
    
    private void OnEqualsClicked(object sender, EventArgs e)
    {
        if (currentState == 2)
        {
            double result = 0;
            switch (mathOperator)
            {
                case "+":
                    result = firstNumber + secondNumber;
                    break;
                case "-":
                    result = firstNumber - secondNumber;
                    break;
                case "*":
                    result = firstNumber * secondNumber;
                    break;
                case "/":
                    if (secondNumber != 0)
                        result = firstNumber / secondNumber;
                    else
                        resultText.Text = "除數不能為零";
                    return;
            }

            resultText.Text = result.ToString("N0");
            firstNumber = result;
            currentState = -1;
        }
    }
//清除按鈕
    private void OnClearClicked(object sender, EventArgs e)
    {
        firstNumber = 0;
        secondNumber = 0;
        currentState = 1;
        resultText.Text = "0";
    }
}