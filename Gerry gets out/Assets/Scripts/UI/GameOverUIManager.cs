using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUIManager : MonoBehaviour
{
    #region Singleton
    private static GameOverUIManager p_Instance;
    public static GameOverUIManager Get { get => p_Instance; }
    #endregion

    [SerializeField]
    private GameObject GameOverUI;

    public bool IsGameOver { get; private set; }

    private void Awake()
    {
        if(p_Instance != null)
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
        GameOverUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitGameOver()
    {
        IsGameOver = true;
        Time.timeScale = 0f;
        ShowGameOver();
    }

    public void RevertGameOver()
    {
        IsGameOver = false;
        Time.timeScale = 1.0f;
        HideGameOver();
    }

    private void ShowGameOver()
    {
        GameOverUI.SetActive(true);
    }
    private void HideGameOver()
    {
        GameOverUI.SetActive(false);
    }
}
