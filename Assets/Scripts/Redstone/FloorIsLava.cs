using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorIsLava : MonoBehaviour
{
    public Transform respawn;

    private void OnTriggerEnter(Collider obj)
    {
        if (obj.tag == "Player")
        {
            obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
            obj.transform.position = respawn.position;
        }
    }


}
