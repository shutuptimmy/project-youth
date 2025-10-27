using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class profileSection : MonoBehaviour
{
    public GameObject profileSectionPanel;
    public newProfile newProfileUI;
    public Button[] profiles;

    public void onClosePanel()
    {
        profileSectionPanel.SetActive(false);
        newProfileUI.newProfilePanel.SetActive(false);
    }
}
