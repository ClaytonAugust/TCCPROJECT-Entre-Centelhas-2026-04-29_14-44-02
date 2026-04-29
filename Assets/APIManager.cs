using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class APIManager : MonoBehaviour
{
    public static APIManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public IEnumerator EnviarParaIA(string mensagem)
    {
        string url = "SUA_API_AQUI";

        string json = "{\"mensagem\": \"" + mensagem + "\"}";

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string resposta = request.downloadHandler.text;

            FindObjectOfType<ChatManager>().ReceberResposta(resposta);
        }
        else
        {
            Debug.LogError("Erro: " + request.error);
        }
    }
}