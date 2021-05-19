using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpObject : MonoBehaviour
{
    [Header("Object Detection")]
    public LayerMask hitLayers;
    public float pickupDistance = 2.5f;
    public float pickupRadius = 0.5f;

    Transform cam;
    Transform pickupOffset;

    Pickable pickedObject;
    PrimitiveShift shift;

    bool shoot = false;

    [Header("User Interface")]
    public Image reticle;

    void Start()
    {
        cam = Camera.main.transform;
        shift = GetComponent<PrimitiveShift>();

        pickupOffset = cam.GetComponentsInChildren<Transform>()[1];
    }

    void Update()
    {
        //pick up object
        if (Input.GetMouseButtonDown(1) && !pickedObject)
        {
            shoot = true;
        }

        //if object picked up
        if (pickedObject)
        {
            //drop object
            if (Input.GetMouseButtonDown(1))
            {
                pickedObject.GetDropped();

                pickedObject = null;
                reticle.enabled = true;
            }
            //throw object
            else if (Input.GetMouseButtonDown(0))
            {
                pickedObject.GetThrown();
                reticle.enabled = true;

                pickedObject = null;
            }
            //rotate object to self
            else if (Input.GetMouseButton(2))
            {
                Debug.Log("MMB");
                pickedObject.UpdateRot(pickupOffset);
            }

            //shift object - scrollwheel
            else if (Input.GetAxisRaw("Mouse ScrollWheel") != 0)
            {
                shift.ScrollShift(Input.GetAxisRaw("Mouse ScrollWheel"));
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

            //scale object
            else if (Input.GetKey(KeyCode.Q))
            {
                pickedObject.ScaleMesh(-0.005f);
            }
            else if (Input.GetKey(KeyCode.E))
            {
                pickedObject.ScaleMesh(0.002f);
            }
        }
    }

    void FixedUpdate()
    {
        if (shoot && !pickedObject)
        {
            shoot = false;
            if (Physics.SphereCast(cam.position, pickupRadius, cam.forward, out RaycastHit hit, pickupDistance, hitLayers))
            {
                pickedObject = hit.transform.GetComponent<Pickable>();
                pickedObject.GetPickedUp();
                reticle.enabled = false;
                shift.SetObject(ref pickedObject);

                Debug.Log("HitObject!");
            }
            else
            {
                Debug.Log("Missed!");
            }
        }
    }
}
