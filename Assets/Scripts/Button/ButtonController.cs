using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ButtonController : MonoBehaviour
{
    private int _squadIndex;
    private float _buttonPressCoolDown;
    private MeshRenderer _buttonMesh;

    [SerializeField] private Ease pressedEase;
    [SerializeField] private Transform pressableButton;
    [SerializeField] private Transform buttonGoTo;
    private bool _isAvailablePress = true;
    private Vector3 _pressableStartLocal;
    private SquadCarsBase _squadCarBase;


    private void Awake()
    {
        _buttonMesh = pressableButton.GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        _pressableStartLocal = pressableButton.localPosition;
    }

    public void Init(int squadIndex, float buttonCoolDown, Color squadColor, SquadCarsBase squadCarsBase)
    {
        _buttonMesh.material.color = squadColor;
        _squadIndex = squadIndex;
        _buttonPressCoolDown = buttonCoolDown;
        _squadCarBase = squadCarsBase;
    }

    private void OnMouseDown()
    {
        if (!_isAvailablePress || GameManager.instance.gameState != GameState.Play) return;
        _squadCarBase.Open();
        Debug.Log("clicked");
        _isAvailablePress = false;
        DOTween.Sequence()
        .Append(pressableButton.DOLocalMove(buttonGoTo.localPosition, _buttonPressCoolDown / 2).SetEase(pressedEase))
        .Append(pressableButton.DOLocalMove(_pressableStartLocal, _buttonPressCoolDown / 2).SetEase(pressedEase))
        .AppendCallback(() => _isAvailablePress = true);
    }
}
