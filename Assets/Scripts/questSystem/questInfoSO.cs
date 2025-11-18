using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "questInfoSO", menuName = "ScriptableObjects/questInfoSO", order = 1)]
public class questInfoSO : ScriptableObject
{
    [field: SerializeField] public string id { get; private set; }

    [Header("General")]
    public string displayName;
    public bool isSubQuest;

    [Header("Requirements")]
    public int chapter = 1;
    public int lvlReq;
    public questInfoSO[] questPrerequisites;


    [Header("Steps")]
    public GameObject[] questStepPrefabs;

    [Header("Rewards")]
    public int expReward;
    // public int rapportExpReward;


    // ensures the id is always the name of the Scriptable Object asset
    private void OnValidate()
    {
#if UNITY_EDITOR
        id = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}
