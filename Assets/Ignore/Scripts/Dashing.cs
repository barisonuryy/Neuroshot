using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dashing : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform playerCam;
    private Rigidbody rb;
    private PlayerMovementGrappling pm;

    [Header("Dashing")]
    public float dashForce;
    public float dashUpwardForce;
    public float maxDashYSpeed;
    public float dashDuration;

    [Header("CameraEffects")]
    public PlayerCam cam;
    public float dashFov;

    [Header("Settings")]
    public bool useCameraForward = true;
    public bool allowAllDirections = true;
    public bool disableGravity = false;
    public bool resetVel = true;

    [Header("Cooldown")]
    public float dashCd;
    private float dashCdTimer;

    [Header("Mesh")]
    public float activeTime = 2f;
    private bool isTrailActive;
    public float meshRefreshRate=0.1f;
    public SkinnedMeshRenderer[] skinnedMeshRenderers;
    public Transform meshTransform;
    public Material objectMaterial;
    public float meshDestroyTime;
    public string shaderVarRef;
    public float shaderVarRate=0.1f;
    public float shaderVarRefreshRate=0.05f;


    [Header("Input")]
    public KeyCode dashKey = KeyCode.E;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovementGrappling>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(dashKey))
            Dash();

        if (dashCdTimer > 0)
            dashCdTimer -= Time.deltaTime;
    }

    private void Dash()
    {
        if (dashCdTimer > 0) return;
        else dashCdTimer = dashCd;

        pm.dashing = true;
        pm.maxYSpeed = maxDashYSpeed;

        cam.DoFov(dashFov);

        Transform forwardT;

        if (useCameraForward)
            forwardT = playerCam; /// where you're looking
        else
            forwardT = orientation; /// where you're facing (no up or down)

        Vector3 direction = GetDirection(forwardT);

        Vector3 forceToApply = direction * dashForce + orientation.up * dashUpwardForce;
        if (!isTrailActive)
        {
            isTrailActive = true;
            StartCoroutine(ActivateTrail(activeTime));
        }
       
        if (disableGravity)
            rb.useGravity = false;

        delayedForceToApply = forceToApply;
        Invoke(nameof(DelayedDashForce), 0.025f);

        Invoke(nameof(ResetDash), dashDuration);
    }
    IEnumerator ActivateTrail(float trailTime)
    {
        Debug.Log("saaa");
        while (trailTime > 0)
        {
           
            trailTime -= meshRefreshRate;
          
                
            
            for(int i = 0; i < skinnedMeshRenderers.Length; i++)
            {
                GameObject gObj = new GameObject();
                gObj.transform.SetPositionAndRotation(meshTransform.position, meshTransform.rotation);
               MeshRenderer mr= gObj.AddComponent<MeshRenderer>();
                MeshFilter mf=gObj.AddComponent<MeshFilter>();
                Mesh mesh=new Mesh();
                skinnedMeshRenderers[i].BakeMesh(mesh);
                mf.mesh = mesh;
                mr.material = objectMaterial;
                StartCoroutine(AnimateMaterialFloat(mr.material,0,shaderVarRate,shaderVarRefreshRate));
                Destroy(gObj, meshDestroyTime);
            }
            yield return new WaitForSeconds(meshRefreshRate); 
        }
        isTrailActive= false;

    }
    IEnumerator AnimateMaterialFloat(Material mat,float goal,float rate,float refreshRate)
    {
        float valueToAnimate = mat.GetFloat(shaderVarRef);
        while (valueToAnimate > goal) {
            valueToAnimate -= rate;
            mat.SetFloat(shaderVarRef,valueToAnimate);
            yield return new WaitForSeconds (refreshRate);
        }
    }

    private Vector3 delayedForceToApply;
    private void DelayedDashForce()
    {
        if (resetVel)
            rb.velocity = Vector3.zero;

        rb.AddForce(delayedForceToApply, ForceMode.Impulse);
    }

    private void ResetDash()
    {
        pm.dashing = false;
        pm.maxYSpeed = 0;

        cam.DoFov(85f);

        if (disableGravity)
            rb.useGravity = true;
    }

    private Vector3 GetDirection(Transform forwardT)
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3();

        if (allowAllDirections)
            direction = forwardT.forward * verticalInput + forwardT.right * horizontalInput;
        else
            direction = forwardT.forward;

        if (verticalInput == 0 && horizontalInput == 0)
            direction = forwardT.forward;

        return direction.normalized;
    }
}
