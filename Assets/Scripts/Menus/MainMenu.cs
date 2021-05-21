using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public CanvasGroup menu;

    void Start()
    {
        menu.alpha = 0;
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        yield return new WaitForSecondsRealtime(1f);

        while (menu.alpha < 1)
        {
            menu.alpha += 0.0001f;
        }
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene("Level");
        }
    }
}
