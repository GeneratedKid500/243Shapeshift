using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupTut : BaseTut
{
    public void CalledOn()
    {
        baseText.text = "Use right click to pick up or drop an object";
        go = true;
    }

    private void Start()
    {
        textGroup.alpha = 0;
    }

    private void Update()
    {
        if (go)
        {
            if (timer < 1.5f)
            {
                textGroup.alpha += 0.01f;
                timer += Time.deltaTime;
            }
            else
            {
                textGroup.alpha -= 0.01f;
                if (textGroup.alpha <= 0)
                {
                    Destroy(this);
                }
            }
        }
    }
}
