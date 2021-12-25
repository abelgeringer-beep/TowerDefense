using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public Dropdown graphicsDd;
    public Dropdown resolutionDd;
    public Dropdown antiAliasingDd;
    public Toggle window;
    public Toggle vSync;
    public Slider brightnessSlider;
    public Slider fpsSlider;

    private void Awake()
    {
        SetSettings();
    }

    private void Start()
    {
        SetResolutionDropDown();
    }

    private void SetSettings()
    {
        SetQuality(PlayerPrefs.GetInt("qualityIdx", 0));
        SetFullscreen(PlayerPrefs.GetInt("isFullscreen") == 1);
        SetResolution(PlayerPrefs.GetInt("resolutionIdx", 1));
        SetFramesPerSecond(PlayerPrefs.GetInt("fps", 30));
        SetAntiAliasing(PlayerPrefs.GetInt("antiAliasingIdx", 0));
        SetVSync(PlayerPrefs.GetInt("isVSync", 0) == 1);
        SetBrightness(PlayerPrefs.GetFloat("brightness", 1.0f));
    }

    private void SetResolutionDropDown()
    {
        if (resolutionDd != null)
            resolutionDd.ClearOptions();

        var resolutionsList = (
            from r in Screen.resolutions
            select r.width
                   + " x " + r.height
                   + " " + r.refreshRate + "Hz").ToList();
        
        resolutionDd.AddOptions(resolutionsList);
        resolutionDd.value = PlayerPrefs.GetInt("resolutionIdx", 1);

        resolutionDd.RefreshShownValue();
    }

    public void SetQuality(int idx)
    {
        QualitySettings.SetQualityLevel(idx);

        graphicsDd.value = idx;

        PlayerPrefs.SetInt("qualityIdx", idx);
    }

    public void SetVSync(bool isVSync)
    {
        QualitySettings.vSyncCount = isVSync ? 1 : 0;

        vSync.isOn = isVSync;

        PlayerPrefs.SetInt("isVSync", isVSync ? 1 : 0);
    }

    public void SetAntiAliasing(int idx)
    {
        QualitySettings.antiAliasing = idx;

        antiAliasingDd.value = idx;

        PlayerPrefs.SetInt("antiAliasingIdx", idx);
    }

    public void SetFullscreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;

        window.isOn = isFullScreen;

        PlayerPrefs.SetInt("isFullscreen", isFullScreen ? 1 : 0);
    }

    private void SetBrightness(float b)
    {
        Screen.brightness = b;

        brightnessSlider.value = b;

        PlayerPrefs.SetFloat("brightness", b);
    }

    private void SetFramesPerSecond(float f)
    {
        var fps = (int) f;

        Application.targetFrameRate = fps;

        fpsSlider.value = f;

        PlayerPrefs.SetInt("fps", fps);
    }

    public void SetResolution(int idx)
    {
        resolutionDd.value = idx;

        Screen.SetResolution(
            Screen.resolutions[idx].width,
            Screen.resolutions[idx].height,
            Screen.fullScreen);

        PlayerPrefs.SetInt("resolutionIdx", idx);
    }
}