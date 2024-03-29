﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.HtmlConverter;

namespace Syncfusion_HtmlToPdf.Pdfs;

[ApiController]
public class ExportPdf : ControllerBase
{
    private readonly IMediator _mediator;
    public ExportPdf(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("/api/export/pdf")]
    public async Task<Result<ExportPdfContent>> ExportPdfAsync(ExportPdfCommand command)
    {
        return await _mediator.Send(command);
    }

    public class ExportPdfCommand : IRequest<Result<ExportPdfContent>>
    {
        public string HtmlString { get; set; }
        public string FilePath { get; set; }
    }

    public class ExportPdfContent
    {
        public string Content { get; set; }
    }

    public class ExportPdfQueryHandler : IRequestHandler<ExportPdfCommand, Result<ExportPdfContent>>
    {
        public async Task<Result<ExportPdfContent>> Handle(ExportPdfCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var htmlConverter = new HtmlToPdfConverter();

                var settings = new BlinkConverterSettings();
                settings.CommandLineArguments.Add("--no-sandbox");
                settings.CommandLineArguments.Add("--disable-setuid-sandbox");
                htmlConverter.ConverterSettings = settings;

                var document = htmlConverter.Convert(request.HtmlString, string.Empty);

                var memoryStream = new MemoryStream();
                document.Save(memoryStream);

                var result = new ExportPdfContent { Content = Convert.ToBase64String(memoryStream.ToArray()) };

                document.Close(true);
                return Result.Ok(result);
            }
            catch (Exception ex)
            {
                return Result.BadRequest<ExportPdfContent>(ex.Message);
            }
        }
    }
}