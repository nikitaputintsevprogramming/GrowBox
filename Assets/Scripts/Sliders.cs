using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

[System.Serializable]
public class WaitTimes
{
    public float waitWater;
    public float waitTemp;
}


public class Sliders : MonoBehaviour
{
    public Slider sliderTemp;
    public Slider sliderWater;

    public TMP_Text textTemp;
    public TMP_Text textWater;

    private Coroutine updateTempCoroutine;
    private Coroutine updateWaterCoroutine;

    private float waitTimeWater;
    private float waitTimeLight;

    private void Start()
    {
        LoadWaitTimes();

        // Устанавливаем начальные значения текста
        UpdateTempText();
        UpdateWaterText();

        // Добавляем слушатели для изменения значений слайдеров
        sliderTemp.onValueChanged.AddListener(delegate { StartTempUpdate(); });
        sliderWater.onValueChanged.AddListener(delegate { StartWaterUpdate(); });
    }

    private void LoadWaitTimes()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "wait.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            WaitTimes waitTimes = JsonUtility.FromJson<WaitTimes>(json);
            waitTimeWater = waitTimes.waitWater;
            waitTimeLight = waitTimes.waitTemp;
        }
        else
        {
            Debug.LogError("Cannot find wait.json file!");
        }
    }

    private void StartTempUpdate()
    {
        if (updateTempCoroutine != null)
        {
            StopCoroutine(updateTempCoroutine);
        }
        updateTempCoroutine = StartCoroutine(UpdateTempTextCoroutine());
    }

    private void StartWaterUpdate()
    {
        if (updateWaterCoroutine != null)
        {
            StopCoroutine(updateWaterCoroutine);
        }
        updateWaterCoroutine = StartCoroutine(UpdateWaterTextCoroutine());
    }

    private IEnumerator UpdateTempTextCoroutine()
    {
        float targetValue = sliderTemp.value;
        float currentValue = textTemp.text != "" ? float.Parse(textTemp.text.Substring(0, textTemp.text.Length - 2)) : targetValue;

        while (Mathf.Abs(currentValue - targetValue) > Mathf.Epsilon)
        {
            yield return new WaitForSeconds(waitTimeLight); // Используем значение из JSON

            if (currentValue < targetValue)
            {
                currentValue += 1;
            }
            else if (currentValue > targetValue)
            {
                currentValue -= 1;
            }

            textTemp.text = currentValue.ToString("0") + "°С";
        }
    }

    private IEnumerator UpdateWaterTextCoroutine()
    {
        float targetValue = sliderWater.value;
        float currentValue = textWater.text != "" ? float.Parse(textWater.text.Substring(0, textWater.text.Length - 1)) : targetValue;

        while (Mathf.Abs(currentValue - targetValue) > Mathf.Epsilon)
        {
            yield return new WaitForSeconds(waitTimeWater); // Используем значение из JSON

            if (currentValue < targetValue)
            {
                currentValue += 1;
            }
            else if (currentValue > targetValue)
            {
                currentValue -= 1;
            }

            textWater.text = currentValue.ToString("0") + "%";
        }
    }

    private void UpdateTempText()
    {
        textTemp.text = sliderTemp.value.ToString("0") + "°С";
    }

    private void UpdateWaterText()
    {
        textWater.text = sliderWater.value.ToString("0") + "%";
    }
}

