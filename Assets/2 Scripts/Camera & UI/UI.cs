using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public struct Orders{
	public string Name;
	public Order Haut;
	public Order Droite;
	public Order Gauche;

	/*public string[] bas;
	public string[] Replique_bas;*/
}

[System.Serializable]
public struct Order{
	public string Name;
	public bool multiple;
	public string[] order_Name;
	public string[] order_Replique;
}

public enum Contextes{
	Canon,
	Navigation,
	Matelots
}

public class UI : MonoBehaviour {

	[Header("ordres")]		
	[SerializeField] private Color m_textColor;
	[SerializeField] private Color m_highlightedColor;
	[SerializeField] private Color m_orderFailColor;
	[SerializeField] public Orders[] m_ordres;

	[Header("References")]	
	[SerializeField] private Transform m_minions;	
	[SerializeField] private FloatingShip m_ship;
	[SerializeField] private Animator m_playerAnim;
	[SerializeField] private Text m_voiceText;
	[SerializeField] private Text m_textHaut;
	[SerializeField] private Text m_textDroite;
	[SerializeField] private Text m_textBas;
	[SerializeField] private Text m_textGauche;

	public EventSystem m_EventSystem;

	private bool textCoroutine;
    private GameObject m_lastSelected;
	private Contextes m_contexte = Contextes.Canon;
	private Color m_colorHighlight;
	
	private string m_repliqueHautTemp;
	private string m_repliqueDroiteTemp;
	private string m_repliqueGaucheTemp;
	private string m_repliqueBasTemp;

    private void Start()
    {
        m_lastSelected = new GameObject();

		m_textHaut.color = m_textColor;
		m_textDroite.color = m_textColor; 
		m_textBas.color = m_textColor; 
		m_textGauche.color = m_textColor;
	}


	void Update()
	{
		if (m_EventSystem.currentSelectedGameObject == null)
            m_EventSystem.SetSelectedGameObject(m_lastSelected);
        else
            m_lastSelected = m_EventSystem.currentSelectedGameObject;
		

		switch (m_contexte)
		{
			case Contextes.Canon:		
				if(Input.GetButtonDown("X360_Y") && !textCoroutine){     //Haut
					StartCoroutine(HighlightText(m_textHaut, m_repliqueHautTemp, false));
				} 
				if(Input.GetButtonDown("X360_B") && !textCoroutine){	//droite
					//m_minions.BroadcastMessage("MoveTorightCannon");
					//m_ship.BroadcastMessage("CanonReload", false, SendMessageOptions.DontRequireReceiver);
					StartCoroutine(HighlightText(m_textDroite, m_repliqueDroiteTemp, m_ship.ReloadCanonRight()));
				}
				if(Input.GetButtonDown("X360_X") && !textCoroutine){	//gauche
					//m_minions.BroadcastMessage("MoveToleftCannon");
					//m_ship.BroadcastMessage("CanonReload", true, SendMessageOptions.DontRequireReceiver);
					StartCoroutine(HighlightText(m_textGauche, m_repliqueGaucheTemp, m_ship.ReloadCanonLeft()));
				}
			break;
			case Contextes.Navigation:
				if(Input.GetButtonDown("X360_Y") && !textCoroutine){     //Haut
					if(m_ship.anchorDown){
						m_textHaut.text = m_ordres[1].Haut.order_Name[0];
						m_repliqueHautTemp = m_ordres[1].Haut.order_Replique[1];
					}else{
						m_textHaut.text = m_ordres[1].Haut.order_Name[1];
						m_repliqueHautTemp = m_ordres[1].Haut.order_Replique[0];
					}

					StartCoroutine(HighlightText(m_textHaut, m_repliqueHautTemp, m_ship.Anchor()));
				} 
				if(Input.GetButtonDown("X360_B") && !textCoroutine){	//droite

					StartCoroutine(HighlightText(m_textDroite, m_repliqueDroiteTemp, m_ship.OrderSailsUp()));

				}
				if(Input.GetButtonDown("X360_X") && !textCoroutine){	//gauche

					StartCoroutine(HighlightText(m_textGauche, m_repliqueGaucheTemp, m_ship.OrderSailsDown()));

				}
			break;
			case Contextes.Matelots:
				if(Input.GetButtonDown("X360_Y") && !textCoroutine){     //Haut
					StartCoroutine(HighlightText(m_textHaut, m_repliqueHautTemp, false));
				} 
				if(Input.GetButtonDown("X360_B") && !textCoroutine){	//droite
					StartCoroutine(HighlightText(m_textDroite, m_repliqueDroiteTemp, false));
				}
				if(Input.GetButtonDown("X360_X") && !textCoroutine){	//gauche
					StartCoroutine(HighlightText(m_textGauche, m_repliqueGaucheTemp, false));
				}
			break;
		}


	}

	public void SelectOrder(int ordernumber){
		m_textHaut.text = m_ordres[ordernumber].Haut.order_Name[0];
		m_repliqueHautTemp = m_ordres[ordernumber].Haut.order_Replique[0];
		//
		m_textDroite.text = m_ordres[ordernumber].Droite.order_Name[0];
		m_repliqueDroiteTemp = m_ordres[ordernumber].Droite.order_Replique[0];
		//
		m_textGauche.text = m_ordres[ordernumber].Gauche.order_Name[0];
		m_repliqueGaucheTemp = m_ordres[ordernumber].Gauche.order_Replique[0];
		//
		/*m_textBas.text = m_ordres[ordernumber].bas;
		m_repliqueBasTemp = m_ordres[ordernumber].Replique_bas;*/

		switch (ordernumber)
		{
			case 0:
				m_contexte = Contextes.Canon;
			break;
			case 1:
				m_contexte = Contextes.Navigation;
			break;
			case 2:
				m_contexte = Contextes.Matelots;
			break;
			default:
				Debug.LogError("Error context");
			break;
		}
	}

	private IEnumerator HighlightText(Text txt, string replique, bool isOrderValid){
		textCoroutine = true;

		Vector3 txtSizeTemp = txt.transform.localScale;
		Color txtColorTemp = txt.color;
		if(isOrderValid){
			m_playerAnim.SetBool("UIbool", true);
			StopCoroutine("Voice");
			StartCoroutine("Voice", replique);
			txt.transform.localScale *= 1.2f;
			txt.color = m_highlightedColor;
		}else
		{
			txt.color = m_orderFailColor;
		}

		yield return new WaitForSeconds(0.5f);
		
		txt.color = txtColorTemp;
		txt.transform.localScale = txtSizeTemp;
		
		textCoroutine = false;
	}

	private IEnumerator Voice(string phrase){
		m_voiceText.text = phrase;
		yield return new WaitForSeconds(2f);
		m_playerAnim.SetBool("UIbool", false);
	}
}
