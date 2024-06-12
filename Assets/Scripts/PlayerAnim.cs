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
        // Verifique se a tecla J foi pressionada e o jogador n�o est� atualmente atacando
        if (Input.GetKeyDown(KeyCode.J) && !isAttacking)
        {
            // Se o jogador estiver correndo, fa�a-o atacar
            if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
            {
                Attack();
            }
            // Se o jogador estiver atacando, fa�a-o voltar a correr
            else if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                Run();
            }
        }
    }

    // M�todo para fazer o jogador atacar
    private void Attack()
    {
        // Defina a vari�vel "isAttacking" como true para impedir a entrada durante o ataque
        isAttacking = true;
        // Inicie a anima��o de ataque
        playerAnimator.Play("Attack");
        // Aguarde o tempo da anima��o de ataque e, em seguida, fa�a o jogador voltar a correr
        StartCoroutine(ReturnToRunning(playerAnimator.GetCurrentAnimatorClipInfo(0).Length));
    }

    // M�todo para fazer o jogador correr
    private void Run()
    {
        // Defina a vari�vel "isAttacking" como false para permitir que o jogador ataque novamente
        isAttacking = false;
        // Inicie a anima��o de correr
        playerAnimator.Play("Run");
    }

    // Corotina para aguardar o tempo da anima��o de ataque e, em seguida, fazer o jogador voltar a correr
    private IEnumerator ReturnToRunning(float attackAnimationLength)
    {
        // Aguarde o tempo da anima��o de ataque
        yield return new WaitForSeconds(attackAnimationLength);
        // Fa�a o jogador voltar a correr
        Run();
    }
}

