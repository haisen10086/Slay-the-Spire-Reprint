using UnityEngine;

public static class ColorUtil
{
    public static string ColorText(string text, string colorName)
    {
        return $"<color={colorName}>{text}</color>";
    }
}
