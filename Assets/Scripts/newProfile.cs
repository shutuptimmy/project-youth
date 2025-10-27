using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class newProfile : MonoBehaviour
{
    public GameObject newProfilePanel;
    public TextMeshProUGUI playerNameInput;
    public TextMeshProUGUI placeholderName;
    public Button boyGender;
    public Button girlGender;


    private string playerName;
    private string boyName = "David Snake";
    private string girlName = "Jane Doe";
    private int charGender;



    public void onClosePanel()
    {
        newProfilePanel.SetActive(false);
        playerName = "";

        // boyGender.interactable = true;
        // girlGender.interactable = true;
    }

    public void createNewProfile()
    {
        playerNameInput.text = playerName;

    }

    public void genderSelection()
    {
        // if the user selected gender button before entering a name or no name, change the placeholder name for each protagonist.
        if (playerNameInput.text == "")
        {
            switch (charGender)
            {
                case 1:
                    placeholderName.text = girlName;
                    break;

                default:
                    placeholderName.text = boyName;
                    break;
            }
        }
    }
}

