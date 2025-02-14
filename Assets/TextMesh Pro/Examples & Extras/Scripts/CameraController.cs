using UnityEngine;
using System.Collections;
using TMPro.Examples;

namespace TMPro.Examples
{
    public class CameraController : MonoBehaviour
    {
        public enum CameraModes { Follow, Isometric, Free, TopDown }

        private Transform cameraTransform;
        private Transform dummyTarget;

        public Transform CameraTarget;

        public float FollowDistance = 30.0f;
        public float MaxFollowDistance = 100.0f;
        public float MinFollowDistance = 2.0f;

        public float ElevationAngle = 30.0f;
        public float MaxElevationAngle = 85.0f;
        public float MinElevationAngle = 0f;

        public float OrbitalAngle = 0f;

        public CameraModes CameraMode = CameraModes.Follow;

        public bool MovementSmoothing = true;
        public bool RotationSmoothing = false;
        private bool previousSmoothing;

        public float MovementSmoothingValue = 25f;
        public float RotationSmoothingValue = 5.0f;

        public float MoveSensitivity = 2.0f;

        private Vector3 currentVelocity = Vector3.zero;
        private Vector3 desiredPosition;
        private Quaternion desiredRotation;
        private float mouseX;
        private float mouseY;
        private Vector3 moveVector;
        private float mouseWheel;

        private float previousElevationAngle;
        private float previousFollowDistance;

        private bool isTransitioning = false;
        private float transitionSpeed = 2.0f;  // 过渡速度

        void Awake()
        {
            cameraTransform = transform;
            previousSmoothing = MovementSmoothing;
        }

        void Start()
        {
            if (CameraTarget == null)
            {
                dummyTarget = new GameObject("Camera Target").transform;
                CameraTarget = dummyTarget;
            }
        }

        void LateUpdate()
        {
            HandleTopDownToggle();

            if (CameraTarget != null)
            {
                if (CameraMode == CameraModes.TopDown)
                {
                    desiredPosition = CameraTarget.position + new Vector3(0, FollowDistance, 0);
                    desiredRotation = Quaternion.Euler(90f, 0, 0);

                    // 平滑过渡到俯视角度
                    if (isTransitioning)
                    {
                        cameraTransform.position = Vector3.Lerp(cameraTransform.position, desiredPosition, Time.deltaTime * transitionSpeed);
                        cameraTransform.rotation = Quaternion.Lerp(cameraTransform.rotation, desiredRotation, Time.deltaTime * transitionSpeed);

                        // 当摄像机足够接近目标位置时，停止过渡
                        if (Vector3.Distance(cameraTransform.position, desiredPosition) < 0.1f &&
                            Quaternion.Angle(cameraTransform.rotation, desiredRotation) < 1f)
                        {
                            isTransitioning = false;
                        }
                    }
                    else
                    {
                        cameraTransform.position = desiredPosition;
                        cameraTransform.rotation = desiredRotation;
                    }
                }
                else if (CameraMode == CameraModes.Isometric)
                {
                    desiredPosition = CameraTarget.position + Quaternion.Euler(ElevationAngle, OrbitalAngle, 0f) * new Vector3(0, 0, -FollowDistance);
                }
                else if (CameraMode == CameraModes.Follow)
                {
                    desiredPosition = CameraTarget.position + CameraTarget.TransformDirection(Quaternion.Euler(ElevationAngle, OrbitalAngle, 0f) * (new Vector3(0, 0, -FollowDistance)));
                }

                if (CameraMode != CameraModes.TopDown)
                {
                    if (MovementSmoothing)
                    {
                        cameraTransform.position = Vector3.SmoothDamp(cameraTransform.position, desiredPosition, ref currentVelocity, MovementSmoothingValue * Time.fixedDeltaTime);
                    }
                    else
                    {
                        cameraTransform.position = desiredPosition;
                    }

                    if (RotationSmoothing)
                        cameraTransform.rotation = Quaternion.Lerp(cameraTransform.rotation, Quaternion.LookRotation(CameraTarget.position - cameraTransform.position), RotationSmoothingValue * Time.deltaTime);
                    else
                        cameraTransform.LookAt(CameraTarget);
                }
            }
        }

        void HandleTopDownToggle()
        {
            // 按上箭头键进入俯视模式
            if (Input.GetKeyDown(KeyCode.UpArrow) && CameraMode != CameraModes.TopDown)
            {
                previousElevationAngle = ElevationAngle;
                previousFollowDistance = FollowDistance;

                CameraMode = CameraModes.TopDown;
                ElevationAngle = 90f;
                FollowDistance = 13f;

                // 启动过渡效果
                isTransitioning = true;
            }

            // 按下箭头键返回正常视角
            if (Input.GetKeyDown(KeyCode.DownArrow) && CameraMode == CameraModes.TopDown)
            {
                CameraMode = CameraModes.Follow;
                ElevationAngle = previousElevationAngle;
                FollowDistance = previousFollowDistance;

                // 启动过渡效果
                isTransitioning = true;
            }
        }

    }
}