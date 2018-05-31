using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public HexMapParameters mapParameters;

    public GameObject[] hexPrefabs;

    public GameObject hexMapParent;

    private HexMap hexMap;

    private void Awake()
    {
        hexMap = new HexMap(mapParameters, hexPrefabs, hexMapParent.transform);
    }

    private void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        hexMap.Initialize();
    }
}
