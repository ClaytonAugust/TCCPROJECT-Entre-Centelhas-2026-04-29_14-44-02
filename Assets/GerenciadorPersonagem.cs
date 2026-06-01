using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Estrutura de dados idêntica à esperada pelo servidor Node.js
[System.Serializable]
public class MetricasEmocionais
{
    public float alegria;
    public float tristeza;
}

[System.Serializable]
public class DadosJogador
{
    public string nome;
    public int idade;
    public string sexo;
    public string historico;
    public MetricasEmocionais metricas;
}

public class GerenciadorPersonagem : MonoBehaviour
{
    public static GerenciadorPersonagem Instancia { get; private set; }

    [Header("Componentes de UI")]
    public TMP_InputField inputNome;
    public TMP_InputField inputIdade;
    public TMP_Dropdown dropdownSexo;
    public TMP_InputField inputHistorico;
    public Button btnIniciarJogo;

    [Header("Dados Armazenados")]
    public DadosJogador dadosAtuais;

    void Awake()
    {
        // Padrão Singleton para os dados persistirem entre a tela de menu e o cenário do jogo
        if (Instancia == null)
        {
            Instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (btnIniciarJogo != null)
            btnIniciarJogo.onClick.AddListener(SalvarEIniciar);
    }

    void SalvarEIniciar()
    {
        if (string.IsNullOrEmpty(inputNome.text) || string.IsNullOrEmpty(inputHistorico.text))
        {
            Debug.LogWarning("Por favor, preencha todos os campos!");
            return;
        }

        // Captura os dados normalmente
        dadosAtuais = new DadosJogador();
        dadosAtuais.nome = inputNome.text;
        dadosAtuais.idade = int.TryParse(inputIdade.text, out int resultIdade) ? resultIdade : 18;
        dadosAtuais.sexo = dropdownSexo.options[dropdownSexo.value].text;
        dadosAtuais.historico = inputHistorico.text;
        dadosAtuais.metricas = AnalisarHistoricoInicial(dadosAtuais.historico);

        // EM VEZ DE MUDAR DE CENA: Avisa o gerenciador de interface para avançar
        GerenciadorInterface gerenciadorUI = FindFirstObjectByType<GerenciadorInterface>();
        if (gerenciadorUI != null)
        {
            gerenciadorUI.FinalizarCriacaoEIniciarJogo();
        }
    }

    private MetricasEmocionais AnalisarHistoricoInicial(string textoHistorico)
    {
        MetricasEmocionais metricasIniciais = new MetricasEmocionais();
        string historicoMinusculo = textoHistorico.ToLower();

        // Valores base neutros
        float baseAlegria = 0.5f;
        float baseTristeza = 0.5f;

        // Dicionários simples de gatilhos para modular o estado inicial do jogador
        string[] termosTristes = { "pressão", "ansiedade", "triste", "sofrimento", "solidão", "trauma", "perda", "falha", "esgotado", "cansado" };
        string[] termosAlegres = { "otimista", "feliz", "sucesso", "animado", "determinado", "superação", "conquista", "paz" };

        foreach (string termo in termosTristes)
        {
            if (historicoMinusculo.Contains(termo))
            {
                baseTristeza += 0.08f;
                baseAlegria -= 0.05f;
            }
        }

        foreach (string termo in termosAlegres)
        {
            if (historicoMinusculo.Contains(termo))
            {
                baseAlegria += 0.08f;
                baseTristeza -= 0.05f;
            }
        }

        // Garante que os limites fiquem estritamente entre 0.0 e 1.0
        metricasIniciais.alegria = Mathf.Clamp(baseAlegria, 0.0f, 1.0f);
        metricasIniciais.tristeza = Mathf.Clamp(baseTristeza, 0.0f, 1.0f);

        return metricasIniciais;
    }
}