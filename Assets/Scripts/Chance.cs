using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chance : MonoBehaviour
{
    public float chance;
    public int firstSection = 0;

    public float GetChance()
    {
        return chance;
    }

    public float GetChance(int section)
    {
        if (section < firstSection)
        {
            return 0;
        }
        
        return chance;
    }
}
