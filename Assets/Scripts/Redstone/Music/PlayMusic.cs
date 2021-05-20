using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusic : MonoBehaviour
{
    public AudioSource priorSource;
    public AudioSource audioSource;

    private void Start()
    {
        audioSource.volume = 0;
    }

    private void OnTriggerEnter(Collider player)
    {
        if (player.tag == "Player")
        {
            priorSource.volume = 0;
            audioSource.volume = 0.3f;
            Destroy(this.gameObject);
        }
    }
}
