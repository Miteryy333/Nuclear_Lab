using UnityEngine;
using UnityEngine.UI;

public class ReticleController : MonoBehaviour
{
    private Image reticleImage;

    void Start()
    {
        reticleImage = GetComponent<Image>();
        // По умолчанию прицел виден (игровой режим)
        reticleImage.enabled = true;
    }

    void Update()
    {
        // Если курсор разблокирован (режим меню) — скрываем прицел
        if (Cursor.lockState == CursorLockMode.None)
        {
            reticleImage.enabled = false;
        }
        else
        {
            reticleImage.enabled = true;
        }
    }
}