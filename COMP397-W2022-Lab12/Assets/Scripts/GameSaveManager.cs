using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class GameSaveManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private PlayerDataScriptableObject PlayerDataSO;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            SaveGame();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadGame();
        }
    }
    void SaveGame()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                BinaryFormatter bf = new BinaryFormatter(); 
                FileStream file = File.Create(Application.persistentDataPath + "/MySaveData.dat"); 
                PlayerData data = new PlayerData();
                
                data.position = new[] {player.position.x, player.position.y, player.position.z};
                data.rotation = new[] {player.rotation.eulerAngles.x, player.rotation.eulerAngles.y, player.rotation.eulerAngles.z};

                bf.Serialize(file, data);
                file.Close();
                Debug.Log("Game data saved!");
                break;
            case RuntimePlatform.WebGLPlayer:
            case RuntimePlatform.WindowsEditor:
                string playerPosition = JsonUtility.ToJson(player.position);
                string playerRotation = JsonUtility.ToJson(player.rotation.eulerAngles);

                PlayerPrefs.SetString("PlayerPosition", playerPosition);
                PlayerPrefs.SetString("PlayerRotation", playerRotation);
                PlayerPrefs.Save();
                Debug.Log("Game data saved!");
                break;
        }

        // scriptable object
        PlayerDataSO.position = player.position;
        PlayerDataSO.rotation = player.rotation;
        PlayerDataSO.health = 50;
        // PlayerDataSO.name = "Player 1";
    }
    void LoadGame()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                if (File.Exists(Application.persistentDataPath + "/MySaveData.dat"))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    FileStream file = File.Open(Application.persistentDataPath + "/MySaveData.dat", FileMode.Open);
                    PlayerData data = (PlayerData)bf.Deserialize(file);
                    file.Close();

                    player.gameObject.GetComponent<CharacterController>().enabled = false;

                    player.position = new Vector3(data.position[0], data.position[1], data.position[2]);
                    player.rotation = Quaternion.Euler(data.rotation[0], data.rotation[1], data.rotation[2]);

                    player.gameObject.GetComponent<CharacterController>().enabled = true;
                    Debug.Log("Game data loaded!");
                }
                else
                {
                    Debug.LogError("There is no save data!");
                }
                break;
            case RuntimePlatform.WebGLPlayer:
            case RuntimePlatform.WindowsEditor:
                if (PlayerPrefs.HasKey("PlayerPosition"))
                {
                    player.gameObject.GetComponent<CharacterController>().enabled = false;

                    player.position = JsonUtility.FromJson<Vector3>(PlayerPrefs.GetString("PlayerPosition"));
                    player.rotation = Quaternion.Euler(JsonUtility.FromJson<Vector3>(PlayerPrefs.GetString("PlayerRotation")));

                    player.gameObject.GetComponent<CharacterController>().enabled = true;
                    Debug.Log("Game data loaded!");
                }
                else
                {
                    Debug.LogError("There is no save data!");
                }
                break;
        }

    }

    void ResetData()
    {
        if (File.Exists(Application.persistentDataPath + "/MySaveData.dat"))
        {
            File.Delete(Application.persistentDataPath + "/MySaveData.dat");
            Debug.Log("Data reset complete!");
        }
        else
        {
            Debug.LogError("No save data to delete.");   
        }
    }

    public void OnSaveButtonPressed()
    {
        SaveGame();
    }

    public void OnLoadButtonPressed()
    {
        LoadGame();        
    }
}
