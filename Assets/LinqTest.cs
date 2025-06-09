using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LinqTest : MonoBehaviour
{
    private void Start()
    {
        List<Student> students = new List<Student>()
        {
            new Student("������",28,"��"),
            new Student("������",22,"��"),
            new Student("���Ͽ�",27,"��"),
            new Student("������",27,"��"),
            new Student("��ä��",25,"��"),
            new Student("�質��",25,"��"),
        };

        // '�÷���' ���� '������'�� '��ȸ(����)'�ϴ� ���� ����.
        // C#�� �̷� ����� �۾��� ���ϰ� �ϱ� ���� LINQ(Language Integrated Query)��� ����� �����Ѵ�.
        // Query(����)��� �ܾ�� '���� (�����͸� ��û�ϰų� �˻��ϴ� ��ɹ�)'��� ���̴�.

        var all = from student in students
                  select student;

        all = students.Where(student => true); // ���� ���� ���

        foreach (var item in all)
        {
            Debug.Log(item);
        }

        Debug.Log("====================================");

        var girls = from student in students where student.Gender == "��" select student;
        girls = students.Where(student => student.Gender == "��"); // ���� ���� ���
        foreach(var item in girls)
        {
            Debug.Log(item);
        }
        Debug.Log("====================================");
        var girls2 = from student in students
                     where student.Gender =="��"
                     orderby student.Age ascending
                     select student;

        girls2 = students.Where(student => student.Gender == "��")
                         .OrderBy(student => student.Age); // ���� ���� ���

        foreach (var item in girls2)
        {
            Debug.Log(item);
        }

        int mensCount = students.Count(student => student.Gender == "��");
        Debug.Log($"���л� ��: {mensCount}");

        int totalAge = students.Sum(student => student.Age);
        Debug.Log($"�л����� ���� ��: {totalAge}");

        double averageAge = students.Average(student => student.Age);
        Debug.Log($"�л����� ���� ���: {averageAge}");

        bool isAllMan = students.All(student => student.Gender == "��");
        Debug.Log($"��� �л��� �����ΰ�? {isAllMan}");

        bool is30 = students.Any(student => student.Age >= 30);
        Debug.Log($"30�� �̻��� �л��� �ִ°�? {is30}");

        // ���� ����
        // Sort ���� �Լ��� ���������� ����ũ�μ���Ʈ�� �̸� ������ ��Ʈ�� ��Ʈ�� ����.
        // ��Ʈ�� ��Ʈ: �������� ũ��, ���� ���� ������ ���� Quick Sort, Heap Sort, Radix Sort�� ȥ���Ͽ� ����ϴ� ���� �˰����̴�.
        students.Sort();
    }
}
