﻿using File_Upload_Patrik_God.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace File_Upload_Patrik_God.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class FileController : ControllerBase
	{
		private readonly IWebHostEnvironment _env;
		public FileController(IWebHostEnvironment env)
		{
			_env = env;
		}

		[HttpPost]
		public async Task<ActionResult<List<UploadResult>>> UploadFile(List<IFormFile> files)
		{
			List<UploadResult> uploadResults = new List<UploadResult>();

			foreach (var file in files)
			{
				var uploadResult = new UploadResult();
				string trustedFileNameForFileStorage;
				var untrustedFileName = file.FileName;

				uploadResult.FileName = untrustedFileName;

				var trustedFileNameForDisplay = WebUtility.HtmlEncode(untrustedFileName);

				trustedFileNameForFileStorage = Path.GetRandomFileName();
                var path = Path.Combine(_env.ContentRootPath, "local_database" , trustedFileNameForFileStorage);

				await using FileStream fs = new(path, FileMode.Create);
				await file.CopyToAsync(fs);

				uploadResult.StoredFileName = trustedFileNameForFileStorage;
				uploadResults.Add(uploadResult);
			}

			return Ok(uploadResults);
		}

	}
}
