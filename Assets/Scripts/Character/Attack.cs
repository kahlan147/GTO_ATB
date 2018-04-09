using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour, UIChoosable {
    
    [Header("Values")]
    [Tooltip("Negative values to heal a target")]
    public int Damage;
    public int APCost;
    public int ManaCost;
    public string AttackName;
    [Tooltip("True if the attack may only target allied characters")]
    public bool TargetAllies;

    public List<Effect.effect> effectsList;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public string getName()
    {
        return AttackName;
    }

    public int getApCost()
    {
        return APCost;
    }
}
