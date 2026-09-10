using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Iza.Core.Integration.Baneco
{
    /// <summary>
    /// Espejo del contrato de <c>POST /api/baneco/qrs</c>. El proxy declara
    /// <c>additionalProperties: false</c>: cualquier campo de mas —en particular <c>accountCredit</c>,
    /// que sale de la cuenta de ahorro del suscriptor y no del cuerpo— provoca un 400.
    /// </summary>
    internal sealed record GenerarQrRequest(
        [property: JsonPropertyName("transactionId")] string TransactionId,
        [property: JsonPropertyName("currency")] string Currency,
        [property: JsonPropertyName("amount")] decimal Amount,
        [property: JsonPropertyName("description")] string? Description,
        [property: JsonPropertyName("dueDate")] DateOnly DueDate,
        [property: JsonPropertyName("singleUse")] bool SingleUse,
        [property: JsonPropertyName("modifyAmount")] bool ModifyAmount,
        [property: JsonPropertyName("branchCode")] string? BranchCode);

    internal sealed record GenerarQrResponse(
        [property: JsonPropertyName("responseCode")] int ResponseCode,
        [property: JsonPropertyName("message")] string? Message,
        [property: JsonPropertyName("qrId")] string? QrId,
        [property: JsonPropertyName("qrImage")] string? QrImage);

    internal sealed record QrPagadoResponse(
        [property: JsonPropertyName("qrId")] string QrId,
        [property: JsonPropertyName("transactionId")] string? TransactionId,
        [property: JsonPropertyName("paymentDate")] DateOnly PaymentDate,
        [property: JsonPropertyName("paymentTime")] string? PaymentTime,
        [property: JsonPropertyName("currency")] string? Currency,
        [property: JsonPropertyName("amount")] decimal Amount,
        [property: JsonPropertyName("senderBankCode")] string? SenderBankCode,
        [property: JsonPropertyName("senderName")] string? SenderName,
        [property: JsonPropertyName("senderDocumentId")] string? SenderDocumentId,
        [property: JsonPropertyName("senderAccount")] string? SenderAccount,
        [property: JsonPropertyName("description")] string? Description,
        [property: JsonPropertyName("branchCode")] string? BranchCode);

    /// <summary>Cuerpo de error del proxy para 400/403/409.</summary>
    internal sealed record ApiResultResponse(
        [property: JsonPropertyName("responseCode")] int ResponseCode,
        [property: JsonPropertyName("message")] string? Message);

    /// <summary>
    /// Falla de la integracion con un mensaje apto para el operador. No transporta la API Key ni el
    /// cuerpo completo del banco: solo el motivo.
    /// </summary>
    public sealed class QrBancoEconomicoException : Exception
    {
        public QrBancoEconomicoException(string message) : base(message) { }
        public QrBancoEconomicoException(string message, Exception inner) : base(message, inner) { }
    }
}
