using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class InitManager : MonoBehaviour
{
    public static InitManager Instance { get; private set; }

    private readonly HashSet<string> _initializedKeys = new HashSet<string>();
    private readonly List<Action> _onAllInitializedCallbacks = new List<Action>();

    [SerializeField] private List<string> _expectedKeys;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ReportInitialized(string key)
    {
        _initializedKeys.Add(key);

        if (IsAllInitialized())
        {
            foreach (var callback in _onAllInitializedCallbacks)
                callback.Invoke();

            _onAllInitializedCallbacks.Clear(); // 중복 방지
        }
    }

    public void RegisterOnAllInitialized(Action callback)
    {
        if (IsAllInitialized()) callback.Invoke();
        else _onAllInitializedCallbacks.Add(callback);
    }

    private bool IsAllInitialized()
    {
        return _expectedKeys.All(k => _initializedKeys.Contains(k));
    }
}
