﻿using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(TimeScaleController))]
public class BulletTimeController : MonoBehaviour
{
	[Serializable]
	public class TargetTrackingSetup
	{
		public CinemachinePathController avaliableTrack;
		public CameraCartController avaliableDolly;
	}

	[Serializable]
	public class BulletTrackingSetup : TargetTrackingSetup
	{
		public float minDistance;
		public float maxDistance;
	}

	[SerializeField] private GameObject canvas;
	[SerializeField] private CinemachineBrain cameraBrain;
	[SerializeField] private BulletTrackingSetup[] bulletTackingSetup;
	[SerializeField] private TargetTrackingSetup[] enemyTrackingSetup;
	[SerializeField] private PlayerShootingController shootingController;
	[SerializeField] private float distanceToChangeCamera;
	[SerializeField] private float finishingCameraDuration;

	private TimeScaleController timeScaleController;
	private CinemachineSmoothPath trackInstance;
	private CameraCartController dollyInstance;
	private Bullet3 activeBullet3;
	private Vector3 targetPosition;
	private List<TargetTrackingSetup> clearTracks = new List<TargetTrackingSetup>();
	private bool isLastCameraActive = false;

	private void Awake()
	{
		timeScaleController = GetComponent<TimeScaleController>();
	}

	internal void StartSequence(Bullet3 activeBullet3, Vector3 targetPosition)
	{
		ResetVariables();
		float distanceToTarget = Vector3.Distance(activeBullet3.transform.position, targetPosition);
		var setupsInRange = bulletTackingSetup.Where(s => distanceToTarget > s.minDistance && distanceToTarget < s.maxDistance).ToArray();
		var selectedTrackingSetup = SelectTrackingSetup(activeBullet3.transform, setupsInRange, activeBullet3.transform.rotation);
		if (selectedTrackingSetup == null)
			return;
		this.activeBullet3 = activeBullet3;
		this.targetPosition = targetPosition;

		CreateBulletPath(activeBullet3.transform, selectedTrackingSetup.avaliableTrack);
		CreateDolly(selectedTrackingSetup);
		cameraBrain.gameObject.SetActive(true);
		shootingController.gameObject.SetActive(false);
		canvas.gameObject.SetActive(false);
		float speed = CalculateDollySpeed();
		dollyInstance.InitDolly(trackInstance, activeBullet3.transform, speed);
	}

	private void CreateDolly(TargetTrackingSetup setup)
	{
		var selectedDolly = setup.avaliableDolly;
		dollyInstance = Instantiate(selectedDolly);
	}

	private void CreateBulletPath(Transform bulletTransform, CinemachinePathController selectedPath)
	{
		trackInstance = Instantiate(selectedPath.path, bulletTransform);
		trackInstance.transform.localPosition = selectedPath.transform.position;
		trackInstance.transform.localRotation = selectedPath.transform.rotation;
	}

	private float CalculateDollySpeed()
	{
		if (trackInstance == null || activeBullet3 == null)
			return 0f;

		float distanceToTarget = Vector3.Distance(activeBullet3.transform.position, targetPosition);
		float speed = activeBullet3.GetBulletSpeed();
		float pathDistance = trackInstance.PathLength;
		return pathDistance * speed / distanceToTarget;
	}


	private void CreateEnemyPath(Transform enemytransform, Transform bulletTransform, CinemachinePathController selectedPath)
	{
		Quaternion rotation = Quaternion.Euler(Vector3.up * bulletTransform.root.eulerAngles.y);
		trackInstance = Instantiate(selectedPath.path, enemytransform.position, rotation);
	}

	private TargetTrackingSetup SelectTrackingSetup(Transform trans, TargetTrackingSetup[] setups, Quaternion orientation)
	{
		clearTracks.Clear();
		for (int i = 0; i < setups.Length; i++)
		{
			if (CheckIfPathIsClear(setups[i].avaliableTrack, trans, orientation))
				clearTracks.Add(setups[i]);
		}
		if (clearTracks.Count == 0)
			return null;
		return clearTracks[UnityEngine.Random.Range(0, clearTracks.Count)];
	}

	private bool CheckIfPathIsClear(CinemachinePathController path, Transform trans, Quaternion orientation)
	{
		return path.CheckIfPathISClear(trans, Vector3.Distance(trans.position, targetPosition), orientation);
	}

	private void Update()
	{
		if (!activeBullet3)
			return;

		if (CheckIfBulletIsNearTarget())
			ChangeCamera();
	}

	private bool CheckIfBulletIsNearTarget()
	{
		return Vector3.Distance(activeBullet3.transform.position, targetPosition) < distanceToChangeCamera;
	}

	private void ChangeCamera()
	{
		if (isLastCameraActive)
			return;
		isLastCameraActive = true;
		DestroyCinemachineSetup();
		Transform hitTransform = activeBullet3.GetHitEnemyTransform();
		if (hitTransform)
		{
			Quaternion rotation = Quaternion.Euler(Vector3.up * activeBullet3.transform.rotation.eulerAngles.y);
			var selectedTrackingSetup = SelectTrackingSetup(hitTransform, enemyTrackingSetup, rotation);
			if (selectedTrackingSetup != null)
			{
				CreateEnemyPath(hitTransform, activeBullet3.transform, selectedTrackingSetup.avaliableTrack);
				CreateDolly(selectedTrackingSetup);
				dollyInstance.InitDolly(trackInstance, hitTransform.transform);
				timeScaleController.SlowDownTime();
			}
		}
		StartCoroutine(FinishSequence());
	}

	private void DestroyCinemachineSetup()
	{
		Destroy(trackInstance.gameObject);
		Destroy(dollyInstance.gameObject);
	}

	private IEnumerator FinishSequence()
	{
		yield return new WaitForSecondsRealtime(finishingCameraDuration);
		cameraBrain.gameObject.SetActive(false);
		shootingController.gameObject.SetActive(true);
		canvas.gameObject.SetActive(true);
		timeScaleController.SpeedUpTime();
		DestroyCinemachineSetup();
		Destroy(activeBullet3.gameObject);
		Helper.DebugLog("AAAAAAAAAAAAAAAAAA");
		ResetVariables();
	}

	private void ResetVariables()
	{
		isLastCameraActive = false;
		trackInstance = null;
		dollyInstance = null;
		activeBullet3 = null;
		clearTracks.Clear();
		targetPosition = Vector3.zero;
	}
}