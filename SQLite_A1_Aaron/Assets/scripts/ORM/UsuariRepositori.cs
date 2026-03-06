using System.Collections.Generic;
using Mono.Data.Sqlite;
using UnityEngine;

public class UsuariRepositori
{
    private readonly string _connectionString;

    public UsuariRepositori(string dbPath)
    {
        _connectionString = "URI=file:" + dbPath;
    }

    public void CrearTaula()
    {
        using (var conn = new SqliteConnection(_connectionString))
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText =
                    "CREATE TABLE IF NOT EXISTS Usuarios (" +
                    "id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                    "usuario TEXT NOT NULL UNIQUE, " +
                    "password TEXT NOT NULL CHECK (length(password) >= 8));";
                cmd.ExecuteNonQuery();
            }
        }
    }

    public void BuidarTaula()
    {
        using (var conn = new SqliteConnection(_connectionString))
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM Usuarios;";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DELETE FROM sqlite_sequence WHERE name='Usuarios';";
                cmd.ExecuteNonQuery();
            }
        }
    }

    public bool InserirUsuari(UsuariORM usuari, out string missatge)
    {
        if (string.IsNullOrWhiteSpace(usuari.Nom))
        {
            missatge = "El nom d'usuari no pot estar buit.";
            return false;
        }
        if (usuari.Pass == null || usuari.Pass.Length < 8)
        {
            missatge = "La contrasenya ha de tenir com a minim 8 caracters.";
            return false;
        }

        try
        {
            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqliteCommand(
                    "INSERT INTO Usuarios (usuario, password) VALUES (@u, @p);", conn))
                {
                    cmd.Parameters.AddWithValue("@u", usuari.Nom.Trim());
                    cmd.Parameters.AddWithValue("@p", usuari.Pass.Trim());
                    cmd.ExecuteNonQuery();
                }
            }
            missatge = $"Usuari '{usuari.Nom}' registrat correctament.";
            return true;
        }
        catch (SqliteException ex) when (ex.Message.Contains("UNIQUE"))
        {
            missatge = $"L'usuari '{usuari.Nom}' ja existeix.";
            return false;
        }
        catch (System.Exception ex)
        {
            missatge = "Error al inserir l'usuari: " + ex.Message;
            Debug.LogError("[ORM] InserirUsuari: " + ex.Message);
            return false;
        }
    }

    public UsuariORM AutenticarUsuari(string nom, string pass)
    {
        try
        {
            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqliteCommand(
                    "SELECT id, usuario, password FROM Usuarios WHERE usuario=@u AND password=@p LIMIT 1;", conn))
                {
                    cmd.Parameters.AddWithValue("@u", nom.Trim());
                    cmd.Parameters.AddWithValue("@p", pass.Trim());
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            return new UsuariORM(reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
                    }
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("[ORM] AutenticarUsuari: " + ex.Message);
        }
        return null;
    }

    public List<UsuariORM> ObtenirTots()
    {
        var llista = new List<UsuariORM>();
        try
        {
            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqliteCommand(
                    "SELECT id, usuario, password FROM Usuarios ORDER BY id;", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        llista.Add(new UsuariORM(reader.GetInt32(0), reader.GetString(1), reader.GetString(2)));
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("[ORM] ObtenirTots: " + ex.Message);
        }
        return llista;
    }

    public bool EsborrarPerId(int id)
    {
        try
        {
            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqliteCommand("DELETE FROM Usuarios WHERE id=@id;", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("[ORM] EsborrarPerId: " + ex.Message);
        }
        return false;
    }

    public bool EsborrarPerNom(string nom)
    {
        try
        {
            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqliteCommand("DELETE FROM Usuarios WHERE usuario=@u;", conn))
                {
                    cmd.Parameters.AddWithValue("@u", nom.Trim());
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("[ORM] EsborrarPerNom: " + ex.Message);
        }
        return false;
    }
}
