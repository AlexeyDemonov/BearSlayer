using UnityEditor;


public class CustomHotkeysSetter
{
    [MenuItem("Edit/CustomHotkeys/PlayOrStop _F5")]
    static void PlayOrStopGame()
    {
        EditorApplication.ExecuteMenuItem("Edit/Play");
    }
}