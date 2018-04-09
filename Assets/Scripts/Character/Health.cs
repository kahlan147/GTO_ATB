using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    public int MaxHealth;
    private int CurrentHealth;

	// Use this for initialization
	void Start () {
        this.CurrentHealth = MaxHealth;	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TakeDamage(int Damage)
    {
        CurrentHealth -= Damage;
        if (CurrentHealth <= 0)
        {
            //Do dieing stuff. 
        }
    }
}
