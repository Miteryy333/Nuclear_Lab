using UnityEngine;

public class MaterialHighlighter : MonoBehaviour
{
    private MaterialData materialData;

    void Start()
    {
        materialData = GetComponent<MaterialData>();
        if (materialData == null)
        {
            Debug.LogError("MaterialHighlighter: MaterialData не найден на " + gameObject.name);
        }
    }

    public void HighlightDonor()
    {
        if (materialData == null) return;
        SetHighlightColor(Color.red);
    }

    public void HighlightAcceptor()
    {
        if (materialData == null) return;
        SetHighlightColor(Color.blue);
    }

    public void ResetHighlight()
    {
        if (materialData != null)
        {
            materialData.RestoreElementColor();
        }
    }

    private void SetHighlightColor(Color color)
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = color;
        }
    }
}