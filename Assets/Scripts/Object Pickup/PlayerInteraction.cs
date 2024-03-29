﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interactables Info")]
    public float castRadius = 0.1f;
    public LayerMask interactableLayer;
    Vector3 rayPos;
    public GameObject lookObj;
    Interactable interactable;
    Camera mainCam;
    [HideInInspector] public PrimitiveShift shift;

    [Header("Pickup")]
    public Transform pickupParent;
    public GameObject currentlyPickedUp;
    Rigidbody objRB;

    [Header("Object Follow")]
    //MOVEMENT
    public float minSpeed = 0;
    public float maxSpeed = 300f;
    public float maxDist = 10f;
    float currentSpeed = 0f;
    float currentDist = 0f;
    //ROTATION
    public float rotSpeed = 100f;
    bool canRot = false;
    Quaternion lookRot;

    [Header("User Interface")]
    //reticle
    public Image reticle;
    Color baseReticle;
    //hotbar
    public CanvasGroup hotbar;
    public RectTransform[] hotbarBoxes; //0 is highlight
    float hotbarTimer;

    [Header("Tutorials")]
    public PickupTut pt;
    public ShapeshiftTut sst;
    public ScaleTut st;
    public RotationRot rt;

    void Awake()
    {
        mainCam = Camera.main;
        pickupParent = mainCam.GetComponentsInChildren<Transform>()[1];

        shift = GetComponent<PrimitiveShift>();

        maxSpeed *= 10;
    }

    void Start()
    {
        baseReticle = reticle.color;
    }
    
    //Input Handler
    private void Update()
    {
        if (currentlyPickedUp && currentDist > maxDist)
            DropObj();

        if (Input.GetMouseButtonDown(1))
        {
            PickUpPutDown();
        }

        if (currentlyPickedUp)
        {
            if (Input.GetMouseButtonDown(2))
            {
                canRot = !canRot;
            }
            else if (Input.GetMouseButtonDown(0))
            {
                ThrowObj();
            }

            else if (Input.GetAxisRaw("Mouse ScrollWheel") != 0 || (Input.GetKeyDown("1")) || 
                (Input.GetKeyDown("2")) || (Input.GetKeyDown("3")) || (Input.GetKeyDown("4")))
            {
                //shift object - scrollwheel
                if (Input.GetAxisRaw("Mouse ScrollWheel") != 0)
                {
                    shift.ScrollShift(-(Input.GetAxisRaw("Mouse ScrollWheel")));
                }
                //shift object - numpad
                else if (Input.GetKeyDown("1"))
                    shift.NumShift(0);
                else if (Input.GetKeyDown("2"))
                    shift.NumShift(1);
                else if (Input.GetKeyDown("3"))
                    shift.NumShift(2);
                else if (Input.GetKeyDown("4"))
                    shift.NumShift(3);

                hotbarTimer = 0;
            }


            //scale object
            else if (Input.GetKey(KeyCode.Q))
            {
                interactable.ScaleMesh(-0.009f);
            }
            else if (Input.GetKey(KeyCode.E))
            {
                interactable.ScaleMesh(0.009f);
            }

        }

        UpdateUI();
    }

    void FixedUpdate()
    {
        IsLookAtObj();

        ObjMove();
    }

    void PickUpPutDown()
    {
        if (!currentlyPickedUp)
        {
            if (lookObj)
            {
                PickUpObj();
            }
        }
        else
        {
            DropObj();
        }
    }

    void IsLookAtObj()
    {
        rayPos = mainCam.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;
        if (Physics.SphereCast(rayPos, castRadius, mainCam.transform.forward, out hit, maxDist / 2, interactableLayer))
            lookObj = hit.collider.transform.gameObject;
        else
            lookObj = null;
    }

    void ObjMove()
    {
        //movement
        if (currentlyPickedUp)
        {
            currentDist = Vector3.Distance(pickupParent.position, objRB.position);
            currentSpeed = Mathf.SmoothStep(minSpeed + 1, maxSpeed, currentDist / maxDist);
            currentSpeed *= Time.deltaTime;

            Vector3 direction = pickupParent.position - objRB.position;
            objRB.velocity = direction.normalized * currentSpeed;

            //rotation
            if (canRot)
            {
                lookRot = Quaternion.LookRotation(mainCam.transform.position - currentlyPickedUp.transform.position);
                lookRot = Quaternion.Slerp(mainCam.transform.rotation, lookRot, rotSpeed * Time.fixedDeltaTime);
                objRB.MoveRotation(lookRot);
            }
        }
    }

    public void PickUpObj()
    {
        currentlyPickedUp = lookObj;
        interactable = lookObj.GetComponentInChildren<Interactable>();

        objRB = interactable.rb;
        objRB.constraints = RigidbodyConstraints.FreezeRotation;

        shift.SetObject(ref interactable);

        //
        if (sst != null)
        {
            sst.CalledOn();
            sst = null;
        }

        else if (interactable.activeShape == 3)
        {
            if (st != null)
            {
                st.CalledOn();
                st = null;
            }
        }
        else if (interactable.activeShape == 2)
        {
            if (rt != null)
            {
                rt.CalledOn();
                rt = null;
            }

        }
        //

        StartCoroutine(interactable.PickUp());
    }

    public void DropObj()
    {
        objRB.constraints = RigidbodyConstraints.None;
        currentlyPickedUp = null;
        interactable.pickedUp = false;
        canRot = false;
        shift.picked = null;
        currentDist = 0;

        StartCoroutine(RedReticle());
    }

    public void ThrowObj()
    {
        DropObj();

        objRB.AddForce(mainCam.transform.forward * interactable.throwForce, ForceMode.Impulse);
    }

    public void UpdateUI()
    {
        UpdateReticle();
        UpdateHotBar();
    }

    #region UpdateReticle
    void UpdateReticle()
    {
        if (reticle.color != Color.red)
        {
            if (lookObj && !currentlyPickedUp)
            {
                reticle.color = Color.green;
                //
                if (pt != null)
                {
                    pt.CalledOn();
                    pt = null;
                }
                //
            }
            else if (currentlyPickedUp || !lookObj)
            {
                reticle.color = baseReticle;
            }
        }

    }

    public IEnumerator RedReticle()
    {
        reticle.color = Color.red;
        yield return new WaitForSecondsRealtime(0.3f);
        reticle.color = baseReticle;

    }
    #endregion

    void UpdateHotBar()
    {
        if (!currentlyPickedUp)
        {
            hotbar.alpha = 0;
            hotbarTimer = 0;
        }
        else
        {
            if (hotbarTimer < 1)
            {
                hotbar.alpha += 0.01f;
                hotbarTimer += Time.deltaTime;
            }
            else
            {
                hotbar.alpha -= 0.01f;
            }

            Vector3 rPos;
            switch (interactable.activeShape) 
            {
                default:
                    rPos = hotbarBoxes[1].transform.localPosition;
                    hotbarBoxes[0].transform.localPosition = rPos;
                    break;
                case 1:
                    rPos = hotbarBoxes[2].transform.localPosition;
                    hotbarBoxes[0].transform.localPosition = rPos;
                    break;
                case 2:
                    rPos = hotbarBoxes[3].transform.localPosition;
                    hotbarBoxes[0].transform.localPosition = rPos;
                    break;
                case 3:
                    rPos = hotbarBoxes[4].transform.localPosition;
                    hotbarBoxes[0].transform.localPosition = rPos;
                    break;

            }

        }
    }
}
