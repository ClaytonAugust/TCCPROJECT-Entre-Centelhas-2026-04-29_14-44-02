using UnityEngine;
using UnityEngine.UI;

public class GerenciadorInterface : MonoBehaviour
{
    [Header("Painéis do Jogo")]
    public GameObject painelMenuInicial;
    public GameObject painelCriacaoPersonagem;
    public GameObject painelDescricaoNpc; // O painel que explica a situação do Desprezo

    [Header("Botões do Menu")]
    public Button btnJogar;

    void Start()
    {
        // O jogo sempre começa mostrando APENAS o Menu Inicial
        painelMenuInicial.SetActive(true);
        painelCriacaoPersonagem.SetActive(false);
        painelDescricaoNpc.SetActive(false);

        // Bloqueia a movimentação do jogador logo no início para ele não andar no menu
        PlayerController.controleBloqueado = true;

        // Configura a ação do botão Jogar
        if (btnJogar != null)
            btnJogar.onClick.AddListener(AbrirCriacaoPersonagem);
    }

    void AbrirCriacaoPersonagem()
    {
        // Transição: Esconde o Menu e mostra a Criação de Personagem
        painelMenuInicial.SetActive(false);
        painelCriacaoPersonagem.SetActive(true);
    }

    // Método que será chamado quando o jogador terminar a criação
    public void FinalizarCriacaoEIniciarJogo()
    {
        painelCriacaoPersonagem.SetActive(false);
        
        // Ativa a tela com a breve descrição da situação do NPC de Desprezo
        painelDescricaoNpc.SetActive(true);
    }
}