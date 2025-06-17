public class PlaySession
{
    private float _elapsedPlayTime = 0f;
    public float ElapsedPlayTime => _elapsedPlayTime;

    public bool IsCleared { get; private set; }

    private bool _isRunning;

    public void Start()
    {
        _isRunning = true;
        _elapsedPlayTime = 0f;
        IsCleared = false;
    }

    public void Update(float deltaTime)
    {
        if (_isRunning)
            _elapsedPlayTime += deltaTime;
    }

    public void MarkCleared()
    {
        IsCleared = true;
        _isRunning = false;
    }

    public void End()
    {
        _isRunning = false;
    }

    public PlaySessionDTO ToDTO()
    {
        return new PlaySessionDTO(this);
    }
}
