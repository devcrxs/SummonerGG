using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    [Serializable]
    public class PoolImagesQueue
    {
        public string nameQueue;
        public Sprite spirteQueue;
    }
    
    private string url;
    private string region;
    public static UIManager instance;
    [SerializeField] private string apiKey;
    public InputField nameSummoner;
    public RawImage imageIconSummoner;
    public Image imageRankedSoloQ;
    public Image imageRankedFlex;
    public Text textSummonerName;
    public Text textSummonerLevel;
    public Text textSummonerLeagueSoloQ;
    public Text textSummonerLeagueFlex;
    public Text textLeaguePointsSoloQ;
    public Text textLeaguePointsFlex;
    public Text textWinLossesSoloQ;
    public Text textWinLossesFlex;
    public Text textActualRegion;
    public Text textWinrateSoloQ;
    public Text textWinRateFlex;
    public List<PoolImagesQueue> poolsImagesQueues;
    public string APIKey => apiKey;
    public string Region => region;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        textActualRegion.text = GetDefaultRegion();
    }

    private string GetDefaultRegion()
    {
        region = Constans.REGION_DEFAULT;
        return region;
    }

    public void SearchSummoner()
    {
        url = String.Format("https://{0}.api.riotgames.com/lol/summoner/v4/summoners/by-name/{1}?api_key={2}",region,nameSummoner.text,apiKey);
        AnimationsUI.instance.JuicyButtonSearchSummoner();
        StartCoroutine(GetInfoBasic.instance.GetBasicInfo(url));
    }

    public void ChangeRegion(string region)
    {
        this.region = region;
        textActualRegion.text = this.region;
        AnimationsUI.instance.CanvasSelectRegion();
    }
}
