using UnityEngine;

namespace Managers
{
    public class GamePreferencesManager : MonoBehaviour
    {
        private void Start()
        {
            LoadPrefs();
        }

        private void OnApplicationQuit()
        {
            SavePrefs();
        }

        private void SavePrefs()
        {

        }

        private void LoadPrefs()
        {

        }
    }
}
