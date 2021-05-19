using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimitiveShift : MonoBehaviour
{
    Pickable picked;

    public Mesh[] shapes;

    public void SetObject(ref Pickable pickedRef)
    {
        picked = pickedRef;
        picked.SetMesh(shapes[picked.activeShape]);
    }

    public void ScrollShift(float direction)
    {
        //Debug.Log(direction);
        if (direction<0)
        {
            if (picked.activeShape > 0)
            {
                picked.activeShape--;
            }
            else
            {
                picked.activeShape = shapes.Length - 1;
            }
        }
        else if (direction > 0)
        {
            if (picked.activeShape < shapes.Length - 1)
            {
                picked.activeShape++;
            }
            else
            {
                picked.activeShape = 0;
            }
        }
        else
        {
            Debug.LogWarning(this + ": 0");
        }
        picked.SetMesh(shapes[picked.activeShape]);
    }

    public void NumShift(int num)
    {
        picked.activeShape = num;
        picked.SetMesh(shapes[picked.activeShape]);
    }
}
