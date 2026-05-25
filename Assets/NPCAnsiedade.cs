using UnityEngine;
using TMPro;

public class NPCAnsiedade : MonoBehaviour
{
    public GameObject painelChat; // Arraste seu Canvas/Painel de chat aqui
    public TextMeshProUGUI chatText; // Arraste o texto do chat aqui
    private bool jogadorPerto = false;
    private bool dialogoIniciado = false;

    void Start()
    {
        // Garante que o chat comece escondido
        painelChat.SetActive(false);
    }

    void Update()
    {
        // Se o jogador estiver perto e apertar a tecla E (ou outra de sua escolha)
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
        painelChat.SetActive(true);
        // Texto de introdução que você queria
        chatText.text = "<color=red>Sistema:</color> Você encontrou a Ansiedade. Ela parece trêmula e evita te olhar.";
        chatText.text += "\n\n<color=yellow>Objetivo:</color> Tente conversar com ela de forma empática para que ela melhore.";
        dialogoIniciado = true;
    }

    // Detecta quando o jogador entra na área
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorPerto = true;
            Debug.Log("Aperte E para interagir com a Ansiedade");
        }
    }

    // Detecta quando o jogador sai da área
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorPerto = false;
            painelChat.SetActive(false); // Fecha o chat ao se afastar
        }
    }
    public GameObject iconeAlerta;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            jogadorPerto = true;
            if (iconeAlerta != null) iconeAlerta.SetActive(true); // Liga o "!"
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            jogadorPerto = false;
            if (iconeAlerta != null) iconeAlerta.SetActive(false); // Desliga o "!"
            painelChat.SetActive(false); 
        }
    }
}
