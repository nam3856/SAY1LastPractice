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
        foreach (var obj in Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None))
        {
            if (obj.scene.name == "DontDestroyOnLoad")
            {
                Object.Destroy(obj);
            }
        }
    }
}