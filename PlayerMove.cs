using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

public class PlayerMove : MonoBehaviourPunCallbacks
{
    public GameObject thirdPersonView;
    public GameObject topDownView;
    public PhotonView playerPhotonView;

    public GameObject joinLength;

    MeshRenderer material;
    [Header("PHYSX")]
    public float deccelSpeed = 0.7f;
    public float maxSpeed = 20;
    public float jumpForce = 8;
    public bool jumpPressed = false;
    public bool isGrounded = true;
    public bool canCastRay;
    public bool canRecieveRay;
    private float maxSpeedStorage;
    private float maxDistance = 8;

    Vector3 moveDirection;

    Rigidbody rb;
    private Vector3 preMousePos = Vector3.zero;

    // Start is called before the first frame update

    private bool switchCamera = true;
    private void Awake()
    {
        playerPhotonView = GetComponent<PhotonView>();
        if (playerPhotonView.IsMine)
        {
            rb = GetComponent<Rigidbody>();
            thirdPersonView.SetActive(true);
        }
    }
    void Start()
    {
        material = GetComponent<MeshRenderer>();

        FindObjectOfType<generateLightbeam>().updateData();

        maxSpeedStorage = maxSpeed;
        Color myColor = Random.ColorHSV();
        if (photonView.IsMine)
        {
            this.photonView.RPC("RPC_SendColor", RpcTarget.AllBuffered, new Vector3(myColor.r, myColor.g, myColor.b));
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (playerPhotonView.IsMine)
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                Vector3 view = (new Vector3(transform.position.x, 0, transform.position.z) - new Vector3(thirdPersonView.transform.position.x, 0, thirdPersonView.transform.position.z)).normalized;

                float lookDirection = Vector3.SignedAngle(transform.TransformDirection(Vector3.forward), view, Vector3.up);

                if (lookDirection != 0 && view != transform.TransformDirection(Vector3.forward))
                {
                    thirdPersonView.transform.SetParent(transform.parent);
                    transform.Rotate(Vector3.up, lookDirection);
                    thirdPersonView.transform.SetParent(transform);
                }
            }

            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            if (Input.GetKeyUp(KeyCode.T))
            {
                thirdPersonView.SetActive(!switchCamera);
                topDownView.SetActive(switchCamera);
                switchCamera = !switchCamera;
            }
            if (Input.GetButtonDown("Jump"))
            {
                jumpPressed = true;
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                Reset();
            }


            //establish line conenction between 2 players
            if (Input.GetMouseButton(0) && GetComponent<JointHandler>().canConnect == true)
            {
                selectConnectionTarget();
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                GetComponent<JointHandler>().setJoint();
            }
        }
    }

    private void FixedUpdate()
    {
        if (playerPhotonView.IsMine)
        {
            if ((rb.velocity.magnitude + moveDirection.magnitude) < new Vector3(maxSpeed, 0, maxSpeed).magnitude)
            {
                rb.velocity = rb.velocity + transform.TransformDirection(moveDirection);
            }

            if (jumpPressed)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isGrounded = false;
                jumpPressed = false;
                if (maxSpeed == maxSpeedStorage)
                {
                    maxSpeed *= 0.3f;
                }
            }

            if (!jumpPressed && !isGrounded && rb.velocity.y < 0)
            {
                rb.AddForce(Vector3.down * 50, ForceMode.Acceleration);
            }


        }
    }

    void selectConnectionTarget()
    {
        if (GetComponent<SpringJoint>() != null)
        {
            var ray = thirdPersonView.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.rigidbody && hit.collider.tag == "Player")
                {
                    if (hit.transform.gameObject.GetPhotonView().ViewID != this.transform.gameObject.GetPhotonView().ViewID)
                    {
                        GetComponent<JointHandler>().setValues((byte)JointParameter.BODY, 0, hit.transform.gameObject.GetComponent<PhotonView>().ViewID);
                    }
                    else
                    {
                        Debug.Log("Cant connect to yourself");
                    }
                }
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        isGrounded = true;
        maxSpeed = maxSpeedStorage;
    }

    private void OnCollisionExit(Collision other)
    {
        isGrounded = false;
    }

    public void Reset()
    {
        if (playerPhotonView.IsMine)
        {
            this.transform.position = new Vector3(0, 2, 0);
        }
    }

    [PunRPC]
    void RPC_SendColor(Vector3 mycolo)
    {
        Color myC = new Color(mycolo.x, mycolo.y, mycolo.z);
        gameObject.GetComponent<MeshRenderer>().material.color = myC;
        gameObject.GetComponent<LineRenderer>().startColor = myC;
        gameObject.GetComponent<LineRenderer>().endColor = myC;
    }
}
