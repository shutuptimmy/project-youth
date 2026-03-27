using UnityEngine;

public class soundPersistenceManagers : MonoBehaviour
{
    public static soundPersistenceManagers instance { get; private set; }
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
    }
}
