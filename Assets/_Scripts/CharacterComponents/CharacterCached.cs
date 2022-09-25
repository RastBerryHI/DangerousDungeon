using UnityEngine;

namespace CharacterComponents
{
    #region RequiredComponents

    [RequireComponent
        (
            typeof(VFXEmittable),
            typeof(Attackable),
            typeof(SFXEmittable)
        )
    ]
    [RequireComponent
        (
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
