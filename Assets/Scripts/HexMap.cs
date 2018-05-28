using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMap {

    Hex[][] hexes;

    public void Initialize(int width, int height, GameObject hexPrefab, float hexDiameter, float dstBetweenHexes = 0f)
    {
        if (width <= 0 || height <= 0)
            Debug.Log("Invalid parameters for hex map");
        else
        {

        }
    }

    public void GenerateNewHexes()
    {

    }
}
