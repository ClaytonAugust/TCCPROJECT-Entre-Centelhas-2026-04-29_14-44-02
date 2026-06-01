using UnityEngine;
using TMPro; // Obrigatório para acessar os componentes do TextMeshPro

public class PlayerController : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float velocidade = 5f;

    [Header("Referência da UI (Opcional)")]
    public TMP_InputField campoTextoChat; // Arraste o InputField do Chat aqui

    private Rigidbody2D rb;
    private Vector2 movimento;

    // Variável global que outros scripts (como o Menu ou o ChatManager) podem alterar
    public static bool controleBloqueado = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 1. CHECAGEM DE BLOQUEIO: Se o jogador estiver digitando no chat ou se o controle foi travado globalmente
        if ((campoTextoChat != null && campoTextoChat.isFocused) || controleBloqueado)
        {
            movimento = Vector2.zero; // Zera o vetor para o personagem parar imediatamente
            return; // Interrompe o Update aqui, ignorando o WASD
        }

        // 2. CAPTURA DE INPUT (Apenas se não estiver bloqueado)
        float moveX = Input.GetAxisRaw("Horizontal"); // A, D, Seta Esquerda, Seta Direita
        float moveY = Input.GetAxisRaw("Vertical");   // W, S, Seta Cima, Seta Baixo

        movimento = new Vector2(moveX, moveY);
    }

    void FixedUpdate()
    {
        // Aplica o movimento físico no Rigidbody2D
        rb.MovePosition(rb.position + movimento.normalized * velocidade * Time.fixedDeltaTime);
    }
}