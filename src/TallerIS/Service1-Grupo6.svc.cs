using MySql.Data.MySqlClient;
using MySql.Data.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfService1
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Service1" en el código, en svc y en el archivo de configuración.
    // NOTE: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione Service1.svc o Service1.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class Service1 : IService1
    {
        MySqlConnection conn;
        MySqlCommand comm;
        MySqlConnectionStringBuilder connStringBuilder;

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void ConnectToDb()
        {
            connStringBuilder = new MySqlConnectionStringBuilder();
            connStringBuilder.Server = "127.0.0.1";
            connStringBuilder.Port = 3306;
            connStringBuilder.UserID = "root";
            connStringBuilder.Password = "chiguivombatidonutria2.0";
            connStringBuilder.Database = "practica1";
            connStringBuilder.ConnectionTimeout = 30;
            connStringBuilder.IntegratedSecurity = true;
            
            conn = new MySqlConnection(connStringBuilder.ToString());
            comm = conn.CreateCommand();
        }
        
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }
        
        public int InsertarProfesor(int idProfesor, int idPersona, int idDepartamento)
        {
            try
            {
                ConnectToDb();
                comm.CommandText = "INSERT INTO profesor VALUES (@idProfesor, @idPersona, @idDepartamento);";
                comm.Parameters.AddWithValue("idProfesor", idProfesor);
                comm.Parameters.AddWithValue("idPersona", idPersona);
                comm.Parameters.AddWithValue("idDepartamento", idDepartamento);

                comm.CommandType = System.Data.CommandType.Text;
                conn.Open();
                log.Info("El profesor fue insertado exitosamente");
                return comm.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        public int BorrarProfesor(int idProfesor)
        {
            try
            {
                ConnectToDb();
                comm.CommandText = "DELETE from profesor WHERE idProfesor=@idProfesor;";
                comm.Parameters.AddWithValue("idProfesor", idProfesor);
                comm.CommandType = System.Data.CommandType.Text;
                conn.Open();

                log.Info("El profesor fue borrado exitosamente");
                return comm.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        public int ActualizarProfesor(int idProfesor, int idPersona, int idDepartamento)
        {
            try
            {
                ConnectToDb();
                comm.CommandText = "UPDATE profesor SET idPersona=@idPersona, idDepartamento=@idDepartamento WHERE idProfesor=@idProfesor;";
                comm.Parameters.AddWithValue("idPersona", idPersona);
                comm.Parameters.AddWithValue("idDepartamento", idDepartamento);
                comm.Parameters.AddWithValue("idProfesor", idProfesor);
               
                comm.CommandType = System.Data.CommandType.Text;
                conn.Open();

                log.Info("Los datos del profesor fueron actualizados exitosamente");
                return comm.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        public List<Profesor> GetProfesores()
        {
            List<Profesor> profesores = new List<Profesor>();
            try
            {
                ConnectToDb();
                comm.CommandText = "SELECT idProfesor, idPersona, idDepartamento FROM profesor";
                comm.CommandType = System.Data.CommandType.Text;

                conn.Open();

                MySqlDataReader reader = comm.ExecuteReader();

                while (reader.Read())
                {
                    Profesor profesor = new Profesor();
                    profesor.IdProfesor = Convert.ToInt32(reader[0]);
                    profesor.IdProfesor = Convert.ToInt32(reader[1]);
                    profesor.IdDepartamento = Convert.ToInt32(reader[2]);
                    profesores.Add(profesor);
                }

                log.Info("La consulta de los profesores fue realizada exitosamente");
                return profesores;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        public Profesor GetProfesor(int idProfesor)
        {
            Profesor profesor = new Profesor();
            try
            {
                ConnectToDb();
                comm.CommandText = "SELECT idPersona, idDepartamento FROM profesor WHERE idProfesor=@idProfesor";
                comm.Parameters.AddWithValue("idProfesor", idProfesor);
                profesor.IdProfesor = idProfesor;
                comm.CommandType = System.Data.CommandType.Text;
                
                conn.Open();

                MySqlDataReader reader = comm.ExecuteReader();

                while (reader.Read())
                {
                    profesor.IdPersona = Convert.ToInt32(reader[0]);
                    profesor.IdDepartamento = Convert.ToInt32(reader[1]);
                }

                log.Info("La consulta del profesor fue realizada exitosamente");
                return profesor;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }

            }

        }
        
        public Profesor GetProfesorByAsignatura()
        {
            try
            {
                Profesor profesor = new Profesor();
                ConnectToDb();
                comm.CommandText = "SELECT A.idPersona, A.nombre, B.idDepartamento, C.nombre, B.idProfesor FROM persona A join profesor B on A.idPersona = B.idPersona join asignatura C on B.idProfesor = C.idProfesor WHERE C.nombre = 'Calculo'";
                comm.CommandType = System.Data.CommandType.Text;

                conn.Open();

                MySqlDataReader reader = comm.ExecuteReader();

                while (reader.Read())
                {
                    profesor.IdPersona = Convert.ToInt32(reader[0]);
                    profesor.NombreProfesor = Convert.ToString(reader[1]);
                    profesor.IdDepartamento = Convert.ToInt32(reader[2]);
                    profesor.nombreAsig = Convert.ToString(reader[3]);
                    profesor.IdProfesor = Convert.ToInt32(reader[4]);
                }

                log.Info("La consulta del profesor por asignatura fue realizada exitosamente");
                return profesor;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }

            }
        } 
    }

}
