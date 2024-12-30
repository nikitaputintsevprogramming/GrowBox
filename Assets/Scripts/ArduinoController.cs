using System.IO.Ports;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Unity.VisualScripting;

public class ArduinoController : MonoBehaviour
{
    SerialPort serialPort;

    void Start()
    {
        string com_number = FindObjectOfType<COMFromJSON>().COM_number.ToString();
        serialPort = new SerialPort("COM" + com_number, 9600); 
        //print("COM" + com_number);
        serialPort.Open();
        serialPort.ReadTimeout = 50;
    }

    //SendSignal(digitsOnly);
    public void SendSignal(string signal)
    {
        if (serialPort.IsOpen)
        {
            serialPort.Write(signal);
        }
        else
        {
            Debug.LogError("Serial port is not open");
        }
    }

    void OnApplicationQuit()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }
}
