using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onur.Template;

[CreateAssetMenu(fileName = "LevelProperties", menuName = "OnurTemplate/LevelProperties")]

public class LevelPropertiesSO : BaseScriptableObject
{
    public GameObject[] cars;
    public Color cameraBackgroundColor;
    public Color roadMeshColor;
    public SquadData[] squads;
}

[System.Serializable]
public class SquadData
{
    public Color color;
    public int squadCarCount;
}
