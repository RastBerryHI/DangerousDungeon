using UnityEngine;
using UnityEngine.AI;

namespace CharacterComponents
{
    public class AIMovable : MonoBehaviour
    {
        [SerializeField] private CharacterCached cachedData;
    
        [SerializeField] private float speed;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private float groundDistance;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private LayerMask groundMask;
        [SerializeField] private FieldOfView fieldOfView;
    
        private float baseSpeed;
        private bool isMotionBanned;
        private bool isGrounded;
        private bool isSlowed;
        private float horizontalVelocity;
    
        private NavMeshAgent agent;
        [SerializeField]private Transform target;
        private CharacterCached cache;
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
    
        public float RotationSpeed => rotationSpeed;

        public float GroundDistance => groundDistance;

        public float BaseSpeed => baseSpeed;

        public float HorizontalVelocity => horizontalVelocity;

        public bool IsMotionBanned => isMotionBanned;

        public bool IsGrounded => isGrounded;

        public bool IsSlowed => isSlowed;

        public FieldOfView Fow => fieldOfView;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            cache = GetComponent<CharacterCached>();
        }
    
        private void Update()
        {
            ApplyGravity();
        
            if (isMotionBanned)
            {
                return;
            }
        
            if (fieldOfView.visibleTargets.Count > 0)
            {
                target = fieldOfView.ClosestTarget;
            }

            if (target)
            {
                Move();
                RotateToTarget();
                return;
            }
            
            agent.isStopped = true;
            horizontalVelocity = 0;
        }

        public void StopMovement() => agent.isStopped = true;
        
        public void ResumeMovement() => agent.isStopped = false;
        
        private void Move()
        {
            float distance = Vector3.Distance(cache.Transform.position, target.position);
        
            if (distance <= agent.stoppingDistance)
            {
                // Stop movement
                agent.isStopped = true;
                horizontalVelocity = 0;
            }
            else
            {
                // Resume
                agent.isStopped = false;
                horizontalVelocity = 1;
                agent.speed = Speed;
                agent.SetDestination(target.position);
            }
        }

        private void RotateToTarget()
        {
            Vector3 direction = (target.position - cache.Transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

            cache.Transform.rotation = Quaternion.RotateTowards(cache.Transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        private void ApplyGravity()
        {
            bool isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (velocity.y >= 0)
            {
                return;
            }
        
            if (isGrounded)
            {
                velocity.y = -2f;
                agent.Move(velocity * Time.deltaTime);
                return;
            }

            velocity.y += -0.00001f * Time.deltaTime;
            agent.Move(velocity);
        }
    }
}
