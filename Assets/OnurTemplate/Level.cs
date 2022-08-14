using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
	public LevelPropertiesSO levelPropertiesSO;
	[SerializeField] Material roadMat;
	private void Awake()
	{
	}
    private void Start()
    {
		
	}

    private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	public void initialize()
	{
		Camera.main.backgroundColor = levelPropertiesSO.cameraBackgroundColor;
		roadMat.color = levelPropertiesSO.roadMeshColor;
	}
}
