using UnityEngine;
using UnityEngine.Assertions;
using System; // [Serializable]
using SSS.SaveLoad;


public class Test_SSS_Core : MonoBehaviour
{
	void Start()
    {
        TestSaving();
    }

    void TestSaving()
    {
        Assert.raiseExceptions = true;

        // Constructor
        SaveManager saveManager = new SaveManager("YourGameFileName", "dat");

        // Helper Get methods
        bool savesExist = saveManager.SavesExist();
        int saveCount = saveManager.GetMaxSaveSlot() + 1;
        Debug.Log(saveManager.GetDirectoryPath());
        Debug.Log("saves already exist: " + savesExist);
        if (savesExist) { Debug.Log("save count: " + saveCount); }

        // Create
        Debug.Log("*******************");
        Debug.Log("Creating game data");
        YourGameSaveData data;
        for (int i = 0; i < 10; i++)
        {
            data = new YourGameSaveData();
            data.playerName += i.ToString();
            data.age += i;
            int newSlot = saveManager.Create(data);
            Debug.Log("Created in slot: " + newSlot);
        }

        // LoadAll, Save
        Debug.Log("*******************");
        Debug.Log("Loading all game data");
        SaveData[] saveData = saveManager.LoadAll();
        foreach (YourGameSaveData item in saveData)
        {
            if (item != null)
            {
                Debug.Log("********************");
                Debug.Log(item.ToString());
                item.age = UnityEngine.Random.Range(5, 100);
                Debug.Log(item.ToString());
                saveManager.Save(item);
            }
        }

        // Load
        Debug.Log("*******************");
        Debug.Log("Loading slot 3 game data");
        SaveData itemFive = saveManager.Load(3);
        if (itemFive != null) { Debug.Log(itemFive.ToString()); }

        // Delete
        Debug.Log("*******************");
        Debug.Log("Deleting slot 3, 4, 5 game data");
        saveManager.Delete(3);
        saveManager.Delete(4);
        saveManager.Delete(5);

        // Verify Deletion
        Debug.Log("*******************");
        Debug.Log("Verify deletion occurred");
        saveData = saveManager.LoadAll();
        Assert.IsTrue(saveCount + 7 == saveData.Length);
        foreach (YourGameSaveData item in saveData)
        {
            if (item != null) Debug.Log(item.ToString());
        }
    }
}

[Serializable]
public class YourGameSaveData : SaveData
{
    public string playerName = "Jon Snow";
    public int age = 14;

    public override string ToString() // You need override or it won't work on the individual load
    {
        return "Slot: " + saveSlot + "\nPlayer Name: " + playerName + "\nAge: " + age;
    }
}
