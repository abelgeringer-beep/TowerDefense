using UnityEngine;

namespace UI
{
    public class CompleteLevel : MonoBehaviour
    {
        public SceneFader sceneFader;

        public string nextLevel = "Level02";
        public int levelToUnlock = 2;

        public string menuSceneName = "mainMenu";

        public void Continue()
        {
            PlayerPrefs.SetInt("levelReached", levelToUnlock);
            sceneFader.FadeTo(nextLevel);
        }

        public void Menu()
        {
            sceneFader.FadeTo(menuSceneName);
        }
    }
}