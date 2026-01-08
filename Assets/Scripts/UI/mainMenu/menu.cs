using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class menu : MonoBehaviour
{
    [Header("First Selected Button")]
    [SerializeField] private Button firstSelected;

    protected virtual void OnEnable()
    {
        setFirstSelected(firstSelected);
    }

    public void setFirstSelected(Button firstSelectedButton)
    {
        firstSelectedButton.Select();

    }
}
