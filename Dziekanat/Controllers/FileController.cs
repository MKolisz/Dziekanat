using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Dziekanat.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Dziekanat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {

        private readonly FileContext _fileContext;
        private readonly EmployeeContext _employeeContext;
        private IMapper _mapper;

        private readonly string storagePath = "C:\\Users\\mkolisz11\\Desktop\\Dziekanat_storage\\";

        public FileController(FileContext fileContext, EmployeeContext employeeContext, IMapper mapper)
        {
            _fileContext = fileContext;
            _employeeContext = employeeContext;
            _mapper = mapper;
        }

        [Authorize(Roles = "Lecturer")]
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            //files are stored on the server, database stores only file info

            //create file object in database storing real name of the file, id and employeeId
            int myId = int.Parse(User.Identity.Name);
            Entities.File fileParam = new Entities.File() { Employee_Id = myId, File_Name = file.FileName, Storage_Name = "tmp" };
            _fileContext.File.Add(fileParam);
            _fileContext.SaveChanges();

            // create the storage file name
            string newFileName = DateTime.Now + "_" + fileParam.File_Id;
            newFileName = newFileName.Replace(".", "-");
            newFileName = newFileName.Replace(":", "-");
            newFileName = newFileName + "." + file.FileName.Split(".")[1];


            //insert storage name into database
            fileParam.Storage_Name = newFileName;
            _fileContext.File.Update(fileParam);
            _fileContext.SaveChanges();

            // create directory for employee
            string dirName = _employeeContext.Employee.Find(myId).First_Name + "_" + _employeeContext.Employee.Find(myId).Last_Name;
            string dirPath = Path.Combine(storagePath, dirName);
            Directory.CreateDirectory(dirPath);

            // create filepath
            var filePath = Path.Combine(dirPath, newFileName);

            // Create a new file in employee directory
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                //copy the contents of the received file to the newly created local file 
                await file.CopyToAsync(stream);
            }
            // return the file name for the locally stored file
            return Ok(newFileName);
        }


        [Authorize(Roles = "Lecturer, Student")]
        [HttpGet("download/{id}")]
        public async Task<IActionResult> Download(int id)
        {
            int employeeId = _fileContext.File.Find(id).Employee_Id;
            string employeeName = _employeeContext.Employee.Find(employeeId).First_Name + "_" + _employeeContext.Employee.Find(employeeId).Last_Name;
            string path = storagePath + employeeName;

            string[] fileNames = Directory.GetFiles(path);

            foreach (string file in fileNames)
            {
                var splt = file.Split("\\");
                if (splt[splt.Length-1].Split("_")[1].Split(".")[0] == id.ToString())
                {
                    path = file;
                    break;
                }
            }

            if (System.IO.File.Exists(path))
            {

                // Get all bytes of the file and return the file with the specified file contents 
                byte[] b = await System.IO.File.ReadAllBytesAsync(path);
                Response.Headers.Add("filename",_fileContext.File.Find(id).File_Name);
                return File(b, "application/octet-stream");

                
            }

            else
            {
                // return error if file not found
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

    }
}
