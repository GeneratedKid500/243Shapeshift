using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(CapsuleCollider))]
public class Pickable : MonoBehaviour
{
    float minSpeed;
    float maxSpeed;
    float rotSpeed;

    Vector3 minSize;
    Vector3 maxSize;
    Vector3 curSize;

    MeshFilter meshFilter;
    Collider[] col;
    Transform player;
    Rigidbody rb;

    [Header("Currently Active Object")]
    public int activeShape = 0;

    Quaternion lookRot;

    float currentDist;
    float maxDist;

    float currentSpeed;

    bool isHolding = false;

    Vector3 pickPos;

    [Header("Variables")]
    public float throwForce = 10;

    void Start()
    {
        player = Camera.main.transform;

        rb = GetComponent<Rigidbody>();

        meshFilter = GetComponent<MeshFilter>();
        col = GetComponents<Collider>();
        CapsuleCollider tCol = col[2].GetComponent<CapsuleCollider>();
        tCol.height = 2;
        tCol.radius = 0.5f;
        col[2] = tCol;
        SetMesh(meshFilter.mesh);

        curSize = transform.localScale;
        minSize = new Vector3(0.3f, 0.3f, 0.3f);
        maxSize = new Vector3(1.3f, 1.3f, 1.3f);
    }

    void FixedUpdate()
    {
        if (isHolding)
            transform.localPosition = pickPos;
    }

    public void GetPickedUp()
    {
        transform.SetParent(player);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        pickPos = transform.localPosition;

        rb.useGravity = false;
        rb.isKinematic = true;

        isHolding = true;

        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public void GetDropped()
    {
        transform.SetParent(null);
        rb.useGravity = true;

        rb.isKinematic = false;
        isHolding = false;

        rb.constraints = RigidbodyConstraints.None;
    }

    public void GetThrown()
    {
        transform.SetParent(null);
        rb.constraints = RigidbodyConstraints.None;
        rb.useGravity = true;
        rb.isKinematic = false;
        isHolding = false;
        rb.AddForce(player.forward * throwForce, ForceMode.Impulse);
    }

    public void UpdatePos(Transform pickupOffset)
    {
        currentDist = Vector3.Distance(pickupOffset.position, transform.position);

        currentSpeed = Mathf.SmoothStep(minSpeed, maxSpeed, currentDist / maxDist);
        currentSpeed *= Time.fixedDeltaTime;

        Vector3 direction = pickupOffset.position - transform.position;
        rb.velocity = direction.normalized * currentSpeed;
    }

    public void UpdateRot(Transform newLoc)
    {
        lookRot = Quaternion.LookRotation(player.position - transform.position);

        if (transform.rotation != lookRot)
        {
            pickPos = newLoc.localPosition;
            rb.MoveRotation(lookRot);
        }
    }

    //change shape
    public void SetMesh(Mesh mesh)
    {

        meshFilter.mesh = mesh;
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
}
