using System;

public class CurrencyEventManager
{
    public event Action<CurrencyChangedEventArgs> OnCurrencyChanged;

    public void RaiseCurrencyChanged(ECurrencyType type, int newValue)
    {
        OnCurrencyChanged?.Invoke(new CurrencyChangedEventArgs(type, newValue));
    }
}
