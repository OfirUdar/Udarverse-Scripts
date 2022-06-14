

namespace Udarverse.UI
{
    public static class FuncUI
    {
        public static string GetAmountClearableText(int amount)
        {
            var amountToDisplay = amount >= 1000f ? (amount / 1000f).ToString("0.0") + "K" : amount.ToString();
            return amountToDisplay;
        }

    }
}