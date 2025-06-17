using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Unity.FPS.UI
{
    public class InGameMenuManager : MonoBehaviour
    {
        [Tooltip("Root GameObject of the menu used to toggle its activation")]
        public GameObject MenuRoot;

        [Tooltip("Master volume when menu is open")] [Range(0.001f, 1f)]
        public float VolumeWhenMenuOpen = 0.5f;

        [Tooltip("Slider component for look sensitivity")]
        public Slider LookSensitivitySlider;

        [Tooltip("Toggle component for shadows")]
        public Toggle ShadowsToggle;

        [Tooltip("Toggle component for invincibility")]
        public Toggle InvincibilityToggle;

        [Tooltip("Toggle component for framerate display")]
        public Toggle FramerateToggle;

        [Tooltip("GameObject for the controls")]
        public GameObject ControlImage;

        PlayerInputHandler m_PlayerInputsHandler;
        Health m_PlayerHealth;
        FramerateCounter m_FramerateCounter;

        public GameObject AchievementUI;
        public GameObject AttendanceUI;
        public GameObject OptionUI;

        private CanvasGroup _achievementCanvasGroup;
        private CanvasGroup _attendanceCanvasGroup;
        private CanvasGroup _optionCanvasGroup;

        private Stack<GameObject> _uiStack = new Stack<GameObject>();

        void Start()
        {
            m_PlayerInputsHandler = FindFirstObjectByType<PlayerInputHandler>();
            DebugUtility.HandleErrorIfNullFindObject<PlayerInputHandler, InGameMenuManager>(m_PlayerInputsHandler,
                this);

            m_PlayerHealth = m_PlayerInputsHandler.GetComponent<Health>();
            DebugUtility.HandleErrorIfNullGetComponent<Health, InGameMenuManager>(m_PlayerHealth, this, gameObject);

            m_FramerateCounter = FindFirstObjectByType<FramerateCounter>();
            DebugUtility.HandleErrorIfNullFindObject<FramerateCounter, InGameMenuManager>(m_FramerateCounter, this);

            MenuRoot.SetActive(false);

            LookSensitivitySlider.value = m_PlayerInputsHandler.LookSensitivity;
            LookSensitivitySlider.onValueChanged.AddListener(OnMouseSensitivityChanged);

            ShadowsToggle.isOn = QualitySettings.shadows != ShadowQuality.Disable;
            ShadowsToggle.onValueChanged.AddListener(OnShadowsChanged);

            InvincibilityToggle.isOn = m_PlayerHealth.Invincible;
            InvincibilityToggle.onValueChanged.AddListener(OnInvincibilityChanged);

            FramerateToggle.isOn = m_FramerateCounter.UIText.gameObject.activeSelf;
            FramerateToggle.onValueChanged.AddListener(OnFramerateCounterChanged);

            _achievementCanvasGroup = AchievementUI.GetComponent<CanvasGroup>();
            _attendanceCanvasGroup = AttendanceUI.GetComponent<CanvasGroup>();
            _optionCanvasGroup = OptionUI.GetComponent<CanvasGroup>();
        }

        void Update()
        {
            // Lock cursor when clicking outside of menu
            if (!MenuRoot.activeSelf && Input.GetMouseButtonDown(0) && _achievementCanvasGroup.alpha == 0 && _attendanceCanvasGroup.alpha == 0)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            if (Input.GetButtonDown(GameConstants.k_ButtonNamePauseMenu)
                || (MenuRoot.activeSelf && Input.GetButtonDown(GameConstants.k_ButtonNameCancel)))
            {
                if(_uiStack.Count == 0)
                {
                    _uiStack.Push(MenuRoot);
                    SetPauseMenuActivation(true);
                }
                else
                {
                    var topUI = _uiStack.Pop();
                    if (topUI == MenuRoot)
                    {
                        SetPauseMenuActivation(false);
                    }
                    else
                    {
                        DeactiveUI(topUI);
                    }
                }
            }

            if (Input.GetAxisRaw(GameConstants.k_AxisNameVertical) != 0)
            {
                if (EventSystem.current.currentSelectedGameObject == null)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                    LookSensitivitySlider.Select();
                }
            }
        }

        private static void DeactiveUI(GameObject topUI)
        {
            var topUIComponent = topUI.GetComponent<CanvasGroup>();
            if (topUIComponent != null)
            {
                topUIComponent.alpha = 0f;
                topUIComponent.interactable = false;
                topUIComponent.blocksRaycasts = false;
            }
            else
            {
                topUI.SetActive(false);
            }
        }

        public void ClosePauseMenu()
        {
            SetPauseMenuActivation(false);
        }

        void SetPauseMenuActivation(bool active)
        {
            MenuRoot.SetActive(active);
            if (MenuRoot.activeSelf)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0f;
                AudioUtility.SetMasterVolume(VolumeWhenMenuOpen);

                EventSystem.current.SetSelectedGameObject(null);
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = 1f;
                AudioUtility.SetMasterVolume(1);
            }
        }

        bool _achievementButtonClicked = false;
        bool _attendanceButtonClicked = false;
        bool _optionButtonClicked = false;
        public void OnAchievementButtonClicked()
        {
            _achievementButtonClicked = !_achievementButtonClicked;
            _achievementCanvasGroup.alpha = _achievementButtonClicked?1f:0f;
            _achievementCanvasGroup.interactable = _achievementButtonClicked;
            _achievementCanvasGroup.blocksRaycasts = _achievementButtonClicked;
            if (_achievementButtonClicked)
            {
                _uiStack.Push(AchievementUI);
            }
            else if (_uiStack.Count > 0)
            {
                _uiStack.Pop();
            }
        }

        public void OnAttendanceButtonClicked()
        {
            _attendanceButtonClicked = !_attendanceButtonClicked;
            _attendanceCanvasGroup.alpha = _attendanceButtonClicked?1f:0f;
            _attendanceCanvasGroup.interactable = _attendanceButtonClicked;
            _attendanceCanvasGroup.blocksRaycasts = _attendanceButtonClicked;
            if (_attendanceButtonClicked)
            {
                _uiStack.Push(AttendanceUI);
            }
            else if (_uiStack.Count > 0)
            {
                _uiStack.Pop();
            }
        }

        public void OnExitButtonClicked()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public void OnOptionButtonClicked()
        {
            _optionButtonClicked = !_optionButtonClicked;
            _optionCanvasGroup.alpha = _optionButtonClicked ? 1f : 0f;
            _optionCanvasGroup.interactable = _optionButtonClicked;
            _optionCanvasGroup.blocksRaycasts = _optionButtonClicked;
            if(_optionButtonClicked)
            {
                _uiStack.Push(OptionUI);
            }
            else if (_uiStack.Count > 0)
            {
                while(_uiStack.Count > 0 && _uiStack.Peek() != OptionUI)
                {
                    _uiStack.Pop();
                }
                if (_uiStack.Count > 0 && _uiStack.Peek() == OptionUI)
                {
                    _uiStack.Pop();
                }
            }
        }


        void OnMouseSensitivityChanged(float newValue)
        {
            m_PlayerInputsHandler.LookSensitivity = newValue;
        }

        void OnShadowsChanged(bool newValue)
        {
            QualitySettings.shadows = newValue ? ShadowQuality.All : ShadowQuality.Disable;
        }

        void OnInvincibilityChanged(bool newValue)
        {
            m_PlayerHealth.Invincible = newValue;
        }

        void OnFramerateCounterChanged(bool newValue)
        {
            m_FramerateCounter.UIText.gameObject.SetActive(newValue);
        }

        public void OnShowControlButtonClicked(bool show)
        {
            ControlImage.SetActive(show);
            if (show)
            {
                _uiStack.Push(ControlImage);
            }
            else if(_uiStack.Count > 0 && _uiStack.Peek() == ControlImage)
            {
                _uiStack.Pop();
            }

        }
    }
}