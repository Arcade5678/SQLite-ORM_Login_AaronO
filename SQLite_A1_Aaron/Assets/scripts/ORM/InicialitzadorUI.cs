using UnityEngine;

[DefaultExecutionOrder(-200)]
public class InicialitzadorUI : MonoBehaviour
{
    [Header("Canvas de l'escena")]
    public GameObject canvasPrincipal;
    public GameObject canvasLogin;
    public GameObject canvasRegistre;
    public GameObject canvasGestio;

    void Awake()
    {
        if (canvasPrincipal != null) canvasPrincipal.SetActive(true);
        if (canvasLogin     != null) canvasLogin.SetActive(false);
        if (canvasRegistre  != null) canvasRegistre.SetActive(false);
        if (canvasGestio    != null) canvasGestio.SetActive(false);
    }
}
