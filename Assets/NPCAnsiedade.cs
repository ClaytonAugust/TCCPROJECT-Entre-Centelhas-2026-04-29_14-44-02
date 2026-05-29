using UnityEngine;
using TMPro;

public class NPCAnsiedade : MonoBehaviour
{
    [Header("Configurações de UI")]
    public GameObject painelChat;    // Arraste seu Canvas/Painel de chat aqui
    public TextMeshProUGUI chatText; // Arraste o texto do chat aqui
    public GameObject iconeAlerta;  // Arraste o objeto "!" aqui

    private bool jogadorPerto = false;
    private bool dialogoIniciado = false;

    void Start()
    {
        // Garante que tudo comece escondido ao iniciar o jogo
        if (painelChat != null) painelChat.SetActive(false);
        if (iconeAlerta != null) iconeAlerta.SetActive(false);
    }

    void Update()
    {
        // Se o jogador estiver perto e apertar a tecla E
        if (jogadorPerto && Input.GetKeyDown(KeyCode.E))
        {
            if (!dialogoIniciado)
            {
                IniciarIntroducao();
            }
            else
            {
                painelChat.SetActive(true);
            }
        }
    }

    void IniciarIntroducao()
    {
        if (painelChat != null) painelChat.SetActive(true);
        
        if (chatText != null)
        {
            chatText.text = "<color=red>Sistema:</color> Você encontrou a Ansiedade. Ela parece trêmula e evita te olhar.";
            chatText.text += "\n\n<color=yellow>Objetivo:</color> Tente conversar com ela de forma empática para que ela melhore.";
        }
        
        dialogoIniciado = true;
    }

    // UNIFICADO: Apenas uma função para entrada
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorPerto = true;
            if (iconeAlerta != null) iconeAlerta.SetActive(true); // Liga o "!"
            Debug.Log("Aperte E para interagir com a Ansiedade");
        }
    }

    // UNIFICADO: Apenas uma função para saída (resolve o erro CS0111)
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorPerto = false;
            if (iconeAlerta != null) iconeAlerta.SetActive(false); // Desliga o "!"
            if (painelChat != null) painelChat.SetActive(false);   // Fecha o chat ao se afastar
        }
    }
}