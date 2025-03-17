using RedLoader;
using SonsSdk;
using SUI;
using System.Collections;
using UnityEngine;

namespace Speedify;

public class Speedify : SonsMod
{
    public Speedify()
    {

        // Uncomment any of these if you need a method to run on a specific update loop.
        OnUpdateCallback = OnUpdateMethod;
        //OnLateUpdateCallback = MyLateUpdateMethod;
        //OnFixedUpdateCallback = MyFixedUpdateMethod;
        //OnGUICallback = MyGUIMethod;

        // Uncomment this to automatically apply harmony patches in your assembly.
        //HarmonyPatchAll = true;
    }

    protected override void OnInitializeMod()
    {
        // Do your early mod initialization which doesn't involve game or sdk references here
        Config.Init();
    }

    protected override void OnSdkInitialized()
    {
        // Do your mod initialization which involves game or sdk references here
        // This is for stuff like UI creation, event registration etc.
        SpeedifyUi.Create();

        // Add in-game settings ui for your mod.
        SettingsRegistry.CreateSettings(this, null, typeof(Config));

        Instance = this;
    }

    protected override void OnGameStart()
    {
        // This is called once the player spawns in the world and gains control.
    }
    public static Speedify Instance { get; private set; }
    private static Coroutines.CoroutineToken speedifyCoroutine;
    private static float userConfigSpeedifyTime;
    private static bool isUsingCustomSpeed = false;

    public static void Run(float? _SpeedifyTime = null, float? _SpeedifyLength = null, float? _SpeedifyResetSpeed = null)
    {
        float SpeedifyTime = _SpeedifyTime ?? Config.SpeedifyTime.Value;
        float SpeedifyLength = _SpeedifyLength ?? Config.SpeedifyLength.Value;
        float SpeedifyResetSpeed = _SpeedifyResetSpeed ?? Config.SpeedifyTime.DefaultValue;
        if (speedifyCoroutine != null)
        {
            speedifyCoroutine.Stop();
            speedifyCoroutine = null;
        }
        //If we turning on speedify
        if (!Config.SpeedifyToggle.Value)
        {
            if (SpeedifyTime != Config.SpeedifyTime.Value)
            {
                userConfigSpeedifyTime = Config.SpeedifyTime.Value;
                Config.SpeedifyTime.Value = SpeedifyTime;
                isUsingCustomSpeed = true;
            }
            else
            {
                isUsingCustomSpeed = false;
            }
            if (SpeedifyLength != 0f)
            {
                speedifyCoroutine = SpeedifyLengthCoro(SpeedifyLength).RunCoro();
            }
        }
        // If we're turning speedify off
        else
        {
            SetTime(SpeedifyResetSpeed);
            if (isUsingCustomSpeed)
            {
                Config.SpeedifyTime.Value = userConfigSpeedifyTime;
                isUsingCustomSpeed = false;
            }
        }

        Config.SpeedifyToggle.Value = !Config.SpeedifyToggle.Value;
    }
    public static void OnUpdateMethod()
    {
        if(Config.SpeedifyToggle.Value)
        {
            SetTime(Config.SpeedifyTime.Value);
        }
    }
    public static void SetTime(float time)
    {
        Time.timeScale = time;
    }
    public static IEnumerator SpeedifyLengthCoro(float SpeedifyLength)
    {
        yield return Config.SpeedifyLengthRealTimeToggle.Value
            ? new WaitForSecondsRealtime(SpeedifyLength * 60f)
            : new WaitForSeconds(SpeedifyLength * 60f);
        if (!Config.SpeedifyToggle.Value) { yield break; }
        Run(null, 0f);
    }
}