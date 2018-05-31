using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HexType
{
    REF_TRANSORM,
}

public struct Hex
{
    public Transform  refrenceTransform;
    public GameObject hexObject;
    public HexType    type;
    public int        index_x;
    public int        index_y;
}