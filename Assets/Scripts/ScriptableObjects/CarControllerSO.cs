using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Onur.Template;

[CreateAssetMenu(fileName = "CarControllerData", menuName = "OnurTemplate/CarControllerData")]
public class CarControllerSO :BaseScriptableObject
    {
    [Header("Car Movement Variables")]
    public float CarSpeed = 1.5f;
    public float PathLenghtSpeed = 500f;
    public Ease MoveEase = Ease.InOutSine;

    [Header("Win Tween Variables")]
    public float Multiplier = 1.1f;
    public float TweenDuration;
    public Ease ScaleEase = Ease.InOutSine;
}
