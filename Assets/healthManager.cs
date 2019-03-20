using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthManager : MonoBehaviour {

    public Slider m_lifebar;
    public float life = 100;

	void Start()
	{
		m_lifebar.maxValue = life;
	}
	
    void Update()
    {
        m_lifebar.value = life;
        if(life <= 0){
            life = 100;
        }
    }

    void OnTriggerEnter(Collider other)
    {
		//print("collide with : " + other.name + " tag : " + other.tag);
        if(other.gameObject.layer == 16){
			//print("hit boat");
			if(other.gameObject.name == "CanonBall")
            	life -= other.gameObject.GetComponent<Bullet>().m_damage;
		}
    }
}
