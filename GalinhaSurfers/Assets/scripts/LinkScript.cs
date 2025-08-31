using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkScript : MonoBehaviour
{
    [Header("Link")]
    public string url;

    public void OpenLink()
    {
        if(!string.IsNullOrEmpty(url))
        {
            Application.OpenURL(url);
        }
        else
        {
            Debug.LogWarning("Sem Link");
        }
    }
}
