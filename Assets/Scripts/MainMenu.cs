using UI;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public string levelToLoad = "MainLevel";

    public SceneFader sceneFader;

    public void Play()
    {
        sceneFader.FadeTo(levelToLoad);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Menu()
    {
        sceneFader.FadeTo("mainMenu");
    }

    public void Settings()
    {
        sceneFader.FadeTo("Settings");
    }

    public void Multiplayer()
    {
        sceneFader.FadeTo("MultiplayerMenu");
    }

    public void CustomMaps()
    {
        sceneFader.FadeTo("CustomMap01");
    }
}