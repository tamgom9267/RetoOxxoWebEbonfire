using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace RetoOxxoWeb.Model
{
    public class DataBaseContext
    {
        public string ConnectionString {get; set;}
        
        public DataBaseContext()
        {
            ConnectionString = "Server=127.0.0.1;Port=3306;Database=oxxojuego;Uid=root;password=root;";
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public usuario AuthenticateUser(string nombre, string password)
        {
            usuario user = null;
            
            using (MySqlConnection conexion = GetConnection())
            {
                conexion.Open();
                string query = "SELECT id_usuario, nombre FROM usuario WHERE nombre = @nombre AND contraseña = @password";
                
                using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    cmd.Parameters.AddWithValue("@password", password);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new usuario
                            {
                                id_usuario = Convert.ToInt32(reader["id_usuario"]),
                                nombre = reader["nombre"].ToString()
                            };
                        }
                    }
                }
            }

            return user;
        }

        public usuario GetUsuarioEncima(int idUsuarioActual)
        {
            usuario usuarioEncima = null;

            using (MySqlConnection conexion = GetConnection())
            {
                conexion.Open();
                string queryArriba = @"
                    SELECT * FROM (
                        SELECT 
                            u.id_usuario, 
                            u.nombre, 
                            AVG(IFNULL(t.puntos,0) + IFNULL(l.puntos,0) + IFNULL(d.puntos,0)) / 3 AS promedio_puntos
                        FROM 
                            usuario u
                        LEFT JOIN 
                            taberna t ON u.id_usuario = t.id_usuario
                        LEFT JOIN 
                            laberinto l ON u.id_usuario = l.id_usuario
                        LEFT JOIN 
                            decision d ON u.id_usuario = d.id_usuario
                        GROUP BY 
                            u.id_usuario
                        ORDER BY 
                            promedio_puntos DESC
                    ) ranking
                    WHERE promedio_puntos < (
                        SELECT AVG(IFNULL(t.puntos,0) + IFNULL(l.puntos,0) + IFNULL(d.puntos,0)) / 3
                        FROM 
                            usuario u
                        LEFT JOIN 
                            taberna t ON u.id_usuario = t.id_usuario
                        LEFT JOIN 
                            laberinto l ON u.id_usuario = l.id_usuario
                        LEFT JOIN 
                            decision d ON u.id_usuario = d.id_usuario
                        WHERE u.id_usuario = @idUsuarioActual
                    )
                    ORDER BY promedio_puntos DESC
                    LIMIT 1;";

                MySqlCommand cmd = new MySqlCommand(queryArriba, conexion);
                cmd.Parameters.AddWithValue("@idUsuarioActual", idUsuarioActual);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        usuarioEncima = new usuario
                        {
                            id_usuario = Convert.ToInt32(reader["id_usuario"]),
                            nombre = reader["nombre"].ToString()
                        };
                    }
                }
            }

            return usuarioEncima;
        }

        public usuario GetUsuarioDebajo(int idUsuarioActual)
        {
            usuario usuarioDebajo = null;

            using (MySqlConnection conexion = GetConnection())
            {
                conexion.Open();
                string queryAbajo = @"
                    SELECT * FROM (
                        SELECT 
                            u.id_usuario, 
                            u.nombre, 
                            AVG(IFNULL(t.puntos,0) + IFNULL(l.puntos,0) + IFNULL(d.puntos,0)) / 3 AS promedio_puntos
                        FROM 
                            usuario u
                        LEFT JOIN 
                            taberna t ON u.id_usuario = t.id_usuario
                        LEFT JOIN 
                            laberinto l ON u.id_usuario = l.id_usuario
                        LEFT JOIN 
                            decision d ON u.id_usuario = d.id_usuario
                        GROUP BY 
                            u.id_usuario
                        ORDER BY 
                            promedio_puntos DESC
                    ) ranking
                    WHERE promedio_puntos > (
                        SELECT AVG(IFNULL(t.puntos,0) + IFNULL(l.puntos,0) + IFNULL(d.puntos,0)) / 3
                        FROM 
                            usuario u
                        LEFT JOIN 
                            taberna t ON u.id_usuario = t.id_usuario
                        LEFT JOIN 
                            laberinto l ON u.id_usuario = l.id_usuario
                        LEFT JOIN 
                            decision d ON u.id_usuario = d.id_usuario
                        WHERE u.id_usuario = @idUsuarioActual
                    )
                    ORDER BY promedio_puntos ASC
                    LIMIT 1;";

                MySqlCommand cmd = new MySqlCommand(queryAbajo, conexion);
                cmd.Parameters.AddWithValue("@idUsuarioActual", idUsuarioActual);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        usuarioDebajo = new usuario
                        {
                            id_usuario = Convert.ToInt32(reader["id_usuario"]),
                            nombre = reader["nombre"].ToString()
                        };
                    }
                }
            }

            return usuarioDebajo;
        }
        public List<usuario> GetAllUsers()
        {
            List<usuario> usuarios = new List<usuario>();

            using (MySqlConnection conexion = GetConnection())
            {
                conexion.Open();
                string query = "SELECT nombre, contraseña FROM usuario";

                using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usuarios.Add(new usuario
                            {
                                nombre = reader["nombre"].ToString(),
                                contraseña = reader["contraseña"].ToString()
                            });
                        }
                    }
                }
            }

            return usuarios;
        }

    }
}
