using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AchievementNotifyUI : MonoBehaviour
{
    [SerializeField] private GameObject _popupPrefab;
    [SerializeField] private Transform _popupParent;
    [SerializeField] private float popupDuration = 2f; // 한 팝업이 유지되는 시간

    private Queue<AchievementDTO> _popupQueue = new Queue<AchievementDTO>();
    private bool _isShowing = false;

    private void Start()
    {
        GameManager.Instance.Events.Achievement.OnAchievementUnlocked += EnqueuePopup;
    }

    private void OnDestroy()
    {
        GameManager.Instance.Events.Achievement.OnAchievementUnlocked -= EnqueuePopup;
    }

    private void EnqueuePopup(AchievementDTO dto)
    {
        _popupQueue.Enqueue(dto);
        if (!_isShowing)
        {
            StartCoroutine(ProcessQueue());
        }
    }

    private IEnumerator ProcessQueue()
    {
        _isShowing = true;

        while (_popupQueue.Count > 0)
        {
            var dto = _popupQueue.Dequeue();
            GameObject go = Instantiate(_popupPrefab, _popupParent);
            go.GetComponent<AchievementPopup>().SetData(dto);

            yield return new WaitForSecondsRealtime(popupDuration);

            Destroy(go);
        }

        _isShowing = false;
    }
}
