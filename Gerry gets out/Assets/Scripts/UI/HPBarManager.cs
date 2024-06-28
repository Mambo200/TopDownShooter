using Assets.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBarManager : MonoBehaviour
{
    #region Singleton
    private static HPBarManager p_Instance;
    public static HPBarManager Get { get => p_Instance; }
    #endregion

    #region private, SerializeField
    [SerializeField]
    private Image p_Healthbar;
    [SerializeField]
    private TMPro.TextMeshProUGUI p_HealthbarText;
    [SerializeField]
    private GameObject p_BaseHealthBarUI;
    #endregion

    public Color m_MaxHealthColor;
    public Color m_CurrentHealthColor;

    private PlayerController p_Player;

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
        if (p_Healthbar == null)
        {
            Debug.LogWarning("Healthbar is not set");
        }

        p_Player = EnemySpawner.Get.m_playerController;

        SetHealth();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetHealth()
    {
        // get percentage
        float maxHP = EnemySpawner.Get.m_playerController.MaxHealth;
        float currentHP = EnemySpawner.Get.m_playerController.Health;

        // get health bar fill
        float percentage = currentHP / maxHP;
        percentage = Mathf.Clamp(percentage, 0, 1);
        p_Healthbar.fillAmount = percentage;

        // set text
        if (p_HealthbarText == null)
            return;
        p_HealthbarText.text = $"{HTMLTagsHelper.StartColor(m_CurrentHealthColor)}{currentHP}{HTMLTagsHelper.EndColor()} / {HTMLTagsHelper.StartColor(m_MaxHealthColor)}{maxHP}{HTMLTagsHelper.EndColor()}";
    }

    public void Hide()
    {
        p_BaseHealthBarUI.SetActive(false);
    }
    public void Show()
    {
        p_BaseHealthBarUI.SetActive(true);
    }
}
