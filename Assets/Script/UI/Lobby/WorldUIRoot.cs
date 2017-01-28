using UnityEngine;
using System.Collections;

public class WorldUIRoot : SequenceController {
    public static WorldUIRoot Instance;

    [HideInInspector]
    public Camera kCamera;

    void Awake()
    {
        Instance = this;

        kCamera = transform.Find("Camera").GetComponent<Camera>();
    }    
}
