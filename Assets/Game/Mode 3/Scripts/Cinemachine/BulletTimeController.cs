using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BulletTimeController : Singleton<BulletTimeController>
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

	public TimeScaleController timeScaleController;
	public CinemachineSmoothPath trackInstance;
	public CameraCartController dollyInstance;
	public Bullet3 activeBullet3;
	public Vector3 targetPosition;
	public List<TargetTrackingSetup> clearTracks = new List<TargetTrackingSetup>();
	public bool isLastCameraActive = false;

	// public void StartSequence(Bullet3 activeBullet3, Vector3 targetPosition)
	public async UniTask StartSequence(Bullet3 activeBullet3, Vector3 targetPosition)
	// public void StartSequence(Bullet3 activeBullet3, Vector3 targetPosition)
	{
		ResetVariables();
		float distanceToTarget = Vector3.Distance(activeBullet3.transform.position, targetPosition);
		var setupsInRange = bulletTackingSetup.Where(s => distanceToTarget > s.minDistance && distanceToTarget < s.maxDistance).ToArray();
		var selectedTrackingSetup = SelectTrackingSetup(activeBullet3.transform, setupsInRange, activeBullet3.transform.rotation);
		if (selectedTrackingSetup == null)
			return;
		this.activeBullet3 = activeBullet3;
		this.targetPosition = targetPosition;

		if (activeBullet3 == null)
		{
			Helper.DebugLog("Bullet NULLLLLLLLLLLL");
		}
		
		if (selectedTrackingSetup == null)
		{
			Helper.DebugLog("TRACK SETUPPPP NULLLLLLLLLLLL");
		}

		CreateBulletPath(activeBullet3.transform);
		CreateDolly(true);
		await UniTask.WaitUntil(() =>
			dollyInstance.isActiveAndEnabled == true && trackInstance.isActiveAndEnabled == true);
		cameraBrain.gameObject.SetActive(true);
		shootingController.gameObject.SetActive(false);
		canvas.gameObject.SetActive(false);
		float speed = CalculateDollySpeed();
		dollyInstance.InitDolly(trackInstance, activeBullet3.transform, speed);
	}

	private void CreateDolly(bool _bulletDolly)
	// private void CreateDolly(bool _bulletDolly)
	{
		// var selectedDolly = setup.avaliableDolly;
		// dollyInstance = Instantiate(selectedDolly);
		if (_bulletDolly)
		{
			GameObject gDolly = PrefabManager.Instance.SpawnDollyPathPool("DollyBulletCart2");
			dollyInstance = gDolly.GetComponent<CameraCartController>();
		}
		else
		{
			GameObject gDolly = PrefabManager.Instance.SpawnDollyPathPool("DollyEnemyCart2");
			dollyInstance = gDolly.GetComponent<CameraCartController>();
		}
	}

	// private void CreateBulletPath(Transform bulletTransform, CinemachinePathController selectedPath)
	// {
	// 	trackInstance = Instantiate(selectedPath.path, bulletTransform).GetComponent<CinemachineSmoothPath>();
	// 	trackInstance.transform.localPosition = selectedPath.transform.position;
	// 	trackInstance.transform.localRotation = selectedPath.transform.rotation;
	// }
	
	// private void CreateBulletPath(Transform bulletTransform, CinemachinePathController selectedPath)
	private async UniTask CreateBulletPath(Transform bulletTransform)
	{
		// trackInstance = Instantiate(selectedPath.path, bulletTransform).GetComponent<CinemachineSmoothPath>();
		GameObject gBulletPath = PrefabManager.Instance.SpawnDollyPathPool("DollyBulletTrack2", bulletTransform.position);
		gBulletPath.transform.SetParent(bulletTransform);
		
		trackInstance = gBulletPath.GetComponent<CinemachineSmoothPath>();
		await UniTask.WaitUntil(() => trackInstance.isActiveAndEnabled == true);


		// gBulletPath.transform.localPosition = selectedPath.transform.position;
		// gBulletPath.transform.localRotation = selectedPath.transform.rotation;
		
		// gBulletPath.transform.localPosition = bulletTransform.position - bulletTransform.forward * 10f;
		gBulletPath.transform.position = bulletTransform.position - bulletTransform.forward * 15f + bulletTransform.up * 2f;
		gBulletPath.transform.LookAt(bulletTransform);

		// if (trackInstance == null)
		// {
		// 	Helper.DebugLog("NULLLLLLLLLLLLLLLLLLL");
		// }
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


	private async UniTask CreateEnemyPath(Transform enemytransform, Transform bulletTransform, CinemachinePathController selectedPath)
	{
		Quaternion rotation = Quaternion.Euler(Vector3.up * enemytransform.root.eulerAngles.y);
		// trackInstance = Instantiate(selectedPath.path, enemytransform.position, rotation);
		GameObject gEnemyPath = PrefabManager.Instance.SpawnDollyPathPool("DollyTrackEnemy2", enemytransform.position + enemytransform.root.right * 30f, rotation);
		// gBulletPath.transform.SetParent(bulletTransform);
		// trackInstance = gEnemyPath.GetComponent<CinemachineSmoothPath>();
		await UniTask.WaitForEndOfFrame();
		trackInstance = gEnemyPath.GetComponent<CinemachineSmoothPath>();
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
		// return path.CheckIfPathISClear(trans, Vector3.Distance(trans.position, targetPosition), orientation);
		return true;
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

	// private void ChangeCamera()
	private async UniTask ChangeCamera()
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
				CreateDolly(false);
				await UniTask.WaitUntil(() => trackInstance.isActiveAndEnabled == true);
				dollyInstance.InitDolly(trackInstance, hitTransform.transform);
				timeScaleController.SlowDownTime();
			}
		}
		StartCoroutine(FinishSequence());
	}

	private void DestroyCinemachineSetup()
	{
		if (trackInstance != null)
		{
			if (trackInstance.gameObject.activeInHierarchy == true)
			{
				PrefabManager.Instance.DespawnPool(trackInstance.gameObject);
			}
		}
		
		if (dollyInstance != null)
		{
			if (dollyInstance.gameObject.activeInHierarchy == true)
			{
				PrefabManager.Instance.DespawnPool(dollyInstance.gameObject);
			}
		}
		
		// Destroy(dollyInstance.gameObject);
	}

	private IEnumerator FinishSequence()
	{
		yield return new WaitForSecondsRealtime(finishingCameraDuration);
		cameraBrain.gameObject.SetActive(false);
		shootingController.gameObject.SetActive(true);
		canvas.gameObject.SetActive(true);
		timeScaleController.SpeedUpTime();
		DestroyCinemachineSetup();
		PrefabManager.Instance.DespawnPool(activeBullet3.gameObject);
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