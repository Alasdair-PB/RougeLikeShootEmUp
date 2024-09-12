using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Player
{

    public enum BindMode { Ship, Human, UI };

    // Using Unity's 'new' Input system to register bindings
    [RequireComponent(typeof(P_Controller), typeof(PlayerMapping))]
    public class P_InputManager : MonoBehaviour
    {
        private PlayerMapping inputBindings;
        private P_Actions my_playerActions;
        private Menu_Actions my_menuActions;
        private BindMode bindMode;

        private void Awake()
        {
            my_playerActions = GetComponent<P_Actions>();
            my_menuActions = GetComponent<Menu_Actions>();
            inputBindings = new PlayerMapping();
            bindMode = BindMode.Ship;
        }

        private void OnEnable()
        {
            inputBindings.Enable();
            AddActionBindings();
        }

        private void OnDisable()
        {
            RemoveActionBindings();
            inputBindings.Disable();
        }

        private void Update()
        {
            if (bindMode == BindMode.Ship)
            {
                var deltaX = inputBindings.TopDownPilot.XAxis.ReadValue<float>();
                var deltaY = inputBindings.TopDownPilot.YAxis.ReadValue<float>();
                my_playerActions.OnMove?.Invoke(new Vector2(deltaX, deltaY));
            }
        }

        private void RemoveCurrentBinds()
        {
            switch (bindMode)
            {
                case BindMode.Ship:
                    RemoveActionBindings();
                    break;
                case BindMode.UI: 
                    RemoveUIBinds();
                    break;
            }
        }

        public void SwitchBindings(BindMode newBindMode)
        {
            switch (newBindMode) {
                case BindMode.Ship:
                    if (bindMode == BindMode.Ship)
                        break;
                    else
                    {
                        RemoveCurrentBinds();
                        AddActionBindings();
                        bindMode = BindMode.Ship;
                        break;
                    }
                case BindMode.UI:
                    if (bindMode == BindMode.UI)
                        break;
                    else
                    {
                        RemoveCurrentBinds();
                        AddUIBinds();
                        bindMode = BindMode.UI;
                        break;
                    }
            }
        }

        private void AddUIBinds()
        {
            inputBindings.Menu.CursorDown.performed += ctx => my_menuActions.MoveCursorDown?.Invoke();
            inputBindings.Menu.CursorUp.performed += ctx => my_menuActions.MoveCusorUp?.Invoke();
            inputBindings.Menu.Select.performed += ctx => my_menuActions.Select?.Invoke();
        }

        private void RemoveUIBinds()
        {
            inputBindings.Menu.CursorDown.performed -= ctx => my_menuActions.MoveCursorDown?.Invoke();
            inputBindings.Menu.CursorUp.performed -= ctx => my_menuActions.MoveCusorUp?.Invoke();
            inputBindings.Menu.Select.performed -= ctx => my_menuActions.Select?.Invoke();
        }


        private void AddActionBindings()
        {
            inputBindings.TopDownPilot.WeaponSwitch.performed += ctx => my_playerActions.SwitchWeapon?.Invoke();
            inputBindings.TopDownPilot.Use_Weapon.performed += ctx => my_playerActions.TryDrop?.Invoke();
            inputBindings.TopDownPilot.QuickStep.performed += ctx => my_playerActions.OnQuickStep?.Invoke();

            inputBindings.TopDownPilot.Dash.performed += ctx => my_playerActions.OnDashEvent?.Invoke(true);
            inputBindings.TopDownPilot.Dash.canceled += ctx => my_playerActions.OnDashEvent?.Invoke(false);
        }

        private void RemoveActionBindings()
        {
            inputBindings.TopDownPilot.WeaponSwitch.performed -= ctx => my_playerActions.SwitchWeapon?.Invoke();
            inputBindings.TopDownPilot.Use_Weapon.performed -= ctx => my_playerActions.TryDrop?.Invoke();
            inputBindings.TopDownPilot.QuickStep.performed -= ctx => my_playerActions.OnQuickStep?.Invoke();


            inputBindings.TopDownPilot.Dash.performed -= ctx => my_playerActions.OnDashEvent?.Invoke(true);
            inputBindings.TopDownPilot.Dash.canceled -= ctx => my_playerActions.OnDashEvent?.Invoke(false);
        }

    }
}
