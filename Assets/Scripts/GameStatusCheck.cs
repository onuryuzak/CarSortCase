using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onur.Template;
using System.Linq;
using DG.Tweening;
using System;

public class GameStatusCheck : BaseSingleton<GameStatusCheck>
{
    private int _gridCount;
    private int _placedGridCount;
    [SerializeField] private float tweenSecondBetweenCar;
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
        _placedGridCount = 0;
        DOVirtual.DelayedCall(0.1f, () => _gridCount = GameManager.instance.carPathCreator.gridHolders.SelectMany(x => x._carGrids).Count());
    }


    public void PlacedAtGrid()
    {
        _placedGridCount++;
        if (_placedGridCount == _gridCount)
        {
            float tweensDuration = 0;
            CarController[] cars = GameManager.instance.carPathCreator.gridHolders
                        .SelectMany(x => x._carGrids.Select(y => y.CurrentCar))
                        .OrderByDescending(x => x.transform.position.z)
                        .ToArray();
            StartCoroutine(WaitForFinishAnimation(cars, tweensDuration));
        }
    }

    IEnumerator WaitForFinishAnimation(CarController[] cars, float tweensDuration)
    {
        for (int i = 0; i < cars.Length; i++)
        {
            cars[i].PlayScaleTween(out tweensDuration);
            yield return new WaitForSeconds(0.1f);
        }
        DOVirtual.DelayedCall(tweensDuration + (tweenSecondBetweenCar * _gridCount), () => GameManager.instance.Success());
    }
}
