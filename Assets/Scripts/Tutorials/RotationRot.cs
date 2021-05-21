using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationRot : BaseTut
{
    public void CalledOn()
    {
        baseText.text = "Use the middle mouse button to adjust rotational values";
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
            if (timer < 1.2f)
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
