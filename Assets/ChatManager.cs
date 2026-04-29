using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatManager : MonoBehaviour
{
    public TMP_InputField inputField;
    public TextMeshProUGUI chatText;

    public void EnviarMensagem()
    {
        string mensagem = inputField.text;

        chatText.text += "\nJogador: " + mensagem;

        // Aqui vamos chamar a IA depois
        StartCoroutine(APIManager.Instance.EnviarParaIA(mensagem));

        inputField.text = "";
    }

    public void ReceberResposta(string resposta)
    {
        chatText.text += "\nNPC: " + resposta;
    }
}