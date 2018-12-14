using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace TallerIS
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        [WebInvoke(Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "api/getData")]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        [OperationContract]
        [WebInvoke(Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "api/consultarPersonas")]
        RespuestaLecturaPersonas ConsultarPersonas();

        [OperationContract]
        [WebInvoke(Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "api/consultarProfesores")]
        RespuestaLecturaProfesores ConsultarProfesores();

        [OperationContract]
        [WebInvoke(Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "api/consultarDepartamentos")]
        RespuestaLecturaDepartamentos ConsultarDepartamentos();

        [OperationContract]
        [WebInvoke(Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "api/agregarDepartamento")]
        RespuestaEscritura AgregarDepartamento(String nombre, float creditos, int curso, int cuatrimestre, int idGrado, int idProfesor, int idTipoAsignatura);
        // TODO: Add your service operations here

        [OperationContract]
        [WebInvoke(Method = "PUT",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "api/modificarDepartamento")]
        RespuestaEscritura ModificarDepartamento(int idDepartamento, string nuevo);

        [OperationContract]
        [WebInvoke(Method = "DELETE",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "api/eliminarDepartamento")]
        RespuestaEscritura EliminarDepartamento(int idDepartamento);
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
    
    [DataContract]
    public abstract class Entidad { }

    [DataContract]
    public class Persona : Entidad {
        string nombre;
        string apellido;
        string nif;
        string fechaNac;

        [DataMember]
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        [DataMember]
        public string Apellido
        {
            get { return apellido; }
            set { apellido = value; }
        }

        [DataMember]
        public string Nif
        {
            get { return nif; }
            set { nif = value; }
        }

        [DataMember]
        public string FechaNac
        {
            get { return fechaNac; }
            set { fechaNac = value; }
        }
    }

    [DataContract]
    public class Departamento : Entidad
    {
        int id;
        string nombre;

        [DataMember]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [DataMember]
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }
    }

    [DataContract]
    public class Profesores : Entidad
    {
        int id;
        string nombre;
        string asignatura;

        [DataMember]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [DataMember]
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        [DataMember]
        public string Asignatura
        {
            get { return asignatura; }
            set { asignatura = value; }
        }
    }

    [DataContract]
    public class ListaPersonas : Entidad {
        List<Persona> lista;

        public ListaPersonas() {
            lista = new List<Persona>();
        }
        
        public void AgregarPersona(Persona persona) {
            this.lista.Add(persona);
        }

        [DataMember]
        public List<Persona> Lista
        {
            get { return lista; }
            set { lista = value; }
        }

    }

    [DataContract]
    public class ListaDepartamentos : Entidad
    {
        List<Departamento> lista;

        public ListaDepartamentos()
        {
            lista = new List<Departamento>();
        }

        public void AgregarDepartamento(Departamento departamento)
        {
            this.lista.Add(departamento);
        }

        [DataMember]
        public List<Departamento> Lista
        {
            get { return lista; }
            set { lista = value; }
        }

    }

    [DataContract]
    public abstract class RespuestaGenerica{
        int status;
        string mensaje;

        [DataMember]
        public int Status
        {
            get { return status; }
            set { status = value; }
        }

        [DataMember]
        public string Mensaje
        {
            get { return mensaje; }
            set { mensaje = value; }
        }
    }

    [DataContract]
    public class RespuestaLecturaPersonas : RespuestaGenerica {

        ListaPersonas listado;

        [DataMember]
        public ListaPersonas Listado
        {
            get { return listado; }
            set { listado = value; }
        }
    }

    [DataContract]
    public class RespuestaLecturaDepartamentos : RespuestaGenerica
    {

        ListaDepartamentos listado;

        [DataMember]
        public ListaDepartamentos Listado
        {
            get { return listado; }
            set { listado = value; }
        }
    }

    [DataContract]
    public class ListaProfesores : Entidad
    {
        List<Profesores> lista;

        public ListaProfesores()
        {
            lista = new List<Profesores>();
        }

        public void AgregarProfesor(Profesores profesor)
        {
            this.lista.Add(profesor);
        }

        [DataMember]
        public List<Profesores> Lista
        {
            get { return lista; }
            set { lista = value; }
        }

    }

    [DataContract]
    public class RespuestaLecturaProfesores : RespuestaGenerica
    {

        ListaProfesores listado;

        [DataMember]
        public ListaProfesores Listado
        {
            get { return listado; }
            set { listado = value; }
        }
    }

    [DataContract]
    public class RespuestaEscritura : RespuestaGenerica
    {
        int afectados;

        [DataMember]
        public int Afectados
        {
            get { return afectados; }
            set { afectados = value; }
        }
    }
}
