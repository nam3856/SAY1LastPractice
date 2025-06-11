using System.Security.Cryptography;
using System.Text;
using UnityEngine;

// 암호화 기능을 제공하는 유틸
public class CryptoUtil : MonoBehaviour
{
    public static string Encryption(string plainText, string salt = "")
    {
        // 해시 암호화 알고리즘 인스턴스를 생성한다.
        SHA256 sha256 = SHA256.Create();

        // 운영체제 혹은 프로그래밍 언어별로 string 표현하는 방식이 다 다르므로
        // UTF8 버전 바이트로 배열로 바꿔야한다.
        byte[] bytes = Encoding.UTF8.GetBytes(plainText + salt);
        byte[] hash = sha256.ComputeHash(bytes);

        string resultText = string.Empty;
        foreach (byte b in hash)
        {
            // byte를 다시 string으로 바꿔서 이어붙이기
            resultText += b.ToString("X2");
        }

        return resultText;
    }

    public static bool Verify(string plainText, string hash, string salt = "")
    {
        return Encryption(plainText, salt) == hash;
    }
}