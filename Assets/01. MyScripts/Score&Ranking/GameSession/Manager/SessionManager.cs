using UnityEngine;

public class SessionManager : MonoBehaviour
{
    public static SessionManager Instance { get; private set; }

    public PlaySession CurrentSession { get; private set; } = new PlaySession();

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        if(CurrentSession == null)
            return;
        CurrentSession.Update(Time.deltaTime);
    }

    public PlaySessionDTO GetCurrentSessionDTO()
    {
        return CurrentSession.ToDTO();
    }

    public void OnStart()
    {
        CurrentSession.Start();
    }

    public void OnClear() => CurrentSession.MarkCleared();
    public void OnGameOver() => CurrentSession.End();
}
