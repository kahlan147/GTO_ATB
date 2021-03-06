﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour {
    [System.Serializable]
    public struct Player
    {
        public Character ActiveCharacter;
        public Character ToBeActiveCharacter;
        public float Charge;
        public bool isAttacking;
        public Transform MeleeLocation;
        
    }

    public Player player1;
    public Player player2;

    public List<Character> characters;

    public AttackPanelManager P1AttackPanel;
    public AttackPanelManager P2AttackPanel;

    public Slider P1ChargeBar;
    public Slider P2ChargeBar;

    public AttackBarManager P1AttackBarManager;
    public AttackBarManager P2AttackBarManager;

    public Health enemyHealth;
    public Health PlayersHealth;

    public GameplaySwitcher switcher;

    //------------------------------------------------------------------------------------------------
    
    private int AmountOfCharges = 8;

    private bool[] P1Charges;
    private bool[] P2Charges;

    private bool P1CharSelect = false;
    private bool P2CharSelect = false;

    private bool CombatTriggered;

    // Use this for initialization
    void Start () {
        
        P1Charges = new bool[AmountOfCharges];
        P2Charges = new bool[AmountOfCharges];

        P1ChargeBar.gameObject.SetActive(false);
        P2ChargeBar.gameObject.SetActive(false);

        player1.isAttacking = false;
        player2.isAttacking = false;
        player1.Charge = 0;
        player2.Charge = 0;

    }

    private void OnEnable()
    {
        GameplaySwitcher.CombatTriggered += TriggerCombat;
    }

    // Update is called once per frame
    void Update (){
        if (CombatTriggered)
        {
            float P1ChargeSpeed = player1.ActiveCharacter.ChargeSpeed;
            float P2ChargeSpeed = player2.ActiveCharacter.ChargeSpeed;

            if (P1AttackBarManager.getApUsed() >= 4 && player1.ToBeActiveCharacter == null && !player1.isAttacking)
            {
                P1AttackPanel.showData(getListOfChooseAbleCharacters());
                P1CharSelect = true;
                if (Input.GetButtonDown("P1Revert")) { P1Revert(); }
            }

            if (P2AttackBarManager.getApUsed() >= 4 && player2.ToBeActiveCharacter == null && !player2.isAttacking)
            {
                P2AttackPanel.showData(getListOfChooseAbleCharacters());
                P2CharSelect = true;
                if (Input.GetButtonDown("P2Revert")){    P2Revert();     }
            }

            if (Input.GetButtonDown("P1Revert")){    P1Revert();     }
            if (Input.GetButtonDown("P2Revert")){    P2Revert();     }

            if (player1.isAttacking)
            {
                P1ChargeSpeed = 0;
            }
            Charge(ref P1ChargeBar, ref P1Charges, ref player1.Charge, P1ChargeSpeed);

            if (player2.isAttacking)
            {
                P2ChargeSpeed = 0;
            }
            Charge(ref P2ChargeBar, ref P2Charges, ref player2.Charge, P2ChargeSpeed);

            checkControls(1);
            checkControls(2);
        }
    }

    private void ShowAttacksForCharacters()
    {
        P1AttackPanel.showData(player1.ActiveCharacter.GetAttacks());
        P2AttackPanel.showData(player2.ActiveCharacter.GetAttacks());
    }

    private void Charge(ref Slider ChargeBar, ref bool[] Charges, ref float Charge, float ChargeSpeed)
    {
        if (Charge < 1)
        {
            Charge += ChargeSpeed * Time.deltaTime;
            if (Charge > 1)
            {
                Charge = 1;
            }
            ChargeBar.value = Charge;

            CalcCharges(Charge, Charges);
        }
    }

    private void CalcCharges(float currentCharge, bool[] Charges)
    {
        int amount = numberOfBarsCharged(Charges);        
        if (currentCharge >= (1.0f / AmountOfCharges) * amount + (1.0f / AmountOfCharges))
        {
            if (amount < Charges.Length) //neccesary to avoid array out of bounds exception
            {
                Charges[amount] = true;
            }
        }
    }

    private int numberOfBarsCharged(bool[] Charges) {
        int amount = 0;
        foreach (bool check in Charges)
        {
            if (check)
            {
                amount++;
            }
        }
        return amount;

    }

    private void checkControls(int PlayerNumber)
    {
        AttackPanelManager attackPanel = null;
        switch (PlayerNumber) {
            case 1:
                attackPanel = P1AttackPanel;
                break;
            case 2:
                attackPanel = P2AttackPanel;
                break;
        }

        if (attackPanel != null)
        {
            //Vertical movement
            if (Input.GetButtonDown("P" + PlayerNumber + "Vertical"))
            {
                int newPosition = 4;
                if (Input.GetAxis("P" + PlayerNumber + "Vertical") < 0)
                {
                    newPosition -= (newPosition * 2);
                }
                attackPanel.ChoiceChanged(newPosition);
            }

            //Horizontal movement
            if (Input.GetButtonDown("P" + PlayerNumber + "Horizontal"))
            {
                int newPosition = 1;
                if (Input.GetAxis("P" + PlayerNumber + "Horizontal") < 0)
                {
                    newPosition -= (newPosition * 2);
                }
                attackPanel.ChoiceChanged(newPosition);
            }

            //Choose controls
            if (Input.GetButtonDown("P" + PlayerNumber + "Choose")){
                int choice = -1;
                switch (PlayerNumber){
                    case 1:
                        choice = P1AttackPanel.GetChoice();
                        if (choice != -1){
                            UIChoosable uiChoosable = null;
                            if (P1CharSelect){
                                if (player1.ToBeActiveCharacter == null)
                                {
                                    uiChoosable = GetCharacterByChoice(choice);
                                    player1.ToBeActiveCharacter = (Character)uiChoosable;
                                    P1AttackPanel.showData(player1.ToBeActiveCharacter.GetAttacks());
                                    P1CharSelect = false;
                                }
                            }
                            else{

                                if (player1.ToBeActiveCharacter != null)
                                {
                                    uiChoosable = GetAttackByChoice(player1.ToBeActiveCharacter, choice);
                                }
                                else
                                {
                                    uiChoosable = GetAttackByChoice(player1.ActiveCharacter, choice);
                                }
                            }
                            if (uiChoosable != null)
                            {
                                P1AttackBarManager.AddAttack(uiChoosable);
                            }
                        }
                        break;
                    case 2:
                        choice = P2AttackPanel.GetChoice();
                        if (choice != -1)
                        {
                            UIChoosable uiChoosable = null;
                            if (P2CharSelect)
                            {
                                if (player2.ToBeActiveCharacter == null)
                                {
                                    uiChoosable = GetCharacterByChoice(choice);
                                    player2.ToBeActiveCharacter = (Character)uiChoosable;
                                    P2AttackPanel.showData(player2.ToBeActiveCharacter.GetAttacks());
                                    P2CharSelect = false;
                                }
                            }
                            else
                            {

                                if (player2.ToBeActiveCharacter != null)
                                {
                                    uiChoosable = GetAttackByChoice(player2.ToBeActiveCharacter, choice);
                                }
                                else
                                {
                                    uiChoosable = GetAttackByChoice(player2.ActiveCharacter, choice);
                                }
                            }
                            if (uiChoosable != null)
                            {
                                P2AttackBarManager.AddAttack(uiChoosable);
                            }
                        }
                        break;
                }
            }

            //Release
            if (Input.GetButtonDown("P" + PlayerNumber + "Release"))
            {
                switch (PlayerNumber)
                {
                    case 1:
                        if (!player1.isAttacking && player1.Charge >= 1)
                        {
                            StartCoroutine(P1Attack());
                        }
                        break;
                    case 2:
                        if (!player2.isAttacking && player2.Charge >= 1)
                        {
                            StartCoroutine(P2Attack());
                        }
                        break;
                }
            }

            //Switch
            if (Input.GetButtonDown("P" + PlayerNumber + "Switch"))
            {
                switch (PlayerNumber) {
                    case 1:
                        if (player1.ToBeActiveCharacter == null)
                        {
                            P1AttackPanel.showData(getListOfChooseAbleCharacters());
                            P1CharSelect = true;
                        }
                        break;
                    case 2:
                        if (player2.ToBeActiveCharacter == null)
                        {
                            P2AttackPanel.showData(getListOfChooseAbleCharacters());
                            P2CharSelect = true;
                        }
                        break;
                }
            }
        }
    }

    private void P1Revert()
    {
        if (P1CharSelect)
        {
            P1CharSelect = false;
            P1AttackPanel.showData(player1.ActiveCharacter.GetAttacks());
            player1.ToBeActiveCharacter = null;
        }
        else
        {
            List<UIChoosable> choosables = P1AttackBarManager.getAttacks();
            if (choosables[choosables.Count - 1] is Character)
            {
                P1AttackPanel.showData(player1.ActiveCharacter.GetAttacks());
                player1.ToBeActiveCharacter = null;
            }
            P1AttackBarManager.RemoveLastAttack();
        }
    }

    private void P2Revert()
    {
        if (P2CharSelect)
        {
            P2CharSelect = false;
            P2AttackPanel.showData(player2.ActiveCharacter.GetAttacks());
            player2.ToBeActiveCharacter = null;
        }
        else
        {
            List<UIChoosable> choosables = P2AttackBarManager.getAttacks();
            if (choosables[choosables.Count - 1] is Character)
            {
                P2AttackPanel.showData(player2.ActiveCharacter.GetAttacks());
                player2.ToBeActiveCharacter = null;
            }
            P2AttackBarManager.RemoveLastAttack();
        }
    }

    private Attack GetAttackByChoice(Character character, int choice)
    {
        List<Attack> attacks = character.GetAttacks().ConvertAll(o => (Attack)o);
        int which = -1;
        switch (choice) {
            case 0:
            case 1:
                which = choice;
                break;
            case 4:
                which = 2;
                break;
            case 5:
                which = 3;
                break;
            case -1:
                return null;
                break;
        }
        return attacks[which];
    }

    private Character GetCharacterByChoice(int choice)
    {
        List<Character> ChooseAbleCharacters = getListOfChooseAbleCharacters().ConvertAll(o => (Character)o);
        int which = -1;
        switch (choice)
        {
            case 0:
            case 1:
                which = choice;
                break;
            case 4:
                which = 2;
                break;
            case 5:
                which = 3;
                break;
            case -1:
                return null;
                break;
        }
        return ChooseAbleCharacters[which];
    }

    private List<UIChoosable> getListOfChooseAbleCharacters()
    {
        List<UIChoosable> ChooseAbleCharacters = new List<UIChoosable>();

        foreach (Character character in characters)
        {
            if (character != player1.ActiveCharacter && character != player2.ActiveCharacter &&
                (player1.ToBeActiveCharacter == null || character != player1.ToBeActiveCharacter) &&
                (player2.ToBeActiveCharacter == null || character != player2.ToBeActiveCharacter) ){
                ChooseAbleCharacters.Add(character);
            }
        }

        return ChooseAbleCharacters;
    }

    private IEnumerator P1Attack() {
        
        player1.isAttacking = true;
        List<UIChoosable> choosables = new List<UIChoosable>();
        choosables.AddRange(P1AttackBarManager.getAttacks());

        foreach (UIChoosable choosable in choosables)
        {
            P1AttackBarManager.RemoveFirstAttack();
            player1.Charge -= ((1f / 8f) * choosable.getApCost());
            if (choosable is Character)
            {
                Character character = (Character)choosable;
                player1.ActiveCharacter = character;
                player1.ToBeActiveCharacter = null;
            }
            else
            {
                Attack attack = (Attack)choosable;
                AttackEnemy(attack, player1);
            }
            yield return new WaitForSeconds(choosable.getApCost());
        }
        player1.isAttacking = false;
        yield return null;
    }

    private IEnumerator P2Attack()
    {
        player2.isAttacking = true;
        List<UIChoosable> choosables = new List<UIChoosable>();
        choosables.AddRange(P2AttackBarManager.getAttacks());

        foreach (UIChoosable choosable in choosables)
        {
            P2AttackBarManager.RemoveFirstAttack();
            player2.Charge -= ((1f / 8f) * choosable.getApCost());
            if (choosable is Character)
            {
                Character character = (Character)choosable;
                player2.ActiveCharacter = character;
                player2.ToBeActiveCharacter = null;
            }
            else
            {
                Attack attack = (Attack)choosable;
                AttackEnemy(attack, player2);
            }
            yield return new WaitForSeconds(choosable.getApCost());
        }
        player2.isAttacking = false;
        yield return null;
    }
    
    private void TriggerCombat(bool triggered){
        CombatTriggered = triggered;
        P1ChargeBar.gameObject.SetActive(triggered);
        P2ChargeBar.gameObject.SetActive(triggered);
        player1.Charge = 0;
        player2.Charge = 0;
        P1Charges = new bool[AmountOfCharges];
        P2Charges = new bool[AmountOfCharges];
        ShowAttacksForCharacters();
    }

    private void AttackEnemy(Attack attack, Player player)
    {
        if (attack.Melee)
        {
            StartCoroutine(MoveToLocation(player.ActiveCharacter, player.MeleeLocation, attack.getApCost()));
        }

        bool dead = false;
        if (attack.TargetAllies)
        {
            PlayersHealth.TakeDamage(attack.Damage);
        }
        else
        {
            dead = enemyHealth.TakeDamage(attack.Damage);
        }
        if (dead)
        {
            StartCoroutine(switcher.EndOfCombat());
        }
    }

    private IEnumerator MoveToLocation(Character character, Transform location, int duration)
    {
        character.MyModel.transform.position = location.position;
        yield return new WaitForSeconds(duration - 0.25f);
        character.MyModel.transform.position = new Vector3(character.transform.position.x, character.transform.position.y + 1, character.transform.position.z);
        yield return null;
    }
}
