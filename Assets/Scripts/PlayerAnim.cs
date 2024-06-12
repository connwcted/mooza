using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    private Animator playerAnimator;
    public GameObject player;
    private bool isAttacking = false;

    void Start()
    {
        // Obtenha o componente Animator do jogador
        playerAnimator = player.GetComponent<Animator>();
    }

    void Update()
    {
        // Verifique se a tecla J foi pressionada e o jogador não está atualmente atacando
        if (Input.GetKeyDown(KeyCode.J) && !isAttacking)
        {
            // Se o jogador estiver correndo, faça-o atacar
            if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
            {
                Attack();
            }
            // Se o jogador estiver atacando, faça-o voltar a correr
            else if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                Run();
            }
        }
    }

    // Método para fazer o jogador atacar
    private void Attack()
    {
        // Defina a variável "isAttacking" como true para impedir a entrada durante o ataque
        isAttacking = true;
        // Inicie a animação de ataque
        playerAnimator.Play("Attack");
        // Aguarde o tempo da animação de ataque e, em seguida, faça o jogador voltar a correr
        StartCoroutine(ReturnToRunning(playerAnimator.GetCurrentAnimatorClipInfo(0).Length));
    }

    // Método para fazer o jogador correr
    private void Run()
    {
        // Defina a variável "isAttacking" como false para permitir que o jogador ataque novamente
        isAttacking = false;
        // Inicie a animação de correr
        playerAnimator.Play("Run");
    }

    // Corotina para aguardar o tempo da animação de ataque e, em seguida, fazer o jogador voltar a correr
    private IEnumerator ReturnToRunning(float attackAnimationLength)
    {
        // Aguarde o tempo da animação de ataque
        yield return new WaitForSeconds(attackAnimationLength);
        // Faça o jogador voltar a correr
        Run();
    }
}

