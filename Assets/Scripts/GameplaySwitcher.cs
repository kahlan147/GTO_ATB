using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplaySwitcher : MonoBehaviour {

    public CameraController cameraController;
    public GameObject VictoryScreen;

    public delegate void TriggerCombat(bool triggered);
    public static event TriggerCombat CombatTriggered;

    private bool combatOngoing = false;

    // Use this for initialization
    void Start () {
        VictoryScreen.SetActive(false);
    }

    private void OnEnable()
    {
        Tile.CombatTriggered += combatTriggered;
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void combatTriggered(ExploringEnemy enemy) {
        cameraController.SwitchCamera();
        Destroy(enemy.gameObject);
        CombatTriggered(true);
    }
    

    public IEnumerator EndOfCombat()
    {
        VictoryScreen.SetActive(true);
        yield return new WaitForSeconds(2);
        VictoryScreen.SetActive(false);
        cameraController.SwitchCamera();
        CombatTriggered(false);
        yield return null;
    }

    public void combatTriggeredDebug() //REMOVE LATER
    {
        cameraController.SwitchCamera();
        combatOngoing = !combatOngoing;
        CombatTriggered(combatOngoing);
    }
}
