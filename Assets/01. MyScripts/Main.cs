using UnityEngine;

public class Main : MonoBehaviour
{
    private void Start()
    {
        // ������: �ذ��ϰ��� �ϴ� ���� ����, ���� ��ü�� �ǹ��Ѵ�.
        // ������ ��(�𵨸�): �����ΰ� �� ��Ģ�� �߻�ȭ�Ͽ� ǥ���ϴ� ��

        Currency gold = new Currency(ECurrencyType.Gold, 100);
        Currency diamond = new Currency(ECurrencyType.Diamond, 50);

        Debug.Log($"Initial {gold.ToString()}, {diamond.ToString()}");

    }
}
