using UnityEngine;

namespace Managers
{
    public class GameMaster : MonoBehaviour
    {
        public static bool GameIsOver;

        public GameObject gameOverUI;

        public GameObject completeLevelUI;

        private void Start()
        {
            GameIsOver = false;
        }

        private void Update()
        {
            if(GameIsOver)
                return;

            if(PlayerStats.Lives <= 0)
                EndGame();
        }

        private void EndGame()
        {
            GameIsOver = true;
            gameOverUI.SetActive(true);
        }

        public void WinLevel()
        {
            GameIsOver = true;
            completeLevelUI.SetActive(true);
        }
    }
}
