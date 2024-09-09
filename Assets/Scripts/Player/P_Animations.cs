using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Animations : MonoBehaviour
{
    private P_Actions pActions;
    [SerializeField] Animator animator;

    private static string transformed = "Transformed";

    private void Awake()
    {
        pActions = GetComponent<P_Actions>();
    }

    private void OnEnable()
    {
        pActions.OnTransformEvent += OnTransformEvent;
    }

    private void OnDisable()
    {
        pActions.OnTransformEvent -= OnTransformEvent;
    }

    private void OnTransformEvent(bool state) => animator.SetBool(transformed, state);

    void FixedUpdate()
    {
        if (animator == null)
            return;


    }
}
