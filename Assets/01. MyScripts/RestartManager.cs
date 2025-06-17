using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.FPS.Game;

public static class RestartManager
{
    public static void Restart(string sceneName)
    {
        EventManager.Clear();
        SceneManager.LoadScene(sceneName);
        DestroySingletons();
    }

    public static void DestroySingletons()
    {
        foreach (var obj in Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None))
        {
            if (obj.scene.name == "DontDestroyOnLoad")
            {
                Object.Destroy(obj);
            }
        }
    }
}