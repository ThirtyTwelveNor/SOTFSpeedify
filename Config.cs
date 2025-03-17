using RedLoader;
using SonsSdk;

namespace Speedify;

public static class Config
{
    public static ConfigCategory SpeedifyConfig { get; private set; }

    public static ConfigEntry<float> SpeedifyTime { get; private set; }
    public static ConfigEntry<float> SpeedifyLength { get; private set; }
    public static ConfigEntry<bool> SpeedifyLengthRealTimeToggle { get; private set; }
    public static KeybindConfigEntry SpeedifyToggleKey { get; private set; }
    public static ConfigEntry<bool> SpeedifyToggle { get; private set; }


    public static void Init()
    {
        SpeedifyConfig = ConfigSystem.CreateFileCategory("Speedify", "Speedify", "Speedify.cfg");

        SpeedifyTime = SpeedifyConfig.CreateEntry(
            "speedify_time",
            1f,
            "Speed: Default: 1",
            "Multiply game speed by: Default: 1"
        );
        SpeedifyTime.DefaultValue = 1f;
        SpeedifyTime.SetRange(0f, 50f);

        SpeedifyLength = SpeedifyConfig.CreateEntry(
            "speedify_length",
            0f,
            "Disable after minutes: Default: 0",
            "Auto Disable speed after defined minutes: Default: 0"
        );
        SpeedifyLength.DefaultValue = 0f;
        SpeedifyLength.SetRange(0f, 1440f);


        SpeedifyLengthRealTimeToggle = SpeedifyConfig.CreateEntry(
            "speedify_length-realtimetoggle",
            false,
            "Length in real time: Default: false",
            "Should Length be Realtime(else ingame time): Default: false"
        );
        SpeedifyLengthRealTimeToggle.DefaultValue = false;

        SpeedifyToggleKey = SpeedifyConfig.CreateKeybindEntry(
            "speedify_toggle_key",
            "numpad6",
            "Toggle Speedify: Default: numpad6",
            "Hotkey for Speedify Toggle: Default: numpad6"
        );
        SpeedifyToggleKey.DefaultValue = "numpad6";
        SpeedifyToggleKey.Notify(() =>
        {
            Speedify.Run();
        });


        SpeedifyToggle = SpeedifyConfig.CreateEntry(
            "speedify_toggle",
            false,
            "Manual speed On/Off: Default: false",
            "Internal Turn on or off speed: Default: false"
        );
        SpeedifyToggle.DefaultValue = false;
    }

    // Same as the callback in "CreateSettings". Called when the settings ui is closed.
    public static void OnSettingsUiClosed()
    {
    }
}