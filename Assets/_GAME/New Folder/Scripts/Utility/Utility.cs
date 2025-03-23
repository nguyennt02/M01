using Unity.Mathematics;
using UnityEngine;

public static class Utility
{
    public enum ColorIndex
    {
        None,
        White,
        Blue,
        Red,
        Green,
        Yellow,
        Cyan,
    }

    public static Color GetColorFrom(in int colorValue = 0)
    {
        var colorIndex = (ColorIndex)colorValue;
        var color = Color.white;
        if (colorIndex == ColorIndex.Blue)
            color = Color.blue;
        else if (colorIndex == ColorIndex.Red)
            color = Color.red;
        else if (colorIndex == ColorIndex.Green)
            color = Color.green;
        else if (colorIndex == ColorIndex.Yellow)
            color = Color.yellow;
        else if (colorIndex == ColorIndex.Cyan)
            color = Color.cyan;
        return color;
    }



    public static Vector2 Rotate(Vector2 v, float delta)
    {
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
    }

    public static float3x3 GetMatrixWith(float degAroundX)
    {
        var o = degAroundX * math.PI / 180;
        var c1 = new float3(1, 0, 0);
        var c2 = new float3(0, math.cos(o), math.sin(o));
        var c3 = new float3(0, -math.sin(o), math.cos(o));
        return new float3x3(c1, c2, c3);
    }

    static public void DrawString(string text, Vector3 worldPos, Color? colour = null)
    {
#if UNITY_EDITOR
        UnityEditor.Handles.BeginGUI();

        var restoreColor = GUI.color;

        if (colour.HasValue) GUI.color = colour.Value;
        var view = UnityEditor.SceneView.currentDrawingSceneView;
        Vector3 screenPos = view.camera.WorldToScreenPoint(worldPos);

        if (screenPos.y < 0 || screenPos.y > Screen.height || screenPos.x < 0 || screenPos.x > Screen.width || screenPos.z < 0)
        {
            GUI.color = restoreColor;
            UnityEditor.Handles.EndGUI();
            return;
        }

        Vector2 size = GUI.skin.label.CalcSize(new GUIContent(text));
        GUI.Label(new Rect(screenPos.x - (size.x / 2), -screenPos.y + view.position.height - 35, size.x, size.y), text);
        GUI.color = restoreColor;
        UnityEditor.Handles.EndGUI();
#endif
    }

    public static void DrawQuad(in float3 worldPos, in float2 size, float degAroundX = 0, in int colorIndex = 0)
    {
#if UNITY_EDITOR
        var color = GetColorFrom(colorIndex);

        var x = size.x / 2;
        var y = size.y / 2;

        var rotatedMatrix = GetMatrixWith(degAroundX);

        var offset = worldPos;
        var pos1 = offset + math.mul(new float3(-x, -y, 0), rotatedMatrix);
        var pos2 = offset + math.mul(new float3(-x, y, 0), rotatedMatrix);
        var pos3 = offset + math.mul(new float3(x, y, 0), rotatedMatrix);
        var pos4 = offset + math.mul(new float3(x, -y, 0), rotatedMatrix);

        Debug.DrawLine(pos1, pos2, color);
        Debug.DrawLine(pos2, pos3, color);
        Debug.DrawLine(pos3, pos4, color);
        Debug.DrawLine(pos4, pos1, color);
#endif
    }

    public static void DrawLine(in float3 start, in float3 end, in int colorIndex = 0)
    {
#if UNITY_EDITOR
        var color = GetColorFrom(colorIndex);

        Debug.DrawLine(start, end, color);
#endif
    }

    public static void DrawRay(in float3 start, in float3 dir, in int colorIndex = 0)
    {
#if UNITY_EDITOR
        var color = GetColorFrom(colorIndex);

        Debug.DrawRay(start, dir, color);
#endif
    }
}