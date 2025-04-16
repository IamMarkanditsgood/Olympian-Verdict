using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemMover : MonoBehaviour
{
    public RectTransform imageRectTransform;
    [SerializeField] private CharacterActionPopup CharacterActionPopup;

    [SerializeField] private AudioClip _moveSound;
    [SerializeField] private AudioSource _moveSource;
    // �������� ������� ��� ����������
    public float targetOffset = -360f;
    public float centerOffset = 0f;

    // �������� ����������
    public float moveSpeed = 5f;

    // ��� ����� ���� ���������� ���
    public float pauseDuration = 0f;

    private Coroutine moveCoroutine;

    // ����� ��� ���������� ����
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
        // ��������� �������
        Vector2 initialPosition = imageRectTransform.anchoredPosition;

        // ��������� ������� ������� �� �� X � ��������� �� ��������
        float targetPositionX = moveRight ? targetOffset : -targetOffset;

        // ������ ����������
        while (Mathf.Abs(imageRectTransform.anchoredPosition.x - targetPositionX) > 0.1f)
        {
            // ˳���� ������������ �� �������� �� �������� �������� �� �� X
            float newX = Mathf.Lerp(imageRectTransform.anchoredPosition.x, targetPositionX, moveSpeed * Time.deltaTime);

            // ��������� ����� ���������� X
            imageRectTransform.anchoredPosition = new Vector2(newX, imageRectTransform.anchoredPosition.y);

            yield return null;
        }

        // ����� ����� ����������� �� ���������� �������
        yield return new WaitForSeconds(pauseDuration);

        // ����������� ���������� �� ���������� ������� �� �� X
        imageRectTransform.anchoredPosition = new Vector2(centerOffset, imageRectTransform.anchoredPosition.y);

        // ��������� ��䳿 ���� ���������� ����������
        if (moveRight)
        {
            CharacterActionPopup.PositiveDone();  // ���������, ��������� ��� ����������� ����������
        }
        else
        {
            CharacterActionPopup.NegativeDone();  // ���������, ��������� ��� ����������� ����������
        }
    }
}
