using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(CapsuleCollider))]
public class Interactable : MonoBehaviour
{
    [Header("Object Stats")]
    public bool shiftable = true;
    public bool scaleable = true;
    public bool respawnable = true;

    [Header("Physics Stats")]
    public bool pickedUp = false;
    [HideInInspector] public PlayerInteraction playerInteractions;
    [HideInInspector] public Rigidbody rb;
    public float throwForce = 10f;
    public float breakOffForce = 35f;

    [Header("Respawn Point")]    
    public GameObject respawnPoint;
    public int deathPoint = -100;

    [Header("Shape Shifting")]
    //shape
    public int activeShape = 0;
    int startingShape;
    MeshFilter meshFilter;
    Collider[] col;
    //scale
    public Vector3 baseSize = new Vector3(.6f, .6f, .6f);
    Vector3 minSize;
    Vector3 maxSize;
    Vector3 curSize;

    private void Start()
    {
        gameObject.layer = 9;
        if (!respawnPoint)
        {
            respawnPoint = GameObject.FindGameObjectWithTag("RespawnPoint");
        }
         
        //stats
        playerInteractions = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerInteraction>();
        rb = GetComponent<Rigidbody>();
        
        //shapes
        meshFilter = GetComponent<MeshFilter>();
        col = GetComponents<Collider>();

        CapsuleCollider tCol = col[2].GetComponent<CapsuleCollider>();
        tCol.height = 2;
        tCol.radius = 0.5f;
        col[2] = tCol;

        Interactable instance = this;
        playerInteractions.shift.SetObject(ref instance);
        playerInteractions.shift.NumShift(activeShape);
        startingShape = activeShape;

        //scale
        curSize = transform.localScale;
        minSize = new Vector3(0.3f, 0.3f, 0.3f);
        maxSize = new Vector3(1.3f, 1.3f, 1.3f);
    }

    private void Update()
    {
        if (transform.position.y < deathPoint)
        {
            if (respawnable)
            {
                transform.localScale = baseSize;
                curSize = baseSize;
                rb.velocity = Vector3.zero;
                transform.rotation = Quaternion.identity;
                transform.position = respawnPoint.transform.position;
            }
            else
            {
                gameObject.SetActive(false);
                Debug.Log("Destroyed");
                Destroy(this.gameObject);
            }

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (pickedUp)
        {
            if (collision.relativeVelocity.magnitude > breakOffForce)
            {
                playerInteractions.DropObj();
                StartCoroutine(playerInteractions.RedReticle());
            }
        }
    }

    public IEnumerator PickUp()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        pickedUp = true;
    }

    //change shape
    public void SetMesh(Mesh mesh)
    {
        if (shiftable)
        {
            meshFilter.mesh = mesh;
            SwitchActive();
        }
        else
        {
            Debug.Log("Unshiftable");
            activeShape = startingShape;
            SwitchActive();
        }

    }

    void SwitchActive()
    {
        switch (activeShape)
        {
            case 0:
                col[0].enabled = true;
                col[1].enabled = false;
                col[2].enabled = false;
                break;

            case 1:
                col[0].enabled = false;
                col[1].enabled = true;
                col[2].enabled = false;
                break;

            case 2:
                col[0].enabled = false;
                col[1].enabled = false;
                col[2].enabled = true;
                break;

            case 3:
                col[0].enabled = false;
                col[1].enabled = false;
                col[2].enabled = true;

                break;

            default:
                col[0].enabled = false;
                col[1].enabled = false;
                col[2].enabled = false;
                break;
        }
    }

    public void ScaleMesh(float increase)
    {
        if (scaleable)
        {
            Vector3 vIncrease = new Vector3(increase, increase, increase);
            if (increase > 0)
            {
                if (curSize.y < maxSize.y)
                {
                    curSize += vIncrease;
                    transform.localScale = curSize;
                }
                else
                {
                    Debug.Log("Max Size");
                }
            }
            else if (increase < 0)
            {
                if (curSize.y > minSize.y)
                {
                    curSize += vIncrease;
                    transform.localScale = curSize;
                }
                else
                {
                    Debug.Log("Min Size");
                }
            }
            else
            {
                Debug.Log(this + ": Scale Error");
            }
        }
        else
        {
            Debug.Log("UnScaleable");
        }
    }

    public void SetUnscaleable(bool set)
    {
        scaleable = set;
    }

    public void SetUnshiftable(bool set)
    {
        shiftable = set;
        activeShape = startingShape;
    }

    public void SetRespawnable(bool set)
    {
        respawnable = set;
    }
}
