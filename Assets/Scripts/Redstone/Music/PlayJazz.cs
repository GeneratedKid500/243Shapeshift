using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayJazz : MonoBehaviour
{
    public AudioSource[] sources;

    private void OnTriggerEnter(Collider other)
    {
       if (other.tag == "Player")
        {
            sources[0].volume = 0;
            sources[1].volume = 0;
            sources[2].volume = 0;
            sources[3].volume = 0;
            sources[4].volume = 0.3f;
            Destroy(this.gameObject);
        } 
    }
}
