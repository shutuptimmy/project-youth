using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistence
{
    void loadData(gameData data);
    void saveData(gameData data);
}
