using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class AccountManager : MonoBehaviour
{
    public static AccountManager Instance;

    private Account _myAccount;
    public AccountDTO CurrentAccount => _myAccount.ToDTO();

    private AccountRepository _repository;

    private const string SALT = "123456";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        Init();
    }

    private void Init()
    {
        _repository = new AccountRepository();
    }

    public Result TryRegister(string email, string nickname, string password)
    {
        AccountSaveData saveData = _repository.Find(email);
        if (saveData != null)
        {
            return Result.Fail("이미 가입한 이메일입니다.");
        }

        string encryptedPassword = CryptoUtil.Encryption(password, SALT);
        Account account = new Account(email, nickname, encryptedPassword);
        _repository.Save(account.ToDTO());

        // 레포 저장

        return Result.Success();
    }

    public bool TryLogin(string email, string password)
    {
        AccountSaveData saveData = _repository.Find(email);
        if (saveData == null)
        {
            return false;
        }

        if (CryptoUtil.Verify(password, saveData.Password, SALT))
        {
            _myAccount = new Account(saveData.Email, saveData.Nickname, saveData.Password);
            return true;
        }

        return false;
    }

    public string GetNicknameByPlayerId(string playerId)
    {
        AccountSaveData saveData = _repository.Find(playerId);
        var nickname = saveData != null ? saveData.Nickname : "Unknown";
        return nickname;
    }

    public string GetMyNickname()
    {
        return _myAccount?.Nickname ?? string.Empty;
    }
    public string GetMyEmail()
    {
        return _myAccount?.Email ?? string.Empty;
    }
}