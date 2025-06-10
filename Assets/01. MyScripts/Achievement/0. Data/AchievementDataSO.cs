using System;
using UnityEngine;
// 런타임시 변하지 않는 값을 SO로 관리하면:
// 1. 기획자가 에디터에서 직접 수정이 가능하다.
// 2. 유지보수와 확장성이 좋아진다.
// 3. 도메인 객체(Achievement)는 상태(CurrentValue, isClaimed 등)만 관리하면 된다.
[CreateAssetMenu(fileName = "AchievementData", menuName = "Achievement/Create New Achievement")]
public class AchievementDataSO : ScriptableObject
{
    [SerializeField] private string _id;
    [SerializeField] private string _name;
    [SerializeField, TextArea] private string _description;
    [SerializeField] private EAchievementCondition _condition;
    [SerializeField] private int _goalValue;
    [SerializeField] private ECurrencyType _rewardCurrencyType;
    [SerializeField] private int _rewardCurrencyAmount;

    public string ID => _id;
    public string Name => _name;
    public string Description => _description;
    public EAchievementCondition Condition => _condition;
    public int GoalValue => _goalValue;
    public ECurrencyType RewardCurrencyType => _rewardCurrencyType;
    public int RewardCurrencyAmount => _rewardCurrencyAmount;
}