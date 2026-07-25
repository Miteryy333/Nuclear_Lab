using UnityEngine;
using UnityEngine.UI;

public class AtomRenderer : MonoBehaviour
{
    // Префабы протонов и нейтронов (перетащите их в инспекторе)
    public GameObject protonPrefab;
    public GameObject neutronPrefab;

    // Параметры отображения (можно настраивать в инспекторе)
    public float baseRadius = 50f;      // Базовый радиус ядра
    public float radiusMultiplier = 1.5f; // Множитель радиуса на каждую частицу
    public float minRadius = 30f;       // Минимальный радиус (если частиц мало)
    public float maxRadius = 200f;      // Максимальный радиус (чтобы не вылетало за экран)

    // Очищает контейнер и рисует атом заново
    public void DrawAtom(int protonCount, int neutronCount, int maxProtons = 30, int maxNeutrons = 30)
    {
        // 1. Очищаем старые частицы
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // 2. Ограничиваем количество частиц (чтобы не перегружать сцену)
        int displayProtons = Mathf.Min(protonCount, maxProtons);
        int displayNeutrons = Mathf.Min(neutronCount, maxNeutrons);
        int totalParticles = displayProtons + displayNeutrons;

        // 3. Рассчитываем радиус ядра в зависимости от количества частиц
        //    Чем больше частиц, тем больше радиус (чтобы они не налезали друг на друга)
        float radius = Mathf.Clamp(
            baseRadius + totalParticles * radiusMultiplier,
            minRadius,
            maxRadius
        );

        // 4. Создаём протоны (красные)
        for (int i = 0; i < displayProtons; i++)
        {
            GameObject proton = Instantiate(protonPrefab, transform);
            Vector2 pos = GetRandomPositionInsideCircle(radius);
            proton.GetComponent<RectTransform>().anchoredPosition = pos;

            // Небольшой случайный поворот для разнообразия (опционально)
            proton.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
        }

        // 5. Создаём нейтроны (синие)
        for (int i = 0; i < displayNeutrons; i++)
        {
            GameObject neutron = Instantiate(neutronPrefab, transform);
            Vector2 pos = GetRandomPositionInsideCircle(radius);
            neutron.GetComponent<RectTransform>().anchoredPosition = pos;

            neutron.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
        }

        // 6. Опционально: выводим в консоль информацию о созданном атоме
        Debug.Log($"Атом создан: протонов={displayProtons}, нейтронов={displayNeutrons}, радиус={radius}");
    }

    // Генерирует случайную точку внутри круга (имитация ядра)
    private Vector2 GetRandomPositionInsideCircle(float radius)
    {
        // Случайное расстояние от центра (0 до radius)
        // Используем квадратный корень для равномерного распределения по площади
        float randomRadius = Mathf.Sqrt(Random.Range(0f, 1f)) * radius;

        // Случайный угол (0 до 360 градусов)
        float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;

        float x = Mathf.Cos(randomAngle) * randomRadius;
        float y = Mathf.Sin(randomAngle) * randomRadius;

        return new Vector2(x, y);
    }

    // Опционально: метод для очистки атома (можно вызывать извне)
    public void ClearAtom()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

}