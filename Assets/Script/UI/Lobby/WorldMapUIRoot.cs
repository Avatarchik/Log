using UnityEngine;
using System.Collections;

public class WorldMapUIRoot : SequenceController {
    public static WorldMapUIRoot Instance;
    void Awake()
    {
        Instance = this;
    }    
}
