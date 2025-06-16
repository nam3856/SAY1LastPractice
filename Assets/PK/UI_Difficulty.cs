using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
public class UI_Difficulty : MonoBehaviour
{
    [Header("Top Left")]
    public TextMeshProUGUI DifficultyLevelText;
    
    [Header("Top Right")]
    public TextMeshProUGUI TImeText;

    
    [Header("Mid")]
    public RectTransform MidScrollContent;
    public float MidSpeed = 10f;
    public float ResetPostionX = -35f;

    [Header("Bottom")]
    public RectTransform DifficultyScrollContent;
    public float DifficultyWitdth = 170f;
    public List<Button> DifficultyButtons;
    private int _currentDifficultyIndex = 0;


    private float _currentTime = 0f;

    private void Start()
    {
        GameManager.Instance.Events.Difficulty.OnSliderChanged += OnSliderChanged;
        GameManager.Instance.Events.Difficulty.OnTierChanged += OnTierChanged;
    }

    private void OnDestroy()
    {
        GameManager.Instance.Events.Difficulty.OnSliderChanged -= OnSliderChanged;
        GameManager.Instance.Events.Difficulty.OnTierChanged -= OnTierChanged;
    }
    private void Update()
    {
        _currentTime += Time.deltaTime;
        _smoothDifficultyLevel = Mathf.Lerp(_smoothDifficultyLevel, _targetDifficultyLevel, Time.deltaTime * SmoothSpeed);

        Refresh();
    }

    private void OnTierChanged(DifficultyDTO tier)
    {
        DifficultyButtons[_currentDifficultyIndex].interactable = false;
        _currentDifficultyIndex++;
        if (_currentDifficultyIndex >= DifficultyButtons.Count)
        {
            return;
        }
        DifficultyButtons[_currentDifficultyIndex].interactable = true;
    }
    private void OnSliderChanged(float value)
    {
        _targetDifficultyLevel = value;
    }

    private float _targetDifficultyLevel = 1;
    private float _smoothDifficultyLevel = 1;
    public float SmoothSpeed = 5f;
    private void Refresh()
    {
        RefreshTop();
        RefreshMid();
        RefreshBottom();
    }
    private void RefreshTop()
    {
        DifficultyLevelText.text = $"레벨 <b>{(int)_smoothDifficultyLevel}</b>";
    }

    private void UpdateTime()
    {
        int minutes = (int)(_currentTime / 60f);
        int seconds = (int)(_currentTime % 60f);
        int centiseconds = (int)((_currentTime % 1f) * 100f);
        TImeText.text = $"{minutes:D2}:{seconds:D2}<voffset=0.4em><size=50%>{centiseconds:D2}</size></voffset>";
    }

    private void RefreshMid()
    {
        Vector2 currentPosition = MidScrollContent.anchoredPosition;
        
        currentPosition.x -= MidSpeed;
        
        if (currentPosition.x <= ResetPostionX)
        {
            currentPosition.x = 0f;
        }
        
        MidScrollContent.anchoredPosition = currentPosition;

        UpdateTime();
    }

    private void RefreshBottom()
    {
        Vector2 difficultyPosition = DifficultyScrollContent.anchoredPosition;
        float multiplier = (_smoothDifficultyLevel - 1f) / 3f;
        difficultyPosition.x = -DifficultyWitdth * multiplier;
        DifficultyScrollContent.anchoredPosition = difficultyPosition;
    }

}
