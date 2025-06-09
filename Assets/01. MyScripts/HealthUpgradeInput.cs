using Unity.FPS.Game;
using UnityEngine;

public class HealthUpgradeInput : MonoBehaviour
{
    [SerializeField] private KeyCode key = KeyCode.Alpha5;
    [SerializeField] private int cost = 500;
    [SerializeField] private float hpIncrease = 100f;

    private PlayerUpgradeService _upgradeService;

    private void Start()
    {
        _upgradeService = new PlayerUpgradeService(
            CurrencyManager.Instance,
            GetComponent<Health>());
    }

    private void Update()
    {
        if (Input.GetKeyDown(key))
        {
            bool success = _upgradeService.TryUpgradeHealth(cost, hpIncrease);
            if (success)
                Debug.Log("체력 업그레이드 완료");
        }
    }
}
