using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBoxData", menuName = "ScriptableObjects/Box Data")]
public class boxDataSO : ScriptableObject
{
    public string boxName;
    public Sprite picture;
    public IBoxType type;
}
