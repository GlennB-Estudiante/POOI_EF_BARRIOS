using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace POOI_EF.Models
{
    // Operaciones conectadas contra SQL Server
    public class ConsumidorDAO
    {
        private readonly string cadena = ConfigurationManager.ConnectionStrings["cn"].ConnectionString;

        private static Consumidor Leer(SqlDataReader dr)
        {
            return new Consumidor
            {
                dnicont = dr["dnicont"].ToString(),
                nomcont = dr["nomcont"].ToString(),
                apecont = dr["apecont"].ToString(),
                emailcont = dr["emailcont"] == System.DBNull.Value ? null : dr["emailcont"].ToString(),
                idpais = dr["idpais"].ToString()
            };
        }

        public List<Consumidor> Listar()
        {
            var lista = new List<Consumidor>();
            using (var cn = new SqlConnection(cadena))
            using (var cmd = new SqlCommand("usp_listar_consumidor", cn) { CommandType = CommandType.StoredProcedure })
            {
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                    while (dr.Read()) lista.Add(Leer(dr));
            }
            return lista;
        }

        public Consumidor Buscar(string dni)
        {
            using (var cn = new SqlConnection(cadena))
            using (var cmd = new SqlCommand("usp_buscar_consumidor", cn) { CommandType = CommandType.StoredProcedure })
            {
                cmd.Parameters.AddWithValue("@dnicont", (object)dni ?? System.DBNull.Value);
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                    return dr.Read() ? Leer(dr) : null;
            }
        }

        public List<string[]> Paises()
        {
            var l = new List<string[]>();
            using (var cn = new SqlConnection(cadena))
            using (var cmd = new SqlCommand("usp_listar_paises", cn) { CommandType = CommandType.StoredProcedure })
            {
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                    while (dr.Read()) l.Add(new[] { dr["idpais"].ToString(), dr["nompais"].ToString() });
            }
            return l;
        }

        private string Ejecutar(string sp, Consumidor c)
        {
            using (var cn = new SqlConnection(cadena))
            using (var cmd = new SqlCommand(sp, cn) { CommandType = CommandType.StoredProcedure })
            {
                cmd.Parameters.AddWithValue("@dnicont", (object)c.dnicont ?? System.DBNull.Value);
                if (sp != "usp_eliminar_consumidor")
                {
                    cmd.Parameters.AddWithValue("@nomcont", (object)c.nomcont ?? System.DBNull.Value);
                    cmd.Parameters.AddWithValue("@apecont", (object)c.apecont ?? System.DBNull.Value);
                    cmd.Parameters.AddWithValue("@emailcont", string.IsNullOrWhiteSpace(c.emailcont) ? (object)System.DBNull.Value : c.emailcont);
                    cmd.Parameters.AddWithValue("@idpais", (object)c.idpais ?? System.DBNull.Value);
                }
                var p = new SqlParameter("@mensaje", SqlDbType.VarChar, 200) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(p);
                cn.Open();
                cmd.ExecuteNonQuery();
                return p.Value.ToString();
            }
        }

        public string Agregar(Consumidor c) { return Ejecutar("usp_agrega_consumidor", c); }
        public string Actualizar(Consumidor c) { return Ejecutar("usp_actualizar_consumidor", c); }
        public string Eliminar(string dni) { return Ejecutar("usp_eliminar_consumidor", new Consumidor { dnicont = dni }); }

        // Pregunta 2: empleados y cantidad de pedidos por aÃ±o (GROUP BY en el SP)
        public List<EmpleadoPedidos> PedidosPorAnio(int anio)
        {
            var l = new List<EmpleadoPedidos>();
            using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cnNorthwind"].ConnectionString))
            using (var cmd = new SqlCommand("usp_empleados_pedidos_anio", cn) { CommandType = CommandType.StoredProcedure })
            {
                cmd.Parameters.AddWithValue("@anio", anio);
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                    while (dr.Read())
                        l.Add(new EmpleadoPedidos { idempleado = (int)dr["idempleado"], nombre = dr["nombre"].ToString(), cantidad = (int)dr["cantidad"] });
            }
            return l;
        }
    }
}

