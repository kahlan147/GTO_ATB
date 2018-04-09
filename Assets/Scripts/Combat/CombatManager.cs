using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour {
    [System.Serializable]
    public struct Player
    {
        public Character ActiveCharacter;
        public Character ToBeActiveCharacter;
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

  //------------------------------------------------------------------------------------------------
    private Health PlayersHealth;

    private int AmountOfCharges = 8;

    private bool[] P1Charges;
    private bool[] P2Charges;

    private float P1Charge = 0;
    private float P2Charge = 0;

    private bool P1CharSelect = false;
    private bool P2CharSelect = false;

    private bool CombatTriggered;

    // Use this for initialization
    void Start () {
        PlayersHealth = this.GetComponent<Health>();
        P1Charges = new bool[AmountOfCharges];
        P2Charges = new bool[AmountOfCharges];

        P1ChargeBar.gameObject.SetActive(false);
        P2ChargeBar.gameObject.SetActive(false);

        

    }

    private void OnEnable()
    {
        GameplaySwitcher.CombatTriggered += TriggerCombat;
        
    }

    // Update is called once per frame
    void Update (){
        if (CombatTriggered)
        {
            Charge(ref P1ChargeBar, ref P1Charges, ref P1Charge, 0.17f);
            Charge(ref P2ChargeBar, ref P2Charges, ref P2Charge, 0.1f);
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
                            UIChoosable uiChoosable;
                            if (P1CharSelect){
                                uiChoosable = GetCharacterByChoice(choice);
                            }
                            else{
                                uiChoosable = GetAttackByChoice(player1.ActiveCharacter, choice);
                                if (uiChoosable == null){
                                    uiChoosable = GetAttackByChoice(player1.ToBeActiveCharacter, choice);
                                }
                            }
                            P1AttackBarManager.AddAttack(uiChoosable);
                        }
                        break;
                    case 2:
                        choice = P2AttackPanel.GetChoice();
                        if (choice != -1)
                        {
                            UIChoosable uiChoosable;
                            if (P2CharSelect)
                            {
                                uiChoosable = GetCharacterByChoice(choice);
                            }
                            else
                            {
                                uiChoosable = GetAttackByChoice(player2.ActiveCharacter, choice);
                                if (uiChoosable == null)
                                {
                                    uiChoosable = GetAttackByChoice(player2.ToBeActiveCharacter, choice);
                                }
                            }
                            P1AttackBarManager.AddAttack(uiChoosable);
                        }
                        break;
                }
            }

            //Revert
            if (Input.GetButtonDown("P" + PlayerNumber + "Revert"))
            {
                switch (PlayerNumber)
                {
                    case 1:
                        if (P1CharSelect)
                        {
                            P1CharSelect = false;
                            P1AttackPanel.showData(player1.ActiveCharacter.GetAttacks());
                        }
                        else
                        {
                            P1AttackBarManager.RemoveLastAttack();
                        }
                        break;
                    case 2:
                        if (P2CharSelect)
                        {
                            P2CharSelect = false;
                            P2AttackPanel.showData(player2.ActiveCharacter.GetAttacks());
                        }
                        else
                        {
                            P2AttackBarManager.RemoveLastAttack();
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
                        P1Attack();
                        break;
                    case 2:
                        P2Attack();
                        break;
                }
            }

            //Switch
            if (Input.GetButtonDown("P" + PlayerNumber + "Switch"))
            {
                switch (PlayerNumber) {
                    case 1:
                        P1AttackPanel.showData(getListOfChooseAbleCharacters());
                        P1CharSelect = true;
                        break;
                    case 2:
                        P2AttackPanel.showData(getListOfChooseAbleCharacters());
                        P2CharSelect = true;
                        break;
                }
            }
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

    private void P1Attack()
    {
        
    }

    private void P2Attack()
    {

    }

    private void TriggerCombat(bool triggered){
        CombatTriggered = triggered;
        P1ChargeBar.gameObject.SetActive(triggered);
        P2ChargeBar.gameObject.SetActive(triggered);
        P1Charge = 0;
        P2Charge = 0;
        P1Charges = new bool[AmountOfCharges];
        P2Charges = new bool[AmountOfCharges];
        ShowAttacksForCharacters();
    }
}
