using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generateLightbeam : MonoBehaviour
{
    public GameObject[] players;
    public LineRenderer lineRenderer;
    public float maxDistance = 8;
    private Vector3[] positions;

    public bool connected = false;

    public Transform targetTransform;


    void Update()
    {
        if (connected)
        {
            GetComponent<LineRenderer>().SetPosition(0, this.transform.position);
            GetComponent<LineRenderer>().SetPosition(1, targetTransform.position);
        }
    }

    public void updateData()
    {

    }

    //TODO: UI für alle Werte zum testen...
    //Spring or fixed joints...
    //Parameter values...
    //Variable joints per click(RAYCAST) on player!!
    void connectPlayers()
    {
        // SpringJoint newJoint = players[players.Length - 2].AddComponent<SpringJoint>();
        // newJoint.connectedBody = players[players.Length - 1].GetComponent<Rigidbody>();
        // newJoint.maxDistance = maxDistance;
        // newJoint.minDistance = 0;
        // newJoint.spring = 20;
        // newJoint.damper = 0.2f;
        // newJoint.autoConfigureConnectedAnchor = false;
        // newJoint.anchor = new Vector3(0,0,0);
        // newJoint.connectedAnchor = new Vector3(0, 0, 0);
    }
}
