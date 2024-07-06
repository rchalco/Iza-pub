// See https://aka.ms/new-console-template for more information
using DevExpress.CodeParser;
using Invoice.Core.Business.InvoiceKind.BasicExpenses;
using Invoice.Core.Business.InvoiceKind.CurrencyExchange;
using Invoice.Core.Business.InvoiceKind.Financial;
using Invoice.Core.Business.InvoiceKind.Orders;
using Invoice.Core.Business.Office;
using Invoice.Core.Domain;
using Invoice.Core.Engines;
using Invoice.Core.ForTesting;
using NPOI.SS.Formula.Functions;
using PlumbingProps.Logger;
using PlumbingProps.Wrapper;

Console.WriteLine("iniciando la consola");
Console.WriteLine("introduzca el numero de iteraciones a generar");
var resul = Console.ReadLine();
try
{
    GetInvoice_Robot(Convert.ToInt32(resul));

}
catch (Exception ex)
{
    Console.WriteLine("error al ehecutar el proceso: " + ex.Message);
}

Console.WriteLine("proceos terminado");


void GetInvoice_Robot(int nroIteraciones)
{
    AuxiliarTest auxiliarTest = new AuxiliarTest();
    auxiliarTest.EjecutarSP("Test.spEventoSignificativo", 1);
    Console.WriteLine("iniciando secuencia: 1 " + " " + DateTime.Now.ToString());
    auxiliarTest = new AuxiliarTest();
    auxiliarTest.EjecutarSP("Test.spEstadoOnline", 1);
    auxiliarTest.EjecutarSP("Test.spCAFC", "");
    //rsulAux = auxiliarTest.EjecutarSP("Test.spCrea500");
    Console.WriteLine("generando 500 facturas offline " + " " + DateTime.Now.ToString());
    //GetInvoiceFinanciera_Success();
    Console.WriteLine("fin de generacion de 500 facturas offline " + " " + DateTime.Now.ToString());
    Console.WriteLine("esperamos 1 1/2 minutos.....");
    System.Threading.Thread.Sleep(90000);
    auxiliarTest.EjecutarSP("Test.spEstadoOnline", 0);
    Console.WriteLine("enviando lote" + " " + DateTime.Now.ToString());
    // EngineBatchInvoiceSender_Success();
    Console.WriteLine("fin de envio lote" + " " + DateTime.Now.ToString());




    for (int i = 0; i < 5; i++)
    {
        auxiliarTest.EjecutarSP("Test.spEventoSignificativo", 1);
        Console.WriteLine("iniciando secuencia: 1 " + i.ToString() + " " + DateTime.Now.ToString());
        auxiliarTest = new AuxiliarTest();
        auxiliarTest.EjecutarSP("Test.spEstadoOnline", 1);
        auxiliarTest.EjecutarSP("Test.spCAFC", "");
        //rsulAux = auxiliarTest.EjecutarSP("Test.spCrea500");
        Console.WriteLine("generando 500 facturas offline " + " " + DateTime.Now.ToString());
        GetInvoiceCambioMoneda_Success(2, 1);
        Console.WriteLine("fin de generacion de 500 facturas offline " + " " + DateTime.Now.ToString());
        Console.WriteLine("esperamos 1 1/2 minutos.....");
        System.Threading.Thread.Sleep(90000);
        auxiliarTest.EjecutarSP("Test.spEstadoOnline", 0);
        Console.WriteLine("enviando lote" + " " + DateTime.Now.ToString());
        EngineBatchInvoiceSender_Success(2);
        Console.WriteLine("fin de envio lote" + " " + DateTime.Now.ToString());
        System.Threading.Thread.Sleep(301000);
    }
    for (int i = 0; i < 5; i++)
    {
        auxiliarTest.EjecutarSP("Test.spEventoSignificativo", 1);
        Console.WriteLine("iniciando secuencia: 1 " + i.ToString() + " " + DateTime.Now.ToString());
        auxiliarTest = new AuxiliarTest();
        auxiliarTest.EjecutarSP("Test.spEstadoOnline", 1);
        auxiliarTest.EjecutarSP("Test.spCAFC", "");
        //rsulAux = auxiliarTest.EjecutarSP("Test.spCrea500");
        Console.WriteLine("generando 500 facturas offline " + " " + DateTime.Now.ToString());
        GetInvoiceCambioMoneda_Success(1, 2);
        Console.WriteLine("fin de generacion de 500 facturas offline " + " " + DateTime.Now.ToString());
        Console.WriteLine("esperamos 1 1/2 minutos.....");
        System.Threading.Thread.Sleep(90000);
        auxiliarTest.EjecutarSP("Test.spEstadoOnline", 0);
        Console.WriteLine("enviando lote" + " " + DateTime.Now.ToString());
        EngineBatchInvoiceSender_Success(1);
        Console.WriteLine("fin de envio lote" + " " + DateTime.Now.ToString());
        System.Threading.Thread.Sleep(301000);
    }




}

void EngineBatchInvoiceSender_Success(int of)
{
    ContextCompany contextCompany = new ContextCompany
    {
        IdCompany = 1,
        IdOffice = of,
        IdOfficeExternal = 0,
        CompanyKey = "GAMATEK"
    };


    EngineBatchInvoiceSender engineBatchInvoiceSender = new EngineBatchInvoiceSender(contextCompany);
    Response response = engineBatchInvoiceSender.RegisterBatchInvoice();
}


void AnularFactura_Success()
{
    AuxiliarTest auxiliarTest = new AuxiliarTest();
    ContextCompany contextCompany = new ContextCompany
    {
        IdCompany = 1,
        IdOffice = 1,
        IdOfficeExternal = 0,
        CompanyKey = "PERFECTO"
    };

    RequestCompraVenta compraVentaDTO = new RequestCompraVenta
    {
        codigoCliente = "000",
        codigoExcepcion = 0,
        codigoMetodoPago = 1,//metodo de pago en efectivo
        codigoTipoDocumentoIdentidad = 1,
        complemento = null,
        descuentoAdicional = 0,
        montoGiftCard = 0,
        montoTotal = 10,
        montoTotalMoneda = 10,
        montoTotalSujetoIva = 10,
        municipio = "La Paz",
        nombreRazonSocial = "Chalco",
        numeroDocumento = "6104627",
        numeroFactura = 65,
        numeroTarjeta = 0,
        mail = "nebur.dario@gmail.com",
        detalle = new List<RequestCompraVentaDetalle>()
    };
    for (int i = 0; i < 125; i++)
    {
        var v = GetInvoiceCambioMoneda_Success2(1);
        EngineInvoiceSender engineInvoiceSender = new EngineInvoiceSender(contextCompany);
        Response response = engineInvoiceSender.AnularFactura(v, 1);
        //Assert.That(response.State, Is.EqualTo(ResponseType.Success));
    }
    for (int i = 0; i < 125; i++)
    {
        var v = GetInvoiceCambioMoneda_Success2(2);
        EngineInvoiceSender engineInvoiceSender = new EngineInvoiceSender(contextCompany);
        Response response = engineInvoiceSender.AnularFactura(v, 1);
        //Assert.That(response.State, Is.EqualTo(ResponseType.Success));
    }
    for (int i = 0; i < 125; i++)
    {
        var v = GetInvoiceServiciosBasicos_Success2(1);
        EngineInvoiceSender engineInvoiceSender = new EngineInvoiceSender(contextCompany);
        Response response = engineInvoiceSender.AnularFactura(v, 1);
        //Assert.That(response.State, Is.EqualTo(ResponseType.Success));
    }
    for (int i = 0; i < 125; i++)
    {
        var v = GetInvoiceServiciosBasicos_Success2(2);
        EngineInvoiceSender engineInvoiceSender = new EngineInvoiceSender(contextCompany);
        Response response = engineInvoiceSender.AnularFactura(v, 1);
        //Assert.That(response.State, Is.EqualTo(ResponseType.Success));
    }
    for (int i = 0; i < 125; i++)
    {
        var v = GetInvoiceFinanciera_Success2(1);
        EngineInvoiceSender engineInvoiceSender = new EngineInvoiceSender(contextCompany);
        Response response = engineInvoiceSender.AnularFactura(v, 1);
        //Assert.That(response.State, Is.EqualTo(ResponseType.Success));
    }
    for (int i = 0; i < 125; i++)
    {
        var v = GetInvoiceFinanciera_Success2(2);
        EngineInvoiceSender engineInvoiceSender = new EngineInvoiceSender(contextCompany);
        Response response = engineInvoiceSender.AnularFactura(v, 1);
        //Assert.That(response.State, Is.EqualTo(ResponseType.Success));
    }

}

int GetInvoiceFinanciera_Success2(int ofi)
{
    int resulf = 0;
    ContextCompany contextCompany = new ContextCompany
    {
        IdCompany = 1,
        IdOffice = ofi,
        IdOfficeExternal = 0,
        CompanyKey = "GAMATEK"
    };
    EngineInvoiceSender engineInvoiceSender = new EngineInvoiceSender(contextCompany);
    RequestEntidadFinanciera financieraDTO = new RequestEntidadFinanciera
    {
        codigoExcepcion = 0,
        municipio = "La Paz",
        nombreRazonSocial = "Chalco",
        codigoTipoDocumentoIdentidad = 1,
        numeroDocumento = "4010898",
        complemento = "",
        codigoCliente = "000",
        mail = "mikyches@gmail.com",
        codigoMetodoPago = 1,//metodo de pago en efectivo
        numeroTarjeta = 0,
        montoTotalArrendamientoFinanciero = 0.0m,
        montoTotal = 500.00m,
        montoTotalSujetoIva = 500.00m,
        codigoMoneda = 1,
        tipoCambio = 6.86m,
        montoTotalMoneda = 500.00m,
        descuentoAdicional = 0.0m,
        tipoCambioOficial = 6.86m,
        numeroFactura = 0,
        detalle = new List<RequestEntidadFinancieraDetalle>()


    };
    financieraDTO.detalle.Add(new RequestEntidadFinancieraDetalle
    {
        cantidad = 1,
        descripcion = "Comisión por cobros",
        montoDescuento = 0,
        precioUnitario = 500.00m,
        subTotal = 500.00m,
    });


    //Response response = engineInvoiceSender.GetInvoice(financieraDTO);
    ResponseObject<ResulInvoice> response = engineInvoiceSender.GetInvoice(financieraDTO);
    resulf = response.Data.nroFactura;
    return resulf;

}

int GetInvoiceServiciosBasicos_Success2(int ofi)
{
    int resulf = 0;
    ContextCompany contextCompany = new ContextCompany
    {
        IdCompany = 1,
        IdOffice = ofi,
        IdOfficeExternal = 0,
        CompanyKey = "GAMATEK"
    };
    EngineInvoiceSender engineInvoiceSender = new EngineInvoiceSender(contextCompany);
    RequestServicioBasico financieraDTO = new RequestServicioBasico
    {
        codigoExcepcion = 0,
        municipio = "La Paz",
        nombreRazonSocial = "Chalco",
        codigoTipoDocumentoIdentidad = 1,
        numeroDocumento = "4010898",
        complemento = "",
        codigoCliente = "000",
        mail = "mikyches@gmail.com",
        codigoMetodoPago = 1,//metodo de pago en efectivo
        numeroTarjeta = 0,
        consumoPeriodo = 13,
        descuentoAdicional = 0.0m,
        detalleAjusteNoSujetoIva = "",
        detalleAjusteSujetoIva = "",
        detalleOtrosPagosNoSujetoIva = "",
        domicilioCliente = "hidráulica 01",
        gestion = 2024,
        mes = "1",
        montoDescuentoLey1886 = 0.00m,
        montoDescuentoTarifaDignidad = 5.230m,
        montoTotal = 17.55m,// (montoTotalMoneda + tasaAlumbrado)-(beneficiarioLey1886+montoDescuentoTarifaDignidad)
        montoTotalMoneda = 17.55m,
        montoTotalSujetoIva = 15.670m,// montoTotalMoneda o precioUnitario - TotalDescuentos(no es válidoParaCrèditoFiscal)
        numeroFactura = 0,
        numeroMedidor = "201",
        otrasTasas = 0.00m,
        otrosPagosNoSujetoIva = 0.00m,
        zona = "105",
        ajusteNoSujetoIva = 0.00m,
        ajusteSujetoIva = 0.00m,
        beneficiarioLey1886 = 0,
        tasaAlumbrado = 1.88m, // descuento
        tasaAseo = 0.00m,
        ciudad = "LA PAZ",
        detalle = new List<RequestServicioBasicoDetalle>()
    };
    financieraDTO.detalle.Add(new RequestServicioBasicoDetalle
    {
        cantidad = 1,
        descripcion = "VENTA ENERGIA",
        montoDescuento = 5.230m,
        precioUnitario = 20.90m,
        subTotal = 15.670m, //precio unitario - monto descuento
    });

    //Response response = engineInvoiceSender.GetInvoice(financieraDTO)
    ResponseObject<ResulInvoice> response = engineInvoiceSender.GetInvoice(financieraDTO);
    resulf = response.Data.nroFactura;
    return resulf;
}


int GetInvoiceCambioMoneda_Success2(int ofi)
{
    int resulf = 0;
    ContextCompany contextCompany = new ContextCompany
    {
        IdCompany = 1,
        IdOffice = ofi,
        IdOfficeExternal = 0,
        CompanyKey = "GAMATEK"
    };
    EngineInvoiceSender engineInvoiceSender = new EngineInvoiceSender(contextCompany);
    RequestCambioMoneda cambioMonedaDTO = new RequestCambioMoneda
    {
        codigoExcepcion = 0,
        municipio = "La Paz",
        nombreRazonSocial = "Chalco",
        codigoTipoDocumentoIdentidad = 1,
        numeroDocumento = "4010898",
        complemento = "",
        codigoCliente = "000",
        mail = "mikyches@gmail.com",
        codigoMetodoPago = 1,//metodo de pago en efectivo
        numeroTarjeta = 0,
        montoTotalArrendamientoFinanciero = 0.0m,
        montoTotal = 348.50m,
        ingresoDiferenciaCambio = 0.11m,
        codigoTipoOperacion = 1,
        montoTotalSujetoIva = 0.0m,
        codigoMoneda = 2,
        tipoCambio = 6.97m, //este el cambio del cambio de moneda
        montoTotalMoneda = 50.00m, ///monto del codigo de moneda para el caso 50$ al tipo de cambio 6.97
        descuentoAdicional = 0.0m,
        tipoCambioOficial = 6.86m,
        numeroFactura = 0,
        detalle = new List<RequestCambioMonedaDetalle>()


    };
    cambioMonedaDTO.detalle.Add(new RequestCambioMonedaDetalle
    {
        cantidad = 50.00m,
        descripcion = "Cantidad 50, venta de dolares tco 6.86/tct 6.97",
        montoDescuento = 0,
        precioUnitario = 6.97m,
        subTotal = 348.5m,
    });

    // Response response = engineInvoiceSender.GetInvoice(cambioMonedaDTO);
    ResponseObject<ResulInvoice> response = engineInvoiceSender.GetInvoice(cambioMonedaDTO);
    resulf = response.Data.nroFactura;
    return resulf;

}
int GetInvoice_Success2(int ofi)
{
    int resulf = 0;
    ContextCompany contextCompany = new ContextCompany
    {
        IdCompany = 1,
        IdOffice = ofi,
        IdOfficeExternal = 0,
        CompanyKey = "GAMATEK"
    };
    EngineInvoiceSender engineInvoiceSender = new EngineInvoiceSender(contextCompany);
    RequestCompraVenta compraVentaDTO = new RequestCompraVenta
    {
        codigoCliente = "000",
        codigoExcepcion = 0,
        codigoMetodoPago = 1,//metodo de pago en efectivo
        codigoTipoDocumentoIdentidad = 1,
        complemento = "",
        descuentoAdicional = 0,
        montoGiftCard = 0,
        montoTotal = 10,
        montoTotalMoneda = 10,
        montoTotalSujetoIva = 10,
        municipio = "La Paz",
        nombreRazonSocial = "Chalco",
        numeroDocumento = "6104627",
        numeroFactura = 0,
        numeroTarjeta = 0,
        mail = "nebur.dario@gmail.com",
        detalle = new List<RequestCompraVentaDetalle>()
    };
    compraVentaDTO.detalle.Add(new RequestCompraVentaDetalle
    {
        cantidad = 1,
        descripcion = "Chicles",
        montoDescuento = 0,
        numeroImei = "",
        numeroSerie = "",
        precioUnitario = 10,
        subTotal = 10,
    });
    ResponseObject<ResulInvoice> response = engineInvoiceSender.GetInvoice(compraVentaDTO);
    resulf = response.Data.nroFactura;
    return resulf;
}

/*****************************************************************/
void GetInvoiceServiciosBasicos_Success(int ofi, int itera)
{
    ContextCompany contextCompany = new ContextCompany
    {
        IdCompany = 1,
        IdOffice = ofi,
        IdOfficeExternal = 0,
        CompanyKey = "GAMATEK"
    };
    EngineInvoiceSender engineInvoiceSender = new EngineInvoiceSender(contextCompany);
    RequestServicioBasico financieraDTO = new RequestServicioBasico
    {
        codigoExcepcion = 0,
        municipio = "La Paz",
        nombreRazonSocial = "Chalco",
        codigoTipoDocumentoIdentidad = 1,
        numeroDocumento = "4010898",
        complemento = "",
        codigoCliente = "000",
        mail = "mikyches@gmail.com",
        codigoMetodoPago = 1,//metodo de pago en efectivo
        numeroTarjeta = 0,
        consumoPeriodo = 13,
        descuentoAdicional = 0.0m,
        detalleAjusteNoSujetoIva = "",
        detalleAjusteSujetoIva = "",
        detalleOtrosPagosNoSujetoIva = "",
        domicilioCliente = "hidráulica 01",
        gestion = 2024,
        mes = "1",
        montoDescuentoLey1886 = 0.00m,
        montoDescuentoTarifaDignidad = 5.230m,
        montoTotal = 17.55m,// (montoTotalMoneda + tasaAlumbrado)-(beneficiarioLey1886+montoDescuentoTarifaDignidad)
        montoTotalMoneda = 17.55m,
        montoTotalSujetoIva = 15.670m,// montoTotalMoneda o precioUnitario - TotalDescuentos(no es válidoParaCrèditoFiscal)
        numeroFactura = 0,
        numeroMedidor = "201",
        otrasTasas = 0.00m,
        otrosPagosNoSujetoIva = 0.00m,
        zona = "105",
        ajusteNoSujetoIva = 0.00m,
        ajusteSujetoIva = 0.00m,
        beneficiarioLey1886 = 0,
        tasaAlumbrado = 1.88m, // descuento
        tasaAseo = 0.00m,
        ciudad = "LA PAZ",
        detalle = new List<RequestServicioBasicoDetalle>()
    };
    financieraDTO.detalle.Add(new RequestServicioBasicoDetalle
    {
        cantidad = 1,
        descripcion = "VENTA ENERGIA",
        montoDescuento = 5.230m,
        precioUnitario = 20.90m,
        subTotal = 15.670m, //precio unitario - monto descuento
    });

    for (int i = 0; i < itera; i++)
    {
        Response response = engineInvoiceSender.GetInvoice(financieraDTO);
        if (response.State != ResponseType.Success)
        {
            Console.WriteLine("error al generar financiera: " + response.Message);
        }
    }

}


void GetInvoiceCambioMoneda_Success(int ofi, int itera)
{
    ContextCompany contextCompany = new ContextCompany
    {
        IdCompany = 1,
        IdOffice = ofi,
        IdOfficeExternal = 0,
        CompanyKey = "GAMATEK"
    };
    EngineInvoiceSender engineInvoiceSender = new EngineInvoiceSender(contextCompany);
    RequestCambioMoneda cambioMonedaDTO = new RequestCambioMoneda
    {
        codigoExcepcion = 0,
        municipio = "La Paz",
        nombreRazonSocial = "Chalco",
        codigoTipoDocumentoIdentidad = 1,
        numeroDocumento = "4010898",
        complemento = "",
        codigoCliente = "000",
        mail = "mikyches@gmail.com",
        codigoMetodoPago = 1,//metodo de pago en efectivo
        numeroTarjeta = 0,
        montoTotalArrendamientoFinanciero = 0.0m,
        montoTotal = 348.50m,
        ingresoDiferenciaCambio = 0.11m,
        codigoTipoOperacion = 1,
        montoTotalSujetoIva = 0.0m,
        codigoMoneda = 2,
        tipoCambio = 6.97m, //este el cambio del cambio de moneda
        montoTotalMoneda = 50.00m, ///monto del codigo de moneda para el caso 50$ al tipo de cambio 6.97
        descuentoAdicional = 0.0m,
        tipoCambioOficial = 6.86m,
        numeroFactura = 0,
        detalle = new List<RequestCambioMonedaDetalle>()


    };
    cambioMonedaDTO.detalle.Add(new RequestCambioMonedaDetalle
    {
        cantidad = 50.00m,
        descripcion = "Cantidad 50, venta de dolares tco 6.86/tct 6.97",
        montoDescuento = 0,
        precioUnitario = 6.97m,
        subTotal = 348.5m,
    });

    for (int i = 0; i < itera; i++)
    {
        Response response = engineInvoiceSender.GetInvoice(cambioMonedaDTO);
        if (response.State != ResponseType.Success)
        {
            Console.WriteLine("error al generar financiera: " + response.Message);
        }
    }
}

void GetInvoiceFinanciera_Success(int ofi, int itera)
{
    ContextCompany contextCompany = new ContextCompany
    {
        IdCompany = 1,
        IdOffice = ofi,
        IdOfficeExternal = 0,
        CompanyKey = "GAMATEK"
    };
    EngineInvoiceSender engineInvoiceSender = new EngineInvoiceSender(contextCompany);
    RequestEntidadFinanciera financieraDTO = new RequestEntidadFinanciera
    {
        codigoExcepcion = 0,
        municipio = "La Paz",
        nombreRazonSocial = "Chalco",
        codigoTipoDocumentoIdentidad = 1,
        numeroDocumento = "4010898",
        complemento = "",
        codigoCliente = "000",
        mail = "mikyches@gmail.com",
        codigoMetodoPago = 1,//metodo de pago en efectivo
        numeroTarjeta = 0,
        montoTotalArrendamientoFinanciero = 0.0m,
        montoTotal = 500.00m,
        montoTotalSujetoIva = 500.00m,
        codigoMoneda = 1,
        tipoCambio = 6.86m,
        montoTotalMoneda = 500.00m,
        descuentoAdicional = 0.0m,
        tipoCambioOficial = 6.86m,
        numeroFactura = 0,
        detalle = new List<RequestEntidadFinancieraDetalle>()


    };
    financieraDTO.detalle.Add(new RequestEntidadFinancieraDetalle
    {
        cantidad = 1,
        descripcion = "Comisión por cobros",
        montoDescuento = 0,
        precioUnitario = 500.00m,
        subTotal = 500.00m,
    });

    for (int i = 0; i < itera; i++)
    {
        Response response = engineInvoiceSender.GetInvoice(financieraDTO);
        if (response.State != ResponseType.Success)
        {
            Console.WriteLine("error al generar financiera: " + response.Message);
        }
    }
    //Assert.That(response.State, Is.EqualTo(ResponseType.Success));
}





void GetInvoice_Success(int of, int itera)
{
    ContextCompany contextCompany = new ContextCompany
    {
        IdCompany = 1,
        IdOffice = of,
        IdOfficeExternal = 0,
        CompanyKey = "GAMATEK"
    };
    EngineInvoiceSender engineInvoiceSender = new EngineInvoiceSender(contextCompany);
    RequestCompraVenta compraVentaDTO = new RequestCompraVenta
    {
        codigoCliente = "000",
        codigoExcepcion = 0,
        codigoMetodoPago = 1,//metodo de pago en efectivo
        codigoTipoDocumentoIdentidad = 1,
        complemento = "",
        descuentoAdicional = 0,
        montoGiftCard = 0,
        montoTotal = 10,
        montoTotalMoneda = 10,
        montoTotalSujetoIva = 10,
        municipio = "La Paz",
        nombreRazonSocial = "Chalco",
        numeroDocumento = "6104627",
        numeroFactura = 0,
        numeroTarjeta = 0,
        mail = "nebur.dario@gmail.com",
        detalle = new List<RequestCompraVentaDetalle>()
    };
    compraVentaDTO.detalle.Add(new RequestCompraVentaDetalle
    {
        cantidad = 1,
        descripcion = "Chicles",
        montoDescuento = 0,
        numeroImei = "",
        numeroSerie = "",
        precioUnitario = 10,
        subTotal = 10,
    });
    for (int i = 0; i < itera; i++)
    {
        Response response = engineInvoiceSender.GetInvoice(compraVentaDTO);
        if (response.State != ResponseType.Success)
        {
            Console.WriteLine("error al generar factura: " + response.Message);
        }
    }

    /*******************ANULAAAAA*******************/
    void AnularFactura_Success()
    {
        ContextCompany contextCompany = new ContextCompany
        {
            IdCompany = 2,
            IdOffice = 3,
            IdOfficeExternal = 0,
            CompanyKey = "PERFECTO"
        };

        RequestCompraVenta compraVentaDTO = new RequestCompraVenta
        {
            codigoCliente = "000",
            codigoExcepcion = 0,
            codigoMetodoPago = 1,//metodo de pago en efectivo
            codigoTipoDocumentoIdentidad = 1,
            complemento = null,
            descuentoAdicional = 0,
            montoGiftCard = 0,
            montoTotal = 10,
            montoTotalMoneda = 10,
            montoTotalSujetoIva = 10,
            municipio = "La Paz",
            nombreRazonSocial = "Chalco",
            numeroDocumento = "6104627",
            numeroFactura = 65,
            numeroTarjeta = 0,
            mail = "nebur.dario@gmail.com",
            detalle = new List<RequestCompraVentaDetalle>()
        };

        EngineInvoiceSender engineInvoiceSender = new EngineInvoiceSender(contextCompany);
        Response response = engineInvoiceSender.AnularFactura(1, 1);
        //Assert.That(response.State, Is.EqualTo(ResponseType.Success));
    }
}