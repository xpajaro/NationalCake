using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class LocalStorage:  MonoBehaviour{

	public static LocalStorage Instance;
	static string FILE_PATH = Application.persistentDataPath + "/playerInfo.dat";

	void Awake() {
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad (gameObject);

		} else if (Instance != this) {
			Destroy (gameObject);
		}

	}

	public static void Save(Player playerData) {

		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Open (FILE_PATH, FileMode.OpenOrCreate, FileAccess.Write); 

		bf.Serialize(file, playerData);
		file.Close();

	}    

	public static Player Load(string gameToLoad) {
		Player playerData = null;

		if(File.Exists(FILE_PATH)) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(FILE_PATH, FileMode.Open);

			playerData = (Player)bf.Deserialize(file);
			file.Close();
		}

		return playerData;
	}
}
