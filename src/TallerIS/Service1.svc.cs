using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace TallerIS
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    // System.Data.SqlClient
    public class Service1 : IService1
    {
        static readonly log4net.ILog log = 
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public RespuestaLecturaPersonas ConsultarPersonas()
        {
            RespuestaLecturaPersonas respuesta = new RespuestaLecturaPersonas();
            MySqlConnection conector =  null;
            ListaPersonas lista = null;
            try
            {
                log.Debug("Entrando al metodo: "+ MethodBase.GetCurrentMethod().Name);
                conector = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString);
                conector.Open();
                MySqlCommand comandoSql = conector.CreateCommand();
                comandoSql.CommandText = string.Format("SELECT nombre, apellido1, nif, fecha_nacimiento FROM persona;");
                MySqlDataReader lectorTablaSQL = comandoSql.ExecuteReader();
                lista = new ListaPersonas();
                Persona persona = null;
                while (lectorTablaSQL.Read())
                {
                    persona = new Persona();
                    persona.Nombre = lectorTablaSQL.GetString(0);
                    persona.Apellido = lectorTablaSQL.GetString(1);
                    persona.Nif = lectorTablaSQL.GetString(2);
                    persona.FechaNac = lectorTablaSQL.GetString(3);
                    lista.AgregarPersona(persona);
                }
                respuesta.Status = 200;
                respuesta.Mensaje = "Lectura exitosa";
                respuesta.Listado=lista;                  
            }
            catch (Exception e)
            {
                log.Error("Error en la conexion a base de datos", e);
                respuesta.Status = 500;
                respuesta.Mensaje = "Error de conexion a la base de datos";
                respuesta.Listado = null;
                return respuesta;
            }
            finally
            {
                if (conector != null) {
                    conector.Close();
                    conector.Dispose();
                }
            }
            log.Debug("Saliendo del metodo: " + MethodBase.GetCurrentMethod().Name);
            return respuesta;
        }

        public RespuestaLecturaDepartamentos ConsultarDepartamentos()
        {
            RespuestaLecturaDepartamentos respuesta = new RespuestaLecturaDepartamentos();
            MySqlConnection conector = null;
            ListaDepartamentos lista = null;
            try
            {
                log.Debug("Entrando al metodo: " + MethodBase.GetCurrentMethod().Name);
                conector = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString);
                conector.Open();
                MySqlCommand comandoSql = conector.CreateCommand();
                comandoSql.CommandText = string.Format("SELECT * FROM departamento;");
                MySqlDataReader lectorTablaSQL = comandoSql.ExecuteReader();
                lista = new ListaDepartamentos();
                Departamento departamento = null;
                while (lectorTablaSQL.Read())
                {
                    departamento = new Departamento();
                    departamento.Id = Convert.ToInt32(lectorTablaSQL.GetString(0));
                    departamento.Nombre = lectorTablaSQL.GetString(1);
                    lista.AgregarDepartamento(departamento);
                }
                respuesta.Status = 200;
                respuesta.Mensaje = "Lectura exitosa";
                respuesta.Listado = lista;
            }
            catch (Exception e)
            {
                log.Error("Error en la conexion a base de datos", e);
                respuesta.Status = 500;
                respuesta.Mensaje = "Error de conexion a la base de datos";
                respuesta.Listado = null;
                return respuesta;
            }
            finally
            {
                if (conector != null)
                {
                    conector.Close();
                    conector.Dispose();
                }
            }
            log.Debug("Saliendo del metodo: " + MethodBase.GetCurrentMethod().Name);
            return respuesta;
        }

        public RespuestaEscritura AgregarDepartamento(String nombre, float creditos, int curso, int cuatrimestre, int idGrado, int idProfesor, int idTipoAsignatura)
        {
            RespuestaEscritura respuesta = new RespuestaEscritura();
            MySqlConnection conector = null;
            try
            {
                log.Debug("Entrando al metodo: " + MethodBase.GetCurrentMethod().Name);
                conector = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString);
                conector.Open();
                MySqlCommand comandoSql = conector.CreateCommand();
                comandoSql.CommandText = string.Format("INSERT INTO asignatura (nombre, creditos, curso, cuatrimestre, idGrado, idProfesor, idTipoAsignatura) VALUES (@0, @1, @2, @3, @4, @5, @6);");
                comandoSql.Parameters.Add(new MySqlParameter("0", nombre ));
                comandoSql.Parameters.Add(new MySqlParameter("1", creditos));
                comandoSql.Parameters.Add(new MySqlParameter("2", curso));
                comandoSql.Parameters.Add(new MySqlParameter("3", cuatrimestre));
                comandoSql.Parameters.Add(new MySqlParameter("4", idGrado));
                comandoSql.Parameters.Add(new MySqlParameter("5", idProfesor));
                comandoSql.Parameters.Add(new MySqlParameter("6", idTipoAsignatura));
              
                var verificador = comandoSql.ExecuteNonQuery();
                respuesta.Status = 200;
                respuesta.Afectados = verificador;
                respuesta.Mensaje = "Insert a la base de datos exitoso!";
            }
            catch (Exception e)
            {
                log.Error("Error en la conexion a base de datos", e);
                respuesta.Status = 500;
                respuesta.Mensaje = "Error en la conexion a base de datos";
            }
            finally
            {
                if (conector != null)
                {
                    conector.Close();
                    conector.Dispose();
                }
            }
            log.Debug("Saliendo del metodo: " + MethodBase.GetCurrentMethod().Name);
            return respuesta;
        }

        public RespuestaLecturaProfesores ConsultarProfesores()
        {
            RespuestaLecturaProfesores respuesta = new RespuestaLecturaProfesores();
            MySqlConnection conector = null;
            Profesores profesor;
            ListaProfesores lista;
            try
            {
                log.Debug("Entrando al metodo: " + MethodBase.GetCurrentMethod().Name);
                conector = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString);
                conector.Open();
                MySqlCommand comandoSql = conector.CreateCommand();
                comandoSql.CommandText = string.Format("SELECT A.idPersona, A.nombre, B.nombre FROM persona as A join profesor as C on A.idPersona = C.idPersona join asignatura as B on B.idProfesor = C.idProfesor;");
                var verificador = comandoSql.ExecuteNonQuery();
                MySqlDataReader lectorTablaSQL = comandoSql.ExecuteReader();
                lista = new ListaProfesores();
                Departamento departamento = null;
                while (lectorTablaSQL.Read())
                {
                    profesor = new Profesores();
                    profesor.Id = Convert.ToInt32(lectorTablaSQL.GetString(0));
                    profesor.Nombre = lectorTablaSQL.GetString(1);
                    profesor.Asignatura = lectorTablaSQL.GetString(2);
                    lista.AgregarProfesor(profesor);
                }
                respuesta.Mensaje = "Lectura exitosa";
                respuesta.Listado = lista;
                respuesta.Status = 200;
                //respuesta.Afectados = verificador;
            }
            catch (Exception e)
            {
                log.Error("Error en la conexion a base de datos", e);
                respuesta.Status = 500;
                respuesta.Mensaje = "Error en la conexion a base de datos";
            }
            finally
            {
                if (conector != null)
                {
                    conector.Close();
                    conector.Dispose();
                }
            }
            log.Debug("Saliendo del metodo: " + MethodBase.GetCurrentMethod().Name);
            return respuesta;
        }

        public RespuestaEscritura ModificarDepartamento(int idDepartamento, string nuevo)
        {
            RespuestaEscritura respuesta = new RespuestaEscritura();
            MySqlConnection conector = null;
            try
            {
                log.Debug("Entrando al metodo: " + MethodBase.GetCurrentMethod().Name);
                conector = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString);
                conector.Open();
                MySqlCommand comandoSql = conector.CreateCommand();
                comandoSql.CommandText = string.Format("UPDATE departamento SET nombre = @0 WHERE idDepartamento = @1;");
                comandoSql.Parameters.Add(new MySqlParameter("0", nuevo));
                comandoSql.Parameters.Add(new MySqlParameter("1", idDepartamento));
                int verificador = comandoSql.ExecuteNonQuery();
                respuesta.Status = 200;
                respuesta.Afectados = verificador;
                if (verificador != 0){
                    respuesta.Mensaje = "Modificacion a la base de datos exitosa!";
                }else{
                    respuesta.Mensaje = "Departamento inexitente";
                }
            }
            catch (Exception e)
            {
                log.Error("Error en la conexion a base de datos", e);
                respuesta.Status = 500;
                respuesta.Mensaje = "Error en la conexion a base de datos";
            }
            finally
            {
                if (conector != null)
                {
                    conector.Close();
                    conector.Dispose();
                }
            }
            log.Debug("Saliendo del metodo: " + MethodBase.GetCurrentMethod().Name);
            return respuesta;
        }

        public string modificarGrupo7(int value)
        {
            string la = "";
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE persona p " +
                "JOIN profesor f " +
                "ON p.idPersona = f.idPersona " +
                "JOIN asignatura a " +
                "ON a.idProfesor = f.idProfesor " +
                "set p.nombre = 'Diego' " +
                "where a.idAsignatura = 2";
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                la = la + dr["idPersona"] + "  " + dr["nombre"] + " - ";
            }
            dr.Close();
            return la;
        }

        public string modificarGrupo7(int value)
        {
            string la = "";
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE persona p " +
                "JOIN profesor f " +
                "ON p.idPersona = f.idPersona " +
                "JOIN asignatura a " +
                "ON a.idProfesor = f.idProfesor " +
                "set p.nombre = 'Diego' " +
                "where a.idAsignatura = 2";
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                la = la + dr["idPersona"] + "  " + dr["nombre"] + " - ";
            }
            dr.Close();
            return la;
        }

        public RespuestaEscritura EliminarDepartamento(int idDepartamento)
        {
            RespuestaEscritura respuesta = new RespuestaEscritura();
            MySqlConnection conector = null;
            try
            {
                log.Debug("Entrando al metodo: " + MethodBase.GetCurrentMethod().Name);
                conector = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString);
                conector.Open();
                MySqlCommand comandoSql = conector.CreateCommand();
                comandoSql.CommandText = string.Format("DELETE FROM departamento WHERE idDepartamento = @0;");
                comandoSql.Parameters.Add(new MySqlParameter("0", idDepartamento));
                int verificador = comandoSql.ExecuteNonQuery();
                respuesta.Status = 200;
                respuesta.Afectados = verificador;
                respuesta.Mensaje = "Delete a la base de datos exitoso!";
            }
            catch (Exception e)
            {
                log.Error("Error en la conexion a base de datos", e);
                respuesta.Status = 500;
                respuesta.Mensaje = "Error en la conexion a base de datos";
            }
            finally
            {
                if (conector != null)
                {
                    conector.Close();
                    conector.Dispose();
                }
            }
            log.Debug("Saliendo del metodo: " + MethodBase.GetCurrentMethod().Name);
            return respuesta;
        }
    }
}
