using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager p_Instance;
    public static GameManager Get { get => p_Instance; }
    #endregion

    [SerializeField]
    private string p_PlaySceneName;
    [SerializeField]
    private string p_MainMenuSceneName;

    private void Awake()
    {
        if (p_Instance != null)
        {
            Debug.LogWarning($"There is already an instance of {nameof(GameOverUIManager)}, it will be deleted.");
            Destroy(this.gameObject);
            return;
        }
        p_Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadGameScene()
    {
        ResetEverything();
        SceneManager.LoadScene(p_PlaySceneName, LoadSceneMode.Single);
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(p_MainMenuSceneName, LoadSceneMode.Single);
    }

    private void ResetEverything()
    {
        Time.timeScale = 1.0f;
        GameOverUIManager.Get.RevertGameOver();
    }
}
