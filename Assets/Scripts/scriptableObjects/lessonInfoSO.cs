using UnityEngine;

[CreateAssetMenu(fileName = "New Lesson", menuName = "ScriptableObjects/Lesson Data")]
public class lessonInfoSO : ScriptableObject
{
    [Header("Lesson Display")]
    public string lessonTitle;

    [TextArea(3, 10)]
    public string lessonDesc;

    [Header("Vocabulary Unlock")]
    [Tooltip("The unique ID of the vocabulary word unlocked by this lesson.")]
    public string vocabularyWordId = "VOCAB_TERM_01";
}
