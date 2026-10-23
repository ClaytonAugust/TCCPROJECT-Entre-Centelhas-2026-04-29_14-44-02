using UnityEngine;

[CreateAssetMenu(
    fileName = "NovoCenario",
    menuName = "Jogo/Cenario Narrativo"
)]
public class DadosCenario : ScriptableObject
{
    [Header("Tela de Seleção")]
    public Sprite imagemCapa;

    [Header("Informações do Cenário")]
    public string nomeCenario;

    [Header("Quadros Narrativos")]
    public Quadro[] quadros;

    [System.Serializable]
    public class Opcao
    {
        [TextArea(2, 4)]
        public string textoOpcao;

        [TextArea(2, 5)]
        public string textoConsequencia;

        public int proximoQuadro;
    }

    [System.Serializable]
    public class Quadro
    {
        public string titulo;

        [TextArea(3, 8)]
        public string texto;

        public Sprite imagem;

        public Opcao[] opcoes;

        public bool exigeEscolha;
    }

}
