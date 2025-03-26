using UnityEngine;

public class SpamManager : MonoBehaviour
{
    [SerializeField] Transform parent;
    [SerializeField] GameObject[] objectPrefs;
    void Start()
    {
        if (parent == null) parent = transform;
        SpawnObject();
    }

    void SpawnObject()
    {
        foreach (var objPref in objectPrefs)
        {
            if(objPref == null) continue;
            
            var obj = Instantiate(objPref, parent);
            if (obj.TryGetComponent(out Canvas canvas))
            {
                canvas.renderMode = RenderMode.ScreenSpaceCamera;
                canvas.worldCamera = Camera.main;
            }
        }
    }
}
