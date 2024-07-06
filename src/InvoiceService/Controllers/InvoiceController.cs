using Invoice.Core.Business.InvoiceKind.BasicExpenses;
using Invoice.Core.Business.InvoiceKind.CurrencyExchange;
using Invoice.Core.Business.InvoiceKind.Financial;
using Invoice.Core.Business.InvoiceKind.Orders;
using Invoice.Core.Business.Office;
using Invoice.Core.Domain;
using Invoice.Core.Engines;
using InvoiceService.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlumbingProps.Wrapper;
using System.Text.Json;

namespace InvoiceService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        [HttpGet("index")]
        public string index()
        {
            return "Servicio levantado exitosamente";
        }

        [HttpPost("GetInvoiceOrder")]
        [EnableCors()]
        public ResponseObject<ResulInvoice> GetInvoiceOrder(RequestCompraVenta requestCompraVenta)
        {
            ResponseObject<ResulInvoice> response = new ResponseObject<ResulInvoice>();
            if (!Request.Headers.Keys.Contains("contextCompany"))
            {
                response.State = ResponseType.Warning;
                response.Message = "Debe enviar la cabecera contextCompany";
                return response;
            }

            string contextCompanyJSON = Request.Headers["contextCompany"]!;
            ContextCompany contextCompany = JsonSerializer.Deserialize<ContextCompany>(contextCompanyJSON)!;
            EngineInvoiceSender engineInvoiceSender = new EngineInvoiceSender(contextCompany);
            response = engineInvoiceSender.GetInvoice(requestCompraVenta);
            if (response.State == ResponseType.Success)
            {
                response.Message = string.Empty;
            }
            return response;
        }

        [HttpPost("AnularInvoiceOrder")]
        [EnableCors()]
        public Response AnularInvoiceOrder(RequestAnularFacturaCompraVenta requestAnularFactura)
        {
            Response response = new Response();
            if (!Request.Headers.Keys.Contains("contextCompany"))
            {
                response.State = ResponseType.Warning;
                response.Message = "Debe enviar la cabecera contextCompany";
                return response;
            }

            string contextCompanyJSON = Request.Headers["contextCompany"]!;
            ContextCompany contextCompany = JsonSerializer.Deserialize<ContextCompany>(contextCompanyJSON)!;
            EngineInvoiceSender engineInvoiceSender = new EngineInvoiceSender(contextCompany);
            response = engineInvoiceSender.AnularFactura(requestAnularFactura.IdInvoice, requestAnularFactura.Motivo);
            if (response.State == ResponseType.Success)
            {
                response.Message = string.Empty;
            }
            return response;
        }

        [HttpPost("RevertirInvoiceOrder")]
        [EnableCors()]
        public Response RevertirInvoiceOrder(RequestAnularFacturaCompraVenta requestAnularFactura)
        {
            Response response = new Response();
            if (!Request.Headers.Keys.Contains("contextCompany"))
            {
                response.State = ResponseType.Warning;
                response.Message = "Debe enviar la cabecera contextCompany";
                return response;
            }

            string contextCompanyJSON = Request.Headers["contextCompany"]!;
            ContextCompany contextCompany = JsonSerializer.Deserialize<ContextCompany>(contextCompanyJSON)!;
            EngineInvoiceSender engineInvoiceSender = new EngineInvoiceSender(contextCompany);
            response = engineInvoiceSender.RevertirFactura(requestAnularFactura.IdInvoice);
            if (response.State == ResponseType.Success)
            {
                response.Message = string.Empty;
            }
            return response;
        }



        [HttpPost("GetInvoicBasicService")]
        [EnableCors()]
        public ResponseObject<ResulInvoice> GetInvoicBasicService(RequestServicioBasico requestServicioBasico)
        {
            ResponseObject<ResulInvoice> response = new ResponseObject<ResulInvoice>();
            if (!Request.Headers.Keys.Contains("contextCompany"))
            {
                response.State = ResponseType.Warning;
                response.Message = "Debe enviar la cabecera contextCompany";
                return response;
            }

            string contextCompanyJSON = Request.Headers["contextCompany"]!;
            ContextCompany contextCompany = JsonSerializer.Deserialize<ContextCompany>(contextCompanyJSON)!;
            EngineInvoiceSender engineInvoiceSender = new EngineInvoiceSender(contextCompany);
            response = engineInvoiceSender.GetInvoice(requestServicioBasico);
            if (response.State == ResponseType.Success)
            {
                response.Message = string.Empty;
            }
            return response;
        }

        [HttpPost("AnularInvoicServicioBasico")]
        [EnableCors()]
        public Response AnularInvoicServicioBasico(RequestAnularFacturaServicioBasico requestAnularFacturaServicioBasico)
        {
            Response response = new Response();
            if (!Request.Headers.Keys.Contains("contextCompany"))
            {
                response.State = ResponseType.Warning;
                response.Message = "Debe enviar la cabecera contextCompany";
                return response;
            }

            string contextCompanyJSON = Request.Headers["contextCompany"]!;
            ContextCompany contextCompany = JsonSerializer.Deserialize<ContextCompany>(contextCompanyJSON)!;
            EngineInvoiceSender engineInvoiceSender = new EngineInvoiceSender(contextCompany);
            response = engineInvoiceSender.AnularFactura(requestAnularFacturaServicioBasico.IdInvoice, requestAnularFacturaServicioBasico.Motivo);
            if (response.State == ResponseType.Success)
            {
                response.Message = string.Empty;
            }
            return response;
        }

        [HttpPost("RevertirServicioBasico")]
        [EnableCors()]
        public Response RevertirServicioBasico(RequestAnularFacturaServicioBasico requestAnularFactura)
        {
            Response response = new Response();
            if (!Request.Headers.Keys.Contains("contextCompany"))
            {
                response.State = ResponseType.Warning;
                response.Message = "Debe enviar la cabecera contextCompany";
                return response;
            }

            string contextCompanyJSON = Request.Headers["contextCompany"]!;
            ContextCompany contextCompany = JsonSerializer.Deserialize<ContextCompany>(contextCompanyJSON)!;
            EngineInvoiceSender engineInvoiceSender = new EngineInvoiceSender(contextCompany);
            response = engineInvoiceSender.RevertirFactura(requestAnularFactura.IdInvoice);
            if (response.State == ResponseType.Success)
            {
                response.Message = string.Empty;
            }
            return response;
        }


        [HttpPost("GetInvoicFinanciero")]
        [EnableCors()]
        public ResponseObject<ResulInvoice> GetInvoicFinanciero(RequestEntidadFinanciera requestEntidadFinanciera)
        {
            ResponseObject<ResulInvoice> response = new ResponseObject<ResulInvoice>();
            if (!Request.Headers.Keys.Contains("contextCompany"))
            {
                response.State = ResponseType.Warning;
                response.Message = "Debe enviar la cabecera contextCompany";
                return response;
            }

            string contextCompanyJSON = Request.Headers["contextCompany"]!;
            ContextCompany contextCompany = JsonSerializer.Deserialize<ContextCompany>(contextCompanyJSON)!;
            EngineInvoiceSender engineInvoiceSender = new EngineInvoiceSender(contextCompany);
            response = engineInvoiceSender.GetInvoice(requestEntidadFinanciera);
            if (response.State == ResponseType.Success)
            {
                response.Message = string.Empty;
            }
            return response;
        }

        [HttpPost("AnularInvoicFinanciero")]
        [EnableCors()]
        public Response AnularInvoicFinanciero(RequestAnularFacturaFinanciero requestAnularFacturaFinanciero)
        {
            Response response = new Response();
            if (!Request.Headers.Keys.Contains("contextCompany"))
            {
                response.State = ResponseType.Warning;
                response.Message = "Debe enviar la cabecera contextCompany";
                return response;
            }

            string contextCompanyJSON = Request.Headers["contextCompany"]!;
            ContextCompany contextCompany = JsonSerializer.Deserialize<ContextCompany>(contextCompanyJSON)!;
            EngineInvoiceSender engineInvoiceSender = new EngineInvoiceSender(contextCompany);
            response = engineInvoiceSender.AnularFactura(requestAnularFacturaFinanciero.IdInvoice, requestAnularFacturaFinanciero.Motivo);
            if (response.State == ResponseType.Success)
            {
                response.Message = string.Empty;
            }
            return response;
        }

        [HttpPost("RevertirInvoiceFinanciero")]
        [EnableCors()]
        public Response RevertirInvoiceFinanciero(RequestAnularFacturaFinanciero requestAnularFactura)
        {
            Response response = new Response();
            if (!Request.Headers.Keys.Contains("contextCompany"))
            {
                response.State = ResponseType.Warning;
                response.Message = "Debe enviar la cabecera contextCompany";
                return response;
            }

            string contextCompanyJSON = Request.Headers["contextCompany"]!;
            ContextCompany contextCompany = JsonSerializer.Deserialize<ContextCompany>(contextCompanyJSON)!;
            EngineInvoiceSender engineInvoiceSender = new EngineInvoiceSender(contextCompany);
            response = engineInvoiceSender.RevertirFactura(requestAnularFactura.IdInvoice);
            if (response.State == ResponseType.Success)
            {
                response.Message = string.Empty;
            }
            return response;
        }


        [HttpPost("RegisterBatchInvoice")]
        [EnableCors()]
        public Response RegisterBatchInvoice()
        {
            Response response = new Response();
            if (!Request.Headers.Keys.Contains("contextCompany"))
            {
                response.State = ResponseType.Warning;
                response.Message = "Debe enviar la cabecera contextCompany";
                return response;
            }

            string contextCompanyJSON = Request.Headers["contextCompany"]!;
            ContextCompany contextCompany = JsonSerializer.Deserialize<ContextCompany>(contextCompanyJSON)!;
            EngineBatchInvoiceSender engineBatchInvoiceSender = new EngineBatchInvoiceSender(contextCompany);
            response = engineBatchInvoiceSender.RegisterBatchInvoice();
            return response;
        }

        [HttpPost("GetInvoicCambioMoneda")]
        [EnableCors()]
        public ResponseObject<ResulInvoice> GetInvoicCambioMoneda(RequestCambioMoneda requestCambioMoneda)
        {
            ResponseObject<ResulInvoice> response = new ResponseObject<ResulInvoice>();
            if (!Request.Headers.Keys.Contains("contextCompany"))
            {
                response.State = ResponseType.Warning;
                response.Message = "Debe enviar la cabecera contextCompany";
                return response;
            }

            string contextCompanyJSON = Request.Headers["contextCompany"]!;
            ContextCompany contextCompany = JsonSerializer.Deserialize<ContextCompany>(contextCompanyJSON)!;
            EngineInvoiceSender engineInvoiceSender = new EngineInvoiceSender(contextCompany);
            response = engineInvoiceSender.GetInvoice(requestCambioMoneda);
            if (response.State == ResponseType.Success)
            {
                response.Message = string.Empty;
            }
            return response;
        }

        [HttpPost("AnularInvoicCambioMoneda")]
        [EnableCors()]
        public Response AnularInvoicCambioMoneda(RequestAnularFacturaFinanciero requestAnularCambioMoneda)
        {
            Response response = new Response();
            if (!Request.Headers.Keys.Contains("contextCompany"))
            {
                response.State = ResponseType.Warning;
                response.Message = "Debe enviar la cabecera contextCompany";
                return response;
            }

            string contextCompanyJSON = Request.Headers["contextCompany"]!;
            ContextCompany contextCompany = JsonSerializer.Deserialize<ContextCompany>(contextCompanyJSON)!;
            EngineInvoiceSender engineInvoiceSender = new EngineInvoiceSender(contextCompany);
            response = engineInvoiceSender.AnularFactura(requestAnularCambioMoneda.IdInvoice, requestAnularCambioMoneda.Motivo);
            if (response.State == ResponseType.Success)
            {
                response.Message = string.Empty;
            }
            return response;
        }

        [HttpPost("RevertirInvoiceCambioMoneda")]
        [EnableCors()]
        public Response RevertirInvoiceCambioMoneda(RequestAnularFacturaFinanciero requestAnularFactura)
        {
            Response response = new Response();
            if (!Request.Headers.Keys.Contains("contextCompany"))
            {
                response.State = ResponseType.Warning;
                response.Message = "Debe enviar la cabecera contextCompany";
                return response;
            }

            string contextCompanyJSON = Request.Headers["contextCompany"]!;
            ContextCompany contextCompany = JsonSerializer.Deserialize<ContextCompany>(contextCompanyJSON)!;
            EngineInvoiceSender engineInvoiceSender = new EngineInvoiceSender(contextCompany);
            response = engineInvoiceSender.RevertirFactura(requestAnularFactura.IdInvoice);
            if (response.State == ResponseType.Success)
            {
                response.Message = string.Empty;
            }
            return response;
        }


        [HttpPost("ReenvioFactura")]
        [EnableCors()]
        public Response ReenvioFactura(RequestReenvioFactura requestReenvioFactura)
        {
            Response response = new Response();
            if (!Request.Headers.Keys.Contains("contextCompany"))
            {
                response.State = ResponseType.Warning;
                response.Message = "Debe enviar la cabecera contextCompany";
                return response;
            }

            string contextCompanyJSON = Request.Headers["contextCompany"]!;
            ContextCompany contextCompany = JsonSerializer.Deserialize<ContextCompany>(contextCompanyJSON)!;
            EngineInvoiceSender engineInvoiceSender = new EngineInvoiceSender(contextCompany);
            response = engineInvoiceSender.EnviarFacturaMail(requestReenvioFactura.idInvoice, requestReenvioFactura.mail, requestReenvioFactura.razonSocial, requestReenvioFactura.operacion).Result;
            if (response.State == ResponseType.Success)
            {
                response.Message = string.Empty;
            }
            return response;
        }

        [HttpPost("ReImpresionFactura")]
        [EnableCors()]
        public Response ReImpresionFactura(RequestReImpresionFactura requestReImpresionFactura)
        {
            Response response = new Response();
            if (!Request.Headers.Keys.Contains("contextCompany"))
            {
                response.State = ResponseType.Warning;
                response.Message = "Debe enviar la cabecera contextCompany";
                return response;
            }

            string contextCompanyJSON = Request.Headers["contextCompany"]!;
            ContextCompany contextCompany = JsonSerializer.Deserialize<ContextCompany>(contextCompanyJSON)!;
            EngineInvoiceSender engineInvoiceSender = new EngineInvoiceSender(contextCompany);
            response = engineInvoiceSender.ObtnerPdfB64PorFactura(requestReImpresionFactura.idInvoice).Result;
            if (response.State == ResponseType.Success)
            {
                response.Message = string.Empty;
            }
            return response;
        }
    }



}
