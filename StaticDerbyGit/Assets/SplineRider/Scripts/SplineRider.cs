using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;

public class SplineRider : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInput playerInput = null;
    [SerializeField] private InputActionReference inputRide = null;
    [SerializeField] private InputActionReference inputRelease = null;
    [SerializeField] private GameObject MoveDirectionOfSplineRider;
    [SerializeField] private Transform orientation;

    [Header("Values to set")]
    [SerializeField] private LayerMask detectMask = default;
    [SerializeField] private float detectRadius = 0.5f;
    [SerializeField] private float jumpForceWire = 8f;
    [SerializeField] private float releaseJumpForceWire = 7f;
    [SerializeField] private Vector3 detectCenter = Vector3.zero;
    [SerializeField] private Color detectedColor = Color.cyan;
    [SerializeField] private Color undetectedColor = Color.red;

    public bool EnteredSplineThisFrame { get; private set; }
    public bool ExitedSplineThisFrame { get; private set; }
    public bool IsRiding { get; private set; }
    public bool IsDetected { get; private set; }

    private Collider[] detectCols = new Collider[1];
    private SplineRideable splineRideable = default;
    private SplineAnimate splineAnimate = default;
    private Transform splineAnimateTrans = default;
    private SplineContainer splineContainer = default;
    private Spline spline = default;

    private float3 rideTargetPos;
    private float3 rideStartPos;
    private float rideStartNormTime;
    private Rigidbody rb;

    private InputAction inputActionRide;
    private InputAction inputActionRelease;

    private bool resettingRotation;
    private float resetTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        inputActionRide = playerInput.actions.FindAction(inputRide.action.id);
        inputActionRelease = playerInput.actions.FindAction(inputRelease.action.id);
    }

    private void Update()
    {
        ResetBoolFlips();
        DetectRideInput();
        DetectReleaseInput();
        CheckEnd();
    }

    private void FixedUpdate()
    {
        DetectSplineRideable();
    }

    private void LateUpdate()
    {
        TickRiding();
        ResetRotation();
    }

    private void OnDrawGizmos()
    {
        var color = IsDetected ? detectedColor : undetectedColor;
        Gizmos.color = color;
        var pos = transform.TransformPoint(detectCenter);
        Gizmos.DrawWireSphere(pos, detectRadius);

        if (!IsRiding) return;
        Gizmos.DrawSphere(rideStartPos, 0.3f);
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(rideTargetPos, 0.2f);
    }

    private void ResetBoolFlips()
    {
        EnteredSplineThisFrame = false;
        ExitedSplineThisFrame = false;
    }

    private void TickRiding()
    {
        if (!IsRiding) return;
        transform.position = splineAnimateTrans.position;
        transform.rotation = splineAnimateTrans.rotation;
    }

    /*
    private void ResetRotation()
    {
        if (IsRiding) return;
        if (!resettingRotation) return;

        var rotY = transform.eulerAngles.y;
        var targetRot = Quaternion.Euler(0, rotY, 0);
        transform.rotation = targetRot;
        resetTimer += Time.deltaTime;
        if (resetTimer > 1) {
            resettingRotation = false;
            resetTimer = 0;
        }
    }
    */

    private void ResetRotation()
    {
        if (IsRiding) return;
        var rotY = transform.eulerAngles.y;
        var targetRot = Quaternion.Euler(0, 0, 0);
        transform.localRotation = targetRot;
    }

    void DetectSplineRideable()
    {
        if (IsRiding) return;

        IsDetected = false;
        splineRideable = null;
        var pos = transform.TransformPoint(detectCenter);
        int numCols = Physics.OverlapSphereNonAlloc(pos, detectRadius, detectCols, detectMask);
        if (numCols > 0)
        {
            if (detectCols[0].TryGetComponent<SplineRideable>(out var rideable))
            {
                IsDetected = true;
                splineRideable = rideable;
            }
        }
    }

    private void DetectReleaseInput()
    {
        if (!IsRiding) return;
        if (inputActionRelease.WasPressedThisFrame())
        {
            JumpReleaseFromSpline();
        }
    }

    private void DetectRideInput()
    {
        if (!IsDetected) return;
        if (IsRiding) return;
        if (inputActionRide.WasPressedThisFrame())
        {
            RideSpline();
        }
    }

    private void RideSpline()
    {
        if (IsRiding) return;

        //handshake
        splineRideable.RideSpline(this.gameObject);

        //vars
        splineAnimate = splineRideable.SplineAnimate;
        splineAnimateTrans = splineAnimate.transform;
        splineContainer = splineRideable.SplineContainer;
        spline = splineContainer.Spline;

        //calculate position
        CalculateSplinePosition();

        //reverse flow?
        bool reversed = false;
        var dir = SplineUtility.EvaluateTangent(spline, rideStartNormTime);
        //see if the player s move direction is facing the same way
        var diff = math.dot(dir, (float3)MoveDirectionOfSplineRider.transform.forward);
        if (diff < 0)
        {
            reversed = true;
            SplineUtility.ReverseFlow(spline);
        }

        //recalculate if reversed
        if (reversed)
            CalculateSplinePosition();

        //start flow animation
        splineAnimate.NormalizedTime = rideStartNormTime;
        splineAnimate.Play();
        transform.position = rideStartPos;
        if (rb)
            rb.isKinematic = true;
        IsRiding = true;
        EnteredSplineThisFrame = true;
    }

    private void CalculateSplinePosition()
    {
        rideTargetPos = transform.position;
        var searchPos = splineContainer.transform.InverseTransformPoint(rideTargetPos);
        SplineUtility.GetNearestPoint(spline, searchPos, out rideStartPos, out rideStartNormTime);

        //clamp norm time so we dont have negative or 1+ values.
        rideStartNormTime = Mathf.Clamp(rideStartNormTime, 0, 1);

        rideStartPos = splineContainer.transform.TransformPoint(rideStartPos);
    }

    public void ReleaseFromSpline()
    {
        if (!IsRiding) return;
        splineAnimate = null;
        splineAnimateTrans = null;
        splineContainer = null;
        spline = null;
        if (rb)
            rb.isKinematic = false;
        IsRiding = false;
        ExitedSplineThisFrame = true;
        if (rb)
            rb.AddForce(Vector3.up * releaseJumpForceWire, ForceMode.Impulse);
        var rotY = transform.eulerAngles.y;
        var targetRot = Quaternion.Euler(0, rotY, 0);
        transform.rotation = targetRot;
        resettingRotation = true;
    }

    private void CheckEnd()
    {
        if (!IsRiding) return;
        var perc = splineAnimate.NormalizedTime;
        if (perc >= 1)
        {
            ReleaseFromSpline();
        }
    }

    private void JumpReleaseFromSpline()
    {
        if (!IsRiding) return;
        splineAnimate = null;
        splineAnimateTrans = null;
        splineContainer = null;
        spline = null;
        if (rb)
            rb.isKinematic = false;
        IsRiding = false;
        ExitedSplineThisFrame = true;
        if (rb)
            rb.AddForce(Vector3.up * jumpForceWire, ForceMode.Impulse);
        var rotY = transform.eulerAngles.y;
        var targetRot = Quaternion.Euler(0, rotY, 0);
        transform.rotation = targetRot;
        resettingRotation = true;
    }
}
