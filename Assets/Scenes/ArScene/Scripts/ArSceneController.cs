using GoogleARCore;
using UnityEngine;
using UnityEngine.UI;

public class ArSceneController : MonoBehaviour
{
    public Camera m_firstPersonCamera;
    private Vector3 m_prevARPosePosition;
    private Quaternion startRot;
    private Gyroscope gyro;
    private bool gyroEnabled;
    private bool trackingStarted = false;

    public void Start()
    {
        m_prevARPosePosition = Vector3.zero;
        gyroEnabled = EnableGyro();
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
            MooveCamera(deltaPosition);
            RotateCamera();
        }
    }

    private bool EnableGyro()
    {
        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;

            transform.rotation = Quaternion.Euler(90f, 90f, 0f);
            startRot = new Quaternion(0, 0, 1, 0);

            return true;
        }

        return false;
    }

    private void MooveCamera(Vector3 deltaPosition)
    {
        m_firstPersonCamera.transform.Translate(deltaPosition.x, deltaPosition.y, deltaPosition.z);
    }

    private void RotateCamera()
    {
        if (gyroEnabled)
        {
            m_firstPersonCamera.transform.localRotation = gyro.attitude * startRot;
        }
    }
}