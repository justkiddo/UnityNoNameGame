using System.Collections.Generic;
using UnityEngine;

namespace root
{
    public class TabGroup : MonoBehaviour
    {
        public List<TabButton> tabButtonsList;
        public TabButton selectedTab;
        public List<GameObject> gameObjectsToSwap;

        private void Update()
        {
            ResetTabs();
        }

        public void Subscribe(TabButton button)
        {
            if (tabButtonsList == null)
            {
                tabButtonsList = new List<TabButton>();
            }
            tabButtonsList.Add(button);
        }

        public void OnTabEnter(TabButton button)
        {
            ResetTabs();
            if (selectedTab == null || button != selectedTab)
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
            selectedTab = button;
            int index = button.transform.GetSiblingIndex();
            for (int i = 0; i< gameObjectsToSwap.Count; i++)
            {
                if (i == index)
                {
                    gameObjectsToSwap[i].SetActive(true);
                }
                else
                {
                    gameObjectsToSwap[i].SetActive(false);
                }
            }
        

        }

        public void ResetTabs()
        {
            foreach (TabButton button in tabButtonsList)
            {
                if (selectedTab != null && button == selectedTab)
                {
                    continue;
                }

                button.background.color = Color.white; // idle 
            }
        }
    
    
    
    }
}