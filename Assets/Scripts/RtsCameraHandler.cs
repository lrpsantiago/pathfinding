using PushingBoxStudios.Controls;
using UnityEngine;

namespace PushingBoxStudios.SteampunkTd.Cameras
{
    // Place the script in the Camera Control group in the component menu
    [AddComponentMenu("Cameras/Real Time Strategy")]
    public class RtsCameraHandler : MonoBehaviour
    {
        [Header("Camera Control")]
        [SerializeField]
        private Transform m_target;

        #region Translation Fields

        [Header("Translation")]
        [SerializeField]
        private bool m_enableTranslation = false;

        [SerializeField]
        private float m_translationSpeed = 10f;

        [SerializeField]
        private float m_translationSmoothTime = 0.2f;

        [SerializeField]
        private Vector3 m_minBounds = new Vector3(-16, 0, -16);

        [SerializeField]
        private Vector3 m_maxBounds = new Vector3(16, 0, 16);

        private Vector3 m_destTranslation;
        private Vector3 m_translationVelocity;

        #endregion

        #region Rotation Fields

        [Header("Rotation")]
        [SerializeField]
        private bool m_enableRotation = false;

        [SerializeField]
        private float m_rotationSmoothTime = 0.2f;

        [SerializeField]
        private EMouseButton m_rotationMouseButton = EMouseButton.MiddleButton;

        private float m_destRotation;
        private float m_rotationVelocity;
        private const float MAX_ROTATION_SPEED = 90f;

        #endregion

        #region Zoom Fields

        [Header("Zoom")]
        [SerializeField]
        private bool m_enableZoom = false;

        [SerializeField]
        private float m_zoomSmoothTime = 0.2f;

        [SerializeField]
        private float m_minZoomDistance = 5f;

        [SerializeField]
        private float m_maxZoomDistance = 10f;

        private Vector3 m_destZoom;
        private Vector3 m_zoomVelocity;

        #endregion

        private Camera m_camera;
        private readonly Quaternion m_defaultRotation = Quaternion.Euler(0, 45, 0);
        private readonly Vector3 m_defaultZoomDistance = new Vector3(0, 7.071067f, -7.071067f);

        public Vector3 MinBounds
        {
            get { return m_minBounds; }
            set { m_minBounds = value; }
        }

        public Vector3 MaxBounds
        {
            get { return m_maxBounds; }
            set { m_maxBounds = value; }
        }

        // Constants
        private const float TRANSLATE_SCREEN_DEADZONE = 20f;

        public void Awake()
        {
            if (m_target == null)
            {
                throw new UnityException("A target is required.");
            }

            m_camera = Camera.main;
            m_camera.transform.LookAt(m_target);

            m_destTranslation = m_target.transform.position;
            m_destRotation = m_target.transform.rotation.y;
            m_destZoom = m_camera.transform.localPosition;
        }

        private void Start()
        {
            this.Reset();
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Home))
            {
                this.Reset();
            }

            if (m_enableRotation)
            {
                this.HandleRotation();
            }

            if (m_enableTranslation)
            {
                this.HandleTranslation();
            }

            if (m_enableZoom)
            {
                this.HandleZooming();
            }

            this.HandleMovement();
        }

        private void HandleRotation()
        {
            if (Input.GetMouseButton((int)m_rotationMouseButton))
            {
                float rotationFactor = Input.GetAxis("Mouse X");
                const float sensitivity = 90;

                m_destRotation += rotationFactor * sensitivity * Time.deltaTime;
            }
        }

        private void HandleTranslation()
        {
            if (Input.GetMouseButton((int)m_rotationMouseButton))
            {
                m_translationVelocity = Vector3.zero;
                m_destTranslation = m_target.transform.position;
                return;
            }

            Vector3 mousePosition = Input.mousePosition;
            Vector3 direction = Vector3.zero;

            bool goingLeft = mousePosition.x < TRANSLATE_SCREEN_DEADZONE || Input.GetKey(KeyCode.LeftArrow);

            if (goingLeft)
            {
                direction -= m_target.transform.right;
            }

            bool goingRight = mousePosition.x > Screen.width - TRANSLATE_SCREEN_DEADZONE ||
                Input.GetKey(KeyCode.RightArrow);

            if (goingRight)
            {
                direction += m_target.transform.right;
            }

            bool goingBack = mousePosition.y < TRANSLATE_SCREEN_DEADZONE || Input.GetKey(KeyCode.DownArrow);

            if (goingBack)
            {
                direction -= m_target.transform.forward;
            }

            bool goingForward = mousePosition.y > Screen.height - TRANSLATE_SCREEN_DEADZONE ||
                Input.GetKey(KeyCode.UpArrow);

            if (goingForward)
            {
                direction += m_target.transform.forward;
            }

            Vector3 movement = direction.normalized * m_translationSpeed * Time.deltaTime;
            movement = Vector3.ClampMagnitude(movement, m_translationSpeed);

            if ((m_target.transform.position.x <= m_minBounds.x && movement.x < 0) ||
                (m_target.transform.position.x >= m_maxBounds.x && movement.x > 0))
            {
                movement.x = 0;
            }

            if ((m_target.transform.position.z <= m_minBounds.z && movement.z < 0) ||
                (m_target.transform.position.z >= m_maxBounds.z && movement.z > 0))
            {
                movement.z = 0;
            }

            m_destTranslation += movement;
        }

        private void HandleZooming()
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            float sensivity = 25f;
            float factor = scroll * sensivity;

            if (Mathf.Abs(factor) > 0)
            {
                Vector3 camPos = m_camera.transform.localPosition;
                Vector3 targetPos = Vector3.zero;
                Vector3 forward = Vector3.Normalize(targetPos - camPos);
                Vector3 destination = camPos + forward * factor;

                m_destZoom = this.ClampZoom(destination, m_minZoomDistance, m_maxZoomDistance);
            }
        }

        private void HandleMovement()
        {
            this.DoRotation();
            this.DoTranslation();
            this.DoZoomming();
        }

        private void DoRotation()
        {
            float rotY = Mathf.SmoothDampAngle(m_target.transform.eulerAngles.y, m_destRotation, ref m_rotationVelocity,
                m_rotationSmoothTime, float.MaxValue, Time.deltaTime);

            m_target.transform.rotation = Quaternion.Euler(0, rotY, 0);
        }

        private void DoTranslation()
        {
            Vector3 targetPos = m_target.transform.position;

            targetPos = Vector3.SmoothDamp(targetPos, m_destTranslation, ref m_translationVelocity,
                m_translationSmoothTime);

            targetPos.x = Mathf.Clamp(targetPos.x, m_minBounds.x, m_maxBounds.x);
            targetPos.z = Mathf.Clamp(targetPos.z, m_minBounds.z, m_maxBounds.z);

            m_target.transform.position = targetPos;
        }

        private void DoZoomming()
        {
            Vector3 camPos = m_camera.transform.localPosition;

            camPos = Vector3.SmoothDamp(camPos, m_destZoom, ref m_zoomVelocity, m_zoomSmoothTime);

            m_camera.transform.localPosition = camPos;
        }

        private void Reset()
        {
            m_destRotation = m_defaultRotation.eulerAngles.y;
            m_destZoom = m_defaultZoomDistance;
        }

        private Vector3 ClampZoom(Vector3 destination, float min, float max)
        {
            destination = Vector3.ClampMagnitude(destination, max);

            float minSide = Mathf.Sqrt(Mathf.Pow(min, 2) / 2);
            Vector3 minDist = Vector3.zero;
            minDist.y = minSide;
            minDist.z = minSide;

            Vector3 distance = Vector3.zero - destination;

            if (distance.magnitude < min || destination.y < 0 || destination.z > 0)
            {
                destination.x = 0;
                destination.y = minDist.y;
                destination.z = -minDist.z;
            }

            return destination;
        }
    }
}
