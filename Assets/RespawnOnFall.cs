using UnityEngine;

public class RespawnOnFall : MonoBehaviour
{
    [Header("Настройки спавна")]
    public float fallThreshold = -10f;  // Если игрок упал ниже этой точки по Y — респавним
    public Vector3 respawnPosition = new Vector3(0, 1, 0); // Стартовая позиция

    private CharacterController controller;
    private bool isRespawning = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        // Запоминаем позицию спавна (можно поменять в инспекторе)
        respawnPosition = transform.position;
    }

    void Update()
    {
        // Если игрок упал ниже порога и мы не в процессе респавна
        if (transform.position.y < fallThreshold && !isRespawning)
        {
            StartCoroutine(Respawn());
        }
    }

    System.Collections.IEnumerator Respawn()
    {
        isRespawning = true;

        // Отключаем контроллер на момент телепортации (чтобы не было конфликтов)
        if (controller != null)
        {
            controller.enabled = false;
        }

        // Телепортируем игрока
        transform.position = respawnPosition;

        // Сбрасываем скорость (если есть компонент PlayerController, сбрасываем velocity)
        PlayerController playerController = GetComponent<PlayerController>();
        if (playerController != null)
        {
            // Если у вас в PlayerController есть переменная velocity, её можно сбросить
            // Это зависит от вашей реализации
        }

        // Ждём один кадр, чтобы физика успокоилась
        yield return null;

        // Включаем контроллер обратно
        if (controller != null)
        {
            controller.enabled = true;
        }

        isRespawning = false;
        Debug.Log("Игрок респавнится!");
    }

    // Опционально: визуализация порога в редакторе
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 thresholdPoint = new Vector3(transform.position.x, fallThreshold, transform.position.z);
        Gizmos.DrawLine(thresholdPoint - Vector3.right * 10f, thresholdPoint + Vector3.right * 10f);
        Gizmos.DrawWireCube(thresholdPoint, new Vector3(20f, 0.1f, 20f));
    }
}