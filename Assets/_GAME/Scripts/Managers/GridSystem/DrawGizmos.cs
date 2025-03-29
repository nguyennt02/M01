using Unity.Mathematics;
using UnityEngine;

public static class DrawGizmos
{
    public static void DrawQuad(in float3 worldPos, in float2 size, in Color color)
    {
        var x = size.x / 2;
        var y = size.y / 2;

        var offset = worldPos;
        var pos1 = offset + new float3(-x, -y, 0);
        var pos2 = offset + new float3(-x, y, 0);
        var pos3 = offset + new float3(x, y, 0);
        var pos4 = offset + new float3(x, -y, 0);

        Debug.DrawLine(pos1, pos2, color);
        Debug.DrawLine(pos2, pos3, color);
        Debug.DrawLine(pos3, pos4, color);
        Debug.DrawLine(pos4, pos1, color);
    }

    public static void DrawString(string text, Vector3 worldPos, Color? color = null)
    {
#if UNITY_EDITOR
        UnityEditor.Handles.BeginGUI();

        var restoreColor = GUI.color;

        if (color.HasValue) GUI.color = color.Value;
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
}
