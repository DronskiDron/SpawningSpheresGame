using UnityEngine.InputSystem;

namespace SpawningSpheresGame.Game.Settings.Controlls
{
    public class InputSchemeManager
    {
        private readonly InputActionAsset _inputActions;
        private readonly string pcScheme = ControlSchemes.PC.ToString();
        private readonly string mobileScheme = ControlSchemes.Mobile.ToString();


        public InputSchemeManager(InputActionAsset actions)
        {
            _inputActions = actions;
        }


        public void AutoDetectScheme()
        {
            string scheme = UnityEngine.SystemInfo.deviceType == UnityEngine.DeviceType.Handheld ? mobileScheme : pcScheme;
            SetControlScheme(scheme);
        }


        public void SetControlScheme(ControlSchemes scheme)
        {
            SetControlScheme(scheme.ToString());
        }


        private void SetControlScheme(string scheme)
        {
            if (_inputActions != null && _inputActions.controlSchemes.Count > 0)
            {
                _inputActions.bindingMask = InputBinding.MaskByGroup(scheme);
            }
        }
    }
}