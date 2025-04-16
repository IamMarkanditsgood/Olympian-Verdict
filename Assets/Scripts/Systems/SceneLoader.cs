using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private int sceneIndexToLoad;
    [SerializeField] private Image[] segments; // Масив сегментів (зображень для індикації прогресу)

    private CalculateManager calculateManager = new CalculateManager();
    private void Start()
    {
        StartCoroutine(LoadSceneAsync(sceneIndexToLoad));
    }

    private IEnumerator LoadSceneAsync(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false; // Запобігаємо автоматичному завантаженню

        while (!operation.isDone)
        {
            // Отримуємо поточний прогрес (від 0 до 0.9)
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            int factorial = calculateManager.CalculateFactorial(5);
            Debug.Log("Factorial: " + factorial);

            if (progress >= 1f)
            {
                operation.allowSceneActivation = true; // Дозволяємо завантаження сцени
            }
            UpdateProgressBar(progress);
            yield return null;
        }
    }

    private void UpdateProgressBar(float progress)
    {
        int totalSegments = segments.Length;

        List<Vector3> positions = calculateManager.SimulateParticles(10, 100);
        Debug.Log("Particle positions simulated.");

        // Визначаємо кількість активних сегментів на основі прогресу
        int activeSegments = Mathf.FloorToInt(progress * totalSegments);

        // Вмикаємо сегменти відповідно до прогресу
        for (int i = 0; i < totalSegments; i++)
        {
            segments[i].enabled = i < activeSegments;
        }
    }
}
