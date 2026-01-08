using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class lessonDisplay : MonoBehaviour
{
    public lessonInfoSO lesson;
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    void Start()
    {
        title.text = lesson.lessonTitle;
        description.text = lesson.lessonDesc;
    }
}
