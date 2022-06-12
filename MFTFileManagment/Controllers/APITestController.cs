using MFTFileManagment.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MFTFileManagment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APITestController : ControllerBase
    {

        private readonly FileDataContext _context;

        //Dependency Injection through Constructor
        public APITestController(FileDataContext context)
        {
            this._context = context;
        }




        //Get all files from database
        [HttpGet]
        [Route("GetFilesFromDB")]
        public async Task<ActionResult<List<FileViewModel>>> GetFilesFromDB()
        {

            return Ok(await this._context.Files.Select(f => new FileViewModel
            {
                Id = f.Id,
                Name = f.Name,
                Path = f.Path,
                Extension = f.Extension,
                MakeBy = f.MakeBy,
                MakeDate = f.MakeDate,
                Remarks = f.Remarks
            }).OrderBy(i => i.Id).ToListAsync());
        }

        //Get single file by id from database
        [HttpGet]
        [Route("GetFileById")]
        public async Task<ActionResult<FileViewModel>> GetFileById(long id)
        {
            var dbFile = await this._context.Files.FindAsync(id);
            if (dbFile == null)
                return BadRequest("File not found!");
            return Ok(new FileViewModel
            {
                Id = dbFile.Id,
                Name = dbFile.Name,
                Path = dbFile.Path,
                Extension = dbFile.Extension,
                MakeBy = dbFile.MakeBy,
                MakeDate = dbFile.MakeDate,
                Remarks = dbFile.Remarks
            });
        }

        //Add a new file in database
        [HttpPost]
        [Route("AddFilesInDB")]
        public async Task<ActionResult<List<FileViewModel>>> AddFilesInDB(FileViewModel file)
        {

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

            return Ok(await this._context.Files.Select(f => new FileViewModel
            {
                Id = f.Id,
                Name = f.Name,
                Path = f.Path,
                Extension = f.Extension,
                MakeBy = f.MakeBy,
                MakeDate = f.MakeDate,
                Remarks = f.Remarks
            }).OrderBy(i => i.Id).ToListAsync());
        }

        //Update a file in database
        [HttpPut]
        [Route("UpdateFileInDB")]
        public async Task<ActionResult<List<FileViewModel>>> UpdateFileInDB(FileViewModel request)
        {
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

            return Ok(await this._context.Files.Select(f => new FileViewModel
            {
                Id = f.Id,
                Name = f.Name,
                Path = f.Path,
                Extension = f.Extension,
                MakeBy = f.MakeBy,
                MakeDate = f.MakeDate,
                Remarks = f.Remarks
            }).OrderBy(i => i.Id).ToListAsync());
        }

        //delete a file from database
        [HttpDelete]
        [Route("DeleteFileFromDB")]
        public async Task<ActionResult<List<FileViewModel>>> DeleteFileFromDB(long id)
        {
            var dbFile = await this._context.Files.FindAsync(id);
            if (dbFile == null)
                return BadRequest("File not found!");
            this._context.Files.Remove(dbFile);
            await this._context.SaveChangesAsync();
            return Ok(await this._context.Files.Select(f => new FileViewModel
            {
                Id = f.Id,
                Name = f.Name,
                Path = f.Path,
                Extension = f.Extension,
                MakeBy = f.MakeBy,
                MakeDate = f.MakeDate,
                Remarks = f.Remarks
            }).OrderBy(i => i.Id).ToListAsync());
        }


    }
}