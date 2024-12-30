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
        string signal = isOn ? "on 1" : "off 1";
        if (arduinoController != null)
        {
            //arduinoController.SendSignal(signal); // ���������� ������ ����� ArduinoController
        }
        else
        {
            Debug.LogError("ArduinoController not found!");
        }
    }
}

