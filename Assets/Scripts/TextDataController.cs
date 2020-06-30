using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextDataController : MonoBehaviour
{
    public TextAsset firstNames;
    public TextAsset surnames;
    public int firstNameCount = 19948;
    public int surnameCount = 3706;
    
    public string GetName(int lineCount)
    {
        System.IO.StreamReader file = new System.IO.StreamReader("Assets/TextFiles/First.txt");
        for(int i = 1; i <lineCount; i++)
        {
            file.ReadLine();
        }
        return file.ReadLine();
    }
    public string GetSurname(int lineCount)
    {
        System.IO.StreamReader file = new System.IO.StreamReader("Assets/TextFiles/surname.txt");
        for (int i = 1; i < lineCount; i++)
        {
            file.ReadLine();
        }
        return file.ReadLine();
    }
}
