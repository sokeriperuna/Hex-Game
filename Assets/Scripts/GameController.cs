using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public HexMapParameters mapParameters;

    public GameObject hexTransformRefrence;
    public GameObject hexMapParent;

    public BiomeParameters[] biomeParameters;

    private HexMap hexMap;

    private void Awake()
    {
        hexMap = new HexMap(mapParameters, biomeParameters, hexMapParent.transform, hexTransformRefrence);
    }

    private void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        hexMap.Initialize();
        hexMap.GenerateNewHexes();
    }
}
