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

						ConnectionString = "Server=mysql-3d246747-tec-acff.b.aivencloud.com;Port=25482;Database=oxxojuego;User Id=avnadmin;Password=AVNS_308DdCWk2oAlMYpGE-Q;SslMode=Required;";

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
                    WITH leaderboard AS (
                        SELECT 
                            u.id_usuario,
                            u.nombre,
                            COALESCE(SUM(1000 - t.puntos), 0) + COALESCE(SUM(d.puntos), 0) as total_puntos,
                            RANK() OVER (ORDER BY COALESCE(SUM(1000 - t.puntos), 0) + COALESCE(SUM(d.puntos), 0) DESC) as ranking
                        FROM usuario u
                        LEFT JOIN taberna t ON u.id_usuario = t.id_usuario
                        LEFT JOIN decision d ON u.id_usuario = d.id_usuario
                        GROUP BY u.id_usuario, u.nombre
                    )
                    SELECT l2.id_usuario, l2.nombre, u.fotografia
                    FROM leaderboard l1
                    JOIN leaderboard l2 ON l2.ranking = l1.ranking - 1
                    JOIN usuario u ON l2.id_usuario = u.id_usuario
                    WHERE l1.id_usuario = @idUsuarioActual
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
														nombre = reader["nombre"].ToString(),
														fotografia = reader["fotografia"].ToString()
												};
										}
								}
						}

						return usuarioEncima;
				}

				//Rodrigo
				public void ActualizarUsuario(usuario u)
				{
						using (MySqlConnection conexion = GetConnection())
						{
								conexion.Open();
								string query = @"
										UPDATE usuario
										SET nom_usuario = @nom_usuario,
												nombre = @nombre,
												apellidop = @apellidop,
												apellidom = @apellidom,
												telefono = @telefono,											
												tipo_empleado = @tipo_empleado,
												contraseña = @contraseña,
												calle = @calle,
												estado = @estado,
												ciudad = @ciudad,
												cp = @cp
										where id_usuario = @id_usuario;";
								MySqlCommand cmd = new MySqlCommand(query, conexion);
								cmd.Parameters.AddWithValue("@id_usuario", u.id_usuario);
								cmd.Parameters.AddWithValue("@nom_usuario", u.nom_usuario);
								cmd.Parameters.AddWithValue("@nombre", u.nombre);
								cmd.Parameters.AddWithValue("@apellidop", u.apellidop);
								cmd.Parameters.AddWithValue("@apellidom", u.apellidom);
								cmd.Parameters.AddWithValue("@telefono", u.telefono);								
								cmd.Parameters.AddWithValue("@tipo_empleado", u.tipo_empleado);
								cmd.Parameters.AddWithValue("@contraseña", u.contraseña);
								cmd.Parameters.AddWithValue("@calle", u.calle);
								cmd.Parameters.AddWithValue("@estado", u.estado);
								cmd.Parameters.AddWithValue("@ciudad", u.ciudad);
								cmd.Parameters.AddWithValue("@cp", u.cp);

								cmd.ExecuteNonQuery();
						}
				}

				public usuario GetUsuarioDebajo(int idUsuarioActual)
				{
						usuario usuarioDebajo = null;

						using (MySqlConnection conexion = GetConnection())
						{
								conexion.Open();
								string queryAbajo = @"
                    WITH leaderboard AS (
                        SELECT 
                            u.id_usuario,
                            u.nombre,
                            COALESCE(SUM(1000 - t.puntos), 0) + COALESCE(SUM(d.puntos), 0) as total_puntos,
                            RANK() OVER (ORDER BY COALESCE(SUM(1000 - t.puntos), 0) + COALESCE(SUM(d.puntos), 0) DESC) as ranking
                        FROM usuario u
                        LEFT JOIN taberna t ON u.id_usuario = t.id_usuario
                        LEFT JOIN decision d ON u.id_usuario = d.id_usuario
                        GROUP BY u.id_usuario, u.nombre
                    )
                    SELECT l2.id_usuario, l2.nombre, u.fotografia
                    FROM leaderboard l1
                    JOIN leaderboard l2 ON l2.ranking = l1.ranking + 1
                    JOIN usuario u ON l2.id_usuario = u.id_usuario
                    WHERE l1.id_usuario = @idUsuarioActual
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
														nombre = reader["nombre"].ToString(),
														fotografia = reader["fotografia"].ToString()
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
												IFNULL(1000 - t.puntos, 0) AS progreso_taberna,
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
												u.id_usuario = @idUsuarioActual;";

								MySqlCommand cmd = new MySqlCommand(queryProgreso, conexion);
								cmd.Parameters.AddWithValue("@idUsuarioActual", idUsuarioActual);

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
						int porcentajeTaberna = ((progresoTaberna * 100)) / 1000;
						int porcentajeLaberinto = (progresoLaberinto * 100) / 1000;
						int porcentajeDecision = (progresoDecision * 100) / 1000;

						return (porcentajeTaberna, porcentajeLaberinto, porcentajeDecision);
				}


				public (int EfectividadHorarios, int FoodService, int Planogramas, int EjecucionPromociones, int ProgramaLealtad, int ClasificacionTiendas) GetPromedioIndicadoresOperativos(int idUsuario)
				{
					int efectividadHorarios = 0;
					int foodService = 0;
					int planogramas = 0;
					int ejecucionPromociones = 0;
					int programaLealtad = 0;
					int clasificacionTiendas = 0;

					using (MySqlConnection conexion = GetConnection())
					{
						conexion.Open();

						string queryPromedios = @"
							SELECT 
								AVG(efectividad_horarios) AS efectividad_horarios,
								AVG(food_service) AS food_service,
								AVG(planogramas) AS planogramas,
								AVG(ejecucion_promociones) AS ejecucion_promociones,
								AVG(programa_lealtad) AS programa_lealtad,
								AVG(clasificacion_tiendas) AS clasificacion_tiendas
							FROM 
								indicadores_operativos
							WHERE 
								id_usuario = @idUsuario;";

						MySqlCommand cmd = new MySqlCommand(queryPromedios, conexion);
						cmd.Parameters.AddWithValue("@idUsuario", idUsuario);

						using (var reader = cmd.ExecuteReader())
						{
							if (reader.Read())
							{
								efectividadHorarios = Convert.ToInt32(reader["efectividad_horarios"]);
								foodService = Convert.ToInt32(reader["food_service"]);
								planogramas = Convert.ToInt32(reader["planogramas"]);
								ejecucionPromociones = Convert.ToInt32(reader["ejecucion_promociones"]);
								programaLealtad = Convert.ToInt32(reader["programa_lealtad"]);
								clasificacionTiendas = Convert.ToInt32(reader["clasificacion_tiendas"]);
							}
						}
					}

					return (efectividadHorarios, foodService, planogramas, ejecucionPromociones, programaLealtad, clasificacionTiendas);
				}


				public string GetNombreUsuario(int idUsuario)
				{
						string nombreUsuario = string.Empty;

						using (MySqlConnection conexion = GetConnection())
						{
								conexion.Open();

								string queryNombre = "SELECT nombre FROM usuario WHERE id_usuario = @idUsuario;";
								MySqlCommand cmd = new MySqlCommand(queryNombre, conexion);
								cmd.Parameters.AddWithValue("@idUsuario", idUsuario);

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
												id_usuario,
												nom_usuario,
												nombre,
												apellidop,
												apellidom,
												telefono,
												fotografia,
												contraseña,
												calle,
												estado,
												ciudad,
												cp,
												tipo_empleado
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
														nom_usuario = reader["nom_usuario"].ToString(),
														nombre = reader["nombre"].ToString(),
														apellidop = reader["apellidop"].ToString(),
														apellidom = reader["apellidom"].ToString(),
														telefono = reader["telefono"].ToString(),
														fotografia = reader["fotografia"].ToString(),
														contraseña = reader["contraseña"].ToString(),
														calle = reader["calle"].ToString(),
														estado = reader["estado"].ToString(),
														ciudad = reader["ciudad"].ToString(),
														cp = Convert.ToInt32(reader["cp"]),
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
								ROUND(COALESCE(SUM(1000 - t.puntos), 0), 2) AS puntos_taberna,
								ROUND(COALESCE(SUM(d.puntos), 0) + COALESCE(SUM(l.puntos), 0) + COALESCE(SUM(1000 - t.puntos), 0), 2) AS puntos_totales
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


		public usuario NewUser(usuario nUsuario) {
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
						return nUsuario;
				}

		public List<UsuarioLogros> GetLogros()
				{
						List<UsuarioLogros> Logros = new List<UsuarioLogros>();
						MySqlConnection conexion = GetConnection();
						conexion.Open();

						MySqlCommand cmd = new MySqlCommand(@"
						SELECT u.id_usuario, u.nombre, 
								(logro_lab1 + logro_lab2 + logro_lab3 + logro_lab4 + 
								logro_tab1 + logro_tab2 + logro_tab3 + logro_tab4 + 
								logro_dec1 + logro_dec2 + logro_dec3 + logro_dec4) AS total_logros
						FROM Logros l
						JOIN usuario u ON l.id_usuario = u.id_usuario
						Order by total_logros desc;", conexion);

						UsuarioPuntaje usr1 = new UsuarioPuntaje();

						using (var reader = cmd.ExecuteReader())
						{
								while (reader.Read())
								{
										Logros.Add(new UsuarioLogros
										{
												id_usuario = reader.GetInt32(0),
												nombre = reader.GetString(1),
												total_logros = reader.GetDecimal(2),
										});
								}
						}

						return Logros;
				}

				public List<InfoJuego> GetDatosJuego()
				{
					List<InfoJuego> DatosJuego = new List<InfoJuego>();
					MySqlConnection conexion = GetConnection();
					conexion.Open();

					MySqlCommand cmd = new MySqlCommand(@"
						SELECT id_info, historia, personajes_nombre, personaje_desc, 
							como_ganar, como_perder, creditos, licensia, Controles
						FROM InfoJuego;
					", conexion);

					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							DatosJuego.Add(new InfoJuego
							{
								id_info = reader.GetInt32("id_info"),
								historia = reader["historia"] as string,
								personajes_nombre = reader["personajes_nombre"] as string,
								personaje_desc = reader["personaje_desc"] as string,
								como_ganar = reader["como_ganar"] as string,
								como_perder = reader["como_perder"] as string,
								creditos = reader["creditos"] as string,
								licensia = reader["licensia"] as string,
								Controles = reader["Controles"] as string
							});
						}
					}

					return DatosJuego;
				}
		}

		

}