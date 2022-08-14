using Onur.Template;
using UnityEngine;

public class LevelManager : BaseSingleton<LevelManager>
{

    [SerializeField] LevelPrefabSO[] _levels;
    [SerializeField] LevelDataSO _levelDataSO;

    private int _levelIndex;

    #region BASE
    #endregion

    #region METHODS
    public void initialize()
    {
        if (_levels.Length <= 0)
        {
            Debug.LogWarning("[LevelManager::initialize] Level count is zero!");
            return;
        }


        _levelIndex = (_levelDataSO.level - 1) % _levels.Length;
    }

    public LevelPrefabSO nextLevel()
    {
        _levelDataSO.level++;
        _levelIndex = (++_levelIndex) % _levels.Length;
        Database.instance.saveGame();

        return _levels[_levelIndex];
    }
    #endregion

    #region HELPER
    public LevelPrefabSO currentLevel => _levels[_levelIndex];
    public int level => _levelDataSO.level;
    #endregion
}