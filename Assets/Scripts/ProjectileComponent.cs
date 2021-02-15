using UnityEngine.Assertions;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
public class ProjectileComponent : MonoBehaviour
{
    [SerializeField]
    private Vector3 m_vInitialVelocity = Vector3.zero;

    private Rigidbody m_rb = null;

    private GameObject m_landingDisplay = null;

    private bool m_bIsGrounded = true;

    private void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        Assert.IsNotNull(m_rb, "Houston, we've got a problem! Rigidbody is not attached!");

        CreateLandingDisplay();
    }

    private void Update()
    {
        UpdateLandingPosition();
    }

    private void CreateLandingDisplay()
    {
        m_landingDisplay = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        m_landingDisplay.transform.position = Vector3.zero;
        m_landingDisplay.transform.localScale = new Vector3(1f, 0.1f, 1f);

        m_landingDisplay.GetComponent<Renderer>().material.color = Color.blue;
        m_landingDisplay.GetComponent<Collider>().enabled = false;
    }

    private void UpdateLandingPosition()
    {
        if (m_landingDisplay != null && m_bIsGrounded)
        {
            m_landingDisplay.transform.position = GetLandingPosition();
        }
    }

    private Vector3 GetLandingPosition()
    {
        float fTime = 2f * (0f - m_vInitialVelocity.y / Physics.gravity.y);

        Vector3 vFlatVel = m_vInitialVelocity;
        vFlatVel.y = 0f;
        vFlatVel *= fTime;

        return transform.position + vFlatVel;
    }

    #region CALLBACKS
    public void OnLaunchProjectile()
    {
        if (!m_bIsGrounded)
        {
            return;
        }

        m_landingDisplay.transform.position = GetLandingPosition();
        m_bIsGrounded = false;

        transform.LookAt(m_landingDisplay.transform.position, Vector3.up);

        m_rb.velocity = m_vInitialVelocity;
    }

    public void OnMoveForward(float fDelta)
    {
        m_vInitialVelocity.z += fDelta;
    }

    public void OnMoveBackward(float fDelta)
    {
        m_vInitialVelocity.z -= fDelta;
    }

    public void OnMoveRight(float fDelta)
    {
        m_vInitialVelocity.x += fDelta;
    }

    public void OnMoveLeft(float fDelta)
    {
        m_vInitialVelocity.x -= fDelta;
    }
    #endregion
}
