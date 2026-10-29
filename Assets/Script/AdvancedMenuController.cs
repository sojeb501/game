using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AdvancedMenuController : MonoBehaviour
{
    [Header("UI Panels (Canvas Groups)")]
    public CanvasGroup mainMenuPanel;
    public CanvasGroup settingsPanel;
    //public CanvasGroup levelSelectPanel;
    public CanvasGroup skinsPanel;

    [Header("Settings UI")]
    public Slider volumeSlider;
    public float fadeDuration = 0.3f; // مدة التلاشي بين القوائم

    private void Start()
    {
        // إظهار القائمة الرئيسية وإخفاء باقي القوائم فوراً عند تشغيل اللعبة
        SetPanelState(mainMenuPanel, true);
        SetPanelState(settingsPanel, false);
        //SetPanelState(levelSelectPanel, false);
        SetPanelState(skinsPanel, false);

        // إعطاء قيمة مبدئية لشريط الصوت
        if (volumeSlider != null)
        {
            volumeSlider.value = AudioListener.volume;
        }
    }

    // ==========================================
    // دوال الأزرار (Navigation)
    // ==========================================

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadLevel(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void OnSettingsClicked()
    {
        StartCoroutine(FadeTransition(mainMenuPanel, settingsPanel));
    }

    //public void OnLevelSelectClicked()
    //{
    //    StartCoroutine(FadeTransition(mainMenuPanel, levelSelectPanel));
    //}

    public void OnSkinsClicked()
    {
        StartCoroutine(FadeTransition(mainMenuPanel, skinsPanel));
    }

    public void OnBackToMainClicked()
    {
        // معرفة أي لوحة مفتوحة حالياً لإخفائها والرجوع للرئيسية
        if (settingsPanel.alpha > 0) StartCoroutine(FadeTransition(settingsPanel, mainMenuPanel));
        //else if (levelSelectPanel.alpha > 0) StartCoroutine(FadeTransition(levelSelectPanel, mainMenuPanel));
        else if (skinsPanel.alpha > 0) StartCoroutine(FadeTransition(skinsPanel, mainMenuPanel));
    }

    public void QuitGame()
    {
        Debug.Log("جاري الخروج من اللعبة...");
        Application.Quit();
    }

    // ==========================================
    // نظام السكنات (Skins System)
    // ==========================================

    public void SelectSkin(int skinIndex)
    {
        // حفظ رقم السكن المختار في ذاكرة اللعبة (الدفتر المخفي)
        PlayerPrefs.SetInt("SelectedSkin", skinIndex);
        PlayerPrefs.Save();

        Debug.Log("تم حفظ السكن رقم: " + skinIndex);
    }

    // ==========================================
    // دوال الإعدادات (Settings)
    // ==========================================

    public void ChangeVolume()
    {
        if (volumeSlider != null)
        {
            AudioListener.volume = volumeSlider.value;
        }
    }

    // ==========================================
    // دوال التلاشي (Fading Logic)
    // ==========================================

    private IEnumerator FadeTransition(CanvasGroup panelToHide, CanvasGroup panelToShow)
    {
        panelToHide.interactable = false;
        panelToHide.blocksRaycasts = false;

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            panelToHide.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            yield return null;
        }
        panelToHide.alpha = 0f;

        elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            panelToShow.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            yield return null;
        }

        panelToShow.alpha = 1f;
        panelToShow.interactable = true;
        panelToShow.blocksRaycasts = true;
    }

    private void SetPanelState(CanvasGroup panel, bool isVisible)
    {
        if (panel == null) return;
        panel.alpha = isVisible ? 1f : 0f;
        panel.interactable = isVisible;
        panel.blocksRaycasts = isVisible;
    }
}