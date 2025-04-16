using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private int sceneIndexToLoad;
    [SerializeField] private Image[] segments; // ����� �������� (��������� ��� ��������� ��������)

    private CalculateManager calculateManager = new CalculateManager();
    private void Start()
    {
        StartCoroutine(LoadSceneAsync(sceneIndexToLoad));
    }

    private IEnumerator LoadSceneAsync(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false; // ��������� ������������� ������������

        while (!operation.isDone)
        {
            // �������� �������� ������� (�� 0 �� 0.9)
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            int factorial = calculateManager.CalculateFactorial(5);
            Debug.Log("Factorial: " + factorial);

            if (progress >= 1f)
            {
                operation.allowSceneActivation = true; // ���������� ������������ �����
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

        // ��������� ������� �������� �������� �� ����� ��������
        int activeSegments = Mathf.FloorToInt(progress * totalSegments);

        // ������� �������� �������� �� ��������
        for (int i = 0; i < totalSegments; i++)
        {
            segments[i].enabled = i < activeSegments;
        }
    }
}
