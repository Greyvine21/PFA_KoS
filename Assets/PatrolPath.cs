using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPath : MonoBehaviour {

	private List<Transform> _nodes = null;
	[SerializeField] private bool updateNodes = false;
	[SerializeField] private bool _drawDebug = false;

	public List<Transform> Nodes
	{
		get
		{
			return _nodes;
		}
	}

	private void OnEnable()
	{
		UpdateNodes();
	}

	void OnValidate()
	{
		UpdateNodes();
	}

	private void Update()
	{
#if UNITY_EDITOR
			if (Application.isPlaying == false && updateNodes == true)
			{
				UpdateNodes();
				updateNodes = false;
			}
#endif // UNITY_EDITOR
	}

	private void UpdateNodes()
	{
		_nodes = new List<Transform>();
		foreach (Transform child in transform)
		{
			_nodes.Add(child);
		}
	}

	private void OnDrawGizmos()
	{
		if (_nodes == null || _drawDebug == false)
		{
			return;
		}

		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(_nodes[0].position, Vector3.one*10);

		int nodeCount = _nodes.Count;
		for (int i = 0; i < nodeCount; i++)
		{
			Vector3 secondPosition = i + 1 < nodeCount ? _nodes[i + 1].position : _nodes[0].position;
			Gizmos.DrawLine(_nodes[i].position, secondPosition);
		}
	}
}
