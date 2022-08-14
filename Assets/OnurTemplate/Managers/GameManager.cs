using System.Collections;
using System.Collections.Generic;
using Onur.Template;
using UnityEngine;
using DG.Tweening;

public class GameManager : BaseSingleton<GameManager>
{
    public CarControllerSO CarControllerSO;
    [SerializeField] LevelManager _levelManager;
    [SerializeField] LevelGenerator _levelGenerator;
    [SerializeField] private float _delayBeforeGameEnd = .75f;
    [SerializeField] GameObject _confetti;
    [HideInInspector] public GameState gameState;
    private CarPathCreator _carPathCreator;
    #region BASE
    private void OnEnable()
    {
        EventManager.LevelLoaded += OnLevelLoaded;
    }
    private void OnDisable()
    {
        EventManager.LevelLoaded -= OnLevelLoaded;
    }

    private void OnLevelLoaded()
    {
        _carPathCreator = FindObjectOfType<CarPathCreator>();
    }
    void Start()
    {
        _confetti.SetActive(false);
        initialize();

        gameState = GameState.TapToPlay;
    }
    #endregion

    #region INGAMEMETHODS
    public void StartLevel()
    {
        gameState = GameState.Play;
    }
    public void SetGameState(GameState nextState, float delay = 0)
    {
        if (gameState == GameState.End) return;
        if (gameState == nextState) return;
        if (nextState == GameState.Success)
        {
            gameState = GameState.End;
            _confetti.SetActive(true);
            EventManager.OnLevelSuccess();
        }
        else if (nextState == GameState.Fail)
        {
            gameState = GameState.End;
            Time.timeScale = 0;
            EventManager.OnLevelFailed();
        }
        gameState = nextState;

    }
    public void Fail()
    {
        SetGameState(GameState.Fail, _delayBeforeGameEnd);
    }
    public void Success()
    {
        SetGameState(GameState.Success, _delayBeforeGameEnd);
    }
    private void OnApplicationQuit() => DOTween.KillAll();
    #endregion
    #region LEVELMETHODS
    void initialize()
    {
        _levelGenerator.initialize();
        _levelManager.initialize();

        LevelPrefabSO levelSO = _levelManager.currentLevel;
        LoadLevel(levelSO);
    }

    void LoadLevel(LevelPrefabSO levelSO)
    {
        Level level = _levelGenerator.loadLevel(levelSO);
        EventManager.OnLevelLoaded();
    }

    public void NextLevel()
    {
        _confetti.SetActive(false);
        LevelPrefabSO nextLevel = _levelManager.nextLevel();
        LoadLevel(nextLevel);
    }

    public void RetryLevel()
    {
        LevelPrefabSO currentLevel = _levelManager.currentLevel;
        LoadLevel(currentLevel);
    }
    #endregion
    public CarPathCreator carPathCreator => _carPathCreator;
}
