using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SailsState
{
    sailsZeroPerCent = 0,
    sailsFiftyPerCent,
    sailsHundredPerCent
}
public class SailsManager : MonoBehaviour
{
    [SerializeField] private bool m_useAnim;
    [SerializeField] public SailsState m_sailsSate = SailsState.sailsZeroPerCent;
    [SerializeField] private float m_MaxForwardSpeed;
    [SerializeField] private bool m_useTransparency;
    [SerializeField] private Material m_SailsMaterial;
    [SerializeField] private Color m_SailsNormalColor;
    [SerializeField] private Color m_SailsFadeColor;

    [Header("Sails Coroutine")]
    [SerializeField] private Transform[] m_sails;
    [SerializeField] public float m_sailsUpSpeed;
    [SerializeField] public float m_sailsDownSpeed;

    [Header("Sails Anim")]
    public float normalizedTimeSails = 0;
    [SerializeField] private Animator[] m_sailsAnim;
    [SerializeField] public float m_sailsUpSpeedAnim;
    [SerializeField] public float m_sailsDownSpeedAnim;

    private bool sailsGoingUp, SailsGoingDown;
    private SailsState m_sailsStateOLD;
    private FloatingShip m_floatingShip;

    // Use this for initialization
    void Start()
    {
        m_floatingShip = GetComponentInParent<FloatingShip>();

        foreach (Transform sail in m_sails)
        {
            switch (m_sailsSate)
            {
                case SailsState.sailsFiftyPerCent:
                    sail.localScale = new Vector3(1, 0.5f, 1);
                    break;
                case SailsState.sailsHundredPerCent:
                    sail.localScale = new Vector3(1, 1, 1);
                    break;
                case SailsState.sailsZeroPerCent:
                    sail.localScale = new Vector3(1, 0.04f, 1);
                    break;
                default:
                    sail.localScale = new Vector3(1, 10, 1);
                    break;
            }
        }
    }

    void Update()
    {
        SailsStateMachine();
    }

    public bool OrderSailsUp()
    {
        if (m_sailsSate != SailsState.sailsZeroPerCent && !sailsGoingUp)
        {
            m_sailsSate--;
            return true;
        }
        return false;
    }

    public bool OrderSailsDown()
    {
        if (m_sailsSate != SailsState.sailsHundredPerCent && !SailsGoingDown)
        {
            m_sailsSate++;
            return true;
        }
        return false;
    }

    protected void SailsStateMachine()
    {
        if (m_useAnim)
        {
            switch (m_sailsSate)
            {
                case SailsState.sailsZeroPerCent:
                    m_floatingShip.forward = m_MaxForwardSpeed / 5;
                    if (m_useTransparency)
                        m_SailsMaterial.color = m_SailsNormalColor;
                    if (normalizedTimeSails > 0)
                    {
                        normalizedTimeSails -= m_sailsUpSpeedAnim / 100;
                    }
                    else if (normalizedTimeSails < 0)
                    {
                        normalizedTimeSails = 0;
                    }
                    break;
                case SailsState.sailsFiftyPerCent:
                    m_floatingShip.forward = m_MaxForwardSpeed / 2;
                    if (m_useTransparency)
                        m_SailsMaterial.color = m_SailsFadeColor;
                    if (normalizedTimeSails > 0.5f)
                    {
                        normalizedTimeSails -= m_sailsUpSpeedAnim / 100;
                    }
                    else if (normalizedTimeSails > 0.49f && normalizedTimeSails < 0.51f)
                    {
                        normalizedTimeSails = 0.5f;
                    }
                    else if (normalizedTimeSails < 0.5f)
                    {
                        normalizedTimeSails += m_sailsDownSpeedAnim / 100;
                    }
                    break;
                case SailsState.sailsHundredPerCent:
                    m_floatingShip.forward = m_MaxForwardSpeed;
                    if (m_useTransparency)
                        m_SailsMaterial.color = m_SailsFadeColor;
                    if (normalizedTimeSails < 1)
                    {
                        normalizedTimeSails += m_sailsDownSpeedAnim / 100;
                    }
                    else if (normalizedTimeSails > 1)
                    {
                        normalizedTimeSails = 1;
                    }
                    break;
                default:
                    break;
            }
            SailsAnimUpdate();
        }
        else
        {
            if (m_sailsSate != m_sailsStateOLD)
            {
                //print("change sails state");
                switch (m_sailsSate)
                {
                    case SailsState.sailsZeroPerCent:
                        m_floatingShip.forward = m_MaxForwardSpeed / 5;
                        StopCoroutine("SailsDown");
                        SailsGoingDown = false;
                        StartCoroutine("SailsUp");
                        break;
                    case SailsState.sailsFiftyPerCent:
                        m_floatingShip.forward = m_MaxForwardSpeed / 2;
                        if (m_sailsStateOLD > m_sailsSate)
                        {
                            StopCoroutine("SailsDown");
                            SailsGoingDown = false;
                            StartCoroutine("SailsUp");
                        }
                        else
                        {
                            StopCoroutine("SailsUp");
                            sailsGoingUp = false;
                            StartCoroutine("SailsDown");
                        }

                        break;
                    case SailsState.sailsHundredPerCent:
                        m_floatingShip.forward = m_MaxForwardSpeed;
                        StopCoroutine("SailsUp");
                        sailsGoingUp = false;
                        StartCoroutine("SailsDown");
                        break;
                    default:
                        break;
                }
                m_sailsStateOLD = m_sailsSate;
            }
        }
    }

    protected void SailsAnimUpdate()
    {
        foreach (Animator sailAnim in m_sailsAnim)
        {
            sailAnim.SetFloat("ControllerFloat", normalizedTimeSails);
            // foreach (AnimationState state in sailAnim)
            // {
            // 	state.speed = speedSails;
            // }
        }
    }

    protected IEnumerator SailsUp()
    {
        sailsGoingUp = true;

        float MinScale;
        switch (m_sailsSate)
        {
            case SailsState.sailsFiftyPerCent:
                MinScale = 0.5f;
                break;
            case SailsState.sailsZeroPerCent:
                MinScale = 0.04f;
                break;
            default:
                MinScale = 1.5f;
                break;
        }

        while (m_sails[0].localScale.y > MinScale)
        {
            foreach (Transform sail in m_sails)
            {
                sail.localScale -= new Vector3(0, 0.01f, 0);
                yield return new WaitForSeconds(1 / m_sailsUpSpeed);
            }
        }

        // foreach (Transform sail in m_sails)
        // {
        // 	sail.localScale = new Vector3(0, MinScale ,0);
        // }

        sailsGoingUp = false;
    }

    protected IEnumerator SailsDown()
    {
        SailsGoingDown = true;

        float MaxScale;
        switch (m_sailsSate)
        {
            case SailsState.sailsFiftyPerCent:
                MaxScale = 0.5f;
                break;
            case SailsState.sailsHundredPerCent:
                MaxScale = 1f;
                break;
            default:
                MaxScale = 1.5f;
                break;
        }

        while (m_sails[0].localScale.y < MaxScale)
        {
            foreach (Transform sail in m_sails)
            {
                sail.localScale += new Vector3(0, 0.05f, 0);
                yield return new WaitForSeconds(1 / m_sailsDownSpeed);
            }
        }

        // foreach (Transform sail in m_sails)
        // {
        // 	sail.localScale = new Vector3(0, MaxScale ,0);
        // }

        SailsGoingDown = false;
    }
}
