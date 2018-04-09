using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, UIChoosable {

    public string CharacterName;
    public Attack attack1;
    public Attack attack2;
    public Attack attack3;
    public Attack attack4;

    private List<UIChoosable> attacks;

	// Use this for initialization
	void Start () {
        attacks = new List<UIChoosable>();
        attacks.Add(attack1);
        attacks.Add(attack2);
        attacks.Add(attack3);
        attacks.Add(attack4);
	}

    // Update is called once per frame
    void Update () {
		
	}

    public string getName()
    {
        return CharacterName;
    }

    public List<UIChoosable> GetAttacks()
    {
        return attacks;
    }

    public int getApCost()
    {
        return 2;
    }
}
