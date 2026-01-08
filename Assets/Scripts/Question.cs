using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Question
{
    [TextArea(3, 15)]
    public string description;
    [TextArea(3, 10)]
    public string trueAnswer;
    [TextArea(3, 10)]
    public string[] answers;

}
