using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class JointStorage
{
    public float spring;
    public float damper;
    public float minDistance;
    public float maxDistance;
    public float tolerance;
    public Rigidbody body;


}
public class JointHandler : MonoBehaviourPun
{
    public bool canConnect = true;

    public bool hasConnection = false;
    public SpringJoint springJoint = null;
    private SpringJoint storage = null;
    private bool joinActive = false;
    void Start()
    {

    }

    public void setJoint()
    {
        this.photonView.RPC("Rpc_setJoint", RpcTarget.All, this.gameObject.GetPhotonView().ViewID);
    }

    [PunRPC]
    public void Rpc_setJoint(int photonIDYourself)
    {
        GameObject yourself = PhotonView.Find(photonIDYourself).gameObject;
        JointHandler jointHandler = yourself.GetComponent<JointHandler>();
        if (jointHandler.joinActive)
        {
            storage = yourself.GetComponent<SpringJoint>();
            jointHandler.springJoint = null;
            Destroy(yourself.GetComponent<SpringJoint>());
            jointHandler.joinActive = false;

            yourself.GetComponent<LineRenderer>().positionCount = 0;
            yourself.GetComponent<generateLightbeam>().connected = false;
            yourself.GetComponent<generateLightbeam>().targetTransform = null;

            if (photonView.IsMine)
            {

                yourself.transform.parent.GetChild(1).gameObject.SetActive(false);
            }
        }
        else
        {

            jointHandler.springJoint = yourself.AddComponent<SpringJoint>();
            jointHandler.springJoint.anchor = Vector3.zero;
            jointHandler.springJoint.autoConfigureConnectedAnchor = false;
            if (storage != null)
            {
                jointHandler.springJoint.spring = storage.spring;
                jointHandler.springJoint.damper = storage.damper;
                jointHandler.springJoint.minDistance = storage.minDistance;
                jointHandler.springJoint.maxDistance = storage.maxDistance;
                jointHandler.springJoint.tolerance = storage.tolerance;
            }
            jointHandler.joinActive = true;

            yourself.GetComponent<LineRenderer>().positionCount = 2;

            if (photonView.IsMine)
            {
                yourself.transform.parent.GetChild(1).gameObject.SetActive(true);
            }
        }
    }

    public void setValues(byte _key, float _value, int photonID)
    {
        this.photonView.RPC("RpcSetValues", RpcTarget.AllBuffered, _key, _value, photonID, this.gameObject.GetPhotonView().ViewID);
    }

    [PunRPC]
    public void RpcSetValues(byte _key, float _value, int photonIDTarget, int photonIDYourself)
    {
        GameObject gTarget = PhotonView.Find(photonIDTarget).gameObject;
        Rigidbody _body = gTarget.GetComponent<Rigidbody>();
        springJoint = PhotonView.Find(photonIDYourself).gameObject.GetComponent<SpringJoint>();
        if (this.gameObject.GetPhotonView().ViewID == photonIDYourself)
        {
            switch ((JointParameter)_key)
            {
                case JointParameter.SPRING:
                    springJoint.spring = _value;
                    break;

                case JointParameter.DAMPER:
                    springJoint.damper = _value;
                    break;

                case JointParameter.MINDISTANCE:
                    springJoint.minDistance = _value;
                    break;

                case JointParameter.MAXDISTANCE:
                    springJoint.maxDistance = _value;
                    break;

                case JointParameter.TOLERANCE:
                    springJoint.tolerance = _value;
                    break;

                case JointParameter.BODY:
                    springJoint.connectedBody = _body;

                    springJoint.GetComponent<generateLightbeam>().targetTransform = gTarget.transform;
                    springJoint.GetComponent<generateLightbeam>().connected = true;
                    springJoint.GetComponent<LineRenderer>().SetPosition(0, springJoint.transform.position);
                    springJoint.GetComponent<LineRenderer>().SetPosition(1, gTarget.transform.position);

                    springJoint.connectedAnchor = Vector3.zero;
                    break;

                default:
                    break;
            }
            GetComponent<MyJointsUI>().updateUIElementsSpring();
        }
    }
}
