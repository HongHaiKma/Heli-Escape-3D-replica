using System.Collections;
using System.Collections.Generic;
using EazyEngine.UI;
using UnityEngine;

public class EazyGroupTabNGUI : MonoBehaviour {
    [SerializeField]
    private List<EazyTabNGUI> groupTab;
    [SerializeField]
    private List<Transform> groupLayer;
    [SerializeField]
    public UnityEventInt onChooseIndex;
    int currentTab;
    [SerializeField] private bool AtleastOneTabChoosed;
    public int CurrentTab
    {
        get
        {
            return currentTab;
        }

        set
        {
            currentTab = value;
        }
    }

    public List<Transform> GroupLayer
    {
        get
        {
            return groupLayer;
        }
    }

    public List<EazyTabNGUI> GroupTab
    {
        get
        {
            if(groupTab == null)
            {
                groupTab = new List<EazyTabNGUI>();
            }
            return groupTab;
        }
    }
    
    public bool isLockOnEnable = false;
    public bool performOnEnable = true;
    public bool refreshPageOnEnable = false;
    bool isFirst = true;
    private void OnEnable()
    {
        reloadTabs();
        if (AtleastOneTabChoosed)
        {
            if (!isFirst && !isLockOnEnable)
            {
                if (currentTab != 0 && !refreshPageOnEnable ) return;
                changeTab(0,performOnEnable);
            }
        }else if (refreshPageOnEnable)
        {
            foreach (var tab in GroupTab)
            {
                tab.Pressed = false;
            }
        }
    }
    // Use this for initialization
     public virtual void  Start () {
        isFirst = false;
        reloadTabs();
        if (currentTab != 0) return;
        if (AtleastOneTabChoosed)
        {
            changeTab(0,performOnEnable);
        }
     }


    public void reloadTabs()
    {
        for (int i = 0; i < GroupTab.Count; i++)
        {
            GroupTab[i].Index = i;
            GroupTab[i].ParentTab = this;
          //  GroupTab[i].UnregisterClick();
          //  GroupTab[i].RegisterClickTab();
        }
    }

    public void changeTab(int index,bool performAction = true)
    {
        if (onChooseIndex != null && performAction)
        {
            onChooseIndex.Invoke(index);
        }
        CurrentTab = index;
       for (int i = 0; i < GroupLayer.Count; i++)
        {
            if (GroupLayer[i] != null && i != index)
            {
                if (GroupLayer[i].gameObject.activeSelf)
                {
                    var uielement = GroupLayer[i].gameObject.GetComponent<UIElement>();
                    if (uielement)
                    {
                        uielement.close();
                        continue;
                    }
                }
                GroupLayer[i].gameObject.SetActive(false);
            }
        }
        if (GroupLayer.Count > 0 && index < GroupLayer.Count)
        {
            if (GroupLayer[index] != null)
            {
                var uielement = GroupLayer[index].gameObject.GetComponent<UIElement>();
                if (uielement)
                {
                    uielement.show();
                }
                else
                {
                    GroupLayer[index].gameObject.SetActive(true);
                }
              
            }
        }
        for (int i = 0; i < GroupTab.Count; i++)
        {
            if (i == index)
            {
                GroupTab[i].Pressed = (true);
            }
            else
            {
                GroupTab[i].Pressed = (false);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
     
	}
}
