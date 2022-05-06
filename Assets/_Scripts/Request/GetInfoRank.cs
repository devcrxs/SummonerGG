using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
[Serializable]
public struct JsonRankData
{
    public string tier;
    public string rank;
    public string queueType;
    public int leaguePoints;
    public int wins;
    public int losses;
}
public class GetInfoRank : MonoBehaviour
{
    public static GetInfoRank instance;
    [SerializeField]private List<JsonRankData> allRank;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public IEnumerator GetRankStats(string id)
    {
        var url = String.Format("https://{0}.api.riotgames.com/lol/league/v4/entries/by-summoner/{1}?api_key={2}",UIManager.instance.Region,id,UIManager.instance.APIKey);
        using (UnityWebRequest webRequestStats = UnityWebRequest.Get(url))
        {
            yield return webRequestStats.SendWebRequest();
            allRank = GetJsonRankData(webRequestStats);
            switch (true)
            {
                case true when allRank.Count > Constans.ZERO:
                    SearchRankedTft();
                    SearchRankeds5VS5();
                    break;
                default:
                    UIManager.instance.imageRankedSoloQ.sprite = UnrankedSoloQ();
                    UIManager.instance.imageRankedFlex.sprite = UnrankedFlex();
                    break;
            }
            AnimationsUI.instance.HideCharging();
            webRequestStats.Dispose();
        }
    }

    private void SearchRankedTft()
    {
        for (int i = 0; i < allRank.Count; i++)
        {
            if (allRank[i].queueType == Constans.RANKED_TFT)
            {
                allRank.Remove(allRank[i]);
            }
        }
    }
    
    private void SearchRankeds5VS5()
    {
        switch (true)
        {
            case true when allRank.Count > Constans.ZERO:
                SearchFirstRankedInList();
                break;
            default:
                UIManager.instance.imageRankedSoloQ.sprite = UnrankedSoloQ();
                UIManager.instance.imageRankedFlex.sprite = UnrankedFlex();
                break;
        }
    }

    private void SearchFirstRankedInList()
    {
        switch (allRank[Constans.ZERO].queueType)
        {
            case Constans.RANKED_SOLOQ:
                ShowSoloQInfo(Constans.ZERO);
                switch (true)
                {
                    case true when allRank.Count - Constans.ONE >= Constans.ONE:
                        ShowFlexInfo(Constans.ONE);
                        break;
                    default:
                        UIManager.instance.imageRankedFlex.sprite = UnrankedFlex();
                        break;
                }
                break;
            case Constans.RANKED_FLEX:
                ShowFlexInfo(Constans.ZERO);
                switch (true)
                {
                    case true when allRank.Count - Constans.ONE >= Constans.ONE:
                        ShowSoloQInfo(Constans.ONE);
                        break;
                    default:
                        UIManager.instance.imageRankedSoloQ.sprite = UnrankedSoloQ();
                        break;
                }
                break;
        }
    }

    private List<JsonRankData> GetJsonRankData(UnityWebRequest webRequest)
    {
        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            allRank = null;
            return JsonHelper.GetList<JsonRankData>(webRequest.downloadHandler.text);
        }

        return null;
    }

    private void ShowSoloQInfo(int positionSoloQInRank)
    {
        UIManager.instance.textSummonerLeagueSoloQ.text = GetLeagueText(positionSoloQInRank);
        UIManager.instance.textLeaguePointsSoloQ.text = GetPointsText(positionSoloQInRank);
        UIManager.instance.textWinLossesSoloQ.text = GetWinLossesText(positionSoloQInRank);
        UIManager.instance.imageRankedSoloQ.sprite = GetRankSprite(positionSoloQInRank);
        UIManager.instance.textWinrateSoloQ.text = GetWinRate(positionSoloQInRank);
    }

    private void ShowFlexInfo(int positionFlexInRank)
    {
        UIManager.instance.textSummonerLeagueFlex.text = GetLeagueText(positionFlexInRank);
        UIManager.instance.textLeaguePointsFlex.text = GetPointsText(positionFlexInRank);
        UIManager.instance.textWinLossesFlex.text = GetWinLossesText(positionFlexInRank);
        UIManager.instance.imageRankedFlex.sprite =  GetRankSprite(positionFlexInRank);
        UIManager.instance.textWinRateFlex.text = GetWinRate(positionFlexInRank);
    }
    private Sprite GetRankSprite(int index)
    {
        for (int i = 0; i < UIManager.instance.poolsImagesQueues.Count; i++)
        {
            if (allRank[index].tier == UIManager.instance.poolsImagesQueues[i].nameQueue)
            {
                return UIManager.instance.poolsImagesQueues[i].spirteQueue;
            }
        }

        return null;
    }

    private string GetLeagueText(int index)
    {
        return allRank[index].tier + " " + allRank[index].rank;
    }

    private string GetPointsText(int index)
    {
        return allRank[index].leaguePoints + "LP";
    }

    private string GetWinLossesText(int index)
    {
        return allRank[index].wins + "V/" + allRank[index].losses + "L";
    }

    private string GetWinRate(int index)
    {
        int totalMatches = allRank[index].wins + allRank[index].losses;
        int winrate = allRank[index].wins * 100 / totalMatches;
        return winrate + "%";
    }

    private Sprite UnrankedSoloQ()
    {
        UIManager.instance.textSummonerLeagueSoloQ.text = Constans.UNRANKED;
        UIManager.instance.textLeaguePointsSoloQ.text =  Constans.UNRANKED;
        UIManager.instance.textWinLossesSoloQ.text =  Constans.UNRANKED;
        UIManager.instance.textWinrateSoloQ.text = Constans.ZERO + "%";
        var imageUnranked = UIManager.instance.poolsImagesQueues.Count - Constans.ONE;
        return UIManager.instance.poolsImagesQueues[imageUnranked].spirteQueue;
    }

    private Sprite UnrankedFlex()
    {
        UIManager.instance.textSummonerLeagueFlex.text = Constans.UNRANKED;
        UIManager.instance.textLeaguePointsFlex.text = Constans.UNRANKED;
        UIManager.instance.textWinLossesFlex.text = Constans.UNRANKED;
        UIManager.instance.textWinRateFlex.text = Constans.ZERO + "%";
        var imageUnranked = UIManager.instance.poolsImagesQueues.Count - Constans.ONE;
        return UIManager.instance.poolsImagesQueues[imageUnranked].spirteQueue;
    }
}
