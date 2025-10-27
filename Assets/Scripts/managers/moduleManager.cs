using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class moduleManager : MonoBehaviour
{

    public moduleQuestStep moduleQuest;

    [Header("Main UI")]
    [SerializeField] private GameObject moduleBox;
    [SerializeField] private TextMeshProUGUI pageList;
    [SerializeField] private TextMeshProUGUI nextText;
    [SerializeField] private GameObject backButton;
    [SerializeField] private GameObject[] pages;

    private int currentPage;

    public bool isModuleActive { get; private set; }
    private static moduleManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more than one module manager in the scene");
        }
        instance = this;

        showPage(currentPage);
    }

    public static moduleManager GetInstance()
    {
        return instance;
    }

    void Start()
    {
        isModuleActive = false;
        moduleBox.SetActive(false);
    }

    void Update()
    {
        if (!isModuleActive)
        {
            return;
        }
    }

    public void enterModuleMode()
    {

        isModuleActive = true;
        moduleBox.SetActive(true);
    }


    private IEnumerator exitModuleMode()
    {
        moduleQuest.moduleFinished();
        yield return new WaitForSeconds(0.2f);

        isModuleActive = false;
        moduleBox.SetActive(false);
        currentPage = 0;
        showPage(currentPage);
    }



    public void nextPage()
    {
        currentPage++;
        if (currentPage >= pages.Length)
        {
            StartCoroutine(exitModuleMode());
        }
        else
        {
            showPage(currentPage);
        }
    }

    public void previousPage()
    {
        currentPage--;
        if (currentPage < 0)
        {
            currentPage = pages.Length - 1;
        }
        showPage(currentPage);
    }

    public void showPage(int page)
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(false);
        }
        pages[page].SetActive(true);
        pageList.text = (currentPage + 1).ToString() + " / " + pages.Length.ToString();

        if (currentPage + 1 == 1)
        {
            backButton.SetActive(false);
        }
        else
        {
            backButton.SetActive(true);
        }

        if (currentPage + 1 == pages.Length)
        {
            nextText.text = "Finish";
        }
        else
        {
            nextText.text = "Next";
        }
    }
}
