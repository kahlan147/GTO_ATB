using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {

    public int MaxHealth;
    public Text HealthText;

    private int CurrentHealth;

	// Use this for initialization
	void Start () {
        this.CurrentHealth = MaxHealth;	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool TakeDamage(int Damage)
    {
        CurrentHealth -= Damage;
        if (CurrentHealth <= 0)
        {
            return true;
        }
        HealthText.text = "Health: " + CurrentHealth + " / " + MaxHealth + ".";
        return false;
    }
}
