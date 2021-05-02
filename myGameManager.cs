using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class myGameManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public GameObject prefab;
    public GameObject spawnButton;
    public GameObject sceneCamera;

    public static List<GameObject> allPlayer = new List<GameObject>();

    public GameObject spawnPoint;

    private void Awake()
    {
        spawnButton.SetActive(true);
    }

    public void SpawnPlayer()
    {
        GameObject lul = PhotonNetwork.Instantiate(prefab.name, spawnPoint.transform.position + new Vector3(0, 2, 0), Quaternion.identity, 0);
        spawnButton.SetActive(false);
        sceneCamera.SetActive(false);
       
    }

    [PunRPC]
    public void addPlayer(GameObject g)
    {
      
    }

}
