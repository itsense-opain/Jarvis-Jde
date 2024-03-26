using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Configuration;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;

namespace Opain.Jarvis.Servicios.Store.Helper
{
    public class Ejecutor
    {
        private readonly IConfiguration configuration;
        private Dictionary<string, Campo> Campos { get; set; }
        private IDataParameter[] ParametrosSalida { get; set; }
        public Ejecutor(IConfiguration cfg)
        {
            this.configuration = cfg;
            this.Campos = new Dictionary<string, Campo>();
        }

        public void LimpiarCampos() => this.Campos = new Dictionary<string, Campo>();
        public void AgregarCampoIn(string key, object valor)
        {
            this.Campos.Add(key, new Campo
            {
                Tipo = TipoCampo.IN,
                Valor = valor
            });
        }

        public void AgregarCampoOut(string key, DbType dbType, object valor)
        {
            this.Campos.Add(key, new Campo
            {
                Tipo = TipoCampo.OUT,
                dbType = dbType,
                Valor = valor
            });
        }

        public T ObtenerCampoSalida<T>(string campo)
        {
            List<IDataParameter> Parametros = ParametrosSalida.Where(p => p.Direction == ParameterDirection.Output).ToList();
            IDataParameter Parametro = Parametros.FirstOrDefault(p => p.ParameterName.Equals("@" + campo));

            if (Parametro == null)
                return default(T);

            return (T)Parametro.Value;
        }

        public DataTable Conexion(string nombreProcedimiento)
        {
            string constr = configuration.GetSection("ConnectionStrings:ConexionJarvisBD").Value;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand(nombreProcedimiento, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    foreach (var item in Campos)
                    {
                        if (item.Value.Tipo == TipoCampo.IN)
                            cmd.Parameters.AddWithValue(string.Format("@{0}", item.Key), item.Value.Valor);

                        if (item.Value.Tipo == TipoCampo.OUT)
                        {
                            MySqlParameter pvNewId = new MySqlParameter();

                            pvNewId.ParameterName = string.Format("@{0}", item.Key);
                            pvNewId.DbType = item.Value.dbType;
                            pvNewId.Direction = ParameterDirection.Output;
                            cmd.Parameters.Add(pvNewId);
                        }                            
                    }

                    try
                    {
                        using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            sda.Fill(dt);
                            this.ParametrosSalida = sda.GetFillParameters();
                            con.Close();
                            con.Dispose();
                            return dt;
                        }
                    }
                    catch (Exception Ex)
                    {
                        con.Close();
                        con.Dispose();
                        throw Ex;
                    }
                    
                }
            }
        }

        public DataTable GetData(string nombreProcedimiento)
        {
            string constr = configuration.GetSection("ConnectionStrings:ConexionJarvisBD").Value;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand(nombreProcedimiento, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    foreach (var item in Campos)
                    {
                        if (item.Value.Tipo == TipoCampo.IN)
                            cmd.Parameters.AddWithValue(string.Format("@{0}", item.Key), item.Value.Valor);

                        if (item.Value.Tipo == TipoCampo.OUT)
                        {
                            MySqlParameter pvNewId = new MySqlParameter();

                            pvNewId.ParameterName = string.Format("@{0}", item.Key);
                            pvNewId.DbType = item.Value.dbType;
                            pvNewId.Direction = ParameterDirection.Output;
                            cmd.Parameters.Add(pvNewId);
                        }
                    }

                    try
                    {
                        DataTable dt;
                        con.Open();
                        using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                        {
                            dt = new DataTable();
                            sda.Fill(dt);
                            this.ParametrosSalida = sda.GetFillParameters();

                        }
                        con.Close();
                        return dt;
                    }
                    catch (Exception Ex)
                    {
                        throw Ex;
                    }

                }
            }
        }

        public int ConexionEx(string nombreProcedimiento)
        {
            string constr = configuration.GetSection("ConnectionStrings:ConexionJarvisBD").Value;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand(nombreProcedimiento, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    foreach (var item in Campos)
                    {
                        if (item.Value.Tipo == TipoCampo.IN)
                            cmd.Parameters.AddWithValue(string.Format("@{0}", item.Key), item.Value.Valor);

                        if (item.Value.Tipo == TipoCampo.OUT)
                        {
                            MySqlParameter pvNewId = new MySqlParameter();

                            pvNewId.ParameterName = string.Format("@{0}", item.Key);
                            pvNewId.DbType = item.Value.dbType;
                            pvNewId.Direction = ParameterDirection.Output;
                            cmd.Parameters.Add(pvNewId);
                        }
                    }
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.StackTrace.ToString());
                        con.Close();
                        throw;
                    }

                   
                    return 1;
                    //using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    //{
                    //    DataTable dt = new DataTable();
                    //    sda.Fill(dt);
                    //    this.ParametrosSalida = sda.GetFillParameters();
                    //    return dt;
                    //}
                }
            }
        }
    }

    public class Campo
    {
        public TipoCampo Tipo { get; set; }
        public DbType dbType { get; set; }

        public object Valor { get; set; }
    }

    public enum TipoCampo
    {
        IN,
        OUT
    }
}