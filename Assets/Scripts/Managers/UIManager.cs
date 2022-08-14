using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onur.Template;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class UIManager : BaseSingleton<UIManager>
{
    [SerializeField] float _changeImageAlphaDelay;
    [Header("TapToPlay")]
    [SerializeField] private GameObject _tapToPlay;
    [SerializeField] private GameObject _tapToPlayText;
    [Header("GameUI")][SerializeField] private GameObject _gameUI;

    [Header("Fail")][SerializeField] private GameObject _fail;
    [SerializeField] Image _failBackGroundImage;

    [Header("Success")][SerializeField] private GameObject _success;
    [SerializeField] Image _successBackGroundImage;

    //[SerializeField] private ParticleSystem confetti;

    Sequence mySequence;
    private void OnEnable()
    {
        EventManager.LevelSuccess += success;
        EventManager.LevelFailed += fail;
        EventManager.LevelLoaded += levelLoaded;
    }
    private void OnDisable()
    {
        EventManager.LevelSuccess -= success;
        EventManager.LevelFailed -= fail;
        EventManager.LevelLoaded -= levelLoaded;
    }
    private void Start()
    {
        mySequence = DOTween.Sequence();
        closeAllUI();
        showOnlyOneUI(_tapToPlay);
        TapToPlayUIAnimation();
    }

    public void StartLevelButtonFunc()
    {
        mySequence.Kill();
        GameManager.instance.StartLevel();
        showOnlyOneUI(_gameUI);
    }

    public void RestartLevelButtonFunc() => GameManager.instance.RetryLevel();
    public void NextLevelButton() => GameManager.instance.NextLevel();
    private void fail()
    {
        showOnlyOneUI(_fail);
        changeAlphaOnTarget(_failBackGroundImage);
    }

    private void success()
    {
        showOnlyOneUI(_success);
        changeAlphaOnTarget(_successBackGroundImage);
    }



    private void showOnlyOneUI(GameObject panel)
    {
        closeAllUI();
        panel.SetActive(true);
    }

    private void closeAllUI()
    {
        _gameUI.SetActive(false);
        _tapToPlay.SetActive(false);
        _fail.SetActive(false);
        _success.SetActive(false);
    }



    private void levelLoaded()
   {

        closeAllUI();
        showOnlyOneUI(_tapToPlay);
    }

    private void changeAlphaOnTarget(Image targetImage)
    {
        Color changeColor = targetImage.color;
        DOVirtual.Float(changeColor.a, 1, _changeImageAlphaDelay, t =>
        {
            changeColor.a = t;
            targetImage.color = changeColor;
        });
    }

    private void TapToPlayUIAnimation()
    {
        mySequence.Append(_tapToPlayText.transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.5f));
        mySequence.Append(_tapToPlayText.transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f));
        mySequence.SetLoops(-1);
    }
}
