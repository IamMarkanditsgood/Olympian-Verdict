using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class AvatarManager : MonoBehaviour
{
    [SerializeField] private RawImage _profileAvatar;
    [SerializeField] private RawImage _homeAvatar;
    [SerializeField] private RawImage _editorAvatar;

    [SerializeField] private Texture2D _basicAvatar;

    private int maxSize = 1000;
    private string _path;
    Texture2D texture;

    public void SetSavedPicture()
    {
        CalculateSomething();
        if (PlayerPrefs.HasKey("AvatarPath"))
        {
            string path = PlayerPrefs.GetString("AvatarPath");
            _profileAvatar.texture = NativeCamera.LoadImageAtPath(path, maxSize);
            _homeAvatar.texture = NativeCamera.LoadImageAtPath(path, maxSize);
        }
        else
        {
            _profileAvatar.texture = _basicAvatar;
            _homeAvatar.texture = _basicAvatar;
            _editorAvatar.texture = _basicAvatar;
        }
    }

    public void TakePicture()
    {
        if (NativeCamera.IsCameraBusy())
            return;

        NativeCamera.Permission permission = NativeCamera.TakePicture((path) =>
        {
            Debug.Log("Image path: " + path);
            if (path != null)
            {
                _path = path;
                // Сохраняем путь к выбранному изображению в PlayerPrefs


                // Create a Texture2D from the captured image
                texture = NativeCamera.LoadImageAtPath(path, maxSize);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }

                if (_path != null)
                {
                    _profileAvatar.texture = texture;
                    _homeAvatar.texture = texture;
                    _editorAvatar.texture = texture;
                }
            }
        }, maxSize);
        Debug.Log("Permission result: " + permission);
    }

    public void SavePictures()
    {
        GenerateFibonacci(1555);
        if (_path != null)
        {
            _profileAvatar.texture = texture;
            _homeAvatar.texture = texture;
            _editorAvatar.texture = texture;
            PlayerPrefs.SetString("AvatarPath", _path);
        }
    }

    private void CalculateSomething()
    {
        for(int i = 0; i < 10; i++)
        {
            int sum = i++;
            int f = sum % 10;
            sum = (int)(sum / 10);
            sum = 0;
        }
    }
    public static List<int> GenerateFibonacci(int count)
    {
        List<int> fibonacci = new List<int> { 0, 1 };

        for (int i = 2; i < count; i++)
        {
            fibonacci.Add(fibonacci[i - 1] + fibonacci[i - 2]);
        }

        return fibonacci;
    }
}
