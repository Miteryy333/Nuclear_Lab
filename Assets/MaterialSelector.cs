using UnityEngine;

public class MaterialSelector : MonoBehaviour
{
    public NuclearReactor reactor;
    private MaterialData materialData;
    private MaterialHighlighter highlighter;

    void Start()
    {
        materialData = GetComponent<MaterialData>();
        highlighter = GetComponent<MaterialHighlighter>();

        if (materialData == null)
        {
            Debug.LogError("На объекте " + gameObject.name + " нет компонента MaterialData!");
        }
        if (highlighter == null)
        {
            Debug.LogError("На объекте " + gameObject.name + " нет компонента MaterialHighlighter!");
        }
    }

    void OnMouseDown()
    {
        if (reactor == null)
        {
            Debug.LogError("Reactor не назначен!");
            return;
        }

        // НОВОЕ: если кликаем по объекту, который уже является донором — закрываем панель
        if (reactor.IsDonor(gameObject))
        {
            reactor.ClosePanel();
            return;
        }

        reactor.SelectMaterial(materialData, gameObject, highlighter);
    }
}