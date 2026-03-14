namespace MyFinanceTracker.Core.Enums
{
    /// <summary>
    /// Transaction type (income or expense). Stored as string in the database.
    /// Used for both transactions and categories; amount is stored as positive, type determines balance effect.
    /// </summary>
    public enum TransactionType
    {
        Income,
        Expense
    }
}
