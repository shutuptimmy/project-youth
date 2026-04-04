using UnityEngine;

public class CreditsPanelUI : MonoBehaviour
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
