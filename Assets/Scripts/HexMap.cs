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

[System.Serializable]
public struct BiomeParameters
{
    public string name;
    public GameObject[] hexPrefabs;
    public BIOME_TYPE type;
}

public enum BIOME_TYPE
{
    PLAIN,
    DESERT,
    VOLCANIC,
    EMUM_MAX
}

public enum HEX_DIRECTION { TOP_LEFT, TOP_RIGHT, LEFT, RIGHT, BOTTOM_LEFT, BOTTOM_RIGHT }

public class HexMap {

    private Hex[,] hexes;

    private BiomeParameters[] biomeParameters;

    private int width;
    private int height;

    private float hexRadius;
    private float dstBetweenHexes;

    private bool mapInitialized;

    private GameObject hexTransformRefrence;

    private Transform hexParent;

    private BIOME_TYPE currentBiome;

    private void SetNewParameters(HexMapParameters parameters)
    {
        width           = parameters.width;
        height          = parameters.height;
        hexRadius       = parameters.hexRadius;
        dstBetweenHexes = parameters.dstBetweenHexes;
    }

    private void InitializeHex(int x, int y, float hexWidth)
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

        hexes[x, y].refrenceTransform = GameObject.Instantiate(hexTransformRefrence, spawnPosition, Quaternion.identity, hexParent).transform;
        hexes[x, y].initialized = true;
    }

    private BiomeParameters GetBiomeParameters(BIOME_TYPE biomeType)
    {
        foreach (BiomeParameters biomeParameter in biomeParameters)
            if (biomeParameter.type == biomeType)
                return biomeParameter;

        Debug.LogError("Trying to use incorrect biome type.");
        return new BiomeParameters();
    }

    private void SpawnHex(ref Hex hex)
    {
        Quaternion hexRotation = new Quaternion(); // Rotation to spawn the hexes in the right alignment
        hexRotation.eulerAngles = new Vector3(-90.0f, 0f, 180f);

        BiomeParameters biomeParameters = GetBiomeParameters(hex.biome);

        GameObject newHex = biomeParameters.hexPrefabs[0];

        hex.hexObject = GameObject.Instantiate(newHex, hex.refrenceTransform.position, hexRotation, hex.refrenceTransform);
    }

    public HexMap(HexMapParameters new_parameters, BiomeParameters[] new_biomeParameters, Transform new_hexParent, GameObject new_hexTransformRefrence)
    {
        hexTransformRefrence = new_hexTransformRefrence;
        biomeParameters = new_biomeParameters;
        SetNewParameters(new_parameters);
        hexParent  = new_hexParent;
    }

    public bool GetHex(int x, int y, out Hex outputHex)
    {
        if ((x < 0 || x >= width) || (y < 0 || y >= height))
        {
            outputHex = new Hex();
            return false;
        }
        else
        {
            outputHex = hexes[x, y];
            return true;
        }
    }

    public bool GetHex(Hex inputHex, HEX_DIRECTION direction, out Hex outputHex)
    {
        outputHex = new Hex();

        switch (direction)
        {
            case HEX_DIRECTION.TOP_LEFT:
                return GetHex(inputHex.index_x - 1, inputHex.index_y + 1, out outputHex);

            case HEX_DIRECTION.TOP_RIGHT:
                return GetHex(inputHex.index_x, inputHex.index_y + 1, out outputHex);

            case HEX_DIRECTION.LEFT:
                return GetHex(inputHex.index_x - 1, inputHex.index_y, out outputHex);

            case HEX_DIRECTION.RIGHT:
                return GetHex(inputHex.index_x + 1, inputHex.index_y, out outputHex);

            case HEX_DIRECTION.BOTTOM_LEFT:
                return GetHex(inputHex.index_x - 1, inputHex.index_y - 1, out outputHex);

            case HEX_DIRECTION.BOTTOM_RIGHT:
                return GetHex(inputHex.index_x, inputHex.index_y - 1, out outputHex);

            default:
                return GetHex(inputHex.index_x, inputHex.index_y, out outputHex);
        }
    }

    /// <summary>
    /// WORK IN PROGRES.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="radius"></param>
    /// <returns></returns>
    public List<Hex> GetHexesInArea(Hex center, int radius)
    {
        return GetHexesInArea(center.index_x, center.index_y, radius);
    }

    /// <summary>
    /// WORK IN PROGRES.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="radius"></param>
    /// <returns></returns>
    public List<Hex> GetHexesInArea(int x, int y, int radius)
    {
        List<Hex> output = new List<Hex>();

        return output;
    }

    public void Initialize()
    {
        if (width <= 0 || height <= 0)
            Debug.LogError("Invalid parameters for hex map.");
        else
        {
            hexes = new Hex[width, height];

            float temp_hexWidth = Mathf.Cos(30f * Mathf.Deg2Rad) * hexRadius;

            int x = 0;
            while(x < width)
            {
                InitializeHex(x, 0, temp_hexWidth);
                int y = 1;
                while (y < height)
                {
                    InitializeHex(x, y, temp_hexWidth);
                    y++;
                }
                x++;
            }

            Debug.Log("Hex map initialized.");
            mapInitialized = true;
        }
    }

    public void GenerateNewHexes()
    {
        currentBiome = ((BIOME_TYPE)Random.Range(0, ((int)BIOME_TYPE.EMUM_MAX)));

        int x = 0;
        while (x < width)
        {
            hexes[x, 0].type  = HEX_TYPE.DEFAULT;
            hexes[x, 0].biome = currentBiome;

            SpawnHex(ref hexes[x, 0]);

            int y = 1;
            while(y < height)
            {
                hexes[x, y].type = HEX_TYPE.DEFAULT;
                hexes[x, y].biome = currentBiome;

                SpawnHex(ref hexes[x, y]);

                y++;
            }
            x++;
        }
    }

    public bool Initialized
    {
        get { return mapInitialized; }
    }

}
