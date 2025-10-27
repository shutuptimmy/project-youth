using UnityEngine;

[CreateAssetMenu(fileName = "New Lesson", menuName = "Lesson Data")]
public class Lesson : ScriptableObject
{
    public string lessonTitle;
    [TextArea(3, 10)]
    public string lessonDescription;
}
