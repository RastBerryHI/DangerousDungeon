using UnityEngine;

namespace CharacterComponents
{
    #region RequiredComponents

    [RequireComponent
        (
            typeof(Attackable),
            typeof(AIMovable),
            typeof(VFXEmittable)

        )
    ]
    [RequireComponent
        (
            typeof(Attackable),
            typeof(Damageble),
            typeof(ItemsHoldable)
        )
    ]
    #endregion
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
