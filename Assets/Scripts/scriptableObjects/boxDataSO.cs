using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ForceType
{
    Contact,
    NonContact
}

[CreateAssetMenu(fileName = "NewBoxData", menuName = "Minigame/Box Data")]
public class boxDataSO : ScriptableObject
{
    public string boxName;
    public Sprite picture;
    public ForceType type;
}
