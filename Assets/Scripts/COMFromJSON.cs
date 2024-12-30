using System.IO;
using UnityEngine;

public class COMFromJSON : MonoBehaviour
{
    //public delegate void ComLoaded();
    //public static event ComLoaded e_ComLoaded;

    // ���������� ��� �������� �����
    public int COM_number;

    // ��������� ��� �������� ������ �� JSON
    [System.Serializable]
    public class COMData
    {
        public int COM;  // �������� ����� � JSON
    }

    void OnEnable()
    {
        LoadCOMFromJSON();
    }

    // ����� ��� ������ ����� com.json
    void LoadCOMFromJSON()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "com.json");

        if (File.Exists(filePath))
        {
            string jsonContent = File.ReadAllText(filePath);
            COMData data = JsonUtility.FromJson<COMData>(jsonContent);
            COM_number = data.COM;
            Debug.Log("�������� COM: " + COM_number);
            //e_ComLoaded();
        }
        else
        {
            Debug.LogError("���� com.json �� ������.");
        }
    }
}
