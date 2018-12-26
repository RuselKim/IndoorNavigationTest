using GoogleARCore;
using UnityEngine;
using UnityEngine.UI;

public class ArSceneController : MonoBehaviour
{
    public Camera m_firstPersonCamera;
    private Vector3 m_prevARPosePosition;
    private bool trackingStarted = false;

    public void Start()
    {
        m_prevARPosePosition = Vector3.zero;
    }

    public void Update()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Vector3 currentARPosition = Frame.Pose.position;
        if (!trackingStarted)
        {
            trackingStarted = true;
            m_prevARPosePosition = Frame.Pose.position;
        }
        //Remember the previous position so we can apply deltas
        Vector3 deltaPosition = currentARPosition - m_prevARPosePosition;
        m_prevARPosePosition = currentARPosition;
        if (m_firstPersonCamera != null)
        {
            // The initial forward vector of the sphere must be aligned with the initial camera direction in the XZ plane.
            // We apply translation only in the XZ plane.
            m_firstPersonCamera.transform.Translate(deltaPosition.x, deltaPosition.y, deltaPosition.z);
            m_firstPersonCamera.transform.localRotation=Frame.Pose.rotation;
            // Set the pose rotation to be used in the CameraFollow script
            // m_firstPersonCamera.GetComponent<FollowTargetArMode> ().targetRot = Frame.Pose.rotation;
        }
    }
}