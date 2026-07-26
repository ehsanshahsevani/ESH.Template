// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.Filters;
// using Persistence;
// using ESH.BuildingBlocks.SampleResult;
//
// namespace Infrastructure.Filters.FilterActions;
//
// public class CheckAttachmentIdActionFilter : ESH.SeedworkSystem.Infrastructure.ActionFilter.IBaseAsyncActionFilter
// {
//     public IUnitOfWork UnitOfWork { get; }
//
//     public CheckAttachmentIdActionFilter(IUnitOfWork unitOfWork)
//     {
//         UnitOfWork = unitOfWork;
//     }
//
//     public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
//     {
//         var result = new FluentResults.Result();
//
//         var id =
//             context.ActionArguments.FirstOrDefault
//             (current =>
//                 current.Value is string).Value as string;
//
//         if (string.IsNullOrWhiteSpace(id) || id == Guid.NewGuid().ToString())
//         {
//             var errorMessage = string.Format(
//                 ESH.Resources.Messages.NotFoundError, ESH.Resources.DataDictionary.Guid);
//             
//             result.WithError(errorMessage);
//         }
//         else
//         {
//             var attachment =
//                 await UnitOfWork
//                     .AttachmentRepository.FindAsync(id);
//
//             if (attachment is null)
//             {
//                 var errorMessage = string.Format(
//                     ESH.Resources.Messages.NotFoundError, ESH.Resources.DataDictionary.Attachment);
//                 
//                 result.WithError(errorMessage);
//             }
//             else
//             {
//                 context.HttpContext.Items[Constants.ProjectKeyName.ObjectKey] = attachment;
//             }
//         }
//
//         if (result.IsSuccess == true)
//         {
//             await next();
//         }
//
//         if (result.IsFailed == true)
//         {
//             var sampleResult = result.ConvertToSampleResult();
//
//             context.Result =
//                 new BadRequestObjectResult(sampleResult);
//         }
//     }
// }