using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillCountUIManager : MonoBehaviour
{
    #region Singleton
    private static KillCountUIManager p_Instance;
    public static KillCountUIManager Get { get => p_Instance; }
    #endregion

    [SerializeField]
    private RawImage p_KillCountImage;
    [SerializeField]
    private TMPro.TextMeshProUGUI p_KillCountText;

    private void Awake()
    {
        if (p_Instance != null)
        {
            Debug.LogWarning($"There is already an instance of {nameof(HPBarManager)}, it will be deleted.");
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

    public void SetKillCountText(string _text)
    {
        p_KillCountText.text = _text;
    }
    public void SetKillCountImage(Texture _image)
    {
        p_KillCountImage.texture = _image;
    }
}
