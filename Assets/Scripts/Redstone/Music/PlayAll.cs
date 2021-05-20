using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAll : MonoBehaviour
{
    public AudioSource[] audioSources;
    private void OnTriggerEnter(Collider other)
    {
        foreach (AudioSource source in audioSources)
        {
            source.Play();
        }
    }
}
