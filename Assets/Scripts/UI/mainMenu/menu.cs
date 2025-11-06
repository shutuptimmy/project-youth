using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class menu : MonoBehaviour
{
    [Header("First Selected Button")]
    [SerializeField] private GameObject firstSelected;

    protected virtual void OnEnable()
    {
        StartCoroutine(setFirstSelected(firstSelected));
    }

    public IEnumerator setFirstSelected(GameObject firstSelectedObject)
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(firstSelectedObject);

    }
}
