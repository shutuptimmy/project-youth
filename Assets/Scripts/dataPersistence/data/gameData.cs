using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class gameData
{
    // public long lastUpdated;
    public string playerName;
    public int playerGender;
    public string playerLocation;
    public int playerChapter;
    public int playerLvl;
    public int playerExp;

    public Vector3 playerPosition;


    // the values defined in this constructor will be the default values
    // the game starts with when there's no data to load
    public gameData(string playerName, int playerGender)
    {
        this.playerName = playerName;
        this.playerGender = playerGender;
        this.playerChapter = 1;
        this.playerLvl = 1;
        this.playerExp = 0;
        this.playerPosition = Vector3.zero;
        this.playerLocation = "bedroom";
    }
}
