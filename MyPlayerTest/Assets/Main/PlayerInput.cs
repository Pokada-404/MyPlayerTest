// GENERATED AUTOMATICALLY FROM 'Assets/Main/PlayerInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerInput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInput"",
    ""maps"": [
        {
            ""name"": ""Action"",
            ""id"": ""fc3a8f14-49f3-4a1c-8777-af6b0168434c"",
            ""actions"": [
                {
                    ""name"": ""Move_UP"",
                    ""type"": ""Button"",
                    ""id"": ""1756dff1-9f2d-45d1-bd74-e60be7db6117"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""Move_Down"",
                    ""type"": ""Button"",
                    ""id"": ""d7c2d1d3-9839-489e-9153-0bbfd618c016"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""Move_Right"",
                    ""type"": ""Button"",
                    ""id"": ""416bbb39-cca2-4950-9122-659511382f1f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""Move_Left"",
                    ""type"": ""Button"",
                    ""id"": ""c4d96b13-5fcc-4e51-a1d5-de7af6a6ce12"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""Jump_Press"",
                    ""type"": ""Button"",
                    ""id"": ""4f15d75b-0cab-4614-a7fa-6e641b68317d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Jump_Release"",
                    ""type"": ""Button"",
                    ""id"": ""f619c269-fb43-450b-970a-e067f41b4e4b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=1)""
                },
                {
                    ""name"": ""InsertOil"",
                    ""type"": ""Button"",
                    ""id"": ""313b425f-229e-4847-a94d-91b708da0043"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Blink"",
                    ""type"": ""Button"",
                    ""id"": ""91429857-151a-434d-86b8-61872cf114e0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""d00c4b12-06a7-4f1a-8af1-312d85a814eb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""9442657b-7c0a-4889-a896-bd751f3b9a39"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move_Down"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6e08686d-4cae-4acd-8ceb-a7cec1f6e6e5"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move_Down"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""160328f4-9efd-4ab9-be09-0dad4a2cea5c"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move_Down"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e25a37e5-90d0-44bf-aa97-714cb5d99441"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump_Press"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c63be656-5b37-4e0c-ad37-e41ceb6f1425"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump_Press"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""91deb874-9435-4a9b-9251-3bd0e74f8f2c"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump_Release"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6a16236b-14c3-4820-962c-921cdb298939"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump_Release"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""db332906-5635-4d8e-baec-18d42561e1ae"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""InsertOil"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""15fb36cf-a006-4b55-9219-5192f0060e05"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""InsertOil"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""29dc4bd7-6363-40d5-8b53-7764242e8265"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Blink"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2a35577f-3162-44e2-859a-8d1e91b5a4c0"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Blink"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""90620aaf-3c66-4c5d-bbc4-f42e70deea12"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Blink"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""909f5e3a-c6f4-4315-8107-992c414f8741"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""329cfea1-b64f-4919-85a9-c93ca4605ad1"",
                    ""path"": ""<XInputController>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9e63d2d3-f075-456f-a2b4-d20de886c66e"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move_UP"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""575abb40-b815-461a-ab19-777ae5584f9f"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move_UP"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""63f45ed2-68e5-427c-9272-3a2c6a13f10d"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move_UP"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c16a1730-9dba-402e-9034-67f415b34222"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move_Right"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""da940d1b-9cea-49b7-a18b-4b332a8111ab"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move_Right"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""07ee1986-14c8-41fd-8939-1da372d5ebb8"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move_Right"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e0cb12bb-84a2-441e-b836-88621f87ee74"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move_Left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""693b574d-0114-4d8f-8fd7-95335137e963"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move_Left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b831037c-ad0b-40fc-bb14-3b6a454e0b61"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move_Left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Action
        m_Action = asset.FindActionMap("Action", throwIfNotFound: true);
        m_Action_Move_UP = m_Action.FindAction("Move_UP", throwIfNotFound: true);
        m_Action_Move_Down = m_Action.FindAction("Move_Down", throwIfNotFound: true);
        m_Action_Move_Right = m_Action.FindAction("Move_Right", throwIfNotFound: true);
        m_Action_Move_Left = m_Action.FindAction("Move_Left", throwIfNotFound: true);
        m_Action_Jump_Press = m_Action.FindAction("Jump_Press", throwIfNotFound: true);
        m_Action_Jump_Release = m_Action.FindAction("Jump_Release", throwIfNotFound: true);
        m_Action_InsertOil = m_Action.FindAction("InsertOil", throwIfNotFound: true);
        m_Action_Blink = m_Action.FindAction("Blink", throwIfNotFound: true);
        m_Action_Attack = m_Action.FindAction("Attack", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Action
    private readonly InputActionMap m_Action;
    private IActionActions m_ActionActionsCallbackInterface;
    private readonly InputAction m_Action_Move_UP;
    private readonly InputAction m_Action_Move_Down;
    private readonly InputAction m_Action_Move_Right;
    private readonly InputAction m_Action_Move_Left;
    private readonly InputAction m_Action_Jump_Press;
    private readonly InputAction m_Action_Jump_Release;
    private readonly InputAction m_Action_InsertOil;
    private readonly InputAction m_Action_Blink;
    private readonly InputAction m_Action_Attack;
    public struct ActionActions
    {
        private @PlayerInput m_Wrapper;
        public ActionActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move_UP => m_Wrapper.m_Action_Move_UP;
        public InputAction @Move_Down => m_Wrapper.m_Action_Move_Down;
        public InputAction @Move_Right => m_Wrapper.m_Action_Move_Right;
        public InputAction @Move_Left => m_Wrapper.m_Action_Move_Left;
        public InputAction @Jump_Press => m_Wrapper.m_Action_Jump_Press;
        public InputAction @Jump_Release => m_Wrapper.m_Action_Jump_Release;
        public InputAction @InsertOil => m_Wrapper.m_Action_InsertOil;
        public InputAction @Blink => m_Wrapper.m_Action_Blink;
        public InputAction @Attack => m_Wrapper.m_Action_Attack;
        public InputActionMap Get() { return m_Wrapper.m_Action; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ActionActions set) { return set.Get(); }
        public void SetCallbacks(IActionActions instance)
        {
            if (m_Wrapper.m_ActionActionsCallbackInterface != null)
            {
                @Move_UP.started -= m_Wrapper.m_ActionActionsCallbackInterface.OnMove_UP;
                @Move_UP.performed -= m_Wrapper.m_ActionActionsCallbackInterface.OnMove_UP;
                @Move_UP.canceled -= m_Wrapper.m_ActionActionsCallbackInterface.OnMove_UP;
                @Move_Down.started -= m_Wrapper.m_ActionActionsCallbackInterface.OnMove_Down;
                @Move_Down.performed -= m_Wrapper.m_ActionActionsCallbackInterface.OnMove_Down;
                @Move_Down.canceled -= m_Wrapper.m_ActionActionsCallbackInterface.OnMove_Down;
                @Move_Right.started -= m_Wrapper.m_ActionActionsCallbackInterface.OnMove_Right;
                @Move_Right.performed -= m_Wrapper.m_ActionActionsCallbackInterface.OnMove_Right;
                @Move_Right.canceled -= m_Wrapper.m_ActionActionsCallbackInterface.OnMove_Right;
                @Move_Left.started -= m_Wrapper.m_ActionActionsCallbackInterface.OnMove_Left;
                @Move_Left.performed -= m_Wrapper.m_ActionActionsCallbackInterface.OnMove_Left;
                @Move_Left.canceled -= m_Wrapper.m_ActionActionsCallbackInterface.OnMove_Left;
                @Jump_Press.started -= m_Wrapper.m_ActionActionsCallbackInterface.OnJump_Press;
                @Jump_Press.performed -= m_Wrapper.m_ActionActionsCallbackInterface.OnJump_Press;
                @Jump_Press.canceled -= m_Wrapper.m_ActionActionsCallbackInterface.OnJump_Press;
                @Jump_Release.started -= m_Wrapper.m_ActionActionsCallbackInterface.OnJump_Release;
                @Jump_Release.performed -= m_Wrapper.m_ActionActionsCallbackInterface.OnJump_Release;
                @Jump_Release.canceled -= m_Wrapper.m_ActionActionsCallbackInterface.OnJump_Release;
                @InsertOil.started -= m_Wrapper.m_ActionActionsCallbackInterface.OnInsertOil;
                @InsertOil.performed -= m_Wrapper.m_ActionActionsCallbackInterface.OnInsertOil;
                @InsertOil.canceled -= m_Wrapper.m_ActionActionsCallbackInterface.OnInsertOil;
                @Blink.started -= m_Wrapper.m_ActionActionsCallbackInterface.OnBlink;
                @Blink.performed -= m_Wrapper.m_ActionActionsCallbackInterface.OnBlink;
                @Blink.canceled -= m_Wrapper.m_ActionActionsCallbackInterface.OnBlink;
                @Attack.started -= m_Wrapper.m_ActionActionsCallbackInterface.OnAttack;
                @Attack.performed -= m_Wrapper.m_ActionActionsCallbackInterface.OnAttack;
                @Attack.canceled -= m_Wrapper.m_ActionActionsCallbackInterface.OnAttack;
            }
            m_Wrapper.m_ActionActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move_UP.started += instance.OnMove_UP;
                @Move_UP.performed += instance.OnMove_UP;
                @Move_UP.canceled += instance.OnMove_UP;
                @Move_Down.started += instance.OnMove_Down;
                @Move_Down.performed += instance.OnMove_Down;
                @Move_Down.canceled += instance.OnMove_Down;
                @Move_Right.started += instance.OnMove_Right;
                @Move_Right.performed += instance.OnMove_Right;
                @Move_Right.canceled += instance.OnMove_Right;
                @Move_Left.started += instance.OnMove_Left;
                @Move_Left.performed += instance.OnMove_Left;
                @Move_Left.canceled += instance.OnMove_Left;
                @Jump_Press.started += instance.OnJump_Press;
                @Jump_Press.performed += instance.OnJump_Press;
                @Jump_Press.canceled += instance.OnJump_Press;
                @Jump_Release.started += instance.OnJump_Release;
                @Jump_Release.performed += instance.OnJump_Release;
                @Jump_Release.canceled += instance.OnJump_Release;
                @InsertOil.started += instance.OnInsertOil;
                @InsertOil.performed += instance.OnInsertOil;
                @InsertOil.canceled += instance.OnInsertOil;
                @Blink.started += instance.OnBlink;
                @Blink.performed += instance.OnBlink;
                @Blink.canceled += instance.OnBlink;
                @Attack.started += instance.OnAttack;
                @Attack.performed += instance.OnAttack;
                @Attack.canceled += instance.OnAttack;
            }
        }
    }
    public ActionActions @Action => new ActionActions(this);
    public interface IActionActions
    {
        void OnMove_UP(InputAction.CallbackContext context);
        void OnMove_Down(InputAction.CallbackContext context);
        void OnMove_Right(InputAction.CallbackContext context);
        void OnMove_Left(InputAction.CallbackContext context);
        void OnJump_Press(InputAction.CallbackContext context);
        void OnJump_Release(InputAction.CallbackContext context);
        void OnInsertOil(InputAction.CallbackContext context);
        void OnBlink(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
    }
}
