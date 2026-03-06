using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IniciarSesionORM : MonoBehaviour
{
    [Header("UI")]
    public TMP_InputField  inputUsuari;
    public TMP_InputField  inputContrasenya;
    public Button          botonLogin;
    public TextMeshProUGUI missatge;

    [Header("Canvas")]
    public GameObject canvasLogin;
    public GameObject canvasPrincipal;

    void Start()
    {
        botonLogin.onClick.AddListener(ComprovarLogin);
        missatge.text = "Introdueix usuari i contrasenya";
    }

    void ComprovarLogin()
    {
        if (CrearBaseDatosORM.Repositori == null)
        {
            missatge.text = "Error: base de dades no inicialitzada.";
            return;
        }

        string nom  = inputUsuari.text.Trim();
        string pass = inputContrasenya.text.Trim();

        if (string.IsNullOrEmpty(nom) || string.IsNullOrEmpty(pass))
        {
            missatge.text = "Omple tots els camps.";
            return;
        }

        UsuariORM trobat = CrearBaseDatosORM.Repositori.AutenticarUsuari(nom, pass);

        if (trobat != null)
        {
            missatge.text = $"Benvingut, {trobat.Nom}!";
            if (canvasPrincipal != null) canvasPrincipal.SetActive(true);
            if (canvasLogin     != null) canvasLogin.SetActive(false);
        }
        else
        {
            missatge.text = "Usuari o contrasenya incorrectes.";
        }
    }
}
