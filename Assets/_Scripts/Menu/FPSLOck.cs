using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSLOck : MonoBehaviour
{
    [SerializeField] private int targetFrameRate;
    private void Awake()
    {
        Application.targetFrameRate = targetFrameRate;
    }
}
