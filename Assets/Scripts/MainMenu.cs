using Photon.Pun;
using UI;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public string levelToLoad = "MainLevel";

    public SceneFader sceneFader;
    private void Awake()
    {
        PhotonNetwork.OfflineMode = true;
    }

    public void Play()
    {
        PhotonNetwork.OfflineMode = true;
        sceneFader.FadeTo(levelToLoad);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Menu()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.OfflineMode = true;
        Time.timeScale = 1f;
        sceneFader.FadeTo("mainMenu");
    }

    public void Settings()
    {
        PhotonNetwork.OfflineMode = true;
        sceneFader.FadeTo("Settings");
    }

    public void Multiplayer()
    {
        PhotonNetwork.OfflineMode = false;
        sceneFader.FadeTo("MultiplayerMenu");
    }

    public void InfiniteWaves()
    {
    }

    public void GeneratedMap()
    {
        PhotonNetwork.OfflineMode = true;
        sceneFader.FadeTo("GeneratedMap");
    }
}