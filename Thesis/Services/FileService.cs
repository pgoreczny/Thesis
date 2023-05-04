using Microsoft.AspNetCore.StaticFiles;
using Thesis.database;
using Thesis.Models;

namespace Thesis.Services
{
    public class FileService
    {
        private readonly CoursesDBContext context;
        private List<string> extensions = new List<string> { ".pdf", ".doc", ".docx", ".jpg", ".png" };
        public FileService(CoursesDBContext context)
        {
            this.context = context;
        }
        public Models.File makeFile(string content, string name, enConnectionType bindingType, int binding)
        {
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(name);
            string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files", fileName);
            try
            {
                using (StreamWriter streamWriter = new StreamWriter(savePath))
                {
                    streamWriter.Write(content);
                    Models.File fileModel = new Models.File { name = fileName, path = savePath, showName = name, bindingType = bindingType, binding = binding };
                    context.files.Add(fileModel);
                    context.SaveChanges();
                    return fileModel;
            }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public Models.File saveFile(IFormFile file, enConnectionType bindingType, int binding)
        {
            if (file != null)
            {
                if (extensions.Contains(Path.GetExtension(file.FileName)) && file.Length < 4194304)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string SavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files", fileName);
                    try
                    {
                        using (var stream = new FileStream(SavePath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                            Models.File fileModel = new Models.File { name = fileName, path = SavePath, showName = file.FileName, bindingType = bindingType, binding = binding };
                            context.files.Add(fileModel);
                            context.SaveChanges();
                            return fileModel;
                        }
                    }
                    catch (Exception e)
                    {
                        return null;
                    }
                }
            }
            return null;
        }
        public List<Models.File> getFiles(enConnectionType binding, int id)
        {
            return context.files
                .Where(x => x.binding == id && x.bindingType == binding)
                .ToList();
                
        }

        public Models.File getFileModel(Guid id)
        {
            Models.File fileModel = context.files.Find(id);
            if (fileModel == null)
            {
                throw new FileNotFoundException();
            }
            return fileModel;
        }

        public byte[] getFile(Models.File file)
        {
            return System.IO.File.ReadAllBytes(file.path);
        }

        public string getFileType(Models.File file)
        {
            FileExtensionContentTypeProvider fileProvider = new FileExtensionContentTypeProvider();
            if (!fileProvider.TryGetContentType(file.name, out string contentType))
            {
                throw new ArgumentOutOfRangeException();
            }
            return contentType;
        }
    }
}
