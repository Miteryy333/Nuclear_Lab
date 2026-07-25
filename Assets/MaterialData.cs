using UnityEngine;

public class MaterialData : MonoBehaviour
{
    public string materialName = "Материал";
    public int protons = 0;
    public int neutrons = 0;
    public string elementSymbol = "X";
    public float atomicMass = 0f;

    public bool isRadioactive = false;
    public float radiationLevel = 0f;
    public float halfLife = 0f;

    public int maxProtons = 30;
    public int maxNeutrons = 30;

    private Color elementColor = Color.white;
    private Renderer cachedRenderer;

    void Start()
    {
        cachedRenderer = GetComponent<Renderer>();
        if (ElementDatabase.Instance != null)
        {
            ElementData element = ElementDatabase.Instance.GetElementByProtons(protons);
            if (element != null)
            {
                if (ColorUtility.TryParseHtmlString(element.colorHex, out Color color))
                {
                    SetElementColor(color);
                }
            }
        }
        UpdateIsotopeInfo();
    }

    void Update()
    {
        if (isRadioactive && radiationLevel > 0)
        {
            UpdateRadiation(Time.deltaTime);
        }
    }

    public void SetElementColor(Color newColor)
    {
        elementColor = newColor;
        ApplyColorToRenderer(newColor);
    }

    private void ApplyColorToRenderer(Color color)
    {
        if (cachedRenderer != null)
        {
            cachedRenderer.material.color = color;
        }
    }

    public void RestoreElementColor()
    {
        ApplyColorToRenderer(elementColor);
    }

    public void UpdateIsotopeInfo()
    {
        if (ElementDatabase.Instance == null) return;

        IsotopeData isotope = ElementDatabase.Instance.GetIsotope(protons, neutrons);

        // --- ПЕРЕЗАПИСЫВАЕМ ВСЁ С НУЛЯ ---
        if (isotope != null)
        {
            atomicMass = isotope.atomicMass;
            halfLife = isotope.halfLife;

            // Чётко устанавливаем статус радиации на основе БАЗЫ ДАННЫХ
            if (isotope.isStable)
            {
                isRadioactive = false;
                radiationLevel = 0f;
                RestoreElementColor(); // Убираем зелёное свечение
            }
            else
            {
                isRadioactive = true;
                radiationLevel = 1f / Mathf.Max(halfLife, 0.1f);
            }
        }
        else
        {
            // Если изотоп не найден в базе — считаем его нестабильным
            isRadioactive = true;
            radiationLevel = 0.5f;
            halfLife = 10f;
            atomicMass = protons + neutrons; // Примерная масса
            Debug.LogWarning($"Изотоп {materialName}-{protons + neutrons} не найден в базе! Помечен как радиоактивный.");
        }
    }

    void UpdateRadiation(float deltaTime)
    {
        if (!isRadioactive || radiationLevel <= 0) return;

        // Свечение зелёным (эффект радиации)
        if (cachedRenderer != null)
        {
            float glow = Mathf.Sin(Time.time * 3f) * 0.3f + 0.7f;
            Color baseColor = elementColor;
            Color radioactiveColor = new Color(
                baseColor.r * (1 - glow * 0.3f),
                Mathf.Min(baseColor.g + glow * 0.5f, 1f),
                baseColor.b * (1 - glow * 0.3f)
            );
            cachedRenderer.material.color = radioactiveColor;
        }

        // Уменьшаем уровень радиации со временем
        radiationLevel -= deltaTime * 0.05f;

        // Альфа-распад (потеря нейтрона)
        if (radiationLevel <= 0.1f && neutrons > 0)
        {
            // Уменьшаем число нейтронов
            neutrons--;
            radiationLevel = 0f;

            // --- ИСПРАВЛЕНИЕ: проверяем новый изотоп, но не перезаписываем isRadioactive сразу ---
            IsotopeData newIsotope = ElementDatabase.Instance?.GetIsotope(protons, neutrons);
            if (newIsotope != null && newIsotope.isStable)
            {
                // Если новый изотоп стабильный — снимаем радиацию
                isRadioactive = false;
                atomicMass = newIsotope.atomicMass;
                RestoreElementColor();
                Debug.Log($"{materialName} стал стабильным! Теперь {materialName}-{protons + neutrons}");
            }
            else
            {
                // Если новый изотоп всё ещё радиоактивный — остаёмся радиоактивными
                // Обновляем атомную массу, но не меняем isRadioactive
                if (newIsotope != null)
                {
                    atomicMass = newIsotope.atomicMass;
                    halfLife = newIsotope.halfLife;
                    radiationLevel = 1f / Mathf.Max(halfLife, 0.1f);
                }
                Debug.Log($"{materialName} потерял нейтрон! Теперь {materialName}-{protons + neutrons} (всё ещё радиоактивен)");
            }

            // Обновляем UI, если панель открыта
            if (NuclearReactor.Instance != null)
            {
                NuclearReactor.Instance.Refresh();
            }
        }
    }

    public void ForceRadioactiveDecay()
    {
        if (isRadioactive && neutrons > 0)
        {
            neutrons--;
            UpdateIsotopeInfo();
            if (!isRadioactive)
            {
                RestoreElementColor();
            }
        }
    }
}