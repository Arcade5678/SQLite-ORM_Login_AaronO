[System.Serializable]
public class UsuariORM
{
    public int    Id   { get; set; }
    public string Nom  { get; set; }
    public string Pass { get; set; }

    public UsuariORM() { }

    public UsuariORM(int id, string nom, string pass)
    {
        Id   = id;
        Nom  = nom;
        Pass = pass;
    }

    public override string ToString() => $"[{Id}] {Nom}";
}
