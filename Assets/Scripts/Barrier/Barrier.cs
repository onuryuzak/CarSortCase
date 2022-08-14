using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Barrier : MonoBehaviour
{
    [SerializeField, Range(0, 1)] private float darkerColorPercent;
    [SerializeField] private int normalMatIndex, darkerMatIndex;
    [SerializeField] private Ease openCloseEase = Ease.InOutSine;
    private MeshRenderer _gateMesh;

    private void Awake() => _gateMesh = GetComponent<MeshRenderer>();

    public void SetColors(Color color)
    {
        _gateMesh.materials[normalMatIndex].color = color;
        _gateMesh.materials[darkerMatIndex].color = color * darkerColorPercent;
    }

    public void Open(float duration)
    {
        DOTween.Sequence()
        .Append(transform.DORotate(Vector3.zero + new Vector3(0, 0, -90), duration / 2).SetEase(openCloseEase))
        .Append(transform.DORotate(Vector3.zero, duration / 2).SetEase(openCloseEase));
    }
}
