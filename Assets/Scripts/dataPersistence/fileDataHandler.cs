using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class fileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    public fileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public gameData load(string profileId)
    {
        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
        gameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                // load the serialized data from the file
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                // deserialize the data from json back to c# object
                loadedData = JsonUtility.FromJson<gameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to load data: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void save(gameData data, string profileId)
    {
        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
        try
        {
            // create dir path if not exists
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // serialize game data to json
            string dataToStore = JsonUtility.ToJson(data, true);

            // write the serialized data to file
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {

            Debug.LogError("Error occured when trying to save data: " + fullPath + "\n" + e);
        }
    }

    public Dictionary<string, gameData> loadAllProfiles()
    {
        Dictionary<string, gameData> profileDictionary = new Dictionary<string, gameData>();

        // loop over all directory names in the data directory path
        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(dataDirPath).EnumerateDirectories();
        foreach (DirectoryInfo dirInfo in dirInfos)
        {
            string profileId = dirInfo.Name;

            // defensive programming - check if data file exists
            // if it doesn't, then this folder isn't a profile and should be skipped
            string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
            if (!File.Exists(fullPath))
            {
                Debug.LogWarning("skipping directory when loading all profiles because it doesn't contain data: " + profileId);
                continue;
            }

            // load the game data for this profile and put it in the dictionary
            gameData profileData = load(profileId);

            // ensure all profiles isn't null. otherwise, something went wrong 
            if (profileData != null)
            {
                profileDictionary.Add(profileId, profileData);
            }
            else
            {
                Debug.LogError("tried to load the profile but something went wrong. ProfileId: " + profileId);
            }
        }

        return profileDictionary;
    }
}
