using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplaySwitcher : MonoBehaviour {

    public CameraController cameraController;

    public delegate void TriggerCombat(bool triggered);
    public static event TriggerCombat CombatTriggered;

    private bool combatOngoing = false;

    // Use this for initialization
    void Start () {
		
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
        //TO BE WORKED ON.
    }

    public void combatTriggeredDebug() //REMOVE LATER
    {
        cameraController.SwitchCamera();
        combatOngoing = !combatOngoing;
        CombatTriggered(combatOngoing);
    }
}
