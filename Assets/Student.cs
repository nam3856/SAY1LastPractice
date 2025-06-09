using UnityEngine;

public class Student
{
    public string Name;
    public int Age;
    public string Gender;
    public Student(string name, int age, string gender)
    {
        Name = name;
        Age = age;
        Gender = gender;
    }
    public override string ToString()
    {
        return $"{Name} ({Age} {Gender})";
    }
}
