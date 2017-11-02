using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using UnityEngine;

public class LocalStorage:  MonoBehaviour{

	public static LocalStorage Instance;
	string FILE_PATH;

	void Awake() {
		if (Instance == null) {
			Instance = this;
			FILE_PATH = Application.persistentDataPath + "/playerInfo.dat";

			DontDestroyOnLoad (gameObject);

		} else if (Instance != this) {
			Destroy (gameObject);
		}

	}

	public void Save(PlayerData playerData) {

		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Open (FILE_PATH, FileMode.OpenOrCreate, FileAccess.Write); 

		bf.Serialize(file, playerData);
		file.Close();

	}    

	public PlayerData Load() {
		PlayerData playerData = null;

		if(File.Exists(FILE_PATH)) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(FILE_PATH, FileMode.Open, FileAccess.Read);

			playerData = (PlayerData)bf.Deserialize(file);
			file.Close();
		}

		if (playerData == null) {
			playerData = new PlayerData ();
			playerData.LastLogin = DateTime.Now;
			playerData.Revenue = 30;
			Save (playerData);
		}

		return playerData;
	}
}
