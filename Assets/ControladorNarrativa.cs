using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ControladorNarrativa : MonoBehaviour
{
    [Header("Dados do cenário")]
    [SerializeField] private DadosCenario cenario;

    [Header("Elementos da tela")]
    [SerializeField] private Image imagemQuadro;
    [SerializeField] private TMP_Text tituloQuadro;
    [SerializeField] private TMP_Text textoQuadro;

    [Header("Escolhas")]
    [SerializeField] private GameObject painelEscolhas;
    [SerializeField] private Button[] botoesOpcoes;

    [Header("Continuar")]
    [SerializeField] private Button botaoContinuar;

    private int quadroAtual = 0;
    private int proximoQuadroEscolhido = -1;
    private int opcaoSelecionada = -1;

    private List<int> respostasJogador = new List<int>();

    private void Start()
    {
        botaoContinuar.onClick.AddListener(Continuar);
        MostrarQuadro();
    }

    private void MostrarQuadro()
    {
        if (quadroAtual < 0 ||
            quadroAtual >= cenario.quadros.Length)
        {
            FinalizarCenario();
            return;
        }

        DadosCenario.Quadro quadro =
            cenario.quadros[quadroAtual];

        tituloQuadro.text = "";
        textoQuadro.text = "";

        tituloQuadro.text = quadro.titulo;
        textoQuadro.text = quadro.texto;

        imagemQuadro.sprite = quadro.imagem;
        imagemQuadro.gameObject.SetActive(
            quadro.imagem != null
        );

        opcaoSelecionada = -1;
        proximoQuadroEscolhido = -1;

        painelEscolhas.SetActive(quadro.exigeEscolha);

        botaoContinuar.interactable = !quadro.exigeEscolha;

        for (int i = 0; i < botoesOpcoes.Length; i++)
        {
            Button botao = botoesOpcoes[i];

            int totalOpcoes = quadro.opcoes == null
                ? 0
                : quadro.opcoes.Length;

            if (i < totalOpcoes && quadro.exigeEscolha)
            {
                botao.gameObject.SetActive(true);
                botao.interactable = true;

                TMP_Text textoBotao =
                    botao.GetComponentInChildren<TMP_Text>();

                textoBotao.text =
                    quadro.opcoes[i].textoOpcao;

                int indice = i;

                botao.onClick.RemoveAllListeners();

                botao.onClick.AddListener(
                    () => SelecionarOpcao(indice)
                );
            }
            else
            {
                botao.gameObject.SetActive(false);
            }
        }
    }

    private void SelecionarOpcao(int indice)
    {
        DadosCenario.Quadro quadro =
            cenario.quadros[quadroAtual];

        if (indice < 0 || indice >= quadro.opcoes.Length)
            return;

        opcaoSelecionada = indice;

        DadosCenario.Opcao opcao =
            quadro.opcoes[indice];

        proximoQuadroEscolhido = opcao.proximoQuadro;

        respostasJogador.Add(indice);

        textoQuadro.text = opcao.textoConsequencia;

        botaoContinuar.interactable = true;

        for (int i = 0; i < botoesOpcoes.Length; i++)
        {
            botoesOpcoes[i].interactable = false;
        }
    }

    private void Continuar()
    {
        if (opcaoSelecionada >= 0)
        {
            quadroAtual = proximoQuadroEscolhido;
        }
        else
        {
            quadroAtual++;
        }

        MostrarQuadro();
    }

    private void FinalizarCenario()
    {
        painelEscolhas.SetActive(false);
        botaoContinuar.gameObject.SetActive(false);

        tituloQuadro.text = "Fim do cenário";
        textoQuadro.text =
            "Você concluiu este cenário. " +
            "Reflita sobre como suas escolhas podem " +
            "influenciar a forma como você compreende " +
            "as experiências de uma pessoa idosa.";

        imagemQuadro.gameObject.SetActive(false);

        Debug.Log("Respostas do jogador:");

        foreach (int resposta in respostasJogador)
        {
            Debug.Log("Opção escolhida: " + (resposta + 1));
        }
    }
}