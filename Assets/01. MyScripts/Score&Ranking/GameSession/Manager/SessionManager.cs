using Unity.FPS.Game;
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
        EventManager.AddListener<AllObjectivesCompletedEvent>(OnClear);
        EventManager.AddListener<PlayerDeathEvent>(OnGameOver);
    }
    void OnDestroy()
    {
        EventManager.RemoveListener<AllObjectivesCompletedEvent>(OnClear);
        EventManager.RemoveListener<PlayerDeathEvent>(OnGameOver);
    }
    private void Start()
    {
        
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

    public void OnClear(AllObjectivesCompletedEvent evt)
    {
        CurrentSession.MarkCleared();
        CurrentSession.End();
        GameManager.Instance.Events.Session.RaiseSessionEnded(CurrentSession.ToDTO());
    }
    public void OnGameOver(PlayerDeathEvent evt)
    {
        CurrentSession.End();
        GameManager.Instance.Events.Session.RaiseSessionEnded(CurrentSession.ToDTO());
    }
}
