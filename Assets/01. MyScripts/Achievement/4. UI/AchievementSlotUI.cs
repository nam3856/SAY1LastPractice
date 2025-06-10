using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Gpm.Ui;

public class AchievementSlotUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _descText;
    [SerializeField] private Slider _progressSlider;
    [SerializeField] private Button _claimButton;
    [SerializeField] private TextMeshProUGUI _claimedDateText;

    private AchievementDTO _achievement;

    private void Start()
    {
        GameManager.Instance.Events.Achievement.OnAchievementUpdated += UpdateUI;
        GameManager.Instance.Events.Achievement.OnAchievementClaimed += UpdateUI;
    }

    private void OnDestroy()
    {
        if(GameManager.Instance != null)
        {
            GameManager.Instance.Events.Achievement.OnAchievementUpdated -= UpdateUI;
            GameManager.Instance.Events.Achievement.OnAchievementClaimed -= UpdateUI;
        }
    }

    private void UpdateUI(AchievementDTO achievement)
    {
        if (_achievement != null && _achievement.ID == achievement.ID)
        {
            SetData(achievement);
        }
    }

    public void SetData(AchievementDTO achievement = null)
    {
        if(achievement != null)
        {
            _achievement = achievement;
        }
        if(_achievement == null)
        {
            Debug.LogError("AchievementSlotUI: Achievement data is null.");
            return;
        }
        _titleText.text = achievement.Name;
        _descText.text = achievement.Description;
        _claimButton.gameObject.SetActive(!achievement.Rewarded);
        float progress = Mathf.Clamp01((float)achievement.CurrentValue / achievement.GoalValue);
        _progressSlider.value = progress;
        _claimedDateText.text = _achievement.ClaimedDate.ToString("yyyy.MM.dd") == "0001.01.01" ? 
            "" : _achievement.ClaimedDate.ToString("yyyy.MM.dd");
        _claimButton.interactable = !achievement.Rewarded && achievement.CurrentValue >= achievement.GoalValue;
        _claimButton.onClick.RemoveAllListeners();
        _claimButton.onClick.AddListener(ClaimReward);
    }

    private void ClaimReward()
    {
        if (_achievement.CurrentValue >= _achievement.GoalValue && !_achievement.Rewarded)
        {
            AchievementManager.Instance.TryClaimReward(_achievement);
            SetData(_achievement);
        }
    }
}
