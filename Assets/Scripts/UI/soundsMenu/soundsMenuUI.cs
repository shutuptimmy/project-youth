using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundsMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject contentParent;


    public void activateMenu()
    {
        contentParent.SetActive(true);
    }

    public void deactivateMenu()
    {
        contentParent.SetActive(false);
    }
}
