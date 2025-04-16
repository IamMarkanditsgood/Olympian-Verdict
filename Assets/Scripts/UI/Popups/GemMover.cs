using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemMover : MonoBehaviour
{
    public RectTransform imageRectTransform;
    [SerializeField] private CharacterActionPopup CharacterActionPopup;

    [SerializeField] private AudioClip _moveSound;
    [SerializeField] private AudioSource _moveSource;
    // Значення зміщення для переміщення
    public float targetOffset = -360f;
    public float centerOffset = 0f;

    // Швидкість переміщення
    public float moveSpeed = 5f;

    // Час паузи після досягнення цілі
    public float pauseDuration = 0f;

    private Coroutine moveCoroutine;

    // Метод для переміщення вліво
    public void MoveToRight()
    {
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        if (PlayerPrefs.GetInt("Audio") == 1)
        {
            _moveSource.clip = _moveSound;
            _moveSource.Play();
        }
        moveCoroutine = StartCoroutine(MoveImage(-targetOffset, true));
    }

    public void MoveToLeft()
    {
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        if (PlayerPrefs.GetInt("Audio") == 1)
        {
            _moveSource.clip = _moveSound;
            _moveSource.Play();
        }
        moveCoroutine = StartCoroutine(MoveImage(-targetOffset, false));
    }

    private IEnumerator MoveImage(float targetOffset, bool moveRight)
    {
        // Початкова позиція
        Vector2 initialPosition = imageRectTransform.anchoredPosition;

        // Визначаємо цільову позицію по осі X в залежності від напрямку
        float targetPositionX = moveRight ? targetOffset : -targetOffset;

        // Плавне переміщення
        while (Mathf.Abs(imageRectTransform.anchoredPosition.x - targetPositionX) > 0.1f)
        {
            // Лінійно інтерполюємо між поточною та цільовою позицією по осі X
            float newX = Mathf.Lerp(imageRectTransform.anchoredPosition.x, targetPositionX, moveSpeed * Time.deltaTime);

            // Оновлюємо тільки координату X
            imageRectTransform.anchoredPosition = new Vector2(newX, imageRectTransform.anchoredPosition.y);

            yield return null;
        }

        // Пауза перед поверненням до центральної позиції
        yield return new WaitForSeconds(pauseDuration);

        // Моментальне повернення до центральної позиції по осі X
        imageRectTransform.anchoredPosition = new Vector2(centerOffset, imageRectTransform.anchoredPosition.y);

        // Викликаємо події після завершення переміщення
        if (moveRight)
        {
            CharacterActionPopup.PositiveDone();  // Наприклад, викликаємо для позитивного результату
        }
        else
        {
            CharacterActionPopup.NegativeDone();  // Наприклад, викликаємо для негативного результату
        }
    }
}
