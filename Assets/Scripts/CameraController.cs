using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Camera ExplorationCamera;
    public Camera CombatCamera;
    public ExploringParty party;

	// Use this for initialization
	void Start () {
        CombatCamera.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        ExploringParty.positionChanged += MoveExplorationCameraToPlayer;
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void SwitchCamera()
    {
        if (ExplorationCamera.isActiveAndEnabled)
        {
            CombatCamera.gameObject.SetActive(true);
            ExplorationCamera.gameObject.SetActive(false);
        }
        else
        {
            ExplorationCamera.gameObject.SetActive(true);
            CombatCamera.gameObject.SetActive(false);
        }
    }

    public void MoveExplorationCameraToPlayer()
    {
        ExplorationCamera.transform.position = party.transform.position + new Vector3(0, 50, 0);
    }
}
