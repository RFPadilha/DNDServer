using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Networking;
using SFB; // Standalone File Browser namespace

public class ImageUploadMenu : MonoBehaviour
{
    string path;
    public RawImage image;
    public SpriteRenderer spriteRenderer;

    private void Awake()
    {
        image = GetComponentInChildren<RawImage>();
    }

    public void OpenExplorer()
    {
        // Abre o explorador de arquivos e permite escolher arquivos de imagem PNG ou JPEG
        var extensions = new[] {
            new ExtensionFilter("Image Files", "png", "jpg", "jpeg")
        };
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Select Image", "", extensions, false);

        if (paths.Length > 0 && !string.IsNullOrEmpty(paths[0]))
        {
            path = paths[0]; // Pega o primeiro arquivo selecionado
            GetImage();
        }
    }

    void GetImage()
    {
        if (!string.IsNullOrEmpty(path))
        {
            UpdateImage();
        }
    }

    void UpdateImage()
    {
        StartCoroutine(DownloadImage());
    }

    IEnumerator DownloadImage()
    {
        string filePath = "file:///" + path;

        // Carregar a imagem como textura
        WWW www = new WWW(filePath);
        yield return www;

        // Criar uma textura 2D
        Texture2D mapTexture = www.texture;

        // Criar um sprite a partir da textura
        Rect rect = new Rect(0, 0, mapTexture.width, mapTexture.height);
        Sprite newMapSprite = Sprite.Create(mapTexture, rect, new Vector2(0.5f, 0.5f));

        // Atribuir o novo sprite ao SpriteRenderer do mapa
        spriteRenderer.sprite = newMapSprite;

        //UnityWebRequest request = UnityWebRequestTexture.GetTexture("file:///" + path);
        //yield return request.SendWebRequest();

        //if (request.result != UnityWebRequest.Result.Success)
        //{
        //    Debug.Log(request.error);
        //}
        //else
        //{
        //    image.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        //}
    }
}
