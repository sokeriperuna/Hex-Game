using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct HexMapParameters
{
    public int width;
    public int height;

    public float hexRadius;
    public float dstBetweenHexes;
}

public enum HEX_DIRECTION { TOP_LEFT, TOP_RIGHT, LEFT, RIGHT, BOTTOM_LEFT, BOTTOM_RIGHT }

public class HexMap {

    private Hex[,] hexes;

    private GameObject[] hexPrefabs;

    private int width;
    private int height;

    private float hexRadius;
    private float dstBetweenHexes;

    private bool mapInitialized;

    private Transform hexParent;

    private void SetNewParameters(HexMapParameters parameters)
    {
        width           = parameters.width;
        height          = parameters.height;
        hexRadius       = parameters.hexRadius;
        dstBetweenHexes = parameters.dstBetweenHexes;
    }

    private void InitializeHex(int x, int y, float hexWidth, Quaternion rotation)
    {
        float xDisplacement;
        float zDisplacement;

        Vector3 spawnPosition;

        hexes[x, y].index_x = x;
        hexes[x, y].index_y = y;

        zDisplacement = ((hexRadius + hexRadius * Mathf.Sin(30 * Mathf.Deg2Rad)) * y) + (dstBetweenHexes + (dstBetweenHexes * y));

        if ((y % 2) == 0)
            xDisplacement = (hexWidth * 2 * x) + (dstBetweenHexes * 2 * x);
        else
            xDisplacement = (hexWidth + (hexWidth * 2 * x)) + (dstBetweenHexes + (dstBetweenHexes * 2 * x));

        spawnPosition = hexParent.position + new Vector3(xDisplacement, 0f, zDisplacement);

        hexes[x, y].refrenceTransform = GameObject.Instantiate(hexPrefabs[(int)HexType.REF_TRANSORM], spawnPosition, rotation, hexParent).transform;
    }

    public HexMap(HexMapParameters new_parameters, GameObject[] new_hexPrefabs, Transform new_hexParent)
    {
        SetNewParameters(new_parameters);
        hexPrefabs = new_hexPrefabs;
        hexParent  = new_hexParent;
    }

    public Hex GetHex(Hex hex, HEX_DIRECTION direction)
    {
        switch (direction)
        {
            case HEX_DIRECTION.TOP_LEFT:
                return GetHex(hex.index_x - 1, hex.index_y + 1);

            case HEX_DIRECTION.TOP_RIGHT:
                return GetHex(hex.index_x, hex.index_y + 1);

            case HEX_DIRECTION.LEFT:
                return GetHex(hex.index_x - 1, hex.index_y);

            case HEX_DIRECTION.RIGHT:
                return GetHex(hex.index_x + 1, hex.index_y);

            case HEX_DIRECTION.BOTTOM_LEFT:
                return GetHex(hex.index_x - 1, hex.index_y - 1);

            case HEX_DIRECTION.BOTTOM_RIGHT:
                return GetHex(hex.index_x + 1, hex.index_y - 1);

            default:
                return GetHex(hex.index_x, hex.index_y);
        }
    }

    public Hex GetHex(int x, int y)
    {

        return new Hex();
    }

    public void Initialize()
    {
        if (width <= 0 || height <= 0)
            Debug.LogError("Invalid parameters for hex map");
        else
        {
            hexes = new Hex[width, height];

            Quaternion hexRotation  = new Quaternion(); // Rotation to spawn the hexes in the right alignment
            hexRotation.eulerAngles = new Vector3(-90f, 0f, 0f);

            float temp_hexWidth = Mathf.Cos(30f * Mathf.Deg2Rad) * hexRadius;

            int x = 0;
            while(x < width)
            {
                InitializeHex(x, 0, temp_hexWidth, hexRotation);
                int y = 1;
                while (y < height)
                {
                    InitializeHex(x, y, temp_hexWidth, hexRotation);
                    y++;
                }
                x++;
            }

            Debug.Log("Hex map initialized.");
            mapInitialized = true;
        }
    }

    public bool Initialized
    {
        get { return mapInitialized; }
    }

}
