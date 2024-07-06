using CoreAccesLayer.Interface;
using Iza.Core.DBEntities;
using PlumbingProps.Config;
using PlumbingProps.Exceptions;
using PlumbingProps.Wrapper;


namespace Iza.Core.Base
{
    public abstract class BaseManager
    {
        /// <summary>
        /// se genera conla siguiente linea de codigo
        /// dotnet tool install --global dotnet-ef
        /// dotnet ef dbcontext scaffold "Data Source=155.138.212.216;Initial Catalog=DBInvoiceSwitch;Persist Security Info=True;User ID=sa;Password=m1k1ches*123;TrustServerCertificate=True" "Microsoft.EntityFrameworkCore.SqlServer" -o DBEntities -f 
        /// dotnet ef dbcontext scaffold "Name=FacturacionBD" "Microsoft.EntityFrameworkCore.SqlServer" -o Data -f  
        /// -----------------para crear BD desde el modelo ejecutar:
        /// dotnet ef migrations add InitialCreate
        /// dotnet ef database update
        /// </summary>
        internal IRepository repositoryPub { get; set; }// = FactoryDataInterfaz.CreateRepository<FacturacionBdContext>("sqlserver");
        public BaseManager()
        {
            string cnxString = ConfigManager.GetConfiguration().GetSection("ConnectionStrings:InvoiceDB").Value!;
            repositoryPub = FactoryDataInterfaz.CreateRepository<DbpubIzaContext>("sqlserver", cnxString);
        }

        public string ProcessError(Exception ex)
        {
            ManagerException managerException = new ManagerException();
            repositoryPub?.Rollback();
            return managerException.ProcessException(ex);
        }

        public string ProcessError(Exception ex, Response response)
        {
            ManagerException managerException = new ManagerException();
            response.State = ResponseType.Error;
            response.Message = managerException.ProcessException(ex);
            repositoryPub?.Rollback();
            return response.Message;
        }
    }

}
