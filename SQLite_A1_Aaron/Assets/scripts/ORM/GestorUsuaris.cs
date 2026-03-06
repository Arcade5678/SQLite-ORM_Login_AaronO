using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GestorUsuaris : MonoBehaviour
{
    [Header("Botons")]
    public Button botonLlistar;
    public Button botonEsborrarTots;

    [Header("Feedback")]
    public TextMeshProUGUI textMissatge;

    [Header("ScrollView - Content")]
    public RectTransform contingutLlista;

    private readonly List<GameObject> _files = new List<GameObject>();

    void Start()
    {
        if (botonLlistar      != null) botonLlistar.onClick.AddListener(CarregarLlista);
        if (botonEsborrarTots != null) botonEsborrarTots.onClick.AddListener(EsborrarTots);
        FixContentRect();
    }

    private void FixContentRect()
    {
        if (contingutLlista == null) return;

        contingutLlista.anchorMin = new Vector2(0f, 1f);
        contingutLlista.anchorMax = new Vector2(1f, 1f);
        contingutLlista.pivot     = new Vector2(0.5f, 1f);
        contingutLlista.offsetMin = new Vector2(0f, contingutLlista.offsetMin.y);
        contingutLlista.offsetMax = new Vector2(0f, contingutLlista.offsetMax.y);

        var csf = contingutLlista.GetComponent<ContentSizeFitter>();
        if (csf != null)
        {
            csf.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            csf.verticalFit   = ContentSizeFitter.FitMode.PreferredSize;
        }

        var vlg = contingutLlista.GetComponent<VerticalLayoutGroup>();
        if (vlg != null)
        {
            vlg.childForceExpandWidth  = true;
            vlg.childForceExpandHeight = false;
            vlg.childControlHeight     = true;
            vlg.childControlWidth      = true;
        }
    }

    public void CarregarLlista()
    {
        if (CrearBaseDatosORM.Repositori == null)
        {
            Missatge("Error: repositori ORM no disponible.");
            return;
        }

        NetejarFiles();

        var usuaris = CrearBaseDatosORM.Repositori.ObtenirTots();

        if (usuaris.Count == 0)
        {
            Missatge("No hi ha usuaris registrats.");
            return;
        }

        Missatge($"{usuaris.Count} usuari(s) registrat(s).");

        foreach (var u in usuaris)
            CriarFila(u);

        StartCoroutine(ReconstruirLayout());
    }

    public void EsborrarTots()
    {
        if (CrearBaseDatosORM.Repositori == null)
        {
            Missatge("Error: repositori ORM no disponible.");
            return;
        }

        CrearBaseDatosORM.Repositori.BuidarTaula();
        Missatge("Tots els usuaris han estat esborrats.");
        CarregarLlista();
    }

    public void EsborrarPerId(int id)
    {
        if (CrearBaseDatosORM.Repositori == null) return;

        bool ok = CrearBaseDatosORM.Repositori.EsborrarPerId(id);
        Missatge(ok ? $"Usuari id={id} esborrat." : $"No s'ha trobat id={id}.");
        CarregarLlista();
    }

    private void CriarFila(UsuariORM usuari)
    {
        var fila  = new GameObject($"Fila_{usuari.Id}");
        fila.transform.SetParent(contingutLlista, false);

        var img   = fila.AddComponent<Image>();
        img.color = new Color(0.18f, 0.18f, 0.22f, 1f);

        var le    = fila.AddComponent<LayoutElement>();
        le.minHeight       = 50f;
        le.preferredHeight = 50f;

        var hlg   = fila.AddComponent<HorizontalLayoutGroup>();
        hlg.childForceExpandWidth  = true;
        hlg.childForceExpandHeight = true;
        hlg.childControlWidth      = true;
        hlg.childControlHeight     = true;
        hlg.spacing = 8f;
        hlg.padding = new RectOffset(12, 8, 5, 5);

        var goText = new GameObject("TextNom");
        goText.transform.SetParent(fila.transform, false);
        var tmp    = goText.AddComponent<TextMeshProUGUI>();
        tmp.text      = $"[{usuari.Id}]   {usuari.Nom}";
        tmp.fontSize  = 20f;
        tmp.color     = Color.white;
        tmp.alignment = TextAlignmentOptions.MidlineLeft;
        var leText = goText.AddComponent<LayoutElement>();
        leText.flexibleWidth = 1f;

        var goBtn  = new GameObject("BotoEsborrar");
        goBtn.transform.SetParent(fila.transform, false);
        var imgBtn = goBtn.AddComponent<Image>();
        imgBtn.color = new Color(0.75f, 0.12f, 0.12f);
        var leBtn  = goBtn.AddComponent<LayoutElement>();
        leBtn.minWidth       = 110f;
        leBtn.preferredWidth = 110f;
        var btn    = goBtn.AddComponent<Button>();
        btn.targetGraphic = imgBtn;
        var cb     = btn.colors;
        cb.normalColor      = new Color(0.75f, 0.12f, 0.12f);
        cb.highlightedColor = new Color(1f,    0.30f, 0.30f);
        cb.pressedColor     = new Color(0.50f, 0.05f, 0.05f);
        btn.colors = cb;

        var goTxt  = new GameObject("TextBtn");
        goTxt.transform.SetParent(goBtn.transform, false);
        var tmpBtn = goTxt.AddComponent<TextMeshProUGUI>();
        tmpBtn.text      = "Esborrar";
        tmpBtn.fontSize  = 17f;
        tmpBtn.color     = Color.white;
        tmpBtn.fontStyle = FontStyles.Bold;
        tmpBtn.alignment = TextAlignmentOptions.Center;
        var rtTxt  = goTxt.GetComponent<RectTransform>();
        rtTxt.anchorMin = Vector2.zero;
        rtTxt.anchorMax = Vector2.one;
        rtTxt.offsetMin = Vector2.zero;
        rtTxt.offsetMax = Vector2.zero;

        int capId = usuari.Id;
        btn.onClick.AddListener(() => EsborrarPerId(capId));

        _files.Add(fila);
    }

    private void NetejarFiles()
    {
        foreach (var f in _files)
            if (f != null) Destroy(f);
        _files.Clear();
    }

    private void Missatge(string msg)
    {
        Debug.Log("[GestorUsuaris] " + msg);
        if (textMissatge != null) textMissatge.text = msg;
    }

    private IEnumerator ReconstruirLayout()
    {
        yield return null;
        yield return null;
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(contingutLlista);
    }
}
