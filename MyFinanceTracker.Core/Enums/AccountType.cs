namespace MyFinanceTracker.Core.Enums
{
    /// <summary>
    /// Type of account (where money is held). Stored as string in the database.
    /// </summary>
    public enum AccountType
    {
        Checking,
        Savings,
        CreditCard,
        Cash
    }
}
