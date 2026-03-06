using UnityEngine;
using System.IO;

[DefaultExecutionOrder(-100)]
public class CrearBaseDatosORM : MonoBehaviour
{
    [Header("Configuracio")]
    public bool borrarDatosAlIniciar = true;
    public string nombreDB = "MyDatabase.sqlite";

    public static UsuariRepositori Repositori { get; private set; }

    private void Awake()
    {
        string dbPath = Application.persistentDataPath + "/" + nombreDB;

        if (borrarDatosAlIniciar && File.Exists(dbPath))
            File.Delete(dbPath);

        if (!File.Exists(dbPath))
            File.Create(dbPath).Close();

        Repositori = new UsuariRepositori(dbPath);
        Repositori.CrearTaula();

        if (borrarDatosAlIniciar)
            Repositori.BuidarTaula();
    }
}
