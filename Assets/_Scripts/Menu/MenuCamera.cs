using DG.Tweening;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    [SerializeField] private Transform characters;
    [SerializeField] private Transform knightTarget;
    [SerializeField] private Transform necromancerTarget;

    public bool tweenStatus;
    private Tween currentTween;

    public void MoveCameraToCharacters()
    {
        if (tweenStatus == true) return;
        currentTween = transform.DOMove(characters.position, 3);
    }

    public void MoveToKnight()
    {
        if (tweenStatus == true) return;
        currentTween = transform.DOMove(knightTarget.position, 0.2f);
    }

    public void MoveToNecromancer()
    {
        if (tweenStatus == true) return;
        currentTween = transform.DOMove(necromancerTarget.position, 0.2f);
    }

    private void FixedUpdate()
    {
        if(currentTween != null) tweenStatus = currentTween.IsActive();
    }
}
