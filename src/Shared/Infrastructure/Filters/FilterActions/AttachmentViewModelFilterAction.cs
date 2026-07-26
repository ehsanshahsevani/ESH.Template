// using ESH.Constant;
// using ESH.SeedworkSystem.Infrastructure.ActionFilter;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.Filters;
// using Persistence;
// using ESH.Resources;
// using ESH.BuildingBlocks.SampleResult;
// using ESH.ViewModels.Marketplace;
// using Result = FluentResults.Result;
//
// namespace Infrastructure.Filters.FilterActions;
//
// public class AttachmentViewModelFilterAction :
//     IBaseAsyncActionFilter
// {
//     public AttachmentViewModelFilterAction(IUnitOfWork unitOfWork)
//     {
//         UnitOfWork = unitOfWork;
//     }
//
//     public IUnitOfWork UnitOfWork { get; }
//
//     public async Task OnActionExecutionAsync(
//         ActionExecutingContext context, ActionExecutionDelegate next)
//     {
//         var result = new Result();
//
//         var action =
//             context.RouteData.Values[ProjectKeyName.ActionKey];
//
//         var controller
//             = context.RouteData.Values[ProjectKeyName.ControllerKey];
//
//         // **************************************************
//         var tryGetValue =
//             context.ActionArguments.TryGetValue("model", out var modelObject);
//         // **************************************************
//
//         if (tryGetValue && modelObject is AttachmentRequestViewModel model)
//         {
//             var resultModel = model.Validate();
//
//             result.WithErrors(resultModel.Errors);
//
//             if (result.IsSuccess)
//                 // 0.5. check for repeat data in database
//                 // var checkForAddResult =
//                 //     await UnitOfWork.AttachmentRepository.IsOkForAddAsync(model);
//                 //
//                 // result.WithErrors(checkForAddResult.Errors);
//                 // 1. check id
//                 // 2. get entity from database if exist
//                 // 3. save in context
//                 // 4. continue to action
//                 //////////////////////////////////////////////
//                 if (string.IsNullOrEmpty(model.Id) == false)
//                 {
//                     var entity =
//                         await UnitOfWork.CategoryRepository.FindAsync(model.Id);
//
//                     if (entity is null)
//                     {
//                         var errorMessage =
//                             string.Format(Messages.NotFoundError, DataDictionary.Category);
//
//                         result.WithError(errorMessage);
//                     }
//                     else
//                     {
//                         context.HttpContext.Items[ProjectKeyName.ObjectKey] = entity;
//                     }
//                 }
//         }
//         else
//         {
//             result.WithError(Messages.RequestNotValid);
//         }
//
//         if (result.IsSuccess)
//         {
//             await next();
//         }
//         else
//         {
//             var sampleResult = result.ConvertToSampleResult();
//
//             context.Result =
//                 new BadRequestObjectResult(sampleResult);
//         }
//     }
// }