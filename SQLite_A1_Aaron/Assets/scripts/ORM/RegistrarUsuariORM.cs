using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RegistrarUsuariORM : MonoBehaviour
{
    [Header("UI")]
    public TMP_InputField inputUsuari;
    public TMP_InputField inputContrasenya;
    public Button         botonRegistrar;
    public TextMeshProUGUI missatge;

    [Header("Canvas")]
    public GameObject canvasRegistre;
    public GameObject canvasPrincipal;

    void Start()
    {
        botonRegistrar.onClick.AddListener(Registrar);
        missatge.text = "Introdueix usuari i contrasenya";
    }

    void Registrar()
    {
        if (CrearBaseDatosORM.Repositori == null)
        {
            missatge.text = "Error: base de dades no inicialitzada.";
            return;
        }

        var nouUsuari = new UsuariORM
        {
            Nom  = inputUsuari.text.Trim(),
            Pass = inputContrasenya.text.Trim()
        };

        bool exit = CrearBaseDatosORM.Repositori.InserirUsuari(nouUsuari, out string msg);
        missatge.text = msg;

        if (exit)
        {
            inputUsuari.text      = "";
            inputContrasenya.text = "";

            if (canvasPrincipal != null) canvasPrincipal.SetActive(true);
            if (canvasRegistre  != null) canvasRegistre.SetActive(false);
        }
    }
}
