using UnityEngine;

public class RendererManager : MonoBehaviour
{
    public static RendererManager Instance { get; private set; }
    [SerializeField] Theme theme;
    void Start()
    {
        Instance = this;
    }

    public Color GetColorAt(int index)
    {
        return theme.colors[index];
    }
}



