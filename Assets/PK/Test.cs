using UnityEngine;
using TMPro;

public class Test : MonoBehaviour
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


    [Range(1, 99)] public float DifficultyLevel = 1f;
    private float _currentTime = 0f;

    private void Update()
    {
        _currentTime += Time.deltaTime;

        Refresh();
    }
    
    private void Refresh()
    {
        RefreshTop();
        RefreshMid();
        RefreshBottom();
    }
    private void RefreshTop()
    {
        DifficultyLevelText.text = $"레벨 <b>{(int)DifficultyLevel}</b>";
        
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
    }

    private void RefreshBottom()
    {
        Vector2 difficultyPosition = DifficultyScrollContent.anchoredPosition;
        float multiplier = (DifficultyLevel - 1f) / 3f;
        difficultyPosition.x = -DifficultyWitdth * multiplier;
        DifficultyScrollContent.anchoredPosition = difficultyPosition;
    }
}
