using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using PathCreation;

public class CarController : MonoBehaviour
{
    [SerializeField] private CarControllerSO _carControllerData;
    [SerializeField] private MeshRenderer meshRenderer;

    private SquadCarsBase _squadBase;
    public int squadIndex;

    private GridController _currentGridController;

    public void Init(Color color, int _squadIndex, SquadCarsBase squadBaseHolder)
    {
        meshRenderer.material.color = color;
        this.squadIndex = _squadIndex;
        _squadBase = squadBaseHolder;
    }

    public void FollowPath((VertexPath, GridController) tuple)
    {
        if (tuple.Item1 == null || tuple.Item2 == null) return;
        float moveDuration = _carControllerData.CarSpeed * (_carControllerData.PathLenghtSpeed / (_carControllerData.PathLenghtSpeed - tuple.Item1.length));
        DOTween.Sequence()
        .Append(DOTween.To(x =>
        {
            transform.position = tuple.Item1.GetPointAtTime(x);
            transform.rotation = tuple.Item1.GetRotation(x);
        }, 0, .99f, moveDuration).SetEase(_carControllerData.MoveEase))
        .AppendCallback(() =>
        {
            _currentGridController = tuple.Item2;
            _currentGridController.Placed(this, squadIndex);
        });
    }

    public void PlayScaleTween(out float duration)
    {
        var tween = transform.DOScale(transform.localScale * _carControllerData.Multiplier, _carControllerData.TweenDuration / 2)
        .SetLoops(2, LoopType.Yoyo)
        .SetEase(_carControllerData.ScaleEase);

        duration = tween.Duration();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Car"))
        {
            GameManager.instance.Fail();
        }
    }
}
