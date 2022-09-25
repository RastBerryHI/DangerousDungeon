using System;
using System.Collections.Generic;
using System.Linq;
using CharacterComponents;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class FieldOfView : MonoBehaviour
    {
        [Range(0, 360)]
        [SerializeField] private float viewAngle;
        [SerializeField] private float viewRadius;

        [SerializeField] private LayerMask targetMask;
        [SerializeField] private LayerMask obstacleMask;

        [SerializeField] private int findTargetsDelay;
        [SerializeField] private string targetTag;
        [SerializeField] private string additionTag;

        private Transform target;
        private CharacterCached cache;
        
        public List<Transform> visibleTargets;
        public float ViewRadius => viewRadius;
        public float ViewAngle => viewAngle;
        public Transform ClosestTarget => target;

        private void Awake()
        {
            visibleTargets = new List<Transform>();
            cache = GetComponent<CharacterCached>();
        }

        private void Start()
        {
            PeriodicUpdater();
        }
    
        public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
        {
            if (!angleIsGlobal)
            {
                angleInDegrees += transform.eulerAngles.y;
            }

            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }

        private Transform GetClosestTarget()
        {
            Transform target = visibleTargets[0];
            float minDistance = 100f;
            
            foreach (Transform t in visibleTargets)
            {
                if (t == null)
                {
                    return t;
                }

                float currentDistance = Vector3.Distance(cache.Transform.position, t.position);
            
                if (currentDistance < minDistance)
                {
                    minDistance = currentDistance;
                    target = t;
                }
            }
        
            return target;
        }
    
        private async void PeriodicUpdater()
        {
            while (true)
            {
                await UniTask.Delay(findTargetsDelay);

                if (!this)
                {
                    return;
                }
                
                FindVisibleTargets();

                if (visibleTargets.Count > 1)
                {
                    target = GetClosestTarget();
                }
                else if (visibleTargets.Count == 1)
                {
                    target = visibleTargets[0];
                }
                else
                {
                    target = null;
                }
            }
        }

        private void FindVisibleTargets()
        {
            visibleTargets.Clear();
        
            Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask)
                .Where(p => p.transform.CompareTag(targetTag) || p.transform.CompareTag(additionTag)).ToArray<Collider>();

            foreach (Collider t in targetsInViewRadius)
            {
                Transform target = t.transform;
                Vector3 dirToTarget = (target.position - transform.position).normalized;

                if (!(Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2))
                {
                    continue;
                }
            
                float distance = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, distance, obstacleMask))
                {
                    visibleTargets.Add(target);
                }
            }
        
            Array.Clear(targetsInViewRadius, 0, targetsInViewRadius.Length);
        }
    
        void OnDestroy()
        {
            visibleTargets.Clear();
        }
    }