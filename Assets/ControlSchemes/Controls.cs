// GENERATED AUTOMATICALLY FROM 'Assets/ControlSchemes/Controls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Controls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""Gameplay"",
            ""id"": ""93ff5803-ea49-4781-a755-6d9f71641c4a"",
            ""actions"": [
                {
                    ""name"": ""Camera Control"",
                    ""type"": ""Value"",
                    ""id"": ""9661de5f-6063-4814-aed6-b655c823e452"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Camera Zoom Control"",
                    ""type"": ""Value"",
                    ""id"": ""d7dd58a2-5de5-4662-8a10-87e94f5024f1"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cursor Control"",
                    ""type"": ""Value"",
                    ""id"": ""61063dec-192c-4d79-9727-5de280bbf815"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Navigate Grid"",
                    ""type"": ""Value"",
                    ""id"": ""c851832d-4bcf-4f48-aa43-02823f069163"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""14e383d7-41b8-4bab-b67d-fb81d3e39e3e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""d617c97c-41a0-4b7a-b6ca-58b3ab9c9570"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""End Turn"",
                    ""type"": ""Button"",
                    ""id"": ""4f8a3dcc-49bd-4306-a7bc-c872d1682b5e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""fc2a335c-22bf-495e-8cd7-e24e0d41dbc4"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera Control"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""987a9dc0-4fee-49f7-a1f6-24eb58465b61"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse And Keyboard"",
                    ""action"": ""Camera Control"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""c8e0f680-f3e7-4876-a4ef-2d7c8d505b8c"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse And Keyboard"",
                    ""action"": ""Camera Control"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""e95008ab-60cc-4521-8bfd-3b96af2df2cc"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse And Keyboard"",
                    ""action"": ""Camera Control"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""8bc75802-51a0-4a73-b82b-9524bfa5640a"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse And Keyboard"",
                    ""action"": ""Camera Control"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""81c139ee-8c35-4e70-9f0e-c31368e1c5b3"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Camera Control"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""29920b77-3ce8-4425-9ab9-0eff5d59fc57"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse And Keyboard"",
                    ""action"": ""Cursor Control"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1533ac4a-613f-4a77-a642-f4904f71e00b"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": ""ScaleVector2(x=10,y=10)"",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Cursor Control"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5d2bf0d3-430f-4c4f-aa4a-1a6d5fc0f623"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse And Keyboard"",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""987cf4b8-728d-48d6-a201-7005f12a0988"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b4c3bc74-01c1-46cd-8225-fca269a9f932"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse And Keyboard"",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""79632050-ca5d-4c90-b935-ceeccd9dbef6"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse And Keyboard"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6d3aec7a-11e3-4828-a52f-5df41e0eaad5"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3c509121-21d8-4f0b-8ef0-23cf9ecfabf2"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse And Keyboard"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7b846bfd-6d4b-4ed9-a091-d13727b2d902"",
                    ""path"": ""<Keyboard>/leftCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse And Keyboard"",
                    ""action"": ""End Turn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""28b7c5b1-42f1-4f9d-9234-fc667251c2c6"",
                    ""path"": ""<Gamepad>/select"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""End Turn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Triggers"",
                    ""id"": ""27bea0b2-f2db-43c9-a811-3a5b95634955"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": ""Scale(factor=60)"",
                    ""groups"": """",
                    ""action"": ""Camera Zoom Control"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""38f7f779-d7c9-4215-aba7-ee08d24a1c40"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Camera Zoom Control"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""15bd6b8b-50d8-4ca3-b900-379731728bbd"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Camera Zoom Control"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""b5c73e1b-399b-41ec-b6d8-0f2de841e12f"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse And Keyboard"",
                    ""action"": ""Camera Zoom Control"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5a47bf08-5a2a-4485-9b63-d2d416d658ef"",
                    ""path"": ""<Gamepad>/dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Navigate Grid"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Arrow Keys"",
                    ""id"": ""3455ff81-cebb-4c60-8224-4ada4cddaa75"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate Grid"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""7eb35801-fe9e-4754-aa62-a33e46100de7"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse And Keyboard"",
                    ""action"": ""Navigate Grid"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""5a8d2f32-2a3a-4872-9b8b-f5cbdcd2d1a1"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse And Keyboard"",
                    ""action"": ""Navigate Grid"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""b9130458-47c2-47b3-a658-c466d867b2e0"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse And Keyboard"",
                    ""action"": ""Navigate Grid"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""64499316-0716-4db4-b6ed-4f2580e65726"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse And Keyboard"",
                    ""action"": ""Navigate Grid"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<XInputController>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Mouse And Keyboard"",
            ""bindingGroup"": ""Mouse And Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Gameplay
        m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
        m_Gameplay_CameraControl = m_Gameplay.FindAction("Camera Control", throwIfNotFound: true);
        m_Gameplay_CameraZoomControl = m_Gameplay.FindAction("Camera Zoom Control", throwIfNotFound: true);
        m_Gameplay_CursorControl = m_Gameplay.FindAction("Cursor Control", throwIfNotFound: true);
        m_Gameplay_NavigateGrid = m_Gameplay.FindAction("Navigate Grid", throwIfNotFound: true);
        m_Gameplay_Select = m_Gameplay.FindAction("Select", throwIfNotFound: true);
        m_Gameplay_Cancel = m_Gameplay.FindAction("Cancel", throwIfNotFound: true);
        m_Gameplay_EndTurn = m_Gameplay.FindAction("End Turn", throwIfNotFound: true);
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

    // Gameplay
    private readonly InputActionMap m_Gameplay;
    private IGameplayActions m_GameplayActionsCallbackInterface;
    private readonly InputAction m_Gameplay_CameraControl;
    private readonly InputAction m_Gameplay_CameraZoomControl;
    private readonly InputAction m_Gameplay_CursorControl;
    private readonly InputAction m_Gameplay_NavigateGrid;
    private readonly InputAction m_Gameplay_Select;
    private readonly InputAction m_Gameplay_Cancel;
    private readonly InputAction m_Gameplay_EndTurn;
    public struct GameplayActions
    {
        private @Controls m_Wrapper;
        public GameplayActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @CameraControl => m_Wrapper.m_Gameplay_CameraControl;
        public InputAction @CameraZoomControl => m_Wrapper.m_Gameplay_CameraZoomControl;
        public InputAction @CursorControl => m_Wrapper.m_Gameplay_CursorControl;
        public InputAction @NavigateGrid => m_Wrapper.m_Gameplay_NavigateGrid;
        public InputAction @Select => m_Wrapper.m_Gameplay_Select;
        public InputAction @Cancel => m_Wrapper.m_Gameplay_Cancel;
        public InputAction @EndTurn => m_Wrapper.m_Gameplay_EndTurn;
        public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void SetCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterface != null)
            {
                @CameraControl.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnCameraControl;
                @CameraControl.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnCameraControl;
                @CameraControl.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnCameraControl;
                @CameraZoomControl.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnCameraZoomControl;
                @CameraZoomControl.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnCameraZoomControl;
                @CameraZoomControl.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnCameraZoomControl;
                @CursorControl.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnCursorControl;
                @CursorControl.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnCursorControl;
                @CursorControl.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnCursorControl;
                @NavigateGrid.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnNavigateGrid;
                @NavigateGrid.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnNavigateGrid;
                @NavigateGrid.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnNavigateGrid;
                @Select.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSelect;
                @Select.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSelect;
                @Select.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSelect;
                @Cancel.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnCancel;
                @Cancel.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnCancel;
                @Cancel.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnCancel;
                @EndTurn.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnEndTurn;
                @EndTurn.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnEndTurn;
                @EndTurn.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnEndTurn;
            }
            m_Wrapper.m_GameplayActionsCallbackInterface = instance;
            if (instance != null)
            {
                @CameraControl.started += instance.OnCameraControl;
                @CameraControl.performed += instance.OnCameraControl;
                @CameraControl.canceled += instance.OnCameraControl;
                @CameraZoomControl.started += instance.OnCameraZoomControl;
                @CameraZoomControl.performed += instance.OnCameraZoomControl;
                @CameraZoomControl.canceled += instance.OnCameraZoomControl;
                @CursorControl.started += instance.OnCursorControl;
                @CursorControl.performed += instance.OnCursorControl;
                @CursorControl.canceled += instance.OnCursorControl;
                @NavigateGrid.started += instance.OnNavigateGrid;
                @NavigateGrid.performed += instance.OnNavigateGrid;
                @NavigateGrid.canceled += instance.OnNavigateGrid;
                @Select.started += instance.OnSelect;
                @Select.performed += instance.OnSelect;
                @Select.canceled += instance.OnSelect;
                @Cancel.started += instance.OnCancel;
                @Cancel.performed += instance.OnCancel;
                @Cancel.canceled += instance.OnCancel;
                @EndTurn.started += instance.OnEndTurn;
                @EndTurn.performed += instance.OnEndTurn;
                @EndTurn.canceled += instance.OnEndTurn;
            }
        }
    }
    public GameplayActions @Gameplay => new GameplayActions(this);
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    private int m_MouseAndKeyboardSchemeIndex = -1;
    public InputControlScheme MouseAndKeyboardScheme
    {
        get
        {
            if (m_MouseAndKeyboardSchemeIndex == -1) m_MouseAndKeyboardSchemeIndex = asset.FindControlSchemeIndex("Mouse And Keyboard");
            return asset.controlSchemes[m_MouseAndKeyboardSchemeIndex];
        }
    }
    public interface IGameplayActions
    {
        void OnCameraControl(InputAction.CallbackContext context);
        void OnCameraZoomControl(InputAction.CallbackContext context);
        void OnCursorControl(InputAction.CallbackContext context);
        void OnNavigateGrid(InputAction.CallbackContext context);
        void OnSelect(InputAction.CallbackContext context);
        void OnCancel(InputAction.CallbackContext context);
        void OnEndTurn(InputAction.CallbackContext context);
    }
}
