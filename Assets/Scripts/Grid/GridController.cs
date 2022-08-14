using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GridController : MonoBehaviour
{
    [SerializeField] private GameObject _canvas;
    [SerializeField] private int _squadIndex;
    [SerializeField] private SpriteRenderer _gridSpriteRenderer;
    [HideInInspector] public CarController CurrentCar;
    public bool IsEmpty { get; private set; } = true;

    private void Start()
    {
        _canvas.transform.localScale = Vector3.zero;
        _gridSpriteRenderer.color = LevelManager.instance.currentLevel.levelPrefab.
            GetComponent<Level>().levelPropertiesSO.squads[_squadIndex].color;
    }
    public void Placed(CarController car, int squadIndex)
    {
        CurrentCar = car;
        IsEmpty = false;
        if (squadIndex != this._squadIndex) GameManager.instance.Fail();
        else
        {
            _canvas.transform.DOScale(Vector3.one, .25f).SetEase(Ease.OutBack);
            GameStatusCheck.instance.PlacedAtGrid();
        }
    }
    public void Targeted(bool status) => IsEmpty = !status;
}
