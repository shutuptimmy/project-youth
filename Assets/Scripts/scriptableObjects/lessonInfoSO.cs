using UnityEngine;

[CreateAssetMenu(fileName = "New Lesson", menuName = "ScriptableObjects/Lesson Data")]
public class lessonInfoSO : ScriptableObject
{
    public string lessonId;

    [Header("Lesson Display")]
    public string lessonTitle;
    public Sprite lessonImage;

    [TextArea(10, 20)]
    public string lessonDesc;
}
