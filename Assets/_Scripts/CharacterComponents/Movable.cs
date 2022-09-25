using UnityEngine;

namespace CharacterComponents
{
    [RequireComponent(typeof(CharacterController))]
    public class Movable : MovementControllable
    {
        [SerializeField] private float speed;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private float groundDistance;
    
        private float baseSpeed;
        private readonly float gravity = 9.81f;
        private float vSpeed;
        private bool isMotionBanned;
        private bool isSlowed;
    
        private CharacterCached cache;
        private CameraHoldable cameraHoldable;
        private CharacterController controller;
        private Transform target;
        private Vector3 velocity;
        
        public float Speed
        {
            get => speed;
            set
            {
                if (value < 0)
                {
                    speed = 0;
                }
                else
                {
                    speed = value;
                }
            }
        }

        public bool IsMotionBanned
        {
            set => isMotionBanned = value;
            get => isMotionBanned;
        }
        
        public float RotationSpeed => rotationSpeed;

        public float GroundDistance => groundDistance;

        public float BaseSpeed => baseSpeed;
        
        public bool IsSlowed => isSlowed;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            cache = GetComponent<CharacterCached>();
            cameraHoldable = GetComponent<CameraHoldable>();
        }

        private void Update()
        {
            ApplyGravity();
            
            if (!isMotionBanned)
            {
                Move(Speed);
            }
            
#if UNITY_ANDROID
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (Input.GetTouch(i).phase == TouchPhase.Began)
                {
                    FindTarget(i); 
                }
            }
#else
            if (Input.GetMouseButton(2))
            {
                target = cameraHoldable.FindTarget();
            }
#endif

            if (target)
            {
                velocity = (target.position - cache.Transform.position).normalized;
            }
            
            velocity.y = 0;
            RotateToDirection(velocity);
        }
        
        private void ApplyGravity()
        {
            if (controller.isGrounded)
            {
                vSpeed = 0;
            }
            
            vSpeed -= gravity * Time.deltaTime;
        }

        public void Move(float speed)
        {
#if UNITY_ANDROID
            float x = joystick.Horizontal;
            float z = joystick.Vertical;
#else
            var x = Input.GetAxisRaw("Horizontal");
            var z = Input.GetAxisRaw("Vertical");
#endif
            var rawVelocity = (new Vector3(-x, 0, -z));
            
            velocity = rawVelocity.normalized;
            velocity.y = vSpeed;

            horizontalVelocity = rawVelocity.magnitude;
            controller.Move(velocity * speed * Time.deltaTime);
        }

        private void RotateToDirection(Vector3 direction)
        {
            if (direction == Vector3.zero)
            {
                return;
            }
            
            var targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            cache.Transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}