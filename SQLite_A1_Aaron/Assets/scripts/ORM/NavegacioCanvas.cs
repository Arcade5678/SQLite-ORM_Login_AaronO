using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-150)]
public class NavegacioCanvas : MonoBehaviour
{
    [Header("Canvas")]
    public GameObject canvasLogin;
    public GameObject canvasRegistre;
    public GameObject canvasGestio;

    [Header("Botons → Login")]
    public Button botonTornarDesDeRegistre;
    public Button botonTornarDesDeGestio;

    [Header("Botons → Anar a")]
    public Button botonAnirRegistre;
    public Button botonAnirGestio;

    void Awake()
    {
        SetCanvas(canvasLogin,    true);
        SetCanvas(canvasRegistre, false);
        SetCanvas(canvasGestio,   false);
    }

    void Start()
    {
        if (botonAnirRegistre        != null) botonAnirRegistre.onClick.AddListener(MostrarRegistre);
        if (botonAnirGestio          != null) botonAnirGestio.onClick.AddListener(MostrarGestio);
        if (botonTornarDesDeRegistre != null) botonTornarDesDeRegistre.onClick.AddListener(MostrarLogin);
        if (botonTornarDesDeGestio   != null) botonTornarDesDeGestio.onClick.AddListener(MostrarLogin);
    }

    public void MostrarLogin()
    {
        SetCanvas(canvasLogin,    true);
        SetCanvas(canvasRegistre, false);
        SetCanvas(canvasGestio,   false);
    }

    public void MostrarRegistre()
    {
        SetCanvas(canvasLogin,    false);
        SetCanvas(canvasRegistre, true);
        SetCanvas(canvasGestio,   false);
    }

    public void MostrarGestio()
    {
        SetCanvas(canvasLogin,    false);
        SetCanvas(canvasRegistre, false);
        SetCanvas(canvasGestio,   true);
    }

    private void SetCanvas(GameObject canvas, bool actiu)
    {
        if (canvas != null) canvas.SetActive(actiu);
    }
}
