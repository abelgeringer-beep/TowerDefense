using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class PausedMenu : MonoBehaviour
    {
        public GameObject ui;

        public string menuSceneName = "mainMenu";

        public SceneFader sceneFader;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
                Toggle();
        }

        public void Toggle()
        {
            ui.SetActive(!ui.activeSelf);
            Time.timeScale = ui.activeSelf ? 0f : 1f;
        }
        [PunRPC]

        public void Menu()
        {
            Toggle();
            sceneFader.FadeTo(menuSceneName);
        }

        public void Restart()
        {
            Toggle();
            sceneFader.FadeTo(SceneManager.GetActiveScene().name);
        }
    }
}