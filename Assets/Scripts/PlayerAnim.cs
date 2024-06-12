using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    public GameObject player;
    private Animator playerAnimator;
    private bool isAttacking = false;
    private bool isJumping = false;
    private Vector3 initialPosition;

    void Start()
    {
            playerAnimator = player.GetComponent<Animator>();
            initialPosition = player.transform.position;
    }

    void Update()
    {
        // Verifique se a tecla J foi pressionada e o jogador n�o est� atualmente atacando ou pulando
        if (Input.GetKeyDown(KeyCode.J) && !isAttacking && !isJumping)
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

        // Verifique se a tecla F foi pressionada e o jogador n�o est� atualmente pulando ou atacando
        if (Input.GetKeyDown(KeyCode.F) && !isJumping && !isAttacking)
        {
            StartCoroutine(Jump());
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

    // Corotina para fazer o jogador saltar
    private IEnumerator Jump()
    {
        // Defina a vari�vel "isJumping" como true para impedir outras a��es durante o salto
        isJumping = true;

        // Aumente a posi��o Y do jogador em 3 unidades
        Vector3 jumpPosition = initialPosition + new Vector3(0, 1.5f, 0);
        float jumpTime = 0.2f; // Tempo para o jogador alcan�ar o ponto mais alto do salto

        float elapsedTime = 0;
        while (elapsedTime < jumpTime)
        {
            player.transform.position = Vector3.Lerp(initialPosition, jumpPosition, (elapsedTime / jumpTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Garanta que o jogador chegue exatamente na posi��o mais alta
        player.transform.position = jumpPosition;

        // Aguarde um pequeno tempo no ponto mais alto
        yield return new WaitForSeconds(0.03f);

        // Volte o jogador para a posi��o inicial
        elapsedTime = 0;
        while (elapsedTime < jumpTime)
        {
            player.transform.position = Vector3.Lerp(jumpPosition, initialPosition, (elapsedTime / jumpTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Garanta que o jogador volte exatamente para a posi��o inicial
        player.transform.position = initialPosition;

        // Defina a vari�vel "isJumping" como false para permitir outras a��es
        isJumping = false;

        // Volte a anima��o de corrida, se necess�rio
        if (!isAttacking)
        {
            Run();
        }
    }
}
