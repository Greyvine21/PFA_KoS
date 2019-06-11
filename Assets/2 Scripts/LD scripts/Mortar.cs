using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mortar : MonoBehaviour
{
    public AudioSource mortar;
    public ParticleSystem explosion;

    public GameObject cercle;
    //public GameObject FirePoint;
    public GameObject boulet;

    public Transform firePointTransform;
    public Transform cercleTransform;

    public Image imageCooldown;
    public float cooldown;
    bool IsCooldown;

    [Header("Spawn range")]
    public float m_y = 40;
    [Space]
    public float m_minXRange = 0;
    public float m_maxXRange = 10;
    [Space]
    public float m_minZRange = 0;
    public float m_maxZRange = 10;

    public int destroyCercle = 5;

    GameObject m_objectToDestroy;
    //GameObject m_firePointToDestroy;

    // Use this for initialization
    void Start ()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (IsCooldown == false)
        {
            explosion.Play();
            mortar.Play();

            //m_firePointToDestroy = Instantiate(FirePoint, firePointTransform.position, firePointTransform.rotation);
            m_objectToDestroy = Instantiate(cercle, cercleTransform.position, cercleTransform.rotation);
            IsCooldown = true;

            Vector3 aleaSpawn1 = new Vector3(cercleTransform.position.x + GetSpawnRange(m_minXRange, m_maxXRange), cercleTransform.position.y + m_y, cercleTransform.position.z + GetSpawnRange(m_minZRange, m_maxZRange));
            Instantiate(boulet, aleaSpawn1, Quaternion.identity);
            Vector3 aleaSpawn2 = new Vector3(cercleTransform.position.x + GetSpawnRange(m_minXRange, m_maxXRange), cercleTransform.position.y + m_y, cercleTransform.position.z + GetSpawnRange(m_minZRange, m_maxZRange));
            Instantiate(boulet, aleaSpawn2, Quaternion.identity);
            Vector3 aleaSpawn3 = new Vector3(cercleTransform.position.x + GetSpawnRange(m_minXRange, m_maxXRange), cercleTransform.position.y + m_y, cercleTransform.position.z + GetSpawnRange(m_minZRange, m_maxZRange));
            Instantiate(boulet, aleaSpawn3, Quaternion.identity);
            Vector3 aleaSpawn4 = new Vector3(cercleTransform.position.x + GetSpawnRange(m_minXRange, m_maxXRange), cercleTransform.position.y + m_y, cercleTransform.position.z + GetSpawnRange(m_minZRange, m_maxZRange));
            Instantiate(boulet, aleaSpawn4, Quaternion.identity);
            Vector3 aleaSpawn5 = new Vector3(cercleTransform.position.x + GetSpawnRange(m_minXRange, m_maxXRange), cercleTransform.position.y + m_y, cercleTransform.position.z + GetSpawnRange(m_minZRange, m_maxZRange));
            Instantiate(boulet, aleaSpawn5, Quaternion.identity);
            Vector3 aleaSpawn6 = new Vector3(cercleTransform.position.x + GetSpawnRange(m_minXRange, m_maxXRange), cercleTransform.position.y + m_y, cercleTransform.position.z + GetSpawnRange(m_minZRange, m_maxZRange));
            Instantiate(boulet, aleaSpawn6, Quaternion.identity);
            Vector3 aleaSpawn7 = new Vector3(cercleTransform.position.x + GetSpawnRange(m_minXRange, m_maxXRange), cercleTransform.position.y + m_y, cercleTransform.position.z + GetSpawnRange(m_minZRange, m_maxZRange));
            Instantiate(boulet, aleaSpawn7, Quaternion.identity);
            Vector3 aleaSpawn8 = new Vector3(cercleTransform.position.x + GetSpawnRange(m_minXRange, m_maxXRange), cercleTransform.position.y + m_y, cercleTransform.position.z + GetSpawnRange(m_minZRange, m_maxZRange));
            Instantiate(boulet, aleaSpawn8, Quaternion.identity);
            Vector3 aleaSpawn9 = new Vector3(cercleTransform.position.x + GetSpawnRange(m_minXRange, m_maxXRange), cercleTransform.position.y + m_y, cercleTransform.position.z + GetSpawnRange(m_minZRange, m_maxZRange));
            Instantiate(boulet, aleaSpawn9, Quaternion.identity);
            Vector3 aleaSpawn10 = new Vector3(cercleTransform.position.x + GetSpawnRange(m_minXRange, m_maxXRange), cercleTransform.position.y + m_y, cercleTransform.position.z + GetSpawnRange(m_minZRange, m_maxZRange));
            Instantiate(boulet, aleaSpawn10, Quaternion.identity);
            OnDestroy();

        }

        if (IsCooldown == true)
        {
            //StopCoroutine("Mortier");
            imageCooldown.fillAmount += 1 / cooldown * Time.deltaTime;

            if (imageCooldown.fillAmount >= 1)
            {
                imageCooldown.fillAmount = 0;
                IsCooldown = false;
            }
        }
	}

    float GetSpawnRange(float minValue, float maxValue)
    {
        return Random.Range(minValue, maxValue);
    }

    private void OnDestroy()
    {
        if (m_objectToDestroy != null)
        {
            Destroy(m_objectToDestroy, destroyCercle);
        }
    }

    /*IEnumerator Mortier ()
    {
        yield return new WaitForSeconds(0);
        explosion.Play();
        mortar.Play();

        FirePoint = Instantiate(FirePoint, firePointTransform.position, firePointTransform.rotation);
        cercle = Instantiate(cercle, cercleTransform.position, cercleTransform.rotation);
        IsCooldown = true;
    }*/
}
