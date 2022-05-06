using System.Collections.Generic;
public class JsonHelper
{
	public static List<T> GetList<T> (string json)
	{
		string newJson = "{\"data\":" + json + "}";
		Wrapper<T> w = UnityEngine.JsonUtility.FromJson<Wrapper<T>> (newJson);
		return w.data;
	}

	[System.Serializable] class Wrapper<T>
	{
		public List<T> data;
	}
}
