using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeshiftTut : BaseTut
{
    public void CalledOn()
    {
        baseText.text = "Use the Mouse Scrollwheel or Number Keys to shapeshift picked up primitives";
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
            if (timer < 1.8f)
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
