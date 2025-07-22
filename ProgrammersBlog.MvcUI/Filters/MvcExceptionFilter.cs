using System.Data.SqlTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using ProgrammersBlog.Core.Entities.Concrete;

namespace ProgrammersBlog.MvcUI.Filters;

public class MvcExceptionFilter : IExceptionFilter
{
    private readonly IHostEnvironment _hostEnvironment;
    private readonly IModelMetadataProvider _modelMetadataProvider;
    private readonly ILogger _logger;
    public MvcExceptionFilter(IHostEnvironment hostEnvironment, IModelMetadataProvider modelMetadataProvider, ILogger<MvcExceptionFilter> logger)
    {
        _hostEnvironment = hostEnvironment;
        _modelMetadataProvider = modelMetadataProvider;
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        if (_hostEnvironment.IsDevelopment())
        {
            context.ExceptionHandled = true;
            var mvcErrorModel = new MvcErrorModel();
            ViewResult result;
            switch (context.Exception)
            {
                case SqlNullValueException sqlNullValueException:
                    mvcErrorModel.Message = "A database error occurred. Please try again later.";
                    mvcErrorModel.Detail = sqlNullValueException.Message;
                    result = new ViewResult { ViewName = "Error" };
                    result.StatusCode = 500;
                    _logger.LogError(context.Exception, context.Exception.Message);
                    break;

                case NullReferenceException nullReferenceException:
                    mvcErrorModel.Message = "A null reference error occurred. Please check your data and try again.";
                    mvcErrorModel.Detail = nullReferenceException.Message;
                    result = new ViewResult { ViewName = "Error" };
                    result.StatusCode = 403;
                    _logger.LogError(context.Exception, "Test Message");

                    break;

                default:
                    mvcErrorModel.Message = "An unexpected error occurred. Please try again later.";
                    mvcErrorModel.Detail = context.Exception.Message;
                    result = new ViewResult { ViewName = "Error" };
                    result.StatusCode = 500;
                    _logger.LogError(context.Exception, context.Exception.Message);

                    break;
            }


            var viewData = new ViewDataDictionary<MvcErrorModel>(_modelMetadataProvider, context.ModelState)
            {
                Model = mvcErrorModel
            };


            result.ViewData = viewData;
            result.ViewData.Add("MvcErrorModel", mvcErrorModel);
            context.Result = result;
        }
    }
}
