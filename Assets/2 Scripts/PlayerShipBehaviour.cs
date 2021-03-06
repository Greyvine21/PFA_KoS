﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipBehaviour : FloatingShip
{

    public bool isDefeated;
    [Header("Player")]
    [Range(0, 45)] public int startCannonRot = 7;
    public bool canMove;
    private CanonManager m_canonsManager;
    private healthManager m_healthManager;

    new void Start()
    {
        base.Start();

        m_canonsManager = GetComponentInChildren<CanonManager>();
        m_healthManager = GetComponentInChildren<healthManager>();
        if (m_healthManager)
            m_healthManager.OnLifeReachZero += Defeat;
        m_rudderInteractZone.OnToggleON += resetCannons;

        if (m_canonsManager)
        {
            m_canonsManager.SetAngleCanonUP(m_canonsManager.m_canonsLeft, startCannonRot);
            m_canonsManager.SetAngleCanonUP(m_canonsManager.m_canonsRight, startCannonRot);

        }

        Anchor();
    }

    void OnDisable()
    {
        if (m_healthManager)
            m_healthManager.OnLifeReachZero -= Defeat;
        m_rudderInteractZone.OnToggleON -= resetCannons;
    }

    void Update()
    {
        TurnCabestan();

        if (m_healthManager.canRegen)
        {
            m_healthManager.Regen();
        }
    }

    void FixedUpdate()
    {
        //float
        Float();

        //Anchor
        if (anchorDown)
        {
            m_shipRB.velocity = Vector3.zero;
        }
        else
        {
            if (canMove)
            {
                AddMainForce(forward);
                TurningForce();
            }
        }
    }

    public void Defeat(object sender)
    {
        if (!isDefeated)
        {
            m_healthManager.m_lifebar.bar.gameObject.SetActive(false);
            m_healthManager.m_impactBridge.Reset();
            m_healthManager.m_impactNavigation.Reset();
            m_healthManager.m_impactSails.Reset();
            isDefeated = true;
            canMove = false;
            StartCoroutine("Sink", 5f);
        }
    }

    private void resetCannons(object sender)
    {
        m_canonsManager.SetAngleCanonUP(m_canonsManager.m_canonsLeft, startCannonRot);
        m_canonsManager.SetAngleCanonUP(m_canonsManager.m_canonsRight, startCannonRot);
        m_canonsManager.SetAngleCanonRIGHT(m_canonsManager.m_canonsLeft, 0);
        m_canonsManager.SetAngleCanonRIGHT(m_canonsManager.m_canonsRight, 0);
    }
}
