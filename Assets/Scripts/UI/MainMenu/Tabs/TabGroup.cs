using System.Collections.Generic;
using UnityEngine;

namespace root
{
    public class TabGroup : MonoBehaviour
    {
        private List<TabButton> _tabButtonsList;
        private TabButton _selectedTab;
        private List<GameObject> _gameObjectsToSwap;

        private void Update()
        {
            ResetTabs();
        }

        public void Subscribe(TabButton button)
        {
            if (_tabButtonsList == null)
            {
                _tabButtonsList = new List<TabButton>();
            }
            _tabButtonsList.Add(button);
        }

        public void OnTabEnter(TabButton button)
        {
            ResetTabs();
            if (_selectedTab == null || button != _selectedTab)
            {
                button.background.color = Color.white;
            }
        }

        public void OnTabExit(TabButton button)
        {
            ResetTabs();
        }

        public void OnTabSelected(TabButton button)
        {
            ResetTabs();
            _selectedTab = button;
            int index = button.transform.GetSiblingIndex();
            for (int i = 0; i< _gameObjectsToSwap.Count; i++)
            {
                if (i == index)
                {
                    _gameObjectsToSwap[i].SetActive(true);
                }
                else
                {
                    _gameObjectsToSwap[i].SetActive(false);
                }
            }
        

        }

        public void ResetTabs()
        {
            foreach (TabButton button in _tabButtonsList)
            {
                if (_selectedTab != null && button == _selectedTab)
                {
                    continue;
                }

                button.background.color = Color.white; // idle 
            }
        }
    
    
    
    }
}