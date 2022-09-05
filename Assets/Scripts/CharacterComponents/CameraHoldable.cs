using System;
using UnityEngine;

namespace CharacterComponents
{
    public class CameraHoldable : MonoBehaviour
    {
        [HideInInspector] public new Camera camera;
        [SerializeField] private CharacterCached cache;
        [SerializeField] private LayerMask characterMask;

        private Transform target;
        

        public Transform FindTarget(int i = 0)
        {
            RaycastHit hit;
#if UNITY_ANDROID
            var ray = camera.ScreenPointToRay(Input.GetTouch(i).position);
#else
            var ray = camera.ScreenPointToRay(Input.mousePosition);
#endif
            if (Physics.Raycast(ray, out hit, 1000, characterMask) && hit.transform != cache.Transform)
            {
                if (!target)
                {
                    target = hit.transform;
                    return target;
                    //target.GetComponent<Character>().ShowHealthBar(true);
                }
                
                if (target == hit.transform)
                {
                    // TODO: Refactor health bar showing on unit select
                        
                    //target.GetComponent<Character>().ShowHealthBar(false);
                    target = null;
                }

                // target.GetComponent<Character>().ShowHealthBar(false);
                // target = hit.transform;
                // target.GetComponent<Character>().ShowHealthBar(true);
            }

            return target;
        }
    }
}
