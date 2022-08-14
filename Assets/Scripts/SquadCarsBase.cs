using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using PathCreation;

public class SquadCarsBase : MonoBehaviour
{
    [SerializeField] private Barrier _barrier;
    [SerializeField] private ButtonController _button;
    [SerializeField] private float _buttonCooldown;
    [SerializeField] private int _squadIndex;

    [Header("Car Spawn and Reorder")]
    [SerializeField] private float _waitForMoveCarTime;

    [SerializeField] private Transform _initCarSpawnPosition;
    [SerializeField] private float _distanceBetweenCar;
    [SerializeField] private float reOrderTimeBetweenCars;
    [SerializeField] private Ease carFillSpaceEase;
    [SerializeField] CarPathCreator _carPathCreator;

    private Color _squadColor;
    private int _squadCarCount;
    private Stack<CarController> _cars = new Stack<CarController>();
    public List<GridHolder> _preferenceGridHolder = new List<GridHolder>();
    private LevelPropertiesSO levelPropertiesSO;

    private void Start()
    {
        levelPropertiesSO = LevelManager.instance.currentLevel.levelPrefab.
            GetComponent<Level>().levelPropertiesSO;

        _squadColor = levelPropertiesSO.squads[_squadIndex].color;
        _squadCarCount = levelPropertiesSO.squads[_squadIndex].squadCarCount;

        _button.Init(_squadIndex, _buttonCooldown, _squadColor, this);
        _barrier.SetColors(_squadColor);

        SpawnCars();
    }

    private void SpawnCars()
    {
        Vector3 spawnPoint = _initCarSpawnPosition.localPosition;
        for (int i = 0; i < _squadCarCount; i++)
        {
            GameObject car = Instantiate(levelPropertiesSO.cars[Random.Range(0, levelPropertiesSO.cars.Length)], Vector3.zero, Quaternion.identity, transform);
            car.transform.localPosition = spawnPoint;
            spawnPoint.z -= _distanceBetweenCar;
            CarController carController = car.GetComponent<CarController>();
            carController.Init(_squadColor, _squadIndex, this);
            _cars.Push(carController);
        }

        _cars = new Stack<CarController>(_cars);
    }

    public void Open()
    {
        if (_cars.Count == 0) return;

        _barrier.Open(_buttonCooldown);
        StartCoroutine(waitForMoveCar());
    }

    private IEnumerator waitForMoveCar()
    {
        yield return new WaitForSeconds(_waitForMoveCarTime);
        (VertexPath, GridController) tuple = GetNextPath(_cars.Peek().transform.position, -1);
        if (tuple.Item1 == null) yield break;
        CarController car = _cars.Pop();
        car.FollowPath(tuple);

        StartCoroutine(WaitForNewCar());
    }

    public (VertexPath, GridController) GetNextPath(Vector3 position, int i)
    {
        (VertexPath, GridController) tuple = _carPathCreator.GetPath(position, _preferenceGridHolder.ToArray());
        if (i + 1 == _preferenceGridHolder.Count - 1) return (null, null);
        if (tuple.Item1 == null)
        {
            _preferenceGridHolder = _carPathCreator.GetEmptyGridHolder(transform.position);
            tuple = GameManager.instance.carPathCreator.GetPath(position, _preferenceGridHolder.ToArray());
        }

        return tuple;
    }

    IEnumerator WaitForNewCar()
    {
        Transform[] selectedCars = _cars.Select(x => x.transform).ToArray();
        for (int i = 0; i < selectedCars.Length; i++)
        {
            selectedCars[i].transform.DOMove(selectedCars[i].transform.position + new Vector3(0, 0, _distanceBetweenCar), _buttonCooldown).SetEase(carFillSpaceEase);

            yield return new WaitForSeconds(reOrderTimeBetweenCars);
        }


    }
}
