using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }
    public Slider slider;
    public TextMeshProUGUI loadingText;
    public GameObject loadingPanel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }

        Destroy(gameObject);
    }

    public void ResetScreen()
    {
        loadingPanel.SetActive(true);
        loadingText.text = "0%";
        slider.value = 0;
    }

    public void SetLoadingValue(float loadingValue)
    {
        loadingText.text = (int)(loadingValue * 100) + " %";
        slider.value = loadingValue;
    }

    public void HideLoadingScreen()
    {
        loadingPanel.SetActive(false); 
    }
}
