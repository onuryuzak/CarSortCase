using System.Collections;
using System.Collections.Generic;
using Onur.Template;
using UnityEngine;


[CreateAssetMenu(fileName = "LevelCountData", menuName = "OnurTemplate/LevelCountData")]
public class LevelDataSO : BaseScriptableObject
{
    public int level;

    public override void reset()
    {
       
        level = 1;
        Debug.Log("[LevelDataSO::reset]");
    }
}

