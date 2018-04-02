using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CanvasGroup))]

public class CanvasGroupButton : MonoBehaviour
{
    CanvasGroup myCanvasGroup = null;

    private void Start()
    {
        if (!myCanvasGroup)
        {
            myCanvasGroup = GetComponent<CanvasGroup>();
        }
    }

    private void OnEnable()
    {
        if (!myCanvasGroup)
        {
            myCanvasGroup = GetComponent<CanvasGroup>();
        }
        myCanvasGroup.interactable = true;
        myCanvasGroup.alpha = 1.0f;
    }

    private void OnDisable()
    {
        myCanvasGroup.interactable = false;
        myCanvasGroup.alpha = 0.5f;
    }

}
