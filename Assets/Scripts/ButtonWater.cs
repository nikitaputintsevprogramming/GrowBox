using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Не забудьте подключить пространство имен для TextMeshPro

[System.Serializable]
public class TimerData
{
    public float wateringTime; // Время полива в секундах
    public float cooldownTime;  // Время ожидания в секундах
}

public class ButtonWater : MonoBehaviour
{
    public Sprite spriteOn;
    public Sprite spriteOff;
    public Sprite spriteCooldown;

    public Image buttonImage;
    public TMP_Text textTimer; // Поле для отображения времени
    public TMP_Text textTimer2; // Поле для отображения времени
    public bool isCooldown = false;

    private ArduinoController arduinoController; // Ссылка на ArduinoController
    private TimerData timerData; // Данные таймера

    void Start()
    {
        textTimer2.gameObject.SetActive(false);
        textTimer.gameObject.SetActive(false);

        buttonImage.sprite = spriteOff; // Установить начальный спрайт (обычное состояние)
        arduinoController = FindObjectOfType<ArduinoController>(); // Находим компонент ArduinoController

        LoadTimerData(); // Загружаем данные таймера из JSON
        UpdateTimerText(0); // Обновляем текст таймера на 00:00:00
    }

    private void LoadTimerData()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "timer.json");

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            timerData = JsonUtility.FromJson<TimerData>(json);
            Debug.Log("Timer data loaded: " + json);
        }
        else
        {
            Debug.LogError("timer.json file not found!");
        }
    }

    public void OnClickButtonWater()
    {
        if (isCooldown) return; // Если находимся в режиме ожидания, ничего не делаем

        StartCoroutine(StartWatering());
    }

    private IEnumerator StartWatering()
    {
        // Меняем спрайт на "в процессе"
        buttonImage.sprite = spriteOn;
        buttonImage.GetComponent<Button>().interactable = false;

        // Отправляем сигнал "on 2" на Arduino
        if (arduinoController != null)
        {
            //arduinoController.SendSignal("on 2");
            Debug.Log("Signal sent: on 2");
        }
        else
        {
            Debug.LogError("ArduinoController not found!");
        }

        // Выводим сообщение "Запуск полива"
        Debug.Log("Запуск полива");

        // Ждем время полива из JSON
        yield return new WaitForSeconds(timerData.wateringTime);

        // После времени полива меняем спрайт на "недоступен"
        buttonImage.sprite = spriteCooldown;
        isCooldown = true;

        textTimer2.gameObject.SetActive(true);
        textTimer.gameObject.SetActive(true);

        // Отправляем сигнал "off 2" на Arduino
        if (arduinoController != null)
        {
            //arduinoController.SendSignal("off 2");
            Debug.Log("Signal sent: off 2");
        }
        else
        {
            Debug.LogError("ArduinoController not found!");
        }

        // Кулдаун
        float cooldownRemaining = timerData.cooldownTime;

        while (cooldownRemaining > 0)
        {
            // Обновляем текст таймера
            UpdateTimerText(cooldownRemaining);

            // Ждем 1 секунду
            yield return new WaitForSeconds(1f);
            cooldownRemaining--;
        }

        // После кулдауна возвращаем кнопку в исходное состояние
        buttonImage.sprite = spriteOff;
        isCooldown = false;
        buttonImage.GetComponent<Button>().interactable = true;

        textTimer2.gameObject.SetActive(false);
        textTimer.gameObject.SetActive(false);


        // Сбрасываем текст таймера
        UpdateTimerText(0);
    }

    private void UpdateTimerText(float time)
    {
        // Преобразуем время в часы, минуты и секунды
        int hours = Mathf.FloorToInt(time / 3600);
        int minutes = Mathf.FloorToInt((time % 3600) / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        // Форматируем строку времени
        string timeFormatted = string.Format("{1:00}:{2:00}", hours, minutes, seconds);
        textTimer.text = timeFormatted; // Обновляем текст на UI
    }
}
