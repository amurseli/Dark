using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlash : MonoBehaviour
{
    private Transform cameraPointer;
    private CharacterController controller;
    private bool canSlash = true;
    public bool canJumpPostSlash { get; set; } = false;
    
    [Header("SlashAttributes")]
    [Tooltip("Recorrido total que va a hacer, además de distancia de chequeo. Si un enemigo esta a 5 y este valor es 10, va a hacer 5 para llegar hasta el y otro 5 para completar los 10, por ejemplo")]
    public float slashRange = 10f;
    public float slashDamage = 1f;
    public float slashCooldown = 0.5f;
    [Tooltip("Cuanto va a tardar el jugador en completar el recorrido. Menos es más rápido.")]
    public float slashSpeed = 0.3f;
    
    private void Start()
    {
        cameraPointer = Camera.main.transform;
        controller = transform.gameObject.GetComponent<CharacterController>();
    }

    IEnumerator DashThroughEnemy(Vector3 startPosition, Vector3 targetPosition, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Calcula la posición entre el inicio y el destino usando Lerp
            Vector3 newPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
        
            // Mueve el CharacterController hacia la nueva posición
            controller.Move(newPosition - transform.position);
        
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Asegúrate de llegar al destino final
        //transform.gameObject.GetComponent<PlayerMovement>().removeFallVelocity();
        controller.Move(targetPosition - transform.position);
        canJumpPostSlash = true;
        yield return new WaitForSeconds(2f);
        canJumpPostSlash = false;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.E) && canSlash)
        {
            canSlash = false;
            
            var playerPosition = transform.position;
            var cameraForward = cameraPointer.forward;
            var slashPosition = new Vector3();
            if (Physics.Raycast(playerPosition, cameraForward, out var hit, slashRange))
            {
                var enemyPos = hit.collider.gameObject.transform.position;
                var enemy = hit.collider.gameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.GetHit(slashDamage);

                    // Calcula la dirección hacia el enemigo y establece la posición final del dash
                    var directionToEnemy = (enemyPos - playerPosition).normalized; // Direccion normalizada hacia el enemigo
                    var targetPosition = enemyPos + directionToEnemy * 20.0f; // Ajusta la posición final justo antes del enemigo

                    Debug.DrawLine(playerPosition, enemyPos, Color.red);
                    Debug.DrawLine(playerPosition, targetPosition, Color.blue);

                    StartCoroutine(DashThroughEnemy(playerPosition, targetPosition, slashSpeed));
                }
            }

            StartCoroutine(SlashCooldown());
        }
    }

    
    IEnumerator SlashCooldown()
    {
        yield return new WaitForSeconds(slashCooldown);
        canSlash = true;
    }
    
    
    
}
