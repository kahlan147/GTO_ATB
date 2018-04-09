using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackPanelManager : MonoBehaviour {
    
    public GameObject LeftTopPanel;
    public GameObject RightTopPanel;
    public GameObject LeftBottomPanel;
    public GameObject RightBottomPanel;

    public Color chosenColor;
    public Color notChosenColor;

    private int Choice = 0;

	// Use this for initialization
	void Start () {
        ChoiceChanged(0);
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void ChoiceChanged(int changed)
    {
        Choice += changed;

        switch (Choice) {
            case 8:
            case 2:
            case 3:
                Choice = 0;
                break;
            case 9:
            case -1:
                Choice = 1;
                break;
            case -4:
            case 6:
                Choice = 4;
                break;
            case -3:
            case 7:
                Choice = 5;
                break;
        }

        GameObject chosenPanel = null;
        switch (Choice) {
            case 0:
                chosenPanel = LeftTopPanel;
                break;
            case 1:
                chosenPanel = RightTopPanel;
                break;
            case 4:
                chosenPanel = LeftBottomPanel;
                break;
            case 5:
                chosenPanel = RightBottomPanel;
                break;
        }
        LeftTopPanel.GetComponent<Image>().color = notChosenColor;
        RightTopPanel.GetComponent<Image>().color = notChosenColor;
        LeftBottomPanel.GetComponent<Image>().color = notChosenColor;
        RightBottomPanel.GetComponent<Image>().color = notChosenColor;

        chosenPanel.GetComponent<Image>().color = chosenColor;
        
    }

    public void showData(List<UIChoosable> items)
    {
        LeftTopPanel.GetComponentInChildren<Text>().text = items[0].getName();
        RightTopPanel.GetComponentInChildren<Text>().text = items[1].getName();
        if (items[2] != null)
        {
            LeftBottomPanel.GetComponentInChildren<Text>().text = items[2].getName();
        }
        else
        {
            LeftBottomPanel.GetComponentInChildren<Text>().text = "unavailable";
        }
        if (items[3] != null)
        {
            RightBottomPanel.GetComponentInChildren<Text>().text = items[3].getName();
        }
        else
        {
            RightBottomPanel.GetComponentInChildren<Text>().text = "unavailable";
        }
    }

    public int GetChoice()
    {
        switch (Choice) {
            case 0:
                if (LeftTopPanel.GetComponentInChildren<Text>().text == "unavailable")
                {
                    return -1;
                }
                break;
            case 1:
                if (RightTopPanel.GetComponentInChildren<Text>().text == "unavailable")
                {
                    return -1;
                }
                break;
            case 4:
                if (LeftBottomPanel.GetComponentInChildren<Text>().text == "unavailable")
                {
                    return -1;
                }
                break;
            case 5:
                if (RightBottomPanel.GetComponentInChildren<Text>().text == "unavailable")
                {
                    return -1;
                }
                break;
        }
        return Choice;
    }


   

}
