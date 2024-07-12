using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Player
{
    // Using Unity's 'new' Input system to register bindings
    [RequireComponent(typeof(P_Controller), typeof(PlayerMapping))]
    public class P_InputManager : MonoBehaviour
    {
        private PlayerMapping inputBindings;
        private P_Actions my_playerActions;

        private void Awake()
        {
            my_playerActions = GetComponent<P_Actions>();
            inputBindings = new PlayerMapping();
        }

        private void OnEnable()
        {
            inputBindings.Enable();
            inputBindings.TopDownPilot.WeaponSwitch.performed += ctx => my_playerActions.TryCollect?.Invoke();
            inputBindings.TopDownPilot.Use_Weapon.performed += ctx => my_playerActions.TryDrop?.Invoke();
            inputBindings.TopDownPilot.QuickStep.performed += ctx => my_playerActions.OnQuickStep?.Invoke();

            inputBindings.TopDownPilot.Dash.performed += ctx => my_playerActions.OnDashEvent?.Invoke(true);
            inputBindings.TopDownPilot.Dash.canceled += ctx => my_playerActions.OnDashEvent?.Invoke(false);
        }

        private void OnDisable()
        {
            inputBindings.TopDownPilot.WeaponSwitch.performed -= ctx => my_playerActions.TryCollect?.Invoke();
            inputBindings.TopDownPilot.Use_Weapon.performed -= ctx => my_playerActions.TryDrop?.Invoke();
            inputBindings.TopDownPilot.QuickStep.performed -= ctx => my_playerActions.OnQuickStep?.Invoke();


            inputBindings.TopDownPilot.Dash.performed -= ctx => my_playerActions.OnDashEvent?.Invoke(true);
            inputBindings.TopDownPilot.Dash.canceled -= ctx => my_playerActions.OnDashEvent?.Invoke(false);

            inputBindings.Disable();
        }

        private void Update()
        {
            var deltaX = inputBindings.TopDownPilot.XAxis.ReadValue<float>();
            var deltaY = inputBindings.TopDownPilot.YAxis.ReadValue<float>();
            my_playerActions.OnMove?.Invoke(new Vector2(deltaX, deltaY));
        }
    }
}
