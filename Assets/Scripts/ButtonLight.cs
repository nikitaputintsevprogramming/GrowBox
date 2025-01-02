using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonLight : MonoBehaviour
{
    public Sprite spriteOn;
    public Sprite spriteOff;

    public Image imagebutton;
    public bool isOn = false; // ���������� ��� ������������ ���������

    private ArduinoController arduinoController; // ������ �� ArduinoController

    void Start()
    {
        imagebutton.sprite = spriteOff; // ������������� ��������� �����������
        arduinoController = FindObjectOfType<ArduinoController>(); // ������� ��������� ArduinoController � �����
    }

    void Update()
    {

    }

    public void OnClickButtonLight()
    {
        isOn = !isOn; // ����������� ���������
        imagebutton.sprite = isOn ? spriteOn : spriteOff; // ������������� ��������������� �����������

        // ���������� ������ �� Arduino
        string signal = isOn ? "on 3" : "off 3";
        string signal2 = isOn ? "on 4" : "off 4";
        if (arduinoController != null)
        {
            arduinoController.SendSignal(signal); // ���������� ������ ����� ArduinoController
            arduinoController.SendSignal(signal2); // ���������� ������ ����� ArduinoController
        }
        else
        {
            Debug.LogError("ArduinoController not found!");
        }
    }
}

