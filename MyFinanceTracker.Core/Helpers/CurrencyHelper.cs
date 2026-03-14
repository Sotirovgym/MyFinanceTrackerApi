using MyFinanceTracker.Core.Enums;

namespace MyFinanceTracker.Core.Helpers
{
    /// <summary>
    /// Helper for currency display (symbol, decimal places).
    /// </summary>
    public static class CurrencyHelper
    {
        public static string GetSymbol(CurrencyCode code) => code switch
        {
            CurrencyCode.USD => "$",
            CurrencyCode.EUR => "€",
            _ => code.ToString()
        };

        public static int GetDecimalPlaces(CurrencyCode code) => 2;
    }
}
