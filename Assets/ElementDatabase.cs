using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ElementData
{
    public string name;
    public string symbol;
    public int protons;
    public float atomicMass;
    public string category;
    public string colorHex;
}

[System.Serializable]
public class IsotopeData
{
    public int protons;
    public int neutrons;
    public float atomicMass;
    public bool isStable;
    public float halfLife; // В секундах (для игры)
    public string decayMode;
}

public class ElementDatabase : MonoBehaviour
{
    public static ElementDatabase Instance;

    private Dictionary<int, ElementData> elementsByProtons = new Dictionary<int, ElementData>();
    private Dictionary<string, IsotopeData> isotopes = new Dictionary<string, IsotopeData>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeElements();
            InitializeIsotopes();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializeElements()
    {
        var allElements = new List<ElementData>
        {
            // 1-10
            new ElementData { name = "Водород", symbol = "H", protons = 1, atomicMass = 1.008f, category = "Неметалл", colorHex = "#FFFFFF" },
            new ElementData { name = "Гелий", symbol = "He", protons = 2, atomicMass = 4.0026f, category = "Благородный газ", colorHex = "#D4E6F1" },
            new ElementData { name = "Литий", symbol = "Li", protons = 3, atomicMass = 6.94f, category = "Щелочной металл", colorHex = "#B3A2C7" },
            new ElementData { name = "Бериллий", symbol = "Be", protons = 4, atomicMass = 9.0122f, category = "Щёлочноземельный", colorHex = "#A8B8C8" },
            new ElementData { name = "Бор", symbol = "B", protons = 5, atomicMass = 10.81f, category = "Металлоид", colorHex = "#A0A0A0" },
            new ElementData { name = "Углерод", symbol = "C", protons = 6, atomicMass = 12.011f, category = "Неметалл", colorHex = "#404040" },
            new ElementData { name = "Азот", symbol = "N", protons = 7, atomicMass = 14.007f, category = "Неметалл", colorHex = "#3050F8" },
            new ElementData { name = "Кислород", symbol = "O", protons = 8, atomicMass = 15.999f, category = "Неметалл", colorHex = "#FF0D0D" },
            new ElementData { name = "Фтор", symbol = "F", protons = 9, atomicMass = 18.998f, category = "Галоген", colorHex = "#90E050" },
            new ElementData { name = "Неон", symbol = "Ne", protons = 10, atomicMass = 20.180f, category = "Благородный газ", colorHex = "#B3E3F5" },
            // 11-20
            new ElementData { name = "Натрий", symbol = "Na", protons = 11, atomicMass = 22.990f, category = "Щелочной металл", colorHex = "#B3A2C7" },
            new ElementData { name = "Магний", symbol = "Mg", protons = 12, atomicMass = 24.305f, category = "Щёлочноземельный", colorHex = "#A8B8C8" },
            new ElementData { name = "Алюминий", symbol = "Al", protons = 13, atomicMass = 26.982f, category = "Постпереходный металл", colorHex = "#CCCCCC" },
            new ElementData { name = "Кремний", symbol = "Si", protons = 14, atomicMass = 28.085f, category = "Металлоид", colorHex = "#A0A0A0" },
            new ElementData { name = "Фосфор", symbol = "P", protons = 15, atomicMass = 30.974f, category = "Неметалл", colorHex = "#FF8000" },
            new ElementData { name = "Сера", symbol = "S", protons = 16, atomicMass = 32.065f, category = "Неметалл", colorHex = "#FFFF30" },
            new ElementData { name = "Хлор", symbol = "Cl", protons = 17, atomicMass = 35.450f, category = "Галоген", colorHex = "#90E050" },
            new ElementData { name = "Аргон", symbol = "Ar", protons = 18, atomicMass = 39.948f, category = "Благородный газ", colorHex = "#D4E6F1" },
            new ElementData { name = "Калий", symbol = "K", protons = 19, atomicMass = 39.098f, category = "Щелочной металл", colorHex = "#B3A2C7" },
            new ElementData { name = "Кальций", symbol = "Ca", protons = 20, atomicMass = 40.078f, category = "Щёлочноземельный", colorHex = "#A8B8C8" },
            // 21-30
            new ElementData { name = "Скандий", symbol = "Sc", protons = 21, atomicMass = 44.956f, category = "Переходный металл", colorHex = "#B3B3B3" },
            new ElementData { name = "Титан", symbol = "Ti", protons = 22, atomicMass = 47.867f, category = "Переходный металл", colorHex = "#B3B3B3" },
            new ElementData { name = "Ванадий", symbol = "V", protons = 23, atomicMass = 50.942f, category = "Переходный металл", colorHex = "#B3B3B3" },
            new ElementData { name = "Хром", symbol = "Cr", protons = 24, atomicMass = 51.996f, category = "Переходный металл", colorHex = "#B3B3B3" },
            new ElementData { name = "Марганец", symbol = "Mn", protons = 25, atomicMass = 54.938f, category = "Переходный металл", colorHex = "#B3B3B3" },
            new ElementData { name = "Железо", symbol = "Fe", protons = 26, atomicMass = 55.845f, category = "Переходный металл", colorHex = "#A8A8A8" },
            new ElementData { name = "Кобальт", symbol = "Co", protons = 27, atomicMass = 58.933f, category = "Переходный металл", colorHex = "#B3B3B3" },
            new ElementData { name = "Никель", symbol = "Ni", protons = 28, atomicMass = 58.693f, category = "Переходный металл", colorHex = "#B3B3B3" },
            new ElementData { name = "Медь", symbol = "Cu", protons = 29, atomicMass = 63.546f, category = "Переходный металл", colorHex = "#B87333" },
            new ElementData { name = "Цинк", symbol = "Zn", protons = 30, atomicMass = 65.380f, category = "Постпереходный металл", colorHex = "#CCCCCC" },
            // 31-40
            new ElementData { name = "Галлий", symbol = "Ga", protons = 31, atomicMass = 69.723f, category = "Постпереходный металл", colorHex = "#CCCCCC" },
            new ElementData { name = "Германий", symbol = "Ge", protons = 32, atomicMass = 72.630f, category = "Металлоид", colorHex = "#A0A0A0" },
            new ElementData { name = "Мышьяк", symbol = "As", protons = 33, atomicMass = 74.922f, category = "Металлоид", colorHex = "#A0A0A0" },
            new ElementData { name = "Селен", symbol = "Se", protons = 34, atomicMass = 78.971f, category = "Неметалл", colorHex = "#FF8000" },
            new ElementData { name = "Бром", symbol = "Br", protons = 35, atomicMass = 79.904f, category = "Галоген", colorHex = "#90E050" },
            new ElementData { name = "Криптон", symbol = "Kr", protons = 36, atomicMass = 83.798f, category = "Благородный газ", colorHex = "#D4E6F1" },
            new ElementData { name = "Рубидий", symbol = "Rb", protons = 37, atomicMass = 85.468f, category = "Щелочной металл", colorHex = "#B3A2C7" },
            new ElementData { name = "Стронций", symbol = "Sr", protons = 38, atomicMass = 87.620f, category = "Щёлочноземельный", colorHex = "#A8B8C8" },
            new ElementData { name = "Иттрий", symbol = "Y", protons = 39, atomicMass = 88.906f, category = "Переходный металл", colorHex = "#B3B3B3" },
            new ElementData { name = "Цирконий", symbol = "Zr", protons = 40, atomicMass = 91.224f, category = "Переходный металл", colorHex = "#B3B3B3" },
            // 41-50
            new ElementData { name = "Ниобий", symbol = "Nb", protons = 41, atomicMass = 92.906f, category = "Переходный металл", colorHex = "#B3B3B3" },
            new ElementData { name = "Молибден", symbol = "Mo", protons = 42, atomicMass = 95.950f, category = "Переходный металл", colorHex = "#B3B3B3" },
            new ElementData { name = "Технеций", symbol = "Tc", protons = 43, atomicMass = 98.000f, category = "Переходный металл", colorHex = "#B3B3B3" },
            new ElementData { name = "Рутений", symbol = "Ru", protons = 44, atomicMass = 101.070f, category = "Переходный металл", colorHex = "#B3B3B3" },
            new ElementData { name = "Родий", symbol = "Rh", protons = 45, atomicMass = 102.906f, category = "Переходный металл", colorHex = "#B3B3B3" },
            new ElementData { name = "Палладий", symbol = "Pd", protons = 46, atomicMass = 106.420f, category = "Переходный металл", colorHex = "#B3B3B3" },
            new ElementData { name = "Серебро", symbol = "Ag", protons = 47, atomicMass = 107.868f, category = "Переходный металл", colorHex = "#C0C0C0" },
            new ElementData { name = "Кадмий", symbol = "Cd", protons = 48, atomicMass = 112.414f, category = "Переходный металл", colorHex = "#B3B3B3" },
            new ElementData { name = "Индий", symbol = "In", protons = 49, atomicMass = 114.818f, category = "Постпереходный металл", colorHex = "#CCCCCC" },
            new ElementData { name = "Олово", symbol = "Sn", protons = 50, atomicMass = 118.710f, category = "Постпереходный металл", colorHex = "#CCCCCC" },
            // 51-60
            new ElementData { name = "Сурьма", symbol = "Sb", protons = 51, atomicMass = 121.760f, category = "Металлоид", colorHex = "#A0A0A0" },
            new ElementData { name = "Теллур", symbol = "Te", protons = 52, atomicMass = 127.600f, category = "Металлоид", colorHex = "#A0A0A0" },
            new ElementData { name = "Иод", symbol = "I", protons = 53, atomicMass = 126.904f, category = "Галоген", colorHex = "#90E050" },
            new ElementData { name = "Ксенон", symbol = "Xe", protons = 54, atomicMass = 131.293f, category = "Благородный газ", colorHex = "#D4E6F1" },
            new ElementData { name = "Цезий", symbol = "Cs", protons = 55, atomicMass = 132.905f, category = "Щелочной металл", colorHex = "#B3A2C7" },
            new ElementData { name = "Барий", symbol = "Ba", protons = 56, atomicMass = 137.327f, category = "Щёлочноземельный", colorHex = "#A8B8C8" },
            new ElementData { name = "Лантан", symbol = "La", protons = 57, atomicMass = 138.905f, category = "Лантаноид", colorHex = "#FFB6C1" },
            new ElementData { name = "Церий", symbol = "Ce", protons = 58, atomicMass = 140.116f, category = "Лантаноид", colorHex = "#FFB6C1" },
            new ElementData { name = "Празеодим", symbol = "Pr", protons = 59, atomicMass = 140.908f, category = "Лантаноид", colorHex = "#FFB6C1" },
            new ElementData { name = "Неодим", symbol = "Nd", protons = 60, atomicMass = 144.243f, category = "Лантаноид", colorHex = "#FFB6C1" },
            // 61-70
            new ElementData { name = "Прометий", symbol = "Pm", protons = 61, atomicMass = 145.000f, category = "Лантаноид", colorHex = "#FFB6C1" },
            new ElementData { name = "Самарий", symbol = "Sm", protons = 62, atomicMass = 150.362f, category = "Лантаноид", colorHex = "#FFB6C1" },
            new ElementData { name = "Европий", symbol = "Eu", protons = 63, atomicMass = 151.964f, category = "Лантаноид", colorHex = "#FFB6C1" },
            new ElementData { name = "Гадолиний", symbol = "Gd", protons = 64, atomicMass = 157.250f, category = "Лантаноид", colorHex = "#FFB6C1" },
            new ElementData { name = "Тербий", symbol = "Tb", protons = 65, atomicMass = 158.925f, category = "Лантаноид", colorHex = "#FFB6C1" },
            new ElementData { name = "Диспрозий", symbol = "Dy", protons = 66, atomicMass = 162.500f, category = "Лантаноид", colorHex = "#FFB6C1" },
            new ElementData { name = "Гольмий", symbol = "Ho", protons = 67, atomicMass = 164.930f, category = "Лантаноид", colorHex = "#FFB6C1" },
            new ElementData { name = "Эрбий", symbol = "Er", protons = 68, atomicMass = 167.259f, category = "Лантаноид", colorHex = "#FFB6C1" },
            new ElementData { name = "Тулий", symbol = "Tm", protons = 69, atomicMass = 168.934f, category = "Лантаноид", colorHex = "#FFB6C1" },
            new ElementData { name = "Иттербий", symbol = "Yb", protons = 70, atomicMass = 173.045f, category = "Лантаноид", colorHex = "#FFB6C1" },
            // 71-80
            new ElementData { name = "Лютеций", symbol = "Lu", protons = 71, atomicMass = 174.967f, category = "Лантаноид", colorHex = "#FFB6C1" },
            new ElementData { name = "Гафний", symbol = "Hf", protons = 72, atomicMass = 178.490f, category = "Переходный металл", colorHex = "#B3B3B3" },
            new ElementData { name = "Тантал", symbol = "Ta", protons = 73, atomicMass = 180.948f, category = "Переходный металл", colorHex = "#B3B3B3" },
            new ElementData { name = "Вольфрам", symbol = "W", protons = 74, atomicMass = 183.840f, category = "Переходный металл", colorHex = "#B3B3B3" },
            new ElementData { name = "Рений", symbol = "Re", protons = 75, atomicMass = 186.207f, category = "Переходный металл", colorHex = "#B3B3B3" },
            new ElementData { name = "Осмий", symbol = "Os", protons = 76, atomicMass = 190.230f, category = "Переходный металл", colorHex = "#B3B3B3" },
            new ElementData { name = "Иридий", symbol = "Ir", protons = 77, atomicMass = 192.217f, category = "Переходный металл", colorHex = "#B3B3B3" },
            new ElementData { name = "Платина", symbol = "Pt", protons = 78, atomicMass = 195.084f, category = "Переходный металл", colorHex = "#B3B3B3" },
            new ElementData { name = "Золото", symbol = "Au", protons = 79, atomicMass = 196.967f, category = "Переходный металл", colorHex = "#FFD700" },
            new ElementData { name = "Ртуть", symbol = "Hg", protons = 80, atomicMass = 200.592f, category = "Постпереходный металл", colorHex = "#CCCCCC" },
            // 81-90
            new ElementData { name = "Таллий", symbol = "Tl", protons = 81, atomicMass = 204.380f, category = "Постпереходный металл", colorHex = "#CCCCCC" },
            new ElementData { name = "Свинец", symbol = "Pb", protons = 82, atomicMass = 207.200f, category = "Постпереходный металл", colorHex = "#CCCCCC" },
            new ElementData { name = "Висмут", symbol = "Bi", protons = 83, atomicMass = 208.980f, category = "Постпереходный металл", colorHex = "#CCCCCC" },
            new ElementData { name = "Полоний", symbol = "Po", protons = 84, atomicMass = 209.000f, category = "Постпереходный металл", colorHex = "#CCCCCC" },
            new ElementData { name = "Астат", symbol = "At", protons = 85, atomicMass = 210.000f, category = "Галоген", colorHex = "#90E050" },
            new ElementData { name = "Радон", symbol = "Rn", protons = 86, atomicMass = 222.000f, category = "Благородный газ", colorHex = "#D4E6F1" },
            new ElementData { name = "Франций", symbol = "Fr", protons = 87, atomicMass = 223.000f, category = "Щелочной металл", colorHex = "#B3A2C7" },
            new ElementData { name = "Радий", symbol = "Ra", protons = 88, atomicMass = 226.000f, category = "Щёлочноземельный", colorHex = "#A8B8C8" },
            new ElementData { name = "Актиний", symbol = "Ac", protons = 89, atomicMass = 227.000f, category = "Актиноид", colorHex = "#FFA500" },
            new ElementData { name = "Торий", symbol = "Th", protons = 90, atomicMass = 232.038f, category = "Актиноид", colorHex = "#FFA500" },
            // 91-100
            new ElementData { name = "Протактиний", symbol = "Pa", protons = 91, atomicMass = 231.036f, category = "Актиноид", colorHex = "#FFA500" },
            new ElementData { name = "Уран", symbol = "U", protons = 92, atomicMass = 238.029f, category = "Актиноид", colorHex = "#FFA500" },
            new ElementData { name = "Нептуний", symbol = "Np", protons = 93, atomicMass = 237.000f, category = "Актиноид", colorHex = "#FFA500" },
            new ElementData { name = "Плутоний", symbol = "Pu", protons = 94, atomicMass = 244.000f, category = "Актиноид", colorHex = "#FFA500" },
            new ElementData { name = "Америций", symbol = "Am", protons = 95, atomicMass = 243.000f, category = "Актиноид", colorHex = "#FFA500" },
            new ElementData { name = "Кюрий", symbol = "Cm", protons = 96, atomicMass = 247.000f, category = "Актиноид", colorHex = "#FFA500" },
            new ElementData { name = "Берклий", symbol = "Bk", protons = 97, atomicMass = 247.000f, category = "Актиноид", colorHex = "#FFA500" },
            new ElementData { name = "Калифорний", symbol = "Cf", protons = 98, atomicMass = 251.000f, category = "Актиноид", colorHex = "#FFA500" },
            new ElementData { name = "Эйнштейний", symbol = "Es", protons = 99, atomicMass = 252.000f, category = "Актиноид", colorHex = "#FFA500" },
            new ElementData { name = "Фермий", symbol = "Fm", protons = 100, atomicMass = 257.000f, category = "Актиноид", colorHex = "#FFA500" },
            // 101-110
            new ElementData { name = "Менделевий", symbol = "Md", protons = 101, atomicMass = 258.000f, category = "Актиноид", colorHex = "#FFA500" },
            new ElementData { name = "Нобелий", symbol = "No", protons = 102, atomicMass = 259.000f, category = "Актиноид", colorHex = "#FFA500" },
            new ElementData { name = "Лоуренсий", symbol = "Lr", protons = 103, atomicMass = 262.000f, category = "Актиноид", colorHex = "#FFA500" },
            new ElementData { name = "Резерфордий", symbol = "Rf", protons = 104, atomicMass = 267.000f, category = "Переходный металл", colorHex = "#B3B3B3" },
            new ElementData { name = "Дубний", symbol = "Db", protons = 105, atomicMass = 268.000f, category = "Переходный металл", colorHex = "#B3B3B3" },
            new ElementData { name = "Сиборгий", symbol = "Sg", protons = 106, atomicMass = 269.000f, category = "Переходный металл", colorHex = "#B3B3B3" },
            new ElementData { name = "Борий", symbol = "Bh", protons = 107, atomicMass = 270.000f, category = "Переходный металл", colorHex = "#B3B3B3" },
            new ElementData { name = "Хассий", symbol = "Hs", protons = 108, atomicMass = 277.000f, category = "Переходный металл", colorHex = "#B3B3B3" },
            new ElementData { name = "Мейтнерий", symbol = "Mt", protons = 109, atomicMass = 278.000f, category = "Переходный металл", colorHex = "#B3B3B3" },
            new ElementData { name = "Дармштадтий", symbol = "Ds", protons = 110, atomicMass = 281.000f, category = "Переходный металл", colorHex = "#B3B3B3" },
            // 111-118
            new ElementData { name = "Рентгений", symbol = "Rg", protons = 111, atomicMass = 282.000f, category = "Переходный металл", colorHex = "#B3B3B3" },
            new ElementData { name = "Коперниций", symbol = "Cn", protons = 112, atomicMass = 285.000f, category = "Переходный металл", colorHex = "#B3B3B3" },
            new ElementData { name = "Нихоний", symbol = "Nh", protons = 113, atomicMass = 286.000f, category = "Постпереходный металл", colorHex = "#CCCCCC" },
            new ElementData { name = "Флеровий", symbol = "Fl", protons = 114, atomicMass = 289.000f, category = "Постпереходный металл", colorHex = "#CCCCCC" },
            new ElementData { name = "Московий", symbol = "Mc", protons = 115, atomicMass = 290.000f, category = "Постпереходный металл", colorHex = "#CCCCCC" },
            new ElementData { name = "Ливерморий", symbol = "Lv", protons = 116, atomicMass = 293.000f, category = "Постпереходный металл", colorHex = "#CCCCCC" },
            new ElementData { name = "Теннессин", symbol = "Ts", protons = 117, atomicMass = 294.000f, category = "Галоген", colorHex = "#90E050" },
            new ElementData { name = "Оганесон", symbol = "Og", protons = 118, atomicMass = 295.000f, category = "Благородный газ", colorHex = "#D4E6F1" }
        };

        foreach (var element in allElements)
        {
            elementsByProtons[element.protons] = element;
        }
    }

    // ==================== ИЗОТОПЫ ====================
    void InitializeIsotopes()
    {
        // Водород
        AddIsotope(1, 0, 1.008f, true, 0, "Стабильный");
        AddIsotope(1, 1, 2.014f, true, 0, "Стабильный");
        AddIsotope(1, 2, 3.016f, false, 12.3f, "Бета-распад");

        // Гелий
        AddIsotope(2, 1, 3.016f, false, 0.8f, "Протонный");
        AddIsotope(2, 2, 4.003f, true, 0, "Стабильный");

        // Литий
        AddIsotope(3, 3, 6.015f, true, 0, "Стабильный");
        AddIsotope(3, 4, 7.016f, true, 0, "Стабильный");

        // Бериллий
        AddIsotope(4, 5, 9.012f, true, 0, "Стабильный");
        AddIsotope(4, 6, 10.013f, false, 1.5f, "Бета-распад");

        // Бор
        AddIsotope(5, 5, 10.013f, true, 0, "Стабильный");
        AddIsotope(5, 6, 11.009f, true, 0, "Стабильный");

        // Углерод
        AddIsotope(6, 6, 12.000f, true, 0, "Стабильный");
        AddIsotope(6, 7, 13.003f, true, 0, "Стабильный");
        AddIsotope(6, 8, 14.003f, false, 5730f, "Бета-распад");

        // Азот
        AddIsotope(7, 7, 14.003f, true, 0, "Стабильный");
        AddIsotope(7, 8, 15.000f, true, 0, "Стабильный");

        // Кислород
        AddIsotope(8, 8, 15.995f, true, 0, "Стабильный");
        AddIsotope(8, 9, 16.999f, true, 0, "Стабильный");
        AddIsotope(8, 10, 17.999f, true, 0, "Стабильный");

        // Железо
        AddIsotope(26, 28, 53.940f, true, 0, "Стабильный");
        AddIsotope(26, 30, 55.935f, true, 0, "Стабильный"); // Fe-56
        AddIsotope(26, 31, 56.935f, true, 0, "Стабильный"); // Fe-57
        AddIsotope(26, 32, 57.933f, true, 0, "Стабильный"); // Fe-58
        AddIsotope(26, 33, 58.935f, false, 44.5f, "Бета-распад"); // Fe-59

        // Медь
        AddIsotope(29, 34, 62.930f, true, 0, "Стабильный"); // Cu-63
        AddIsotope(29, 35, 63.929f, false, 12.7f, "Бета-распад"); // Cu-64
        AddIsotope(29, 36, 64.928f, true, 0, "Стабильный"); // Cu-65

        // Золото
        AddIsotope(79, 118, 196.967f, true, 0, "Стабильный"); // Au-197
        AddIsotope(79, 119, 197.968f, false, 2.7f, "Бета-распад"); // Au-198
    }

    void AddIsotope(int protons, int neutrons, float mass, bool stable, float halfLife, string decay)
    {
        string key = $"{protons}_{neutrons}";
        isotopes[key] = new IsotopeData
        {
            protons = protons,
            neutrons = neutrons,
            atomicMass = mass,
            isStable = stable,
            halfLife = halfLife,
            decayMode = decay
        };
    }

    public IsotopeData GetIsotope(int protons, int neutrons)
    {
        string key = $"{protons}_{neutrons}";
        isotopes.TryGetValue(key, out IsotopeData isotope);
        return isotope;
    }

    public ElementData GetElementByProtons(int protons)
    {
        elementsByProtons.TryGetValue(protons, out ElementData element);
        return element;
    }

    public bool IsElementStable(int protons)
    {
        return protons >= 1 && protons <= 118;
    }
}