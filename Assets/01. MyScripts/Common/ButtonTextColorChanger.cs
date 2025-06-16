using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // EventSystem 관련 인터페이스를 사용하기 위해 추가

public class ButtonTextColorChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, ISelectHandler, IDeselectHandler, IPointerClickHandler
{
    public Color OriginalColor = Color.black; // 기본 색상
    public Color HoverColor = Color.blue;     // 마우스 오버 시 색상
    public Color PressedColor = Color.red;    // 클릭 시 색상
    public Color SelectedColor = Color.green; // 선택되었을 때 색상 (예: 키보드 네비게이션)
    public Color DisabledColor = Color.gray;  // 비활성화되었을 때 색상

    private TMP_Text buttonText; // 버튼에 연결된 TextMeshPro 텍스트 컴포넌트
    private Button button;       // 버튼 컴포넌트

    void Awake()
    {
        // 현재 GameObject 또는 자식에서 TMP_Text 컴포넌트 찾기
        buttonText = GetComponentInChildren<TMP_Text>();
        if (buttonText == null)
        {
            Debug.LogError("ButtonTextColorChanger: No TMPro text component found in children.", this);
        }

        // 현재 GameObject에서 Button 컴포넌트 찾기
        button = GetComponent<Button>();
        if (button == null)
        {
            Debug.LogError("ButtonTextColorChanger: No Button component found on this GameObject.", this);
        }

        // 초기 색상 설정
        if (buttonText != null)
        {
            buttonText.color = OriginalColor;
        }

    }

    void OnEnable()
    {
        // 버튼이 활성화될 때 (특히 비활성화에서 활성화로 전환될 때)
        UpdateTextColor();
    }

    void OnDisable()
    {
        // 버튼이 비활성화될 때
        if (buttonText != null)
        {
            buttonText.color = DisabledColor;
        }
    }

    // 모든 상태 변경 시 호출하여 텍스트 색상을 업데이트하는 함수
    public void UpdateTextColor()
    {
        if (button == null || buttonText == null) return;

        if (!button.interactable)
        {
            buttonText.color = DisabledColor;
        }
        else if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject == gameObject)
        {
            buttonText.color = SelectedColor;
        }
        else
        {
            buttonText.color = OriginalColor;
        }
    }


    // 마우스 포인터가 버튼 위로 진입했을 때
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button.interactable)
        {
            buttonText.color = HoverColor;
        }
    }

    // 마우스 포인터가 버튼 밖으로 나갔을 때
    public void OnPointerExit(PointerEventData eventData)
    {
        if (button.interactable)
        {
            // 선택된 상태가 아니거나, 클릭 중이 아니라면 원래 색상으로
            if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject == gameObject)
            {
                buttonText.color = SelectedColor;
            }
            else
            {
                buttonText.color = OriginalColor;
            }
        }
    }

    // 마우스 버튼이 눌렸을 때
    public void OnPointerDown(PointerEventData eventData)
    {
        if (button.interactable)
        {
            buttonText.color = PressedColor;
        }
    }

    // 마우스 버튼에서 손을 떼었을 때
    public void OnPointerUp(PointerEventData eventData)
    {
        if (button.interactable)
        {
            if (eventData.IsPointerMoving()) // 마우스가 움직여서 버튼 밖으로 나갔을 경우
            {
                if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject == gameObject)
                {
                    buttonText.color = SelectedColor;
                }
                else
                {
                    buttonText.color = OriginalColor;
                }
            }
            else // 마우스가 버튼 위에 그대로 있을 경우 (클릭 발생 전)
            {
                buttonText.color = HoverColor;
            }
        }
    }

    // 버튼이 클릭되었을 때 (PointerDown -> PointerUp이 동일 오브젝트에서 발생 시)
    public void OnPointerClick(PointerEventData eventData)
    {
        if (button.interactable)
        {
            if (eventData.pointerEnter == gameObject) // 아직 버튼 위에 있다면
            {
                buttonText.color = HoverColor;
            }
            else
            {
                buttonText.color = OriginalColor;
            }
        }
    }

    // UI 요소가 선택되었을 때 (예: 키보드 네비게이션으로 버튼 선택)
    public void OnSelect(BaseEventData eventData)
    {
        if (button.interactable)
        {
            buttonText.color = SelectedColor;
        }
    }

    // UI 요소의 선택이 해제되었을 때
    public void OnDeselect(BaseEventData eventData)
    {
        if (button.interactable)
        {
            buttonText.color = OriginalColor;
        }
    }

    
}