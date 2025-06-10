public class CurrencyChangedEventArgs
{
    public ECurrencyType CurrencyType { get; }
    public int NewValue { get; }

    public CurrencyChangedEventArgs(ECurrencyType type, int newValue)
    {
        CurrencyType = type;
        NewValue = newValue;
    }
}