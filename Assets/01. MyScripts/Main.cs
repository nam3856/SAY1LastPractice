using UnityEngine;

public class Main : MonoBehaviour
{
    private void Start()
    {
        // 도메인: 해결하고자 하는 문제 영역, 지식 자체를 의미한다.
        // 도메인 모델(모델링): 도메인과 그 규칙을 추상화하여 표현하는 것

        Currency gold = new Currency(ECurrencyType.Gold, 100);
        Currency diamond = new Currency(ECurrencyType.Diamond, 50);

        Debug.Log($"Initial {gold.ToString()}, {diamond.ToString()}");

    }
}
