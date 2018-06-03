using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HEX_TYPE
{
    DEFAULT,
    ENUM_MAX
}

public struct Hex
{
    public Transform  refrenceTransform;
    public GameObject hexObject;
    public int        index_x;
    public int        index_y;
    public BIOME_TYPE biome;
    public HEX_TYPE type;

    public bool initialized;
}