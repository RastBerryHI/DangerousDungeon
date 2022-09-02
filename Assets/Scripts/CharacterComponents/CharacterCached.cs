using UnityEngine;

namespace CharacterComponents
{
    public class CharacterCached : MonoBehaviour
    {
        private Transform trm;

        public Transform Transform => trm;

        private void Awake()
        {
            trm = transform;
        }
    }
}
