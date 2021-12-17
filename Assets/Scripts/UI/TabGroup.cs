using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class TabGroup : MonoBehaviour
    {
        public List<TabButton> tabButtons;
        public List<GameObject> objectsToSwap;
        private int _selectedButtonIndex;

        public void Subscribe(TabButton btn)
        {
            if (tabButtons == null)
                tabButtons = new List<TabButton>();

            tabButtons.Add(btn);
        }

        public void OnTabSelected(TabButton tabButton)
        {
            _selectedButtonIndex = tabButton.transform.GetSiblingIndex() - 1;
            
            for (var i = 0; i < objectsToSwap.Count; i++)
            {
                objectsToSwap[i].SetActive(i == _selectedButtonIndex);
            }
        }

        private void Update()
        {
            if(Input.GetKeyDown("q") && _selectedButtonIndex > 0)
            {
                objectsToSwap[_selectedButtonIndex].SetActive(false);
                --_selectedButtonIndex;
                objectsToSwap[_selectedButtonIndex].SetActive(true);
            }
            else if(Input.GetKeyDown("e") && _selectedButtonIndex < objectsToSwap.Count - 1)
            {
                objectsToSwap[_selectedButtonIndex].SetActive(false);
                ++_selectedButtonIndex;
                objectsToSwap[_selectedButtonIndex].SetActive(true);
            }
        }
    }
}
