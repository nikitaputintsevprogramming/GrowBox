using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Sliders : MonoBehaviour
{
    public Slider sliderTemp;
    public Slider sliderWater;

    public TMP_Text textTemp;
    public TMP_Text textWater;

    private void Start()
    {
        textTemp.text = sliderTemp.value.ToString("0") + "°С";
        textWater.text = sliderWater.value.ToString("0") + "%";
    }

    public void SliderTempChange()
    {
        StartCoroutine(UpdateTempTextWithDelay(sliderTemp.value));
    }

    public void SliderWaterChange()
    {
        StartCoroutine(UpdateWaterTextWithDelay(sliderWater.value));
    }

    private IEnumerator UpdateTempTextWithDelay(float targetValue)
    {
        float currentValue = sliderTemp.value;
        float duration = 0.5f; // Длительность анимации
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newValue = Mathf.Lerp(currentValue, targetValue, elapsedTime / duration);
            textTemp.text = Mathf.Round(newValue).ToString() + "°С"; // Округляем до целого числа
            yield return null; // Ждем следующего кадра
        }

        // Устанавливаем окончательное значение
        textTemp.text = Mathf.Round(targetValue).ToString() + "°С"; // Округляем до целого числа
    }

    private IEnumerator UpdateWaterTextWithDelay(float targetValue)
    {
        float currentValue = sliderWater.value;
        float duration = 0.5f; // Длительность анимации
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newValue = Mathf.Lerp(currentValue, targetValue, elapsedTime / duration);
            textWater.text = Mathf.Round(newValue).ToString() + "%"; // Округляем до целого числа
            yield return null; // Ждем следующего кадра
        }

        // Устанавливаем окончательное значение
        textWater.text = Mathf.Round(targetValue).ToString() + "%"; // Округляем до целого числа
    }
}

