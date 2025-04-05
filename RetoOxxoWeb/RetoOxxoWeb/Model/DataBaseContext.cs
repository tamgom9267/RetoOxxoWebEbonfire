using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using Org.BouncyCastle.Tls;

namespace RetoOxxoWeb.Model
{
    public class DataBaseContext
    {
        public string ConnectionString {get; set;}
        public DataBaseContext()
        {
            ConnectionString = "Server=127.0.0.1;Port=3306;Database=oxxojuego;Uid=root;password=;";

            // No se de quien es pero estaba antes = "Server=127.0.0.1;Port=3306;Database=oxxojuego;Uid=root;password=root;"

            //Santiago = "Server=127.0.0.1;Port=3306;Database=oxxojuego;Uid=root;password=November-26-2004;"
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

        /*
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
        */

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
            List<usuario> ListaUsuarios = new List<usuario>();
            MySqlConnection conexion = GetConnection();
            conexion.Open();

            MySqlCommand cmd = new MySqlCommand("SELECT id_usuario, nom_usuario, nombre, contraseña FROM usuario", conexion);
            
            usuario usr1 = new usuario();

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    usr1 = new usuario();
                    usr1.id_usuario = Convert.ToInt32(reader["id_usuario"]);
                    usr1.nom_usuario = reader["nom_usuario"].ToString();
                    usr1.nombre = reader["nombre"].ToString();
                    usr1.contraseña = reader["contraseña"].ToString();
                    ListaUsuarios.Add(usr1);
                }
            }
    
            return ListaUsuarios;
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
                    calle, estado, ciudad, tipo_empleado
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
                        cp = Convert.ToInt32(reader["cp"]),
                        calle = reader["calle"].ToString(),
                        estado = reader["estado"].ToString(),
                        ciudad = reader["ciudad"].ToString(),
                        tipo_empleado = Convert.ToByte(reader["tipo_empleado"])
                    };
                }
            }
        }

        return usuario;
    }

    public List<UsuarioPuntaje> GetPuntos()
        {
            List<UsuarioPuntaje> Puntajes = new List<UsuarioPuntaje>();
            MySqlConnection conexion = GetConnection();
            conexion.Open();

            MySqlCommand cmd = new MySqlCommand(@"
            SELECT 
                u.id_usuario,
                us.nombre AS nombre_usuario, -- Obtener el nombre del usuario
                ROUND(COALESCE(SUM(d.puntos), 0), 2) AS puntos_decision,
                ROUND(COALESCE(SUM(l.puntos), 0), 2) AS puntos_laberinto,
                ROUND(COALESCE(SUM(t.puntos), 0), 2) AS puntos_taberna,
                ROUND(COALESCE(SUM(d.puntos), 0) + COALESCE(SUM(l.puntos), 0) + COALESCE(SUM(t.puntos), 0), 2) AS puntos_totales
            FROM (
                SELECT DISTINCT id_usuario FROM decision 
                UNION 
                SELECT DISTINCT id_usuario FROM laberinto 
                UNION 
                SELECT DISTINCT id_usuario FROM taberna
            ) u
            JOIN usuario us ON u.id_usuario = us.id_usuario -- Unir con la tabla usuario
            LEFT JOIN decision d ON u.id_usuario = d.id_usuario
            LEFT JOIN laberinto l ON u.id_usuario = l.id_usuario
            LEFT JOIN taberna t ON u.id_usuario = t.id_usuario
            GROUP BY u.id_usuario, us.nombre -- Agrupar por nombre
            ORDER BY puntos_totales desc;", conexion);
            
            UsuarioPuntaje usr1 = new UsuarioPuntaje();

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Puntajes.Add(new UsuarioPuntaje
                    {
                        id_usuario = reader.GetInt32(0),
                        nombre = reader.GetString(1),
                        puntos_decision = reader.GetDecimal(2),
                        puntos_laberinto = reader.GetDecimal(3),
                        puntos_taberna = reader.GetDecimal(4),
                        puntos_totales = reader.GetDecimal(5)
                    });
                }
            }
    
            return Puntajes;
        }

    public void NewUser(usuario nUsuario) {
        MySqlConnection conexion = GetConnection();
        conexion.Open();
        
        MySqlCommand cmd = new MySqlCommand(
            @"INSERT into `usuario` 
            (`nom_usuario`, `nombre`, `apellidoP`, `apellidoM`, `telefono`, `contraseña`)
            values (
            @nom_usuario, @nombre, @apellidoP, @apellidoM, @telefono, @contraseña)",
            conexion);

            cmd.Parameters.AddWithValue("@nom_usuario", nUsuario.nom_usuario);
            cmd.Parameters.AddWithValue("@nombre", nUsuario.nombre);
            cmd.Parameters.AddWithValue("@apellidoP", nUsuario.apellidop);
            cmd.Parameters.AddWithValue("@apellidoM", nUsuario.apellidom);
            cmd.Parameters.AddWithValue("@telefono", nUsuario.telefono);
            cmd.Parameters.AddWithValue("@contraseña", nUsuario.contraseña);

            cmd.ExecuteNonQuery();
            }
    }

}