using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // �� �������� ���������� ������������ ���� ��� TextMeshPro

[System.Serializable]
public class TimerData
{
    public float wateringTime; // ����� ������ � ��������
    public float cooldownTime;  // ����� �������� � ��������
}

public class ButtonWater : MonoBehaviour
{
    public Sprite spriteOn;
    public Sprite spriteOff;
    public Sprite spriteCooldown;

    public Image buttonImage;
    public TMP_Text textTimer; // ���� ��� ����������� �������
    public TMP_Text textTimer2; // ���� ��� ����������� �������
    public bool isCooldown = false;

    private ArduinoController arduinoController; // ������ �� ArduinoController
    private TimerData timerData; // ������ �������

    void Start()
    {
        textTimer2.gameObject.SetActive(false);
        textTimer.gameObject.SetActive(false);

        buttonImage.sprite = spriteOff; // ���������� ��������� ������ (������� ���������)
        arduinoController = FindObjectOfType<ArduinoController>(); // ������� ��������� ArduinoController

        LoadTimerData(); // ��������� ������ ������� �� JSON
        UpdateTimerText(0); // ��������� ����� ������� �� 00:00:00
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
        if (isCooldown) return; // ���� ��������� � ������ ��������, ������ �� ������

        StartCoroutine(StartWatering());
    }

    private IEnumerator StartWatering()
    {
        // ������ ������ �� "� ��������"
        buttonImage.sprite = spriteOn;
        buttonImage.GetComponent<Button>().interactable = false;

        // ���������� ������ "on 2" �� Arduino
        if (arduinoController != null)
        {
            //arduinoController.SendSignal("on 2");
            Debug.Log("Signal sent: on 2");
        }
        else
        {
            Debug.LogError("ArduinoController not found!");
        }

        // ������� ��������� "������ ������"
        Debug.Log("������ ������");

        // ���� ����� ������ �� JSON
        yield return new WaitForSeconds(timerData.wateringTime);

        // ����� ������� ������ ������ ������ �� "����������"
        buttonImage.sprite = spriteCooldown;
        isCooldown = true;

        textTimer2.gameObject.SetActive(true);
        textTimer.gameObject.SetActive(true);

        // ���������� ������ "off 2" �� Arduino
        if (arduinoController != null)
        {
            //arduinoController.SendSignal("off 2");
            Debug.Log("Signal sent: off 2");
        }
        else
        {
            Debug.LogError("ArduinoController not found!");
        }

        // �������
        float cooldownRemaining = timerData.cooldownTime;

        while (cooldownRemaining > 0)
        {
            // ��������� ����� �������
            UpdateTimerText(cooldownRemaining);

            // ���� 1 �������
            yield return new WaitForSeconds(1f);
            cooldownRemaining--;
        }

        // ����� �������� ���������� ������ � �������� ���������
        buttonImage.sprite = spriteOff;
        isCooldown = false;
        buttonImage.GetComponent<Button>().interactable = true;

        textTimer2.gameObject.SetActive(false);
        textTimer.gameObject.SetActive(false);


        // ���������� ����� �������
        UpdateTimerText(0);
    }

    private void UpdateTimerText(float time)
    {
        // ����������� ����� � ����, ������ � �������
        int hours = Mathf.FloorToInt(time / 3600);
        int minutes = Mathf.FloorToInt((time % 3600) / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        // ����������� ������ �������
        string timeFormatted = string.Format("{1:00}:{2:00}", hours, minutes, seconds);
        textTimer.text = timeFormatted; // ��������� ����� �� UI
    }
}
