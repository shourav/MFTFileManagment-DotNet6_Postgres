using AutoMapper;
using MFTFileManagment.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MFTFileManagment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileManagementAPIController : ControllerBase
    {
        #region "Variables"

        private readonly FileDataContext _context;
        private readonly ILogger<FileManagementAPIController> _logger;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        #endregion "Variables"

        #region "Constructor"

        //Dependency Injection through Constructor
        public FileManagementAPIController(FileDataContext context, ILogger<FileManagementAPIController> logger, IConfiguration config, IMapper mapper)
        {
            this._context = context;
            _logger = logger;
            _config = config;
            _mapper = mapper;
        }

        #endregion "Constructor"

        #region "Basic CRUD"

        //Get all files from database
        [HttpGet]
        [Route("GetFilesFromDB")]
        public async Task<ActionResult<List<FileViewModel>>> GetFilesFromDB()
        {
            this._logger.LogInformation("GetFilesFromDB: ");
            var dbRecords = await this._context.Files.OrderBy(j => j.Id).ToListAsync();
            var ret = _mapper.Map<List<FileViewModel>>(dbRecords);

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
            var ret = _mapper.Map<FileViewModel>(dbFile);
            this._logger.LogInformation("Response Data: " + System.Environment.NewLine + ret.ToString());
            return Ok(ret);
        }

        //Add a new file in database
        [HttpPost]
        [Route("AddFilesInDB")]
        public async Task<ActionResult<List<FileViewModel>>> AddFilesInDB(FileViewModel file)
        {
            this._logger.LogInformation("AddFilesInDB: " + System.Environment.NewLine + file.ToString());
            var dbFile = _mapper.Map<Documents.Data.File>(file);
            this._context.Files.Add(dbFile);
            await this._context.SaveChangesAsync();
            var dbRecords = await this._context.Files.OrderBy(j => j.Id).ToListAsync();
            var ret = _mapper.Map<List<FileViewModel>>(dbRecords);
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
            dbFile.CreationTime = request.CreationTime;
            await this._context.SaveChangesAsync();
            var dbRecords = await this._context.Files.OrderBy(j => j.Id).ToListAsync();
            var ret = _mapper.Map<List<FileViewModel>>(dbRecords);
            var combined = string.Join(", ", ret);
            this._logger.LogInformation("Response Data: " + System.Environment.NewLine + combined.ToString());
            return Ok(ret);
        }

        //delete a file from database
        [HttpDelete]
        [Route("DeleteFileByIdFromDB")]
        public async Task<ActionResult<List<FileViewModel>>> DeleteFileByIdFromDB(long id)
        {
            this._logger.LogInformation("DeleteFileFromDB: " + id.ToString());
            var dbFile = await this._context.Files.FindAsync(id);
            if (dbFile == null)
                return BadRequest("File not found!");
            this._context.Files.Remove(dbFile);
            await this._context.SaveChangesAsync();
            var dbRecords = await this._context.Files.OrderBy(j => j.Id).ToListAsync();
            var ret = _mapper.Map<List<FileViewModel>>(dbRecords);
            var combined = string.Join(", ", ret);
            this._logger.LogInformation("Response Data: " + System.Environment.NewLine + combined.ToString());
            return Ok(ret);
        }

        //delete all files from database
        [HttpDelete]
        [Route("DeleteAllFilesFromDB")]
        public async Task<ActionResult<string>> DeleteAllFilesFromDB()
        {
            this._logger.LogInformation("DeleteAllFilesFromDB: ");
            string ret = "All files deleted from database";
            var records = from m in _context.Files
                          select m;
            foreach (var record in records)
            {
                _context.Files.Remove(record);
            }
            await this._context.SaveChangesAsync();

            this._logger.LogInformation(ret);
            return Ok(ret);
        }

        #endregion "Basic CRUD"

        #region "SFTP Server"

        //Get all files from server
        [HttpGet]
        [Route("GetFilesFromServer")]
        public async Task<ActionResult<List<string>>> GetFilesFromServer()
        {
            this._logger.LogInformation("GetFilesFromServer: ");
            var server = _config.GetValue<string>("SFTPHost");
            var license = _config.GetValue<string>("SFTPLicense");
            var port = Convert.ToInt32(_config.GetValue<string>("SFTPPort"));
            var user = _config.GetValue<string>("SFTPUser");
            var password = _config.GetValue<string>("SFTPPassword");
            var localAttachmentPath = _config.GetValue<string>("LocalAttachmentPath");
            var ret = await SFTP.GetFileNamesfromServer(license, server, port, user, password);
            var combined = string.Join(", ", ret);
            this._logger.LogInformation("Response Data: " + Environment.NewLine + combined.ToString());
            return Ok(ret);
        }

        //Get all files from server
        [HttpPost]
        [Route("DownloadFilesFromServer")]
        public async Task<ActionResult<List<string>>> DownloadFilesFromServer(string makeBy)
        {
            this._logger.LogInformation("DownloadFilesFromServer: ");
            var server = _config.GetValue<string>("SFTPHost");
            var license = _config.GetValue<string>("SFTPLicense");
            var port = Convert.ToInt32(_config.GetValue<string>("SFTPPort"));
            var user = _config.GetValue<string>("SFTPUser");
            var password = _config.GetValue<string>("SFTPPassword");
            var localAttachmentPath = _config.GetValue<string>("LocalAttachmentPath");
            this._logger.LogInformation("Connecting to SFTP Server:"
                                                + " server: " + server
                                                + " port: " + port
                                                + " user: " + user
                                                + " password: " + password
                                                + " license: " + license
                                                + " | Attempting to save in local folder: " + localAttachmentPath
                                                );
            var ret = await SFTP.DownloadFileNamesfromServer(makeBy, this._context, _mapper, license, server, port, user, password, localAttachmentPath);
            var combined = string.Join(Environment.NewLine, ret);
            this._logger.LogInformation("Saved following files: " + Environment.NewLine + combined.ToString());
            return Ok(ret);
        }

        #endregion "SFTP Server"
    }
}