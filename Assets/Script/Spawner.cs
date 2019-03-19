using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	public Transform m_spawnPoint;
	public float m_spawnCooldown = 2;
	
	[Header("----------MINIONS------------")]
	[Header("Gunners")]
	public GameObject m_gunnerPrefab;
	public Transform m_gunnerParent;
	[SerializeField] [Range(0, 10)] private int m_gunnerCount = 1;	

	[Header("Sailors")]
	public GameObject m_sailorPrefab;
	public Transform m_sailorParent;
	[SerializeField] [Range(0, 10)] private int m_sailorCount = 1;	

	[Header("Buccaneers")]
	public GameObject m_buccaneerPrefab;
	public Transform m_buccaneerParent;
	[SerializeField] [Range(0, 10)] private int m_buccaneerCount = 1;

	private bool CDon = false;

	void Update()
	{
		if(CDon){
			if(m_gunnerParent.childCount < m_gunnerCount){
				Spawn(m_gunnerPrefab, m_gunnerParent);
			}

			if(m_sailorParent.childCount < m_sailorCount){
				Spawn(m_sailorPrefab, m_sailorParent);
			}

			if(m_buccaneerParent.childCount < m_buccaneerCount){
				Spawn(m_buccaneerPrefab, m_buccaneerParent);
			}
		}
	}

	private void Spawn(GameObject go, Transform parent)
	{
		StartCoroutine(SpawnCooldown());
		//GameObject minionTemp = Instantiate(go, m_spawnPoint.position, Quaternion.identity, parent) as GameObject;
	}

	IEnumerator SpawnCooldown(){
		CDon = true;
		yield return new WaitForSeconds(m_spawnCooldown);
		CDon = false;
	}
}
