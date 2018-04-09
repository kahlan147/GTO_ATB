using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    public bool hasPlayer;
    public ExploringEnemy enemy;
    public bool NotWalkable;
    public bool PlayerSpawnTile;
    public bool EnemySpawnTile;
    public bool EndTile;

    public delegate void TriggerCombat(ExploringEnemy enemy);
    public static event TriggerCombat CombatTriggered;

    private Level level;
    private int SpawnChance = 2;


	// Use this for initialization
	void Start () {
        level = this.GetComponentInParent<Level>();
	}

    private void OnEnable()
    {
        if (this.EnemySpawnTile)
        {
            ExploringParty.positionChanged += SpawnEnemy;
        }
    }

    // Update is called once per frame
    void Update () {
		
	}

    public bool CanWalkHerePlayer()
    {
        if (EndTile)
        {
            Debug.Log("You win!");
        }

        if (enemy != null)
        {
            CombatTriggered(enemy);
        }

        if (NotWalkable)
        {
            return false;
        }
        

        return true;
    }

    public bool CanWalkHereEnemy(ExploringEnemy requestingEnemy)
    {
        if (NotWalkable)
        {
            return false;
        }
        if (enemy != null)
        {
            return false;
        }
        if (hasPlayer)
        {
            CombatTriggered(requestingEnemy);
        }
        return true;
    }

    private void SpawnEnemy() {
        if (this.enemy == null)
        {
            int chance = Random.Range(0,150);
            if (chance >= 30 && chance <= (30 + SpawnChance))
            {
                ExploringEnemy newEnemy = level.GetRandomEnemy(this.transform.position);
                if (newEnemy != null)
                {
                    this.enemy = Instantiate(newEnemy, this.transform.position, new Quaternion(0, 0, 0, 0));
                    enemy.myTile = this;
                }
            }
        }
    }
    

}
