using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    public static int CallerType; // 用于记录调用者名称
}

public class MenuManager : MonoBehaviour
{
    public void StartGameDigital()
    {
        SceneLoadManager.CallerType = 2;
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }
    public void StartGameMobile()
    {
        SceneLoadManager.CallerType = 0;
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }
    public void StartGameMatlab()
    {
        SceneLoadManager.CallerType = 1;
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }

    // 退出游戏的方法
    public void QuitGame()
    {
        // 在编辑器中也能测试退出功能
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    
    void Update()
    {
        
        // 在开始界面检测Esc键以退出游戏
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }
}