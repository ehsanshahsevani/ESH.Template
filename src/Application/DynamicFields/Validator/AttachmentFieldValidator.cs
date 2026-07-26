using FluentResults;
using AngleSharp.Text;
using DynamicFields.Configs;
using DynamicFields.Abstraction;
using ESH.ViewModels.Abstraction;
using Microsoft.AspNetCore.Http;

namespace DynamicFields.Validator;

public class AttachmentFieldValidator : IFieldValidator
{
	public async Task<Result> Validate(object value, IFieldTypeConfig config)
	{
		var attachmentConfig = (AttachmentConfig)config;

		if (value is not IEnumerable<IFormFile> files)
		{
			throw new ArgumentException("type in field is Attachment but value is not a file array");
		}

		if (files.Count() > attachmentConfig.MaxCount)
		{
			var errorMessage = string.Format(
				ESH.Resources.Messages.CountFileError,
				attachmentConfig.MaxCount);

			var result = Result.Fail(errorMessage);

			return result;
		}

		foreach (var file in files)
		{
			if (file.Length > attachmentConfig.MaxSizeMB * 1024 * 1024)
			{
				var errorMessage = string.Format(
					ESH.Resources.Messages.VolumeFileError,
					file.FileName,
					attachmentConfig.MaxSizeMB);

				var result =
					Result.Fail(errorMessage);

				return result;
			}
		}

		foreach (var file in files)
		{
			var extension =
				Path.GetExtension(file.FileName).ToLower().TrimStart('.');

			if (attachmentConfig.AllowedExtensions.Contains(extension) == false)
			{
				var extensions =
					string.Join(", ", attachmentConfig.AllowedExtensions);

				var errorMessage = string.Format(
					ESH.Resources.Messages.FileExtensionError, file.FileName, extensions);

				var result = Result.Fail(errorMessage);

				return result;
			}
		}

		return Result.Ok();
	}
}

public class AttachmentFieldWithOutCheckCountForUpdate : IFieldValidator
{
	public async Task<Result> Validate(object value, IFieldTypeConfig config)
	{
		var attachmentConfig = (AttachmentConfig)config;

		if (value is not IEnumerable<IFormFile> files)
		{
			throw new ArgumentException("type in field is Attachment but value is not a file array");
		}

		foreach (var file in files)
		{
			if (file.Length > attachmentConfig.MaxSizeMB * 1024 * 1024)
			{
				var errorMessage = string.Format(
					ESH.Resources.Messages.VolumeFileError,
					file.FileName,
					attachmentConfig.MaxSizeMB);

				var result =
					Result.Fail(errorMessage);

				return result;
			}
		}

		foreach (var file in files)
		{
			var extension =
				Path.GetExtension(file.FileName).ToLower().TrimStart('.');

			if (attachmentConfig.AllowedExtensions.Contains(extension) == false)
			{
				var extensions =
					string.Join(", ", attachmentConfig.AllowedExtensions);

				var errorMessage = string.Format(
					ESH.Resources.Messages.FileExtensionError, file.FileName, extensions);

				var result = Result.Fail(errorMessage);

				return result;
			}
		}

		return Result.Ok();
	}
}