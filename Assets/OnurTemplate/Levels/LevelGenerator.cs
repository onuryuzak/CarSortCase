using Onur.Template;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : BaseSingleton<LevelGenerator>
{

    private LevelPrefabSO _levelSO;
    private Level _generatedLevel;

    #region METHODS
    public void initialize()
    {
    }

    public Level loadLevel(LevelPrefabSO levelSO)
    {
        _levelSO = levelSO;
        destroyLevel();

        _generatedLevel = Instantiate(levelSO.levelPrefab);
        _generatedLevel.transform.position = Vector3.zero;
        _generatedLevel.initialize();
        return _generatedLevel;
    }

    void destroyLevel()
    {
        if (_generatedLevel)
        {
            Destroy(_generatedLevel.gameObject);
        }
    }
    #endregion
}
