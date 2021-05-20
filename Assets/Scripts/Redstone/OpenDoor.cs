using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public GameObject door;

    [Header("Requirements")]
    public PressurePlate[] neededBools;
    bool yep = false;

    private void Update()
    {
        if (yep)
        {
            StartCoroutine(OpenTheDoor());
        }
        else
        {
            CloseTheDoor();
        }
    }

    void LateUpdate()
    {
        if (neededBools.Length > 0)
        {
            for (int i = 0; i < neededBools.Length; i++)
            {
                if (!neededBools[i].correctObj)
                {
                    yep = false;
                    break;
                }
                yep = true;
            }
        }
    }

    IEnumerator OpenTheDoor()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        door.SetActive(false);
    }

    void CloseTheDoor()
    {
        door.SetActive(true);
    }
}
