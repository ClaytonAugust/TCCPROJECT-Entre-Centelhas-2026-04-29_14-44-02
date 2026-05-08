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
        // 1. Log para ver o que chegou exatamente
        Debug.Log("Chegou no Unity: " + respostaJson);

        RespostaNPC npc = JsonUtility.FromJson<RespostaNPC>(respostaJson);

        // 2. Checagem de segurança para o objeto NPC
        if (npc == null) {
            Debug.LogError("O JsonUtility não conseguiu converter a resposta!");
            return;
        }

        // 3. Checagem de segurança para o componente de texto
        if (chatText != null) {
            chatText.text += "\nNPC: " + npc.dialogo;
        } else {
            Debug.LogError("ERRO: O campo 'Chat Text' não foi arrastado no Inspector!");
        }
    }
}

[System.Serializable]
public class RespostaNPC
{
    public string dialogo;
    public float nivel_abertura;
    public string emocao_detectada;
}