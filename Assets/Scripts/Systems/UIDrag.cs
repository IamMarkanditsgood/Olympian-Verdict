using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDrag : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] RectTransform rectTransform;
    private Canvas canvas;
    private bool isDragging = false;

    [Header("Цільова позиція для переміщення після відпускання")]
    public Vector2 targetPosition = Vector2.zero; // Позиція, до якої потрібно переміститися
    public float moveSpeed = 10f; // Швидкість переміщення до цільової точки
    private bool shouldMoveToTarget = false;

    [SerializeField] private RectTransform targetHell;
    [SerializeField] private RectTransform targetHeaven;
    [SerializeField] private RectTransform targetReincarnation;
    [SerializeField] private GameManager gameManager;
    public bool _canMove = false;
    [SerializeField] private AudioManager _audioManager;

    [SerializeField] private GameObject _fire;
    [SerializeField] private GameObject _star;

    public string _currentCharacterName;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        if (canvas == null)
        {
            Debug.LogError("Не знайдено Canvas у батьківських елементах!");
        }
    }

    // Метод для обробки натискання миші на елемент
    public void OnPointerDown(PointerEventData eventData)
    {
        if (_canMove)
        {
            isDragging = true;
            shouldMoveToTarget = false;
        }
    }

    // Метод для обробки відпускання миші або пальця
    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        shouldMoveToTarget = true;
    }

    // Метод для обробки перетягування елемента
    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging && canvas != null)
        {
            Vector2 mouseDelta = eventData.delta / canvas.scaleFactor;
            rectTransform.anchoredPosition += mouseDelta;
        }
    }
   
    private void Update()
    {
        // Переміщуємо об'єкт до цільової точки після відпускання миші або пальця
        if (shouldMoveToTarget)
        {
            float distanceHell = Vector2.Distance(rectTransform.anchoredPosition, targetHell.anchoredPosition);
            float distanceHeaven = Vector2.Distance(rectTransform.anchoredPosition, targetHeaven.anchoredPosition);
            float distanceReincarnation = Vector2.Distance(rectTransform.anchoredPosition, targetReincarnation.anchoredPosition);
            float distanceMain = Vector2.Distance(rectTransform.anchoredPosition, targetPosition);
            if (distanceHell < distanceMain && distanceHell < distanceHeaven && distanceHell < distanceReincarnation)
            {
                float lengh = _audioManager.GetLenghCurrentSound();

                gameManager.CharacterCondemnedHell(lengh);
                rectTransform.anchoredPosition = targetPosition;
                shouldMoveToTarget = false;
                _canMove = false;
            }
            else if (distanceHeaven < distanceMain && distanceHeaven < distanceHell && distanceHeaven < distanceReincarnation)
            {
                float lengh = _audioManager.GetLenghCurrentSound();
                gameManager.CharacterCondemnedHeaven(lengh);
                rectTransform.anchoredPosition = targetPosition;
                shouldMoveToTarget = false;
                _canMove = false;
            }
            else if (distanceReincarnation < distanceMain && distanceReincarnation < distanceHeaven &&  distanceReincarnation < distanceHell)
            {
                float lengh = _audioManager.GetLenghCurrentSound();
                gameManager.CharacterCondemnedReincarnedAsync(lengh);
                rectTransform.anchoredPosition = targetPosition;
                shouldMoveToTarget = false;
                _canMove = false;
            }
            _star.SetActive(false);
            _fire.SetActive(false);
        }
        SoundsEffect();
    }
    private void SoundsEffect()
    {
        float distanceHell = Vector2.Distance(rectTransform.anchoredPosition, targetHell.anchoredPosition);
        float distanceHeaven = Vector2.Distance(rectTransform.anchoredPosition, targetHeaven.anchoredPosition);
        float distanceReincarnation = Vector2.Distance(rectTransform.anchoredPosition, targetReincarnation.anchoredPosition);
        float distanceMain = Vector2.Distance(rectTransform.anchoredPosition, targetPosition);
        if (_audioManager.GetLenghCurrentSound() == 0 && distanceMain < distanceHell && distanceMain < distanceHeaven && distanceMain < distanceReincarnation)
        {
            _star.SetActive(false);
            _fire.SetActive(false);
            if (PlayerPrefs.GetInt("Audio") == 1)
            {
                _audioManager.Stop();
            }

        }
        if (distanceHell < distanceMain && distanceHell < distanceHeaven && distanceHell < distanceReincarnation)
        {
            _fire.SetActive(true);
            _star.SetActive(false);
            if (PlayerPrefs.GetInt("Audio") == 1)
            {
                _audioManager.PlayHell(_currentCharacterName);
            }

        }
        else if (distanceHeaven < distanceMain && distanceHeaven < distanceHell && distanceHeaven < distanceReincarnation)
        {
            _fire.SetActive(false);
            _star.SetActive(true);
            if (PlayerPrefs.GetInt("Audio") == 1)
            {
                _audioManager.PlayHeaven();
            }

        }
        else if (distanceReincarnation < distanceMain && distanceReincarnation < distanceHeaven && distanceReincarnation < distanceHell)
        {
            _star.SetActive(false);
            _fire.SetActive(false);
            if (PlayerPrefs.GetInt("Audio") == 1)
            {
                _audioManager.ReincarnatePlay();
            }

        }
    }
    public void MoveToCenter()
    {
        rectTransform.anchoredPosition = Vector2.Lerp(
                rectTransform.anchoredPosition,
                targetPosition,
                Time.deltaTime * moveSpeed
            );

        // Перевірка, чи досягли ми цільової точки
        if (Vector2.Distance(rectTransform.anchoredPosition, targetPosition) < 0.1f)
        {
            rectTransform.anchoredPosition = targetPosition;
            shouldMoveToTarget = false;
        }
    }
}
