using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Diagnostics; // Para depuración

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
                        WHERE u.id_usuario = 4
                    )
                    ORDER BY promedio_puntos DESC
                    LIMIT 1;";

                MySqlCommand cmd = new MySqlCommand(queryArriba, conexion);
                cmd.Parameters.AddWithValue("4", idUsuarioActual);

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
                        WHERE u.id_usuario = 4
                    )
                    ORDER BY promedio_puntos ASC
                    LIMIT 1;";

                MySqlCommand cmd = new MySqlCommand(queryAbajo, conexion);
                cmd.Parameters.AddWithValue("4", idUsuarioActual);

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
        // Hola!
        public (int ProgresoTaberna, int ProgresoLaberinto, int ProgresoDecision) GetProgresoUsuario(int idUsuarioActual)
        {
            int progresoTaberna = 0;
            int progresoLaberinto = 0;
            int progresoDecision = 0;

            using (MySqlConnection conexion = GetConnection())
            {
                conexion.Open();

                string queryProgreso = @"
                    SELECT 
                        IFNULL(t.puntos, 0) AS progreso_taberna,
                        IFNULL(l.puntos, 0) AS progreso_laberinto,
                        IFNULL(d.puntos, 0) AS progreso_decision
                    FROM 
                        usuario u
                    LEFT JOIN 
                        taberna t ON u.id_usuario = t.id_usuario
                    LEFT JOIN 
                        laberinto l ON u.id_usuario = l.id_usuario
                    LEFT JOIN 
                        decision d ON u.id_usuario = d.id_usuario
                    WHERE 
                        u.id_usuario = 4;";

                MySqlCommand cmd = new MySqlCommand(queryProgreso, conexion);
                cmd.Parameters.AddWithValue("4", idUsuarioActual);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        progresoTaberna = Convert.ToInt32(reader["progreso_taberna"]);
                        progresoLaberinto = Convert.ToInt32(reader["progreso_laberinto"]);
                        progresoDecision = Convert.ToInt32(reader["progreso_decision"]);
                    }
                }
            }
            int porcentajeTaberna = (progresoTaberna * 100) / 500;
            int porcentajeLaberinto = (progresoLaberinto * 100) / 300;
            int porcentajeDecision = (progresoDecision * 100) / 200;

            return (porcentajeTaberna, porcentajeLaberinto, porcentajeDecision);
        }

        public (int FoodService, int EjecucionPromociones, int EquiposCompletos, int Rotacion, int FaltanteEfectivo) GetMetricasDeTienda(int idUsuario)
        {
            int foodService = 0;
            int ejecucionPromociones = 0;
            int equiposCompletos = 0;
            int rotacion = 0;
            int faltanteEfectivo = 0;

            using (MySqlConnection conexion = GetConnection())
            {
                conexion.Open();

                string queryMetricas = @"
                    SELECT 
                        food_service,
                        ejecucion_promociones,
                        equipos_completos,
                        rotacion,
                        faltante_efectivo
                    FROM 
                        metricas_de_tienda
                    WHERE 
                        id_usuario = 4;";

                MySqlCommand cmd = new MySqlCommand(queryMetricas, conexion);
                cmd.Parameters.AddWithValue("4", idUsuario);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        foodService = Convert.ToInt32(reader["food_service"]);
                        ejecucionPromociones = Convert.ToInt32(reader["ejecucion_promociones"]);
                        equiposCompletos = Convert.ToInt32(reader["equipos_completos"]);
                        rotacion = Convert.ToInt32(reader["rotacion"]);
                        faltanteEfectivo = Convert.ToInt32(reader["faltante_efectivo"]);
                    }
                }
            }

            return (foodService, ejecucionPromociones, equiposCompletos, rotacion, faltanteEfectivo);
        }

        public string GetNombreUsuario(int idUsuario)
        {
            string nombreUsuario = string.Empty;

            using (MySqlConnection conexion = GetConnection())
            {
                conexion.Open();

                string queryNombre = "SELECT nombre FROM usuario WHERE id_usuario = 4;";
                MySqlCommand cmd = new MySqlCommand(queryNombre, conexion);
                cmd.Parameters.AddWithValue("4", idUsuario);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        nombreUsuario = reader["nombre"].ToString();
                    }
                }
            }

            return nombreUsuario;
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
                                nombre = reader["nombre"].ToString().Trim(),
                                contraseña = reader["contraseña"].ToString().Trim()
                            });
                        }
                    }
                }
            }

            Debug.WriteLine("Usuarios obtenidos de la base de datos:");
            foreach (var u in usuarios)
            {
                Debug.WriteLine($"Usuario: {u.nombre}, Contraseña: {u.contraseña}");
            }

            return usuarios;
        }

        public usuario GetUsuarioPorId(int idUsuario)
        {
        usuario usuario = null;

        using (MySqlConnection conexion = GetConnection())
        {
            conexion.Open();
            string query = @"
                SELECT 
                    id_usuario, nombre, apellidop, apellidom, telefono, fotografia, 
                    codigo_postal, calle, estado, ciudad, tipoempleado
                FROM usuario 
                WHERE id_usuario = @id";
            MySqlCommand cmd = new MySqlCommand(query, conexion);
            cmd.Parameters.AddWithValue("@id", idUsuario);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    usuario = new usuario
                    {
                        id_usuario = Convert.ToInt32(reader["id_usuario"]),
                        nombre = reader["nombre"].ToString(),
                        apellidop = reader["apellidop"].ToString(),
                        apellidom = reader["apellidom"].ToString(),
                        telefono = reader["telefono"].ToString(),
                        fotografia = reader["fotografia"].ToString(),
                        codigo_postal = reader["codigo_postal"].ToString(),
                        calle = reader["calle"].ToString(),
                        estado = reader["estado"].ToString(),
                        ciudad = reader["ciudad"].ToString(),
                        tipoempleado = Convert.ToByte(reader["tipoempleado"])
                    };
                }
            }
        }

        return usuario;
    }

    }
}