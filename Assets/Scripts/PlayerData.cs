using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PlayerData{

	public Data data;
	BinaryFormatter bf;

	public PlayerData()
	{
		data = new Data ();
		bf = new BinaryFormatter ();
	}

	public void Save()
	{
		FileStream file = File.Create(Application.persistentDataPath+"/playerData.dat");
		bf.Serialize (file, data);
		file.Close ();
	}

	public bool Load()
	{
		if (File.Exists (Application.persistentDataPath + "/playerData.dat"))
		{
			FileStream file = File.Open (Application.persistentDataPath + "/playerData.dat",FileMode.Open);
			data = (Data)bf.Deserialize (file);
			return true;
		} else {
			//If file doesn't exist then return false
			return false;
		}
	}
}

[Serializable]
public class Data{
	private int highScore;
	private bool adWatched;

	public int HighScore {
		get{ 
			return highScore; 
		}
		set{
			highScore = value;
		}
	}

	public bool AdWatched{
		get{
			return adWatched;
		}
		set{
			adWatched = value;
		}
	}
}

