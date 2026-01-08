using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectPoolManager : MonoBehaviour
{
    [Header("Pool Settings")]
    [SerializeField] private GameObject prefab;
    [SerializeField] private int initialSize = 10;

    // A Queue is perfect for "First In, First Out" recycling
    private Queue<GameObject> poolQueue = new Queue<GameObject>();
    private Transform parentContainer;

    public void Initialize(Transform parent)
    {
        parentContainer = parent;

        // Pre-fill the pool so we don't lag at the start of the game
        for (int i = 0; i < initialSize; i++)
        {
            CreateNewObject();
        }
    }

    private GameObject CreateNewObject()
    {
        GameObject obj = Instantiate(prefab, parentContainer);
        obj.SetActive(false); // Start hidden
        poolQueue.Enqueue(obj);
        return obj;
    }

    public GameObject GetObject()
    {
        if (poolQueue.Count == 0)
        {
            // If we ran out, make a new one (expandable pool)
            CreateNewObject();
        }

        // Pull from the queue
        GameObject obj = poolQueue.Dequeue();
        obj.SetActive(true);
        return obj;
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        // Reset rotation/physics here if generic, or let the specific script do it
        obj.transform.rotation = Quaternion.identity;
        poolQueue.Enqueue(obj);
    }

    // Helper to clear everything (e.g., when quitting minigame)
    public void ReturnAllActive()
    {
        // This is a bit manual, usually handled by the manager loop
        foreach (Transform child in parentContainer)
        {
            if (child.gameObject.activeSelf)
            {
                ReturnObject(child.gameObject);
            }
        }
    }
}
