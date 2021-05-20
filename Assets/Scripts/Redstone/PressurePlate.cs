using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [Header("Answer Checks")]
    public bool correctObj = false;
    public string[] acceptedTags;
    List<string> onBoardTags;
    int totalOnBoard = 0;

    [Header("Movement")]
    Vector3 startingPos;
    Vector3 correctPos;

    [Header("Colouring")]
    public Material red;
    public Material green;
    Material baseMat;
    Renderer rend;

    [Header("Sounds")]
    public AudioClip down;
    public AudioClip up;
    AudioSource audioSource;

    private void Start()
    {
        startingPos = transform.position;
        correctPos = new Vector3(startingPos.x, startingPos.y / 10, startingPos.z);

        onBoardTags = new List<string>();

        rend = GetComponent<Renderer>();
        baseMat = rend.material;

        audioSource = GetComponentInParent<AudioSource>();
    }

    private void OnTriggerEnter(Collider obj)
    {
        totalOnBoard++;
        if (totalOnBoard == 1 && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(down);
        }

        foreach (string tag in onBoardTags)
        {
            if (tag == obj.tag)
            {
                onBoardTags.Remove(obj.tag);
                totalOnBoard--;
                break;
            }
        }
        onBoardTags.Add(obj.tag);

        if (acceptedTags.Length > 0)
        {
            foreach (string tag in acceptedTags)
            {
                if (obj.tag == tag)
                {
                    correctObj = true;
                    break;
                }
            }
        }

        if (totalOnBoard > 1)
            correctObj = false;
    }

    private void OnTriggerExit(Collider obj)
    {
        totalOnBoard--;
        if (totalOnBoard < 0) totalOnBoard = 0;
        
        if (totalOnBoard == 0 && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(up);
        }

        if (acceptedTags.Length > 0)
        {
            foreach (string tag in acceptedTags)
            {
                if (obj.tag == tag)
                {
                    correctObj = false;
                    break;
                }
            }
        }

        onBoardTags.Remove(obj.tag);

        foreach(string tag in acceptedTags)
        {
            foreach (string objTag in onBoardTags)
            {
                if (objTag == tag)
                {
                    correctObj = true;
                    break;
                }
            }
        }

        if (totalOnBoard > 1)
        {
            correctObj = false;
        }
    }

    void FixedUpdate()
    {
        if (totalOnBoard > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, correctPos, Time.fixedDeltaTime);
            if (correctObj)
            {
                StartCoroutine(ColourChange(green));
            }
            else
            {
                StartCoroutine(ColourChange(red));
            }
        }
        else if (totalOnBoard == 0) 
        {
            transform.position = Vector3.MoveTowards(transform.position, startingPos, Time.fixedDeltaTime);
            StartCoroutine(ColourChange(baseMat));
        }
    }

    IEnumerator ColourChange(Material colour)
    {
        yield return new WaitForSecondsRealtime(0.3f);
        rend.material = colour;

    }

    public bool CheckAns()
    {
        return correctObj;
    }
}
