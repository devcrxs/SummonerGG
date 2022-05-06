using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
public struct JsonData
{
    public string id;
    public string profileIconId;
    public string name;
    public string summonerLevel;
}
public class GetInfoBasic : MonoBehaviour
{
    private JsonData dataSummoner;
    public static GetInfoBasic instance;
    private void Awake()
    {
        if (instance == null) instance = this;
    }
    
    public IEnumerator GetBasicInfo(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            AnimationsUI.instance.ShowCharging();
            yield return webRequest.SendWebRequest();
            switch (true)
            {
                case true when webRequest.result == UnityWebRequest.Result.Success:
                    dataSummoner = JsonUtility.FromJson<JsonData>(webRequest.downloadHandler.text);
                    UIManager.instance.textSummonerLevel.text = dataSummoner.summonerLevel;
                    UIManager.instance.textSummonerName.text = dataSummoner.name;
                    StartCoroutine(GetIconSummoner(dataSummoner.profileIconId));
                    break;
                default:
                    AnimationsUI.instance.HideCharging();
                    AnimationsUI.instance.ShowPopError();
                    break;
            }
            webRequest.Dispose();
        }
    }

    private IEnumerator GetIconSummoner(string profileIconId)
    {
        var url = String.Format("http://ddragon.leagueoflegends.com/cdn/12.2.1/img/profileicon/{0}.png", profileIconId);
        using (UnityWebRequest webRequestTexture = UnityWebRequestTexture.GetTexture(url))
        {
            yield return webRequestTexture.SendWebRequest();
            UIManager.instance.imageIconSummoner.texture = GetTextureIcon(webRequestTexture);
            webRequestTexture.Dispose();
        }
    }

    private Texture GetTextureIcon(UnityWebRequest webRequest)
    {
        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            StartCoroutine(GetInfoRank.instance.GetRankStats(dataSummoner.id));
            return ((DownloadHandlerTexture) webRequest.downloadHandler).texture;
        }

        return null;
    }
}
