using System.Net;

namespace OrderProcessingService.Dtos;

public record ErrorDto(string Message, HttpStatusCode StatusCode);