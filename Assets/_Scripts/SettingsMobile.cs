using UnityEngine;
public class SettingsMobile : MonoBehaviour
{
    private int frameRate = 60;
    void Start()
    {
        Application.targetFrameRate = frameRate;
        Screen.SetResolution(Screen.width,Screen.height,true);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
}
