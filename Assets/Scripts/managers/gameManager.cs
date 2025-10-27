using UnityEngine;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{
    [SerializeField] private string currentBg = "";
    [SerializeField] private bool isAftermath = false;

    // public static gameManager instance;

    // [Header("Persistent Objects")]
    // public GameObject[] persistentObjects;

    // private void Awake()
    // {
    //     if (instance != null)
    //     {
    //         CleanUpAndDestroy();
    //         return;
    //     }
    //     else
    //     {
    //         instance = this;
    //         DontDestroyOnLoad(gameObject);
    //         MarkPersistentObjects();
    //     }
    // }

    // private void MarkPersistentObjects()
    // {
    //     foreach (GameObject obj in persistentObjects)
    //     {
    //         if (obj != null)
    //         {
    //             DontDestroyOnLoad(obj);
    //         }
    //     }
    // }

    // private void CleanUpAndDestroy()
    // {
    //     foreach (GameObject obj in persistentObjects)
    //     {
    //         Destroy(obj);
    //     }
    //     Destroy(gameObject);
    // }
}