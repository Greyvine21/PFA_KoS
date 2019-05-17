using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MvtController : MonoBehaviour {

	[Header("Moving")]
    [SerializeField] private float m_moveSpeed = 15;
	public LayerMask m_LayerGround;
	private float m_inputH;
	private float m_inputV;
	/*[HideInInspector]*/ public bool m_isInteracting;
    private Transform m_Cam;
    private Vector3 m_CamForward;
	public Vector3 m_Move;

    public bool canMove = true, canInteract = true;

	void Start () {    
		
		if (Camera.main != null)
            m_Cam = Camera.main.transform;
        else
            Debug.LogWarning("Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
	}
	

	void Update () 
	{
		CharInput();
        DetectingGround();

        transform.rotation = Quaternion.Euler(0, 0, 0);        
		
		// calculate move direction to pass to character
        if (m_Cam != null)
        {
            m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
            m_Move = m_inputV * m_CamForward * m_moveSpeed * Time.deltaTime + m_inputH * m_Cam.right * m_moveSpeed * Time.deltaTime;
        }
        else
            Debug.LogError("No main Camera");

        
        if(canMove){
            transform.Translate(m_Move);
            //m_rbPlayer.MoveRotation(m_rbPlayer.rotation * Quaternion.Euler(m_Move));
        }
	}

    private void DetectingGround()
    {
        RaycastHit hitInfo;
#if UNITY_EDITOR
        // helper to visualise the ground check ray in the scene view
        //Debug.DrawLine(transform.position + (Vector3.up * 0.4f), transform.position + (Vector3.up * 0.4f) + (Vector3.down * m_groundCheckDistance));
        Debug.DrawRay(transform.position + (Vector3.up * 0.1f), Vector3.down * 0.3f, Color.magenta);
#endif
        // 0.1f is a small offset to start the ray from inside the character
        // it is also good to note that the transform position in the sample assets is at the base of the character
        
        if (Physics.SphereCast(transform.position + Vector3.up * 0.1f, 0.35f, Vector3.down, out hitInfo, 0.3f, m_LayerGround))
        //if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_groundCheckDistance, m_LayerGround))
        {
            //Debug.DrawRay(transform.position, hitInfo.normal,Color.blue, 2);
            //print(hitInfo.collider.gameObject.name + "  /  " + hitInfo.point);
        }
    }
	private void CharInput(){
        //MVT
        m_inputH = Input.GetAxis("Horizontal");
        if (Mathf.Abs(m_inputH) > 1)
            m_inputH = 1;
        
        m_inputV = Input.GetAxis("Vertical");
        if (Mathf.Abs(m_inputV) > 1)
            m_inputV = 1;

        //INTERACT
        if(Input.GetButtonDown("X360_A") && canInteract){
            m_isInteracting = true;
        }
        if(Input.GetButtonUp("X360_A") && canInteract){
            m_isInteracting = false;
        }
	}
}
