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

    public void ReceberResposta(string respostaJson)
    {
        // Converte o JSON que veio do Node.js para a classe C#
        RespostaNPC npc = JsonUtility.FromJson<RespostaNPC>(respostaJson);

        // Mostra apenas o texto limpo para o jogador!
        chatText.text += "\nNPC: " + npc.dialogo;
    }
}

[System.Serializable]
public class RespostaNPC
{
    public string dialogo;
    public float nivel_abertura;
    public string emocao_detectada;
}