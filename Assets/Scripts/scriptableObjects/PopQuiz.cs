using UnityEngine;

[CreateAssetMenu(fileName = "New Pop Quiz", menuName = "ScriptableObjects/Pop Quiz Data")]
public class PopQuiz : ScriptableObject
{
    [TextArea(3, 10)]
    public string questionDescription;
    public bool isTrue;
    public string[] quizChoices;
    [TextArea(3, 10)]
    public string fact;
    [TextArea(3, 10)]
    public string wrong;
}
