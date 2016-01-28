/**********************************************************************
 * Sacred Seed Studio - Unity Namespace
 * Core Module
 * v0.1.0
 *
 * Here you will find a whole host of useful functionality including
 * - Saving/Loading Data
 * - TODO: Game Controller
 *
 * Created: August 2 2015
 * Modified: January 28 2016
 *
 * The only rules:
 * 1. If you modify something, verify its documentation is still valid.
 * 2. Write some tests.
 * 3. Update the Modified date.
 *********************************************************************/

using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace SSS
{
    namespace SaveLoad
    {
        public class SaveManager
        {
            private string applicationFileName;         // Example "SSSFarmer"
            private string fileExtension;               // Example "dat"

            /// <summary>
            /// Manages SaveData. Save slots are zero indexed.
            /// </summary>
            /// <param name="applicationFileName">The directory name your saves are in. Example "SSSFarmer"</param>
            /// <param name="fileExtension">The file extension your saves are. Example "dat"</param>
            public SaveManager(string applicationFileName, string fileExtension)
            {
                this.applicationFileName = applicationFileName;
                this.fileExtension = fileExtension;
            }

            /// <summary>
            /// Get the instance's example directory path
            /// </summary>
            /// <returns>A string of the save path</returns>
            public string GetDirectoryPath()
            {
                return Application.persistentDataPath + "/" + applicationFileName + "#." + fileExtension;
            }

            /// <summary>
            /// Get the full file path of a slot
            /// </summary>
            /// <param name="slot">the save slot for the path</param>
            /// <returns>The file path</returns>
            private string GetFilePath(int slot)
            {
                return Application.persistentDataPath + "/" + applicationFileName + slot + "." + fileExtension;
            }

            /// <summary>
            /// Checks to see if any save games already exist.
            /// </summary>
            /// <returns>If SaveData exists.</returns>
            public bool SavesExist()
            {
                try { if (File.Exists(GetFilePath(0))) { return true; } }
                catch (Exception e) { Debug.LogException(e); }
                return false;
            }

            /// <summary>
            /// Returns the maximum slot of gamesave. Ensure you verify existance with SavesExist() first.
            /// </summary>
            /// <returns>The maximum save slot stored.</returns>
            public int GetMaxSaveSlot()
            {
                int slot = 0;
                try
                {
                    while (File.Exists(GetFilePath(slot)))
                    {
                        slot++;
                    }
                }
                catch (Exception e) { Debug.LogException(e); }
                return slot - 1;
            }

            /// <summary>
            /// Create SaveData on the local file system.
            /// </summary>
            /// <param name="data">The SaveData to create, saveSlot will be automatically chosen</param>
            /// <returns>The slot the data was saved in</returns>
            public int Create(SaveData data)
            {
                int slot = 0;
                try
                {
                    // Get the save slot to use
                    if (SavesExist()) { slot = GetMaxSaveSlot() + 1; }
                    data.saveSlot = slot;
                    // Save to file
                    BinaryFormatter bf = new BinaryFormatter();
                    FileStream file = File.Create(GetFilePath(slot));
                    bf.Serialize(file, data);
                    file.Close();
                }
                catch (Exception e) { Debug.LogException(e); }
                return slot;
            }

            /// <summary>
            /// Load SaveData from the local file system.
            /// </summary>
            /// <param name="slot">The slot to load SaveData from</param>
            /// <returns>SaveData or null</returns>
            public SaveData Load(int slot)
            {
                try
                {
                    if (File.Exists(GetFilePath(slot)))
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        FileStream file = File.Open(GetFilePath(slot), FileMode.Open);
                        SaveData data = (SaveData)bf.Deserialize(file);
                        file.Close();
                        return data;
                    }
                    else { Debug.LogError("File doesn't exist: " + GetFilePath(slot)); }
                }
                catch (Exception e) { Debug.LogException(e); }
                return null;
            }

            /// <summary>
            /// Load all the SavaData on the local file system.
            /// </summary>
            /// <returns>An array of SaveData. If any are corrupt/missing the array will contain null elements.</returns>
            public SaveData[] LoadAll()
            {
                SaveData[] datum;
                try
                {
                    if (SavesExist())
                    {
                        int saveCount = GetMaxSaveSlot() + 1;
                        datum = new SaveData[saveCount];
                        BinaryFormatter bf = new BinaryFormatter();
                        FileStream file;
                        for (int i = 0; i < saveCount; i++)
                        {
                            if (File.Exists(GetFilePath(i)))
                            {
                                file = File.Open(GetFilePath(i), FileMode.Open);
                                datum[i] = (SaveData)bf.Deserialize(file);
                                file.Close();
                            }
                            else
                            {
                                Debug.LogError("File doesn't exist: " + GetFilePath(i));
                                datum[i] = null;
                            }
                        }
                        return datum;
                    }
                }
                catch (Exception e) { Debug.LogException(e); }

                Debug.LogError("An error with the data or no files exist for path: " + GetFilePath(0));
                datum = new SaveData[1];
                datum[0] = null;
                return datum;
            }

            /// <summary>
            /// Save SaveData to the local file system.
            /// </summary>
            /// <param name="data">The SavaData to save</param>
            public void Save(SaveData data)
            {
                try
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    FileStream file = File.Open(GetFilePath(data.saveSlot), FileMode.Open);
                    bf.Serialize(file, data);
                    file.Close();
                }
                catch (Exception e) { Debug.LogException(e); }
            }

            /// <summary>
            /// Delete SaveData from the local file system. All other SaveData slots will be adjusted
            /// </summary>
            /// <param name="slot">The slot to delete</param>
            public void Delete(int slot)
            {
                try
                {
                    if (File.Exists(GetFilePath(slot)))
                    {
                        SaveData[] existingData = LoadAll();
                        foreach (var item in existingData)
                        {
                            File.Delete(GetFilePath(item.saveSlot));
                        }
                        foreach (var item in existingData)
                        {
                            if (item != null && item.saveSlot != slot)
                            {
                                Create(item);
                            }
                        }
                    }
                    else { Debug.LogError("Can't delete non existent file: " + GetFilePath(slot)); }
                }
                catch (Exception e) { Debug.LogException(e); }
            }
        }//SaveManager

        /// <summary>
        /// Inherit this for your game's data structure
        /// </summary>
        [Serializable]
        public class SaveData
        {
            public int saveSlot = 0;
        }//SaveData
    }//SaveLoad
}//SSS