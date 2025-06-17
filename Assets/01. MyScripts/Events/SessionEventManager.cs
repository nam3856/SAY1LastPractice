using System;

public class SessionEventManager
{
    public event Action OnSessionStarted;
    public event Action<PlaySessionDTO> OnSessionEnded;

    public void RaiseSessionStarted()
    {
        OnSessionStarted?.Invoke();
    }
    public void RaiseSessionEnded(PlaySessionDTO dto)
    {
        OnSessionEnded?.Invoke(dto);
    }
}