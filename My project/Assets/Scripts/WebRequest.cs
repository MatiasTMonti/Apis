using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class WebRequest : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI texto;
    [SerializeField] private Button button;
    [SerializeField] private RawImage image;

    public void Change()
    {
        // A correct website page.
        StartCoroutine(GetRequest("https://dog.ceo/api/breeds/image/random"));
        StartCoroutine(GetTexture());
    }

    IEnumerator GetRequest(string uri)
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(uri);

        yield return webRequest.SendWebRequest();

        string resultado = webRequest.downloadHandler.text;

        Debug.Log(resultado);

        CatFact catFact = JsonUtility.FromJson<CatFact>(resultado);

        texto.text = catFact.message;
    }

    IEnumerator GetTexture()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("https://images.dog.ceo/breeds/spaniel-blenheim/n02086646_1514.jpg");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture2D myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            image.texture = myTexture;
        }
        
    }
}

class CatFact
{
    public string message;
    public string status;
}
