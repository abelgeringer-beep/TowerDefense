using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class GameOver : MonoBehaviour
    {
        public SceneFader sceneFader;

        public string menuSceneName = "mainMenu";

        public void Awake()
        {
            sceneFader.gameObject.SetActive(true);
        }

        public void Retry()
        {
            sceneFader.FadeTo(SceneManager.GetActiveScene().name);
        }

        public void Menu()
        {
            sceneFader.FadeTo(menuSceneName);
        }
    }
}