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

        MySqlConnection conn;
        private static readonly ILog logger = LogManager.GetLogger(typeof(CacheSetting));
        public Service1() {
            logger.Debug("Se inicio Service1");

            //TATO -> CREANDO CONEXION CON BASE DE DATOS
            /*
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = "127.0.0.1";
            builder.Port = 3306;
            builder.UserID = "root";
            builder.Password = "tato1598";
            builder.Database = "practica1";

            conn = new MySqlConnection(builder.ToString());
            */

            string ConnectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            conn = new MySqlConnection(ConnectionString);

        }

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


        //METODO A BD
        public Persona ConsultarPersonaPorId(int id) {
            logger.Debug("Se empezo a ejecutar ConsultarPersonaPorId()");
            Persona persona = new Persona();
            try {
                MySqlCommand command = conn.CreateCommand();
                command.CommandText = "SELECT * FROM PERSONA  WHERE idPersona=@idPersona";
                command.Parameters.AddWithValue("idPersona", id);
                command.CommandType = CommandType.Text;

                conn.Open();

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read()) {
                    //otras dos formas de acceder
                    //persona.IdPersona = reader.GetString(0);
                    //persona.IdPersona = reader["idPersona"];
                    persona.IdPersona = Convert.ToInt32(reader[0]);
                    persona.Nif = Convert.ToString(reader[1]);
                    persona.Nombre = Convert.ToString(reader[2]);
                    persona.Apellido1 = Convert.ToString(reader[3]);
                    persona.Apellido2 = Convert.ToString(reader[4]);
                    persona.Direccion = Convert.ToString(reader[5]);
                    persona.Telefono = Convert.ToString(reader[6]);
                    persona.Fecha_nacimiento = Convert.ToString(reader[7]);
                    persona.IdGenero = Convert.ToInt32(reader[8]);
                    persona.IdTipoPersona = Convert.ToInt32(reader[9]);
                }
                return persona;
            } catch (MySqlException ex) {
                logger.Error("MySqlException catched: " + ex.Message);
                throw new WebFaultException<string>(ex.Message, HttpStatusCode.InternalServerError);
            } finally {
                if (conn != null)
                    conn.Close();
                logger.Debug("Se termino de ejecutar ConsultarPersonaPorId()");
            }

        }

        public ListaProfesor ConsultarProfesores() {
            logger.Debug("Se empezo a ejecutar ConsultarProfesores()");

            try {
                MySqlCommand command = conn.CreateCommand();
                command.CommandText = "select A.nombre, C.nombre from persona A join profesor B on B.idPersona = A.idPersona join asignatura C on C.idProfesor = B.idProfesor";
                command.CommandType = CommandType.Text;

                conn.Open();

                ListaProfesor listaPersona = new ListaProfesor();

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read()) {
                    Profesor profesor = new Profesor();

                    profesor.nombre_profesor = Convert.ToString(reader[0]);
                    profesor.nombre_asignatura = Convert.ToString(reader[1]);

                    listaPersona.agregarProfesor(profesor);
                }

                reader.Dispose();

                return listaPersona;
            } catch (Exception ex) {
                logger.Error("MySqlException catched: " + ex.Message);
                throw new WebFaultException<string>(ex.Message, HttpStatusCode.InternalServerError);
            } finally {
                if (conn != null)
                    conn.Close();
                logger.Debug("Se termino de ejecutar ConsultarProfesores()");
            }
        }

        public ListaPersona ConsultarPersonas() {
            logger.Debug("Se empezo a ejecutar ConsultarPersonas()");

            try {
                MySqlCommand command = conn.CreateCommand();
                command.CommandText = "SELECT * FROM PERSONA";
                command.CommandType = CommandType.Text;

                conn.Open();

                ListaPersona listaPersona = new ListaPersona();

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read()) {
                    Persona persona = new Persona();

                    persona.IdPersona = Convert.ToInt32(reader[0]);
                    persona.Nif = Convert.ToString(reader[1]);
                    persona.Nombre = Convert.ToString(reader[2]);
                    persona.Apellido1 = Convert.ToString(reader[3]);
                    persona.Apellido2 = Convert.ToString(reader[4]);
                    persona.Direccion = Convert.ToString(reader[5]);
                    persona.Telefono = Convert.ToString(reader[6]);
                    persona.Fecha_nacimiento = Convert.ToString(reader[7]);
                    persona.IdGenero = Convert.ToInt32(reader[8]);
                    persona.IdTipoPersona = Convert.ToInt32(reader[9]);

                    listaPersona.agregarPersona(persona);
                }

                reader.Dispose();

                return listaPersona;
            } catch (Exception ex) {
                logger.Error("MySqlException catched: " + ex.Message);
                throw new WebFaultException<string>(ex.Message, HttpStatusCode.InternalServerError);
            } finally {
                if (conn != null)
                    conn.Close();
                logger.Debug("Se termino de ejecutar ConsultarPersonas()");
            }
        }


        public Respuesta InsertarPersona(Persona p) {
            logger.Debug("Se empezo a ejecutar InsertarPersona()");

            try {

                MySqlCommand command = conn.CreateCommand();

                command.CommandText = "INSERT INTO persona VALUES (0" + ",'" + p.Nif + "','" + p.Nombre + "','" + p.Apellido1 + "','" + p.Apellido2 + "','" + p.Direccion + "','" + p.Telefono + "','" + p.Fecha_nacimiento + "'," + p.IdGenero.ToString() + "," + p.IdTipoPersona.ToString() + ")";

                command.CommandType = CommandType.Text;

                conn.Open();

                command.ExecuteNonQuery();

                return new Respuesta("Se ha insertado persona correctamente", "OKAY", null);
            } catch (MySqlException ex) {
                logger.Error("MySqlException catched: " + ex.Message);
                return new Respuesta(ex.Message, "ERROR", null);
            } finally {
                if (conn != null)
                    conn.Close();
                logger.Debug("Se termino de ejecutar InsertarPersona()");
            }

        }

        public Respuesta EliminarPersonaPorId(int id) {
            logger.Debug("Se empezo a ejecutar EliminarPersonaPorId()");

            try {
                MySqlCommand command = conn.CreateCommand();

                command.CommandText = "DELETE FROM persona WHERE idPersona=" + id.ToString();
                command.CommandType = CommandType.Text;

                conn.Open();


                if (command.ExecuteNonQuery() > 0)
                    return new Respuesta("Se ha eliminado persona correctamente", "OKAY", null);
                else
                    return new Respuesta("Persona ya habia sido eliminada previamente", "OKAY", null);
            } catch (MySqlException ex) {
                logger.Error("MySqlException catched: " + ex.Message);
                return new Respuesta(ex.Message, "ERROR", null);
            } finally {
                if (conn != null)
                    conn.Close();
                logger.Debug("Se termino de ejecutar EliminarPersonaPorId()");
            }

        }

        public Respuesta ActualizarNombreProfesoresEstadistica(string nombre) {
            logger.Debug("Se empezo a ejecutar ActualizarNombreProfesoresEstadistica()");

            try {
                MySqlCommand command = conn.CreateCommand();

                command.CommandText = "update persona, profesor, asignatura set persona.nombre = @nombre where asignatura.idprofesor = profesor.idprofesor and profesor.idpersona = persona.idpersona and asignatura.nombre = 'Estadística'";
                command.Parameters.AddWithValue("nombre", nombre);
                command.CommandType = CommandType.Text;

                conn.Open();

                if (command.ExecuteNonQuery() > 0)
                    return new Respuesta("Se ha modificado profesores correctamente", "OKAY", null);
                else
                    return new Respuesta("Ya ha sido modificado", "ERROR", null);
            } catch (MySqlException ex) {
                logger.Error("MySqlException catched: " + ex.Message);
                return new Respuesta(ex.Message, "ERROR", null);
            } finally {
                if (conn != null)
                    conn.Close();
                logger.Debug("Se termino de ejecutar ActualizarNombreProfesoresEstadistica()");
            }
        }

        public Respuesta ActualizarPersonaApellido1PorId(int id, string nuevoApellido1) {
            logger.Debug("Se empezo a ejecutar ActualizarPersonaApellido1PorId()");

            try {
                MySqlCommand command = conn.CreateCommand();

                command.CommandText = "UPDATE persona SET apellido1=@apellido1 WHERE idPersona=@id";
                command.Parameters.AddWithValue("apellido1", nuevoApellido1);
                command.Parameters.AddWithValue("id", id);
                command.CommandType = CommandType.Text;

                conn.Open();

                if (command.ExecuteNonQuery() > 0)
                    return new Respuesta("Se ha modificado persona correctamente", "OKAY", null);
                else
                    return new Respuesta("Ya ha sido modificado o no existe id solicitado", "ERROR", null);
            } catch (MySqlException ex) {
                logger.Error("MySqlException catched: " + ex.Message);
                return new Respuesta(ex.Message, "ERROR", null);
            } finally {
                if (conn != null)
                    conn.Close();
                logger.Debug("Se termino de ejecutar ActualizarPersonaApellido1PorId()");
            }

        }

        /*
        public Respuesta InsertarAsignatura(Asignatura a) {
            logger.Debug("Se empezo a ejecutar InsertarAsignatura()");

            try {

                MySqlCommand command = conn.CreateCommand();

                command.CommandText = "INSERT INTO persona VALUES (0" + ",'" + a.idAsignatura + "','" + a.nombre + "','" + a.creditos + "','" + a.curso + "','" + a.cuatrimestre + "','" + a.idGrado + "','" + a.idProfesor + "'," + a.idTipoAsignatura ")";

                command.CommandType = CommandType.Text;

                conn.Open();

                command.ExecuteNonQuery();

                return new Respuesta("Se ha insertado persona correctamente", "OKAY", null);
            } catch (MySqlException ex) {
                logger.Error("MySqlException catched: " + ex.Message);
                return new Respuesta(ex.Message, "ERROR", null);
            } finally {
                if (conn != null)
                    conn.Close();
                logger.Debug("Se termino de ejecutar InsertarPersona()");
            }
        }
        */

    }
}
}
