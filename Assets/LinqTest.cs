using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LinqTest : MonoBehaviour
{
    private void Start()
    {
        List<Student> students = new List<Student>()
        {
            new Student("허정범",28,"남"),
            new Student("백지헌",22,"여"),
            new Student("송하영",27,"여"),
            new Student("박지원",27,"여"),
            new Student("이채영",25,"여"),
            new Student("김나경",25,"여"),
        };

        // '컬렉션' 에서 '데이터'를 '조회(나열)'하는 일이 많다.
        // C#은 이런 빈번한 작업을 편하게 하기 위해 LINQ(Language Integrated Query)라는 기능을 제공한다.
        // Query(쿼리)라는 단어는 '질의 (데이터를 요청하거나 검색하는 명령문)'라는 뜻이다.

        var all = from student in students
                  select student;

        all = students.Where(student => true); // 위와 같은 결과

        foreach (var item in all)
        {
            Debug.Log(item);
        }

        Debug.Log("====================================");

        var girls = from student in students where student.Gender == "여" select student;
        girls = students.Where(student => student.Gender == "여"); // 위와 같은 결과
        foreach(var item in girls)
        {
            Debug.Log(item);
        }
        Debug.Log("====================================");
        var girls2 = from student in students
                     where student.Gender =="여"
                     orderby student.Age ascending
                     select student;

        girls2 = students.Where(student => student.Gender == "여")
                         .OrderBy(student => student.Age); // 위와 같은 결과

        foreach (var item in girls2)
        {
            Debug.Log(item);
        }

        int mensCount = students.Count(student => student.Gender == "남");
        Debug.Log($"남학생 수: {mensCount}");

        int totalAge = students.Sum(student => student.Age);
        Debug.Log($"학생들의 나이 합: {totalAge}");

        double averageAge = students.Average(student => student.Age);
        Debug.Log($"학생들의 나이 평균: {averageAge}");

        bool isAllMan = students.All(student => student.Gender == "남");
        Debug.Log($"모든 학생이 남자인가? {isAllMan}");

        bool is30 = students.Any(student => student.Age >= 30);
        Debug.Log($"30세 이상의 학생이 있는가? {is30}");

        // 정렬 문제
        // Sort 내장 함수는 내부적으로 마이크로소프트가 이름 지어준 인트로 소트를 쓴다.
        // 인트로 소트: 데이터의 크기, 종류 등의 성질에 따라 Quick Sort, Heap Sort, Radix Sort를 혼합하여 사용하는 정렬 알고리즘이다.
        students.Sort();
    }
}
