using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class LevelSelector : MonoBehaviour
    {
        public SceneFader fader;

        public GameObject ui;

        public Button[] levelButtons;

        private void Start()
        {
            var levelReached = PlayerPrefs.GetInt("levelReached", 1);

            for (var i = 0; i < levelButtons.Length; i++)
                if (i + 1 > levelReached)
                {
                    levelButtons[i].interactable = false;
                }
        }

        public void Select(string levelName)
        {
            fader.FadeTo(levelName);
        }

        public void Yes()
        {
            PlayerPrefs.DeleteAll();
            ui.SetActive(false);
            fader.FadeTo(SceneManager.GetActiveScene().name);
        }

        public void No()
        {
            ui.SetActive(false);
        }

        public void ResetLevelReached()
        {
            ui.SetActive(true);
        }
    }
}