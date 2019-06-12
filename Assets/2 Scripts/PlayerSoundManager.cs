using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundManager : MonoBehaviour {

	public AudioSource m_source;
	public AudioClip[] m_runClips;
	public float m_runVolume = 1;

	public void RunSound(){
		m_source.PlayOneShot(m_runClips[Random.Range(0, m_runClips.Length)], m_runVolume);
	}

	public void StopSource(){
		m_source.Stop();
	}
}
