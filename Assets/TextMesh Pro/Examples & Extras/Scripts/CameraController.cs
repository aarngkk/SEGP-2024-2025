using UnityEngine;

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
        public float MinElevationAngle = 5f;  // 避免翻转

        public float OrbitalAngle = 0f;

        public CameraModes CameraMode = CameraModes.Follow;

        public bool MovementSmoothing = true;
        public bool RotationSmoothing = false;

        public float MovementSmoothingValue = 25f;
        public float RotationSmoothingValue = 5.0f;

        public float MoveSensitivity = 2.0f;
        public float MouseSensitivity = 2.0f;  // 鼠标灵敏度
        public float ScrollSensitivity = 5.0f; // 滚轮缩放速度

        private Vector3 currentVelocity = Vector3.zero;
        private Vector3 desiredPosition;
        private Quaternion desiredRotation;
        private float mouseX;
        private float mouseY;

        void Awake()
        {
            cameraTransform = transform;
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
            HandleMouseRotation();  // 处理鼠标右键拖拽
            HandleMouseZoom();      // 处理鼠标滚轮缩放
            CalculateDesiredCameraTransform();
            ApplyCameraTransformation();
        }

        void HandleMouseRotation()
        {
            if (Input.GetMouseButton(1)) // 右键拖拽
            {
                float deltaX = Input.GetAxis("Mouse X") * MouseSensitivity;
                float deltaY = Input.GetAxis("Mouse Y") * MouseSensitivity;

                OrbitalAngle += deltaX;
                ElevationAngle -= deltaY; // 反转 Y 轴，让上移是抬高视角
                ElevationAngle = Mathf.Clamp(ElevationAngle, MinElevationAngle, MaxElevationAngle);
            }
        }

        void HandleMouseZoom()
        {
            float scrollInput = Input.GetAxis("Mouse ScrollWheel"); // 获取滚轮输入
            if (scrollInput != 0)
            {
                FollowDistance -= scrollInput * ScrollSensitivity; // 根据滚轮方向调整距离
                FollowDistance = Mathf.Clamp(FollowDistance, MinFollowDistance, MaxFollowDistance); // 限制范围
            }
        }

        void CalculateDesiredCameraTransform()
        {
            if (CameraMode == CameraModes.Follow)
            {
                desiredPosition = CameraTarget.position + CameraTarget.TransformDirection(Quaternion.Euler(ElevationAngle, OrbitalAngle, 0f) * (new Vector3(0, 0, -FollowDistance)));
                desiredRotation = Quaternion.LookRotation(CameraTarget.position - desiredPosition);
            }
        }

        void ApplyCameraTransformation()
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
                cameraTransform.rotation = Quaternion.Lerp(cameraTransform.rotation, desiredRotation, RotationSmoothingValue * Time.deltaTime);
            else
                cameraTransform.rotation = desiredRotation;
        }
    }

}
