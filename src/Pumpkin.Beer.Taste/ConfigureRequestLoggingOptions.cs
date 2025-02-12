namespace Pumpkin.Beer.Taste;

using Microsoft.Extensions.Options;
using Serilog;
using Serilog.AspNetCore;
using Serilog.Events;

/// <summary>
/// Configures serilog HTTP request logging. Adds additional properties to each log.
/// See https://github.com/serilog/serilog-aspnetcore.
/// </summary>
public class ConfigureRequestLoggingOptions : IConfigureOptions<RequestLoggingOptions>
{
    private const string MessageTemplate = "{Protocol} {RequestMethod} {RequestPath} responded {StatusCode} {ContentType} in {Elapsed:0.0000} ms";

    private const string HostPropertyName = "Host";
    private const string ProtocolPropertyName = "Protocol";
    private const string SchemePropertyName = "Scheme";
    private const string QueryStringPropertyName = "QueryString";
    private const string EndpointNamePropertyName = "EndpointName";
    private const string ContentTypePropertyName = "ContentType";
    private const string RemoteIpPropertyName = "RemoteIP";
    private const string RemotePortPropertyName = "RemotePort";

    public void Configure(RequestLoggingOptions options)
    {
        options.EnrichDiagnosticContext = EnrichDiagnosticContext;
        options.MessageTemplate = MessageTemplate;
    }

    private static void EnrichDiagnosticContext(IDiagnosticContext diagnosticContext, HttpContext httpContext)
    {
        var request = httpContext.Request;
        var response = httpContext.Response;

        var remoteIp = httpContext.Connection.RemoteIpAddress;
        var remotePort = httpContext.Connection.RemoteIpAddress;

        diagnosticContext.Set(HostPropertyName, request.Host);
        diagnosticContext.Set(ProtocolPropertyName, request.Protocol);
        diagnosticContext.Set(SchemePropertyName, request.Scheme);
        diagnosticContext.Set(RemoteIpPropertyName, remoteIp == null ? string.Empty : remoteIp);
        diagnosticContext.Set(RemotePortPropertyName, remotePort == null ? string.Empty : remotePort);

        var queryString = request.QueryString;
        if (queryString.HasValue)
        {
            diagnosticContext.Set(QueryStringPropertyName, queryString.Value);
        }

        var endpoint = httpContext.GetEndpoint();
        if (endpoint is not null)
        {
            diagnosticContext.Set(EndpointNamePropertyName, endpoint.DisplayName ?? string.Empty);
        }

        diagnosticContext.Set(ContentTypePropertyName, response.ContentType ?? string.Empty);
    }
}
