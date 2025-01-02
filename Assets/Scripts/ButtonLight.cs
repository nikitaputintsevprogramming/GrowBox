using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonLight : MonoBehaviour
{
    public Sprite spriteOn;
    public Sprite spriteOff;

    public Image imagebutton;
    public bool isOn = false; // Переменная для отслеживания состояния

    private ArduinoController arduinoController; // Ссылка на ArduinoController

    void Start()
    {
        imagebutton.sprite = spriteOff; // Устанавливаем начальное изображение
        arduinoController = FindObjectOfType<ArduinoController>(); // Находим компонент ArduinoController в сцене
    }

    void Update()
    {

    }

    public void OnClickButtonLight()
    {
        isOn = !isOn; // Переключаем состояние
        imagebutton.sprite = isOn ? spriteOn : spriteOff; // Устанавливаем соответствующее изображение

        // Отправляем сигнал на Arduino
        string signal = isOn ? "on 3" : "off 3";
        string signal2 = isOn ? "on 4" : "off 4";
        if (arduinoController != null)
        {
            arduinoController.SendSignal(signal); // Отправляем сигнал через ArduinoController
            arduinoController.SendSignal(signal2); // Отправляем сигнал через ArduinoController
        }
        else
        {
            Debug.LogError("ArduinoController not found!");
        }
    }
}

