using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackBarManager : MonoBehaviour {

    public GameObject AttackBar;
    
    private List<GameObject> AttackBars;
    private List<UIChoosable> attacks;
    private int ApUsed = 0;
    private int MaxAp = 8;

	// Use this for initialization
	void Start () {
        AttackBars = new List<GameObject>();
        attacks = new List<UIChoosable>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddAttack(UIChoosable attack)
    {
        if (ApUsed < MaxAp && (ApUsed + attack.getApCost()) <= MaxAp)
        {
            GameObject attackBarUI = Instantiate(AttackBar, this.transform);
            RectTransform attackBarRT = attackBarUI.GetComponent<RectTransform>();
            attackBarRT.anchoredPosition = CalcLocation(ApUsed, attack.getApCost());
            AttackBars.Add(attackBarUI);
            attackBarUI.GetComponent<Text>().text = attack.getName();
            attackBarRT.sizeDelta = new Vector2(attackBarRT.rect.width * attack.getApCost(), attackBarRT.rect.height);
            attacks.Add(attack);
            ApUsed += attack.getApCost();

        }
    }

    public void RemoveLastAttack()
    {
        removeAttack(attacks.Count - 1);
    }

    public void RemoveFirstAttack()
    {
        removeAttack(0);
        int x = 0;
        int apUsed = 0;
        foreach (GameObject attackBarUI in AttackBars)
        {
            UIChoosable attack = attacks[x];
            RectTransform attackBarRT = attackBarUI.GetComponent<RectTransform>();
            attackBarRT.anchoredPosition = CalcLocation(apUsed, attack.getApCost());
            apUsed += attack.getApCost();
            x++;
        }
    }

    private void removeAttack(int location)
    {
        if (attacks.Count > 0)
        {
            UIChoosable toBeRemovedAttack = attacks[location];
            ApUsed -= toBeRemovedAttack.getApCost();
            attacks.RemoveAt(location);
            GameObject attackBarUI = AttackBars[location];
            AttackBars.RemoveAt(location);
            Destroy(attackBarUI);
        }
    }

    private Vector3 CalcLocation(int apUsed, int ApCost)
    {
        float x = -160 + (46 * apUsed) + 22.8f * (ApCost-1);
        return new Vector3(x, 0, 0);
    }
}
