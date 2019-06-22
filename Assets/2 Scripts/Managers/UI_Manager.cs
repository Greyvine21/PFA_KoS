using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] private PlayerShipBehaviour m_PlayerShip;
    [SerializeField] private EnnemyShipBehaviour m_EnemyShip;
    [SerializeField] private Fort_IA_Manager m_EnemyFort;
    [Header("Inputs")]
    [SerializeField] private GameObject m_Inputs;
    [SerializeField] private GameObject m_select;
    [Header("Speed")]
    [SerializeField] private GameObject m_speed_1;
    [SerializeField] private GameObject m_speed_2;
    [SerializeField] private GameObject m_speed_3;
    [Header("Anchor")]
    [SerializeField] private GameObject m_anchor;
    [Header("Direction")]
    [SerializeField] private GameObject m_direction;
    [Header("Death")]
    [SerializeField] private Animator fade;
    [SerializeField] private Text endText;

    private bool sceneEnded = false;

    //[SerializeField] private Transform m_enemyShip;
    //[SerializeField] private Transform m_UIText;


    // Update is called once per frame
    void Update()
    {
        //m_UIText.transform.position = Camera.main.WorldToScreenPoint(m_enemyShip.position);

        if (Input.GetButton("X360_Start") || Input.GetKey(KeyCode.Tab))
        {
            m_Inputs.SetActive(true);
            m_select.SetActive(false);
        }
        else
        {
            m_Inputs.SetActive(false);
            m_select.SetActive(true);
        }


        if (m_PlayerShip.anchorDown)
        {
            m_speed_1.SetActive(false);
            m_speed_2.SetActive(false);
            m_speed_3.SetActive(false);
            m_anchor.SetActive(true);
        }
        else
        {
            m_anchor.SetActive(false);
            switch (m_PlayerShip.m_sailsManager.m_sailsSate)
            {
                case SailsState.sailsZeroPerCent:
                    m_speed_1.SetActive(true);
                    m_speed_2.SetActive(false);
                    m_speed_3.SetActive(false);
                    break;

                case SailsState.sailsFiftyPerCent:
                    m_speed_1.SetActive(true);
                    m_speed_2.SetActive(true);
                    m_speed_3.SetActive(false);
                    break;

                case SailsState.sailsHundredPerCent:
                    m_speed_1.SetActive(true);
                    m_speed_2.SetActive(true);
                    m_speed_3.SetActive(true);
                    break;
            }
        }

        m_direction.transform.rotation = Quaternion.Euler(0, 0, m_PlayerShip.RudderBladeRotation_Y * 2);

        if (!sceneEnded)
        {
            if (m_EnemyShip.isDefeated && m_EnemyFort.isDefeated)
            {
                sceneEnded = true;
                StartCoroutine(delayEnd(7, "Victoire"));
            }
            if (m_PlayerShip.isDefeated)
            {
                sceneEnded = true;
                StartCoroutine(delayEnd(3, "Défaite"));
                endText.text = "Défaite";
            }
        }
    }

    private IEnumerator delayEnd(float delay, string txt)
    {

        yield return new WaitForSeconds(delay);
        endText.text = txt;
        fade.SetBool("EndScene", true);
    }

}
