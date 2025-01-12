using UnityEngine;

public enum TYPEBLOCK
{
    BLUE,
    BROWN,
    GREEN,
    ORANGE,
    PINK,
    PURPLE,
    RED,
    YELLOW,
    DESTRUCTIBLEBLOCK = 11,
    SPECIALSUBBLOCK = 12,
}

public struct Block
{
    public int index;
    public int[] subBlockIndex;
}

public class LevelEdittor : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
