using UnityEngine;

public class AccountDTO
{
    public string Email;
    public string Nickname;
    public string Password;

    public AccountDTO(string email, string nickname, string password)
    {
        Email = email;
        Nickname = nickname;
        Password = password;
    }
}