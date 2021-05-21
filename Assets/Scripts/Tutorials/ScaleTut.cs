using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleTut : BaseTut
{
    public void CalledOn()
    {
        baseText.text = "Use Q to shrink and E to enlarge objects you are holding";
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
            if (timer < 2f)
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
