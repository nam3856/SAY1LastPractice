using System;
using System.Security.Cryptography;
using System.Text;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class UI_InputFields
{
    public TextMeshProUGUI ResultText;  // 결과 텍스트
    public TMP_InputField EmailInputField;
    public TMP_InputField NicknameInputField;
    public TMP_InputField PasswordInputField;
    public TMP_InputField PasswordComfirmInputField;
    public Button ConfirmButton;   // 로그인 or 회원가입 버튼
}

public class UI_LoginScene : MonoBehaviour
{
    [Header("패널")]
    public GameObject LoginPanel;
    public GameObject ResisterPanel;

    [Header("로그인")]
    public UI_InputFields LoginInputFields;

    [Header("회원가입")]
    public UI_InputFields RegisterInputFields;

    private const string PREFIX = "ID_";
    private const string SALT = "10043420";



    // 게임 시작하면 로그인 켜주고 회원가입은 꺼주고..
    private void Start()
    {
        LoginPanel.SetActive(true);
        ResisterPanel.SetActive(false);

        LoginInputFields.ResultText.text = string.Empty;
        RegisterInputFields.ResultText.text = string.Empty;
    }

    // 회원가입하기 버튼 클릭
    public void OnClickGoToResisterButton()
    {
        LoginPanel.SetActive(false);
        ResisterPanel.SetActive(true);
    }

    public void OnClickGoToLoginButton()
    {
        LoginPanel.SetActive(true);
        ResisterPanel.SetActive(false);
    }


    // 회원가입
    public void Resister()
    {
        // 1. 이메일 도메인 규칙을 확인한다.
        string email = RegisterInputFields.EmailInputField.text;
        var emailSpecification = new AccountEmailSpecification();
        if (!emailSpecification.IsSatisfiedBy(email))
        {
            RegisterInputFields.ResultText.text = emailSpecification.ErrorMessage;
            return;
        }

        // 2. 닉네임 도메인 규칙을 확인한다.
        string nickname = RegisterInputFields.NicknameInputField.text;
        var nicknameSpecification = new AccountNicknameSpecification();
        if (!nicknameSpecification.IsSatisfiedBy(nickname))
        {
            RegisterInputFields.ResultText.text = nicknameSpecification.ErrorMessage;
            return;
        }

        // 2. 1차 비밀번호 입력을 확인한다.
        string password = RegisterInputFields.PasswordInputField.text;
        var passwordSpecification = new AccountPasswordSpecification();
        if (!passwordSpecification.IsSatisfiedBy(password))
        {
            RegisterInputFields.ResultText.text = passwordSpecification.ErrorMessage;
            return;
        }

        // 3. 2차 비밀번호 입력을 확인하고, 1차 비밀번호 입력과 같은지 확인한다.
        string password2 = RegisterInputFields.PasswordComfirmInputField.text;
        if (!passwordSpecification.IsSatisfiedBy(password2))
        {
            RegisterInputFields.ResultText.text = passwordSpecification.ErrorMessage;
            return;
        }

        if (password != password2)
        {
            RegisterInputFields.ResultText.text = "비밀번혹가 다릅니다.";
            return;
        }

        if (AccountManager.Instance.TryRegister(email, nickname, password))
        {
            // 5. 로그인 창으로 돌아간다.
            // (이때 아이디는 자동 입력되어 있다.)
            OnClickGoToLoginButton();
        }

    }


    public void Login()
    {
        // 1. 이메일 입력을 확인한다.
        string email = LoginInputFields.EmailInputField.text;
        var emailSpecification = new AccountEmailSpecification();
        if (!emailSpecification.IsSatisfiedBy(email))
        {
            LoginInputFields.ResultText.text = emailSpecification.ErrorMessage;
            return;
        }

        // 2. 비밀번호 입력을 확인한다.
        string password = LoginInputFields.PasswordInputField.text;
        var passwordSpecification = new AccountPasswordSpecification();
        if (!passwordSpecification.IsSatisfiedBy(password))
        {
            LoginInputFields.ResultText.text = passwordSpecification.ErrorMessage;
            return;
        }

        if (AccountManager.Instance.TryLogin(email, password))
        {
            SceneManager.LoadScene(1);

        }
    }
}