using UnityEngine;

public class SimpleSceneSwitcher : MonoBehaviour
{
    public void ChangeScene(string _sceneName)
    {
        LevelManager.Instance.ChangeScene(_sceneName);
    }
    public void AddScene(string _sceneName)
    {
        LevelManager.Instance.AddScene(_sceneName);
    }
}
