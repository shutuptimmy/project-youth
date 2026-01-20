using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundPersistenceManagers : MonoBehaviour
{
    public static soundPersistenceManagers instance { get; private set; }
    [SerializeField] private GameObject[] soundManagers;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more than one Sound Persistence Manager in the scene. Destroying newest one.");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        foreach (GameObject obj in soundManagers)
        {
            DontDestroyOnLoad(obj);
        }
    }
}
