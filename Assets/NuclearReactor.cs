using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class NuclearReactor : MonoBehaviour
{
    public static NuclearReactor Instance;

    public TextMeshProUGUI donorNameText;
    public TextMeshProUGUI acceptorNameText;
    public TextMeshProUGUI infoText;
    public Button transferProtonButton;
    public Button transferNeutronButton;
    public Button closeButton;
    public AtomRenderer atomRenderer;
    public GameObject panel;

    private MaterialData donor;
    private MaterialData acceptor;
    private GameObject donorObject;
    private GameObject acceptorObject;
    private MaterialHighlighter donorHighlighter;
    private MaterialHighlighter acceptorHighlighter;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (panel != null) panel.SetActive(false);

        transferProtonButton.interactable = false;
        transferNeutronButton.interactable = false;

        transferProtonButton.onClick.AddListener(TransferProton);
        transferNeutronButton.onClick.AddListener(TransferNeutron);

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(ClosePanel);
        }
    }

    public void SelectMaterial(MaterialData material, GameObject obj, MaterialHighlighter highlighter)
    {
        if (panel != null && !panel.activeSelf) panel.SetActive(true);

        if (donor == null)
        {
            donor = material;
            donorObject = obj;
            donorHighlighter = highlighter;
            donorHighlighter.HighlightDonor();
            UpdateUI();
            Debug.Log($"Выбран донор: {donor.materialName}");
            return;
        }

        if (acceptor == null)
        {
            if (obj == donorObject)
            {
                Debug.Log("Нельзя выбрать тот же материал в качестве акцептора!");
                return;
            }

            acceptor = material;
            acceptorObject = obj;
            acceptorHighlighter = highlighter;
            acceptorHighlighter.HighlightAcceptor();
            UpdateUI();
            Debug.Log($"Выбран акцептор: {acceptor.materialName}");

            transferProtonButton.interactable = true;
            transferNeutronButton.interactable = true;
            RedrawAtom();
            return;
        }

        ResetSelection();
        donor = material;
        donorObject = obj;
        donorHighlighter = highlighter;
        donorHighlighter.HighlightDonor();
        UpdateUI();
        Debug.Log($"Выбор сброшен. Новый донор: {donor.materialName}");
    }

    void TransferProton()
    {
        if (donor == null || acceptor == null) return;
        if (donor.protons <= 0)
        {
            Debug.Log("У донора нет протонов!");
            return;
        }

        if (acceptor.protons >= 118)
        {
            Debug.Log($"Нельзя добавить протон к {acceptor.materialName}: достигнут предел (118 протонов)!");
            StartCoroutine(ShowRadiationWarning("⚠️ ДОСТИГНУТ ЛИМИТ ЭЛЕМЕНТОВ! (118) ⚠️"));
            return;
        }

        donor.protons--;
        acceptor.protons++;

        // Теперь всё обновляется через UpdateMaterialName (который внутри вызывает UpdateIsotopeInfo)
        UpdateMaterialName(donor);
        UpdateMaterialName(acceptor);

        UpdateUI();
        RedrawAtom();
        Debug.Log($"Передан протон от {donor.materialName} к {acceptor.materialName}");
    }

    void TransferNeutron()
    {
        if (donor == null || acceptor == null) return;
        if (donor.neutrons <= 0)
        {
            Debug.Log("У донора нет нейтронов!");
            return;
        }

        donor.neutrons--;
        acceptor.neutrons++;

        UpdateMaterialName(donor);
        UpdateMaterialName(acceptor);
        donor.UpdateIsotopeInfo();
        acceptor.UpdateIsotopeInfo();
        UpdateUI();
        RedrawAtom();
        Debug.Log($"Передан нейтрон от {donor.materialName} к {acceptor.materialName}");
    }

    void UpdateUI()
    {
        if (donorNameText != null)
        {
            string donorInfo = donor != null ?
                $"{donor.materialName} ({donor.elementSymbol})" :
                "Донор: не выбран";
            if (donor != null && donor.isRadioactive)
            {
                donorInfo += " [RAD]";
            }
            donorNameText.text = donorInfo;
        }

        if (acceptorNameText != null)
        {
            string acceptorInfo = acceptor != null ?
                $"{acceptor.materialName} ({acceptor.elementSymbol})" :
                "Акцептор: не выбран";
            if (acceptor != null && acceptor.isRadioactive)
            {
                acceptorInfo += " [RAD]";
            }
            acceptorNameText.text = acceptorInfo;
        }

        if (infoText != null)
        {
            if (donor != null && acceptor != null)
            {
                string donorRadio = donor.isRadioactive ? " [RAD]" : "";
                string acceptorRadio = acceptor.isRadioactive ? " [RAD]" : "";

                infoText.text = $"Передать:\n" +
                               $"{donor.materialName}{donorRadio} ({donor.elementSymbol}):\n" +
                               $"P={donor.protons} N={donor.neutrons} A={donor.atomicMass:F2}\n" +
                               $"{acceptor.materialName}{acceptorRadio} ({acceptor.elementSymbol}):\n" +
                               $"P={acceptor.protons} N={acceptor.neutrons} A={acceptor.atomicMass:F2}";
            }
            else if (donor != null)
            {
                string donorRadio = donor.isRadioactive ? " [RAD]" : "";
                infoText.text = $"Выбран донор: {donor.materialName}{donorRadio} ({donor.elementSymbol})\nВыберите акцептор";
            }
            else
            {
                infoText.text = "Выберите материал-донор (первый клик)";
            }
        }
    }

    void RedrawAtom()
    {
        if (atomRenderer != null && acceptor != null)
        {
            atomRenderer.DrawAtom(acceptor.protons, acceptor.neutrons);
        }
    }

    void UpdateMaterialName(MaterialData material)
    {
        if (ElementDatabase.Instance == null)
        {
            Debug.LogError("ElementDatabase не найден!");
            return;
        }

        // 1. Обновляем название и символ
        ElementData element = ElementDatabase.Instance.GetElementByProtons(material.protons);
        if (element != null)
        {
            material.materialName = element.name;
            material.elementSymbol = element.symbol;

            if (ColorUtility.TryParseHtmlString(element.colorHex, out Color newColor))
            {
                material.SetElementColor(newColor);
            }
        }
        else
        {
            material.materialName = $"Элемент {material.protons}";
            material.elementSymbol = $"E{material.protons}";
        }

        // 2. Пересчитываем радиоактивность по новому изотопу
        material.UpdateIsotopeInfo();

        // 3. !!! НОВОЕ: если элемент стал стабильным — возвращаем нормальный цвет !!!
        if (!material.isRadioactive)
        {
            material.RestoreElementColor();
        }
    }

    IEnumerator ShowRadiationWarning(string message)
    {
        if (infoText != null)
        {
            string originalText = infoText.text;
            Color originalColor = infoText.color;

            infoText.text = message;
            infoText.color = Color.red;
            yield return new WaitForSeconds(2f);

            infoText.text = originalText;
            infoText.color = originalColor;
        }
    }

    public void ClosePanel()
    {
        ResetSelection();
        if (panel != null) panel.SetActive(false);
        Debug.Log("Панель закрыта");
    }

    public void ResetSelection()
    {
        if (donorHighlighter != null) donorHighlighter.ResetHighlight();
        if (acceptorHighlighter != null) acceptorHighlighter.ResetHighlight();

        donor = null;
        acceptor = null;
        donorObject = null;
        acceptorObject = null;
        donorHighlighter = null;
        acceptorHighlighter = null;

        transferProtonButton.interactable = false;
        transferNeutronButton.interactable = false;

        UpdateUI();
        Debug.Log("Выбор сброшен");
    }

    public bool IsDonor(GameObject obj)
    {
        return donorObject != null && donorObject == obj;
    }

    public void Refresh()
    {
        UpdateUI();
        RedrawAtom();
    }
}