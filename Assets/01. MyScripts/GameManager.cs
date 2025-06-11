using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameEventSystem Events { get; private set; } = new GameEventSystem();
    public SaveManager SaveManager { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {

        SaveManager = new SaveManager(AccountManager.Instance?.CurrentAccount.Email ?? "DefaultAccountID");
        LoadAll();
    }

    private void LoadAll()
    {
        SaveManager.LoadCurrencyData();
        SaveManager.LoadAchievementData();
        SaveManager.LoadAttendanceData();
    }
    public void SaveRequested()
    {
        SaveAll();
    }

    private void SaveAll()
    {
        SaveManager.SaveCurrencyData();
        SaveManager.SaveAchievementData();
        SaveManager.SaveAttendanceData();
    }
}