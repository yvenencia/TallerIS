using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfService1
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IService1" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    public interface IService1
    {

        //  AQUI EMPIEZA EL CODIGO DE ANDREA - ALBA - MIGUE
        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json)]
        string GetData(int value);
        
        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json)]
        int InsertarProfesor(int idProfesor, int idPersona, int idDepartamento);
     
        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        int ActualizarProfesor(int idProfesor, int idPersona, int idDepartamento);

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json)]
        int BorrarProfesor(int idProfesor);

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json)]
        Profesor GetProfesor(int idProfesor);

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json)]
        List<Profesor> GetProfesores();

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json)]
        Profesor GetProfesorByAsignatura();
        // TODO: agregue aquí sus operaciones de servicio
    }


    // Utilice un contrato de datos, como se ilustra en el ejemplo siguiente, para agregar tipos compuestos a las operaciones de servicio.
    [DataContract]
    public class Profesor
    {
        int idProfesor;
        int idPersona;
        string nombreAsignatura;
        string nombreProfesor;
        int idDepartamento;

        [DataMember]
        public int IdProfesor
        {
            get { return this.idProfesor; }
            set { idProfesor = value; }
        }

        [DataMember]
        public string nombreAsig
        {
            get { return this.nombreAsignatura; }
            set { nombreAsignatura = value; }
        }

        [DataMember]
        public string NombreProfesor
        {
            get { return this.nombreProfesor; }
            set { nombreProfesor = value; }
        }

        [DataMember]
        public int IdPersona
        {
            get { return idPersona; }
            set { idPersona = value; }
        }

        [DataMember]
        public int IdDepartamento
        {
            get { return idDepartamento; }
            set { idDepartamento = value; }
        }

        //  AQUI TERMINA EL CODIGO DE ANDREA - ALBA - MIGUE


        //  AQUI EMPIEZA EL CODIGO DE RAFA - ALEX - LUIS
        /*[OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string Consulta(string command);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string Operar(string command);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);
 
    }

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
        }*/
        //  AQUI TERMINA EL CODIGO DE RAFA - ALEX - LUIS



    }

}
