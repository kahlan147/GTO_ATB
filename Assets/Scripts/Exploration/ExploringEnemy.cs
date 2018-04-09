using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploringEnemy : MonoBehaviour {

    [HideInInspector]
    public Tile myTile;
    public List<Character> characters;

    private bool canMove = true;

	// Use this for initialization
	void Start () {
        this.transform.position += new Vector3(0, 1, 0);
	}

    private void OnEnable()
    {
        ExploringParty.positionChanged += Move;
        Tile.CombatTriggered += MayNotMove;
    }

    // Update is called once per frame
    void Update () {
		
	}

    private void MayNotMove(ExploringEnemy enemy) {
        canMove = false;
    }

    private void Move()
    {
        if (canMove)
        {
            int moveLocation = Random.Range(0, 5);
            Vector3 movement = new Vector3(0, 0, 0);
            switch (moveLocation)
            {
                case 1:
                    movement = new Vector3(1, 0, 0);
                    break;
                case 2:
                    movement = new Vector3(-1, 0, 0);
                    break;
                case 3:
                    movement = new Vector3(0, 0, 1);
                    break;
                case 4:
                    movement = new Vector3(0, 0, -1);
                    break;
            }

            if (moveLocation != 0)
            {
                StartCoroutine(DelayedMove(movement));
            }
        }
        canMove = true;

    }

    private IEnumerator DelayedMove(Vector3 movement)
    {
        yield return new WaitForSeconds(.2f);

        Vector3 newPosition = this.transform.position + movement * 10;

        RaycastHit hit;
        Physics.Raycast(newPosition, Vector3.down * 3, out hit);

        if (hit.collider != null)
        {
            if (hit.collider.transform.parent.gameObject.tag == "Player")
            {
                Debug.Log("oh");
            }
                if (hit.collider.transform.parent.gameObject.tag == "Tile")
            {
                Tile tile = hit.collider.transform.parent.gameObject.GetComponent<Tile>();
                if (tile.CanWalkHereEnemy(this))
                {
                    myTile.enemy = null;
                    myTile = tile;
                    myTile.enemy = this;
                    this.transform.position = newPosition;
                }
            }
        }

        yield return null;
    }

    private void OnDestroy()
    {
        ExploringParty.positionChanged -= Move;
        myTile.enemy = null;
    }
}
