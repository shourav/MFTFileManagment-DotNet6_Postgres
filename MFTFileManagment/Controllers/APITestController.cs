using MFTFileManagment.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MFTFileManagment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APITestController : ControllerBase
    {

        private readonly FileDataContext _context;
        private readonly ILogger<APITestController> _logger;
        private readonly IConfiguration _config;

        //Dependency Injection through Constructor
        public APITestController(FileDataContext context, ILogger<APITestController> logger, IConfiguration config)
        {
            this._context = context;
            _logger = logger;
            _config = config;
        }




        //Get all files from database
        [HttpGet]
        [Route("GetFilesFromDB")]
        public async Task<ActionResult<List<FileViewModel>>> GetFilesFromDB()
        {
            this._logger.LogInformation("GetFilesFromDB: ");
            var ret = await this._context.Files.Select(f => new FileViewModel
            {
                Id = f.Id,
                Name = f.Name,
                Path = f.Path,
                Extension = f.Extension,
                MakeBy = f.MakeBy,
                MakeDate = f.MakeDate,
                Remarks = f.Remarks
            }).OrderBy(i => i.Id).ToListAsync();
            var combined = string.Join(", ", ret);
            this._logger.LogInformation("Response Data: " + System.Environment.NewLine + combined.ToString());
            return Ok(ret);
        }

        //Get single file by id from database
        [HttpGet]
        [Route("GetFileById")]
        public async Task<ActionResult<FileViewModel>> GetFileById(long id)
        {
            this._logger.LogInformation("GetFileById: " + id.ToString());
            var dbFile = await this._context.Files.FindAsync(id);
            if (dbFile == null)
            {
                this._logger.LogInformation("File not found!");
                return BadRequest("File not found!");
            }
            FileViewModel ret = new FileViewModel
            {
                Id = dbFile.Id,
                Name = dbFile.Name,
                Path = dbFile.Path,
                Extension = dbFile.Extension,
                MakeBy = dbFile.MakeBy,
                MakeDate = dbFile.MakeDate,
                Remarks = dbFile.Remarks
            };
            this._logger.LogInformation("Response Data: " + System.Environment.NewLine + ret.ToString());
            return Ok(ret);
        }

        //Add a new file in database
        [HttpPost]
        [Route("AddFilesInDB")]
        public async Task<ActionResult<List<FileViewModel>>> AddFilesInDB(FileViewModel file)
        {
            this._logger.LogInformation("AddFilesInDB: " + System.Environment.NewLine + file.ToString());
            this._context.Files.Add(new Documents.Data.File
            {
                Name = file.Name,
                Path = file.Path,
                Extension = file.Extension,
                MakeBy = file.MakeBy,
                MakeDate = DateTime.UtcNow,
                Remarks = file.Remarks
            });
            await this._context.SaveChangesAsync();

            var ret = await this._context.Files.Select(f => new FileViewModel
            {
                Id = f.Id,
                Name = f.Name,
                Path = f.Path,
                Extension = f.Extension,
                MakeBy = f.MakeBy,
                MakeDate = f.MakeDate,
                Remarks = f.Remarks
            }).OrderBy(i => i.Id).ToListAsync();
            var combined = string.Join(", ", ret);
            this._logger.LogInformation("Response Data: " + System.Environment.NewLine + combined.ToString());
            return Ok(ret);
        }

        //Update a file in database
        [HttpPut]
        [Route("UpdateFileInDB")]
        public async Task<ActionResult<List<FileViewModel>>> UpdateFileInDB(FileViewModel request)
        {
            this._logger.LogInformation("UpdateFileInDB: " + System.Environment.NewLine + request.ToString());
            var dbFile = await this._context.Files.FindAsync(request.Id);
            if (dbFile == null)
                return BadRequest("File not found!");

            dbFile.Name = request.Name;
            dbFile.Path = request.Path;
            dbFile.Extension = request.Extension;
            dbFile.MakeBy = request.MakeBy;
            dbFile.MakeDate = DateTime.UtcNow;
            dbFile.Remarks = request.Remarks;

            await this._context.SaveChangesAsync();

            var ret = await this._context.Files.Select(f => new FileViewModel
            {
                Id = f.Id,
                Name = f.Name,
                Path = f.Path,
                Extension = f.Extension,
                MakeBy = f.MakeBy,
                MakeDate = f.MakeDate,
                Remarks = f.Remarks
            }).OrderBy(i => i.Id).ToListAsync();
            var combined = string.Join(", ", ret);
            this._logger.LogInformation("Response Data: " + System.Environment.NewLine + combined.ToString());
            return Ok(ret);
        }

        //delete a file from database
        [HttpDelete]
        [Route("DeleteFileFromDB")]
        public async Task<ActionResult<List<FileViewModel>>> DeleteFileFromDB(long id)
        {
            this._logger.LogInformation("DeleteFileFromDB: " + id.ToString());
            var dbFile = await this._context.Files.FindAsync(id);
            if (dbFile == null)
                return BadRequest("File not found!");
            this._context.Files.Remove(dbFile);
            await this._context.SaveChangesAsync();
            var ret = await this._context.Files.Select(f => new FileViewModel
            {
                Id = f.Id,
                Name = f.Name,
                Path = f.Path,
                Extension = f.Extension,
                MakeBy = f.MakeBy,
                MakeDate = f.MakeDate,
                Remarks = f.Remarks
            }).OrderBy(i => i.Id).ToListAsync();
            var combined = string.Join(", ", ret);
            this._logger.LogInformation("Response Data: " + System.Environment.NewLine + combined.ToString());
            return Ok(ret);
        }


        //Get all files from server
        [HttpGet]
        [Route("GetFilesFromServer")]
        public ActionResult<List<string>> GetFilesFromServer()
        {
            this._logger.LogInformation("GetFilesFromServer: ");
            var server = _config.GetValue<string>("SFTPHost");
            var license = _config.GetValue<string>("SFTPLicense");
            var port = Convert.ToInt32(_config.GetValue<string>("SFTPPort"));
            var user = _config.GetValue<string>("SFTPUser");
            var password = _config.GetValue<string>("SFTPPassword");
            var localAttachmentPath = _config.GetValue<string>("LocalAttachmentPath");
            var ret = SFTP.GetFileNamesfromServer(license, server, port, user, password);
            var combined = string.Join(", ", ret);
            this._logger.LogInformation("Response Data: " + System.Environment.NewLine + combined.ToString());
            return Ok(ret);
        }


        //Get all files from server
        [HttpPost]
        [Route("DownloadFilesFromServer")]
        public ActionResult<string> DownloadFilesFromServer()
        {
            this._logger.LogInformation("DownloadFilesFromServer: ");
            var server = _config.GetValue<string>("SFTPHost");
            var license = _config.GetValue<string>("SFTPLicense");
            var port = Convert.ToInt32(_config.GetValue<string>("SFTPPort"));
            var user = _config.GetValue<string>("SFTPUser");
            var password = _config.GetValue<string>("SFTPPassword");
            var localAttachmentPath = _config.GetValue<string>("LocalAttachmentPath");
            var ret = SFTP.DownloadFileNamesfromServer(license, server, port, user, password, localAttachmentPath);

            this._logger.LogInformation("Download Done!");
            return Ok("Download Complete");
        }


    }
}