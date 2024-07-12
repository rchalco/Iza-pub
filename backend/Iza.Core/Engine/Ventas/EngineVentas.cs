using CoreAccesLayer.Implement.SQLServer;
using Iza.Core.Base;
using Iza.Core.Domain.Venta.Caja;
using PlumbingProps.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iza.Core.Engine.Ventas
{
    public class EngineVentas: BaseManager
    {

        #region Cajas

        public ResponseObject<AperturaCajaResponse> AperturaCaja(SaldoCajaDTO requestAperturaCajae)
        {
            ParamOut poRespuesta = new ParamOut(0);
            ParamOut poLogRespuesta = new ParamOut("");
            poLogRespuesta.Size = 100;
            ResponseObject<AperturaCajaResponse> response = new ResponseObject<AperturaCajaResponse> { Message = "La caja se aperturo correctamente", State = ResponseType.Success };
            try
            {
                response.Data = new AperturaCajaResponse();
                ParamOut idFechaProceso = new ParamOut(0);
                ParamOut FechaProceso = new ParamOut(DateTime.Now);
                ParamOut idOperacionDiariaCaja = new ParamOut(0);
                //validamos que no exita una caja abierta en otra fecha 
                repositoryPub.CallProcedure<AperturaCajaResponse>("shFabula.spAperturaCaja",
                        requestAperturaCajae.idSesion,
                        requestAperturaCajae.idCaja,
                        requestAperturaCajae.idAlmacen,
                        requestAperturaCajae.SaldoInicial,
                        requestAperturaCajae.Observacion,
                        idOperacionDiariaCaja,
                        idFechaProceso,
                        FechaProceso,
                        poRespuesta,
                        poLogRespuesta);

                repositoryPub.Commit();

                if (Convert.ToInt32(poRespuesta.Valor) != 0)
                {
                    response.Message = poLogRespuesta.Valor.ToString();
                    response.State = ResponseType.Error;
                    return response;
                }
                response.Message = poLogRespuesta.Valor.ToString();
                response.Data.idFechaProceso = Convert.ToInt64(idFechaProceso.Valor);
                response.Data.FechaProceso = Convert.ToDateTime(FechaProceso.Valor);
                response.Data.idOperacionDiaria = Convert.ToInt32(idOperacionDiariaCaja.Valor);
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }

        public ResponseObject<SaldoCajaDTO> CierreCaja(SaldoCajaDTO requestAperturaCaja)
        {
            ParamOut poRespuesta = new ParamOut(false);
            ParamOut poLogRespuesta = new ParamOut("");
            ResponseObject<SaldoCajaDTO> response = new ResponseObject<SaldoCajaDTO> { Message = "¨La caja se cerro correctamente", State = ResponseType.Success };
            try
            {

                response.Data = repositoryPub.GetDataByProcedure<SaldoCajaDTO>("shFabula.spCierreCaja", requestAperturaCaja.idSesion, requestAperturaCaja.idCaja, requestAperturaCaja.SaldoUsuario, requestAperturaCaja.Observacion, poRespuesta, poLogRespuesta).FirstOrDefault();
                if (response.Data == null)
                {
                    response.State = ResponseType.Error;
                    response.Message = "Exisitio un error al cerrar la caja " + requestAperturaCaja.idOperacionDiariaCaja.ToString();
                }
                if ((bool)poRespuesta.Valor)
                {
                    response.Message = poLogRespuesta.Valor.ToString();
                    response.State = ResponseType.Error;
                    return response;
                }
            }
            catch (Exception ex)
            {
                ProcessError(ex, response);
            }
            return response;
        }

        #endregion
    }
}
