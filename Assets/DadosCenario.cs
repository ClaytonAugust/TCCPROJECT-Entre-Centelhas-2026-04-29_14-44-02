
using UnityEngine;

[CreateAssetMenu(
    fileName = "NovoCenario",
    menuName = "Jogo/Cenario Narrativo"
)]
public class DadosCenario : ScriptableObject
{
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

    public string nomeCenario;

    public Quadro[] quadros;
}
