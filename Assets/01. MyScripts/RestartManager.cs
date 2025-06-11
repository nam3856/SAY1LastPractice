using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.FPS.Game;

public static class RestartManager
{
    public static void Restart(string sceneName)
    {
        DestroySingletons();
        EventManager.Clear();
        SceneManager.LoadScene(sceneName);
    }

    public static void DestroySingletons()
    {
        if (GameManager.Instance != null)
            Object.Destroy(GameManager.Instance.gameObject);

        if (CurrencyManager.Instance != null)
            Object.Destroy(CurrencyManager.Instance.gameObject);

        if (AchievementManager.Instance != null)
            Object.Destroy(AchievementManager.Instance.gameObject);
    }
}