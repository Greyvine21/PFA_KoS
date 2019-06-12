using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mortar : MonoBehaviour
{
    public bool canShoot;
    public AudioSource mortar;
    public ParticleSystem explosion;

    public GameObject cercle;
    //public GameObject FirePoint;
    public GameObject boulet;

    public Transform cercleTransform;
    public Image imageCooldown;
    public float cooldown;
    bool IsCooldown;

    [Header("Spawn range")]
    public int nbBullet = 10;
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
        if(canShoot){
            if (IsCooldown == false)
            {
                explosion.Play();
                mortar.Play();
                
                imageCooldown.fillAmount = 0;
                //m_firePointToDestroy = Instantiate(FirePoint, firePointTransform.position, firePointTransform.rotation);
                m_objectToDestroy = Instantiate(cercle, cercleTransform.position, Quaternion.identity);
                IsCooldown = true;

                for (int i = 0; i < nbBullet; i++)
                {            
                    Vector3 aleaSpawn = new Vector3(cercleTransform.position.x + GetSpawnRange(m_minXRange, m_maxXRange), cercleTransform.position.y + m_y + GetSpawnRange(0, 10), cercleTransform.position.z + GetSpawnRange(m_minZRange, m_maxZRange));
                    GameObject bullet = Instantiate(boulet, aleaSpawn, Quaternion.identity);
                    bullet.name = "CanonBall mortar";
                    bullet.layer = 21;
                }
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
