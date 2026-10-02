
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ControladorNarrativa : MonoBehaviour
{
    [System.Serializable]
    private class RegistroEscolha
    {
        public string nomeCenario;
        public string nomeQuadro;
        public int numeroOpcao;
        public string textoOpcao;
        public string consequencia;
        public int pontuacao;

        public RegistroEscolha(
            string cenario,
            string quadro,
            int numero,
            string opcao,
            string efeito,
            int pontos)
        {
            nomeCenario = cenario;
            nomeQuadro = quadro;
            numeroOpcao = numero;
            textoOpcao = opcao;
            consequencia = efeito;
            pontuacao = pontos;
        }
    }

    [Header("Lista de cenários")]
    [SerializeField] private List<DadosCenario> cenarios =
        new List<DadosCenario>();

    [Header("Elementos da tela")]
    [SerializeField] private Image imagemQuadro;
    [SerializeField] private TMP_Text tituloQuadro;
    [SerializeField] private TMP_Text textoQuadro;

    [Header("Escolhas")]
    [SerializeField] private GameObject painelEscolhas;
    [SerializeField] private Button[] botoesOpcoes;

    [Header("Continuar")]
    [SerializeField] private Button botaoContinuar;

    private int cenarioAtual = 0;
    private int quadroAtual = 0;

    private int proximoQuadroEscolhido = -1;
    private int opcaoSelecionada = -1;

    private bool aguardandoProximoCenario = false;
    private bool jogoFinalizado = false;

    private readonly List<RegistroEscolha> respostasJogador =
        new List<RegistroEscolha>();

    private void Start()
    {
        botaoContinuar.onClick.AddListener(Continuar);

        if (cenarios == null || cenarios.Count == 0)
        {
            Debug.LogError("Nenhum cenário foi adicionado!");
            return;
        }

        MostrarQuadro();
    }

    private void MostrarQuadro()
    {
        if (cenarioAtual >= cenarios.Count)
        {
            FinalizarJogo();
            return;
        }

        DadosCenario cenario = cenarios[cenarioAtual];

        if (cenario == null || cenario.quadros == null)
        {
            Debug.LogError("Cenário ou quadros não configurados.");
            return;
        }

        if (quadroAtual < 0 ||
            quadroAtual >= cenario.quadros.Length)
        {
            FinalizarCenario();
            return;
        }

        DadosCenario.Quadro quadro =
            cenario.quadros[quadroAtual];

        tituloQuadro.text = quadro.titulo;
        textoQuadro.text = quadro.texto;

        imagemQuadro.sprite = quadro.imagem;
        imagemQuadro.gameObject.SetActive(
            quadro.imagem != null
        );

        opcaoSelecionada = -1;
        proximoQuadroEscolhido = -1;
        aguardandoProximoCenario = false;

        botaoContinuar.gameObject.SetActive(true);
        botaoContinuar.interactable = !quadro.exigeEscolha;

        DefinirTextoBotao("Continuar");

        painelEscolhas.SetActive(quadro.exigeEscolha);

        int totalOpcoes = quadro.opcoes == null
            ? 0
            : quadro.opcoes.Length;

        for (int i = 0; i < botoesOpcoes.Length; i++)
        {
            Button botao = botoesOpcoes[i];

            botao.onClick.RemoveAllListeners();

            if (i < totalOpcoes && quadro.exigeEscolha)
            {
                botao.gameObject.SetActive(true);
                botao.interactable = true;

                TMP_Text textoBotao =
                    botao.GetComponentInChildren<TMP_Text>();

                if (textoBotao != null)
                {
                    textoBotao.text =
                        quadro.opcoes[i].textoOpcao;
                }

                int indice = i;

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
        DadosCenario cenario = cenarios[cenarioAtual];

        DadosCenario.Quadro quadro =
            cenario.quadros[quadroAtual];

        if (quadro.opcoes == null ||
            indice < 0 ||
            indice >= quadro.opcoes.Length)
        {
            return;
        }

        // Impede registrar mais de uma escolha no mesmo quadro.
        if (opcaoSelecionada >= 0)
            return;

        opcaoSelecionada = indice;

        DadosCenario.Opcao opcao =
            quadro.opcoes[indice];

        proximoQuadroEscolhido = opcao.proximoQuadro;

        // Regra reflexiva: opção 1 = -2; opção 5 = +2.
        int pontuacao = indice - 2;

        RegistroEscolha registro = new RegistroEscolha(
            cenario.nomeCenario,
            quadro.titulo,
            indice + 1,
            opcao.textoOpcao,
            opcao.textoConsequencia,
            pontuacao
        );

        respostasJogador.Add(registro);

        textoQuadro.text = opcao.textoConsequencia;

        botaoContinuar.interactable = true;

        foreach (Button botao in botoesOpcoes)
        {
            botao.interactable = false;
        }
    }

    private void Continuar()
    {
        if (jogoFinalizado)
            return;

        if (aguardandoProximoCenario)
        {
            cenarioAtual++;

            if (cenarioAtual >= cenarios.Count)
            {
                FinalizarJogo();
                return;
            }

            quadroAtual = 0;
            opcaoSelecionada = -1;
            proximoQuadroEscolhido = -1;

            MostrarQuadro();
            return;
        }

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
        aguardandoProximoCenario = true;

        painelEscolhas.SetActive(false);
        imagemQuadro.gameObject.SetActive(false);

        tituloQuadro.text = "Cenário concluído";

        if (cenarioAtual < cenarios.Count - 1)
        {
            textoQuadro.text =
                "Você concluiu este cenário. Reflita sobre " +
                "as decisões tomadas e como elas podem " +
                "influenciar a autonomia e o bem-estar " +
                "das pessoas envolvidas.";

            DefinirTextoBotao("Próximo cenário");
        }
        else
        {
            textoQuadro.text =
                "Você concluiu o último cenário. " +
                "Continue para ver sua reflexão final.";

            DefinirTextoBotao("Ver resultado");
        }

        botaoContinuar.gameObject.SetActive(true);
        botaoContinuar.interactable = true;
    }

    private void FinalizarJogo()
    {
        jogoFinalizado = true;

        painelEscolhas.SetActive(false);
        imagemQuadro.gameObject.SetActive(false);

        tituloQuadro.text = "Sua reflexão final";
        textoQuadro.text = GerarFeedbackFinal();

        botaoContinuar.gameObject.SetActive(false);

        RegistrarRespostasNoConsole();
    }

    private string GerarFeedbackFinal()
    {
        if (respostasJogador.Count == 0)
        {
            return "Você concluiu as histórias. Pense em como " +
                   "ouvir as pessoas idosas e respeitar suas " +
                   "decisões pode contribuir para relações " +
                   "mais respeitosas entre gerações.";
        }

        int totalPontos = 0;

        List<string> cenariosPositivos = new List<string>();
        List<string> cenariosReflexao = new List<string>();

        // Agrupa os pontos por cenário.
        Dictionary<string, int> pontosPorCenario =
            new Dictionary<string, int>();

        Dictionary<string, int> escolhasPorCenario =
            new Dictionary<string, int>();

        foreach (RegistroEscolha registro in respostasJogador)
        {
            totalPontos += registro.pontuacao;

            if (!pontosPorCenario.ContainsKey(registro.nomeCenario))
            {
                pontosPorCenario[registro.nomeCenario] = 0;
                escolhasPorCenario[registro.nomeCenario] = 0;
            }

            pontosPorCenario[registro.nomeCenario] +=
                registro.pontuacao;

            escolhasPorCenario[registro.nomeCenario]++;
        }

        foreach (string nome in pontosPorCenario.Keys)
        {
            int pontos = pontosPorCenario[nome];

            if (pontos > 0)
            {
                cenariosPositivos.Add(nome);
            }
            else if (pontos < 0)
            {
                cenariosReflexao.Add(nome);
            }
        }

        float media =
            (float)totalPontos / respostasJogador.Count;

        StringBuilder mensagem = new StringBuilder();

        if (media >= 1f)
        {
            mensagem.Append(
                "Em suas escolhas, apareceu uma tendência a " +
                "considerar a autonomia, a escuta e a participação " +
                "das pessoas idosas nas decisões. "
            );

            mensagem.Append(
                "Continue valorizando o diálogo e oferecendo " +
                "apoio sem presumir que a idade impede alguém " +
                "de decidir por si."
            );
        }
        else if (media <= -0.5f)
        {
            mensagem.Append(
                "Suas escolhas abrem espaço para refletir sobre " +
                "como a intenção de ajudar pode, em algumas " +
                "situações, acabar retirando a autonomia de " +
                "uma pessoa idosa. "
            );

            mensagem.Append(
                "Antes de decidir por alguém, procure ouvir " +
                "o que essa pessoa pensa, deseja e consegue " +
                "fazer, oferecendo apoio sem presumir suas limitações."
            );
        }
        else
        {
            mensagem.Append(
                "Suas escolhas apresentaram diferentes formas " +
                "de lidar com as situações. "
            );

            mensagem.Append(
                "Uma reflexão importante é equilibrar o cuidado " +
                "com a autonomia: perguntar antes de ajudar, " +
                "ouvir a pessoa e considerar sua opinião nas decisões."
            );
        }

        if (cenariosPositivos.Count > 0)
        {
            mensagem.Append("\n\nNos cenários ");

            mensagem.Append(string.Join(", ", cenariosPositivos));

            mensagem.Append(
                ", suas escolhas favoreceram a participação " +
                "ou a autonomia da pessoa idosa."
            );
        }

        if (cenariosReflexao.Count > 0)
        {
            mensagem.Append("\n\nNos cenários ");

            mensagem.Append(string.Join(", ", cenariosReflexao));

            mensagem.Append(
                ", vale refletir sobre como incluir mais " +
                "a pessoa nas decisões e ouvir sua versão."
            );
        }

        mensagem.Append(
            "\n\nEste retorno é uma reflexão baseada nas " +
            "alternativas escolhidas durante o jogo, não " +
            "um diagnóstico ou uma avaliação psicológica."
        );

        return mensagem.ToString();
    }

    private void RegistrarRespostasNoConsole()
    {
        Debug.Log("===== REGISTRO DAS ESCOLHAS =====");

        foreach (RegistroEscolha registro in respostasJogador)
        {
            Debug.Log(
                "Cenário: " + registro.nomeCenario +
                "\nQuadro: " + registro.nomeQuadro +
                "\nOpção: " + registro.numeroOpcao +
                "\nTexto: " + registro.textoOpcao +
                "\nConsequência: " + registro.consequencia +
                "\nIndicador reflexivo: " + registro.pontuacao
            );
        }

        Debug.Log("===== FIM DO REGISTRO =====");
    }

    private void DefinirTextoBotao(string texto)
    {
        TMP_Text textoBotao =
            botaoContinuar.GetComponentInChildren<TMP_Text>();

        if (textoBotao != null)
        {
            textoBotao.text = texto;
        }
    }
}