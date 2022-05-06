using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class AnimationsUI : MonoBehaviour
{
    private bool isShowCanvasSearchSummoner;
    private bool isShowCanvasSelectRegion;
    private float defaultPositionYCanvasSearchSummoner;
    private float maxPositionYCanvasSearchSummoner = 140;
    private float maxPositionYCanvasPopError = 500;
    private float timeMove = 0.3f;
    private float timeScale = 0.2f;
    private float timeFade = 0.2f;
    private float timeFadePopError = 0.5f;
    private float timeRotation = 0.3f;
    public static AnimationsUI instance;
    [SerializeField] private GameObject canvasCharging;
    [SerializeField] private Transform buttonSerchSummoner;
    [SerializeField] private Transform canvasSearchSummoner;
    [SerializeField] private Transform buttonSelectRegion;
    [SerializeField] private Transform canvasSelectRegion;
    [SerializeField] private Transform canvasPopError;
    [SerializeField] private Image backgroundPopError;
    [SerializeField] private Image[] imagesSelectRegion;
    [SerializeField] private Image[] imagesPopError;
    [SerializeField] private Image[] imagesCharging;
    private void Awake()
    {
        if (instance == null) instance = this;
    }
    private void Start()
    {
        DOTween.Init();
        defaultPositionYCanvasSearchSummoner = canvasSearchSummoner.localPosition.y;
        DoScaleY(canvasSelectRegion,Constans.ZERO,Constans.ZERO);
        SetBackgrounds(imagesSelectRegion,Constans.ZERO,Constans.ZERO);
        DoMoveY(canvasPopError,-maxPositionYCanvasPopError,Constans.ZERO);
        SetBackgrounds(imagesPopError,Constans.ZERO,Constans.ZERO);
        DoFade(backgroundPopError,Constans.ZERO,Constans.ZERO);
        SetRaycastPopError(false);
        SetBackgrounds(imagesCharging,Constans.ZERO,Constans.ZERO);
        canvasCharging.SetActive(false);
    }

    private void SetRaycastPopError(bool valueRaycast)
    {
        foreach (var image in imagesPopError)
        {
            image.raycastTarget = valueRaycast;
        }
        backgroundPopError.raycastTarget = valueRaycast;
    }

    public void CanvasSearchSummoner()
    {
        isShowCanvasSearchSummoner = !isShowCanvasSearchSummoner;
        if (isShowCanvasSearchSummoner)
        {
            DoMoveY(canvasSearchSummoner,maxPositionYCanvasSearchSummoner,timeMove);
            return;
        }
        DoMoveY(canvasSearchSummoner,defaultPositionYCanvasSearchSummoner,timeMove);
    }

    public void CanvasSelectRegion()
    {
        isShowCanvasSelectRegion = !isShowCanvasSelectRegion;
        if (isShowCanvasSelectRegion)
        {
            Vector3 rotation = new Vector3(Constans.ZERO,Constans.ZERO,-90);
            DoRotate(buttonSelectRegion,rotation,timeRotation);
            DoScaleY(canvasSelectRegion,Constans.ONE,timeScale);
            SetBackgrounds(imagesSelectRegion,Constans.ONE,timeFade);
            return;
        }
        HideSelectArea();
    }

    private void HideSelectArea()
    {
        DoRotate(buttonSelectRegion,Vector3.zero,timeRotation);
        DoScaleY(canvasSelectRegion,Constans.ZERO,timeScale);
        SetBackgrounds(imagesSelectRegion,Constans.ZERO,timeFade);
    }

    public void ShowPopError()
    {
        DoMoveY(canvasPopError,Constans.ZERO,timeMove);
        SetBackgrounds(imagesPopError,Constans.ONE,timeFade);
        DoFade(backgroundPopError,timeFadePopError,timeFade);
        SetRaycastPopError(true);
    }

    public void HidePopError()
    {
        DoMoveY(canvasPopError,-maxPositionYCanvasPopError,timeMove);
        SetBackgrounds(imagesPopError,Constans.ZERO,timeFade);
        DoFade(backgroundPopError,Constans.ZERO,timeFade);
        SetRaycastPopError(false);
    }

    public void ShowCharging()
    {
        canvasCharging.SetActive(true);
        SetBackgrounds(imagesCharging,Constans.ONE,timeFade);
    }

    public void HideCharging()
    {
        SetBackgrounds(imagesCharging,Constans.ZERO,timeFade);
        canvasCharging.SetActive(false);
    }

    public void JuicyButtonSearchSummoner()
    {
        var factorScale = 1.1f;
        buttonSerchSummoner.DOScale(Vector3.one / factorScale,timeScale)
            .OnComplete(() => buttonSerchSummoner.DOScale(Vector3.one, timeScale));
    }
    private void SetBackgrounds(Image[] targets, float endValue, float duration)
    {
        foreach (var image in targets)
        {
            DoFade(image,endValue,duration);
        }
    }
    
    private void DoMoveY(Transform target, float endValue, float duration)
    {
        target.DOLocalMoveY(endValue, duration);
    }

    private void DoScaleY(Transform target, float endValue, float duration)
    {
        target.DOScaleY(endValue, duration);
    }

    private void DoFade(Image target, float endValue, float duration)
    {
        target.DOFade(endValue, duration);
    }

    private void DoRotate(Transform target, Vector3 rotation, float duration)
    {
        target.DOLocalRotate(rotation, duration);
    }
}
