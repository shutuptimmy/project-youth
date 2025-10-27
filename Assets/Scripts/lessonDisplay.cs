using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class lessonDisplay : MonoBehaviour
{
    public Lesson lesson;
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    void Start()
    {
        title.text = lesson.lessonTitle;
        description.text = lesson.lessonDescription;
    }
}
