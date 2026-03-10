using System.Collections.Generic;
using UnityEngine;


// Make the dictionary for quests to be serializable
[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<TKey> keys = new List<TKey>();
    [SerializeField] private List<TValue> values = new List<TValue>();

    // Save the dictionary to lists
    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    // Load the dictionary from lists
    public void OnAfterDeserialize()
    {
        this.Clear();
        if (keys.Count != values.Count)
        {
            Debug.LogError("Tried to deserialize a SerializableDictionary, but the amount of keys (" + keys.Count + ") did not match the amount of values (" + values.Count + ") - which indicates data corruption.");
        }

        for (int i = 0; i < keys.Count; i++)
        {
            this.Add(keys[i], values[i]);
        }
    }
}

// Holds the saved state of all quests for serialization
[System.Serializable]
public class SerializableQuestDataMap : SerializableDictionary<string, questData> { }

[System.Serializable]
public class gameData
{
    public string playerName;
    public int playerGender;
    public string playerLocation;
    public int playerChapter;
    public int playerLvl;
    public int playerExp;
    public int playTime;

    public Vector3 playerPosition;
    public SerializableQuestDataMap questDataMap;
    public List<string> unlockedRewardIds;

    public float tugOfWarScore;
    public float sortingBoxesScore;
    public float fragileDeskScore;
    public float fallingApplesScore;
    public float backToEarthScore;


    // the values defined in this constructor will be the default values
    // the game starts with when there's no data to load
    public gameData(string playerName, int playerGender)
    {
        this.playerName = playerName;
        this.playerGender = playerGender;
        this.playerChapter = 1;
        this.playerLvl = 1;
        this.playerExp = 0;
        this.playTime = 0;
        this.playerPosition = new Vector3(-0.9f, -0.1f, 0);
        this.playerLocation = "bedroom";

        // initialize new list of reward
        this.unlockedRewardIds = new List<string>();

        // initialize new quest data map
        this.questDataMap = new SerializableQuestDataMap();
    }
}
