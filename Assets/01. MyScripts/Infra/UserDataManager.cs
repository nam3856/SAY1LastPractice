using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Firestore;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class UserDataManager : MonoBehaviour
{
    FirebaseFirestore db;
    FirebaseAuth auth;

    public void Init()
    {
        db = FirebaseFirestore.DefaultInstance;
        auth = FirebaseAuth.DefaultInstance;

        Register();
    }

    public async Task CreateOrUpdateUserDocument(string nickname)
    {
        // 현재 로그인된 사용자 가져오기
        FirebaseUser user = auth.CurrentUser;

        if (user != null)
        {
            string userId = user.UserId; // 현재 사용자의 UID (이것을 문서 ID로 사용!)
            string userEmail = user.Email; // 사용자의 이메일

            // Firestore에 저장할 데이터 (딕셔너리 또는 커스텀 클래스)
            Dictionary<string, object> userData = new Dictionary<string, object>
            {
                { "email", userEmail },
                { "nickname", nickname } // 예시로 닉네임 필드 추가
                // 비밀번호는 저장하지 않습니다!
            };

            // 'users' 컬렉션에서 UID를 문서 ID로 하는 문서에 데이터 설정 (추가 또는 업데이트)
            // SetAsync를 사용하면 해당 ID의 문서가 없으면 생성하고, 있으면 덮어씁니다.
            await db.Collection("users").Document(userId).SetAsync(userData);

            Debug.Log($"UID {userId} 사용자의 데이터가 Firestore에 저장되었습니다.");
        }
        else
        {
            Debug.LogWarning("로그인된 사용자가 없습니다. 사용자 데이터를 저장할 수 없습니다.");
        }
    }

    // 사용자의 닉네임을 Firestore에서 불러오는 예시
    public async Task<string> GetUserNickname()
    {
        FirebaseUser user = auth.CurrentUser;

        if (user != null)
        {
            string userId = user.UserId;
            DocumentSnapshot docSnap = await db.Collection("users").Document(userId).GetSnapshotAsync();

            if (docSnap.Exists)
            {
                // 문서에서 닉네임 필드 가져오기
                string nickname = docSnap.GetValue<string>("nickname");
                Debug.Log($"UID {userId} 사용자의 닉네임: {nickname}");
                return nickname;
            }
            else
            {
                Debug.LogWarning($"UID {userId} 사용자의 Firestore 문서가 없습니다.");
                return null;
            }
        }
        else
        {
            Debug.LogWarning("로그인된 사용자가 없습니다. 사용자 닉네임을 가져올 수 없습니다.");
            return null;
        }
    }

    private void Register()
    {
        string email = "nam3856@gmail.com";//예시
        string password = "123456";//예시

        if (auth == null)
        {
            Debug.LogError("Firebase Auth is not initialized.");
            return;
        }


        auth.CreateUserWithEmailAndPasswordAsync(email, password)
            .ContinueWithOnMainThread(task =>
            {
                // 이 안에서는 boolean 값을 반환하지 않습니다.
                if (task.IsCanceled)
                {
                    Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    if(task.Exception.GetBaseException() is Firebase.FirebaseException firebaseException)
                    {
                        Debug.LogError($"Firebase Error Code: {firebaseException.ErrorCode}");
                    }
                    // Firebase Auth 예외 처리 로직 추가 가능 (예: weak-password, email-already-in-use 등)
                    // ex: if (task.Exception.GetBaseException() is FirebaseAuthException authException) { Debug.LogError($"Auth Error Code: {authException.ErrorCode}"); }
                    return;
                }

                // 성공 시 task.Result는 AuthResult 타입
                AuthResult authResult = task.Result;
                FirebaseUser newUser = authResult.User; // AuthResult에서 User 객체를 가져옵니다.

                Debug.LogFormat("Firebase user created successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);

                // 여기서 회원가입 성공 후 필요한 로직을 실행합니다.
                // 예를 들어, UserDataManager의 CreateOrUpdateUserDocument를 호출하여
                // Firestore에 사용자 닉네임 등을 저장할 수 있습니다.
                // string userNickname = "새로운유저닉네임"; // UI에서 입력받거나 기본값 설정
                // UserDataManager.Instance.CreateOrUpdateUserDocument(userNickname); // 싱글톤 사용 예시

            });
    }

}