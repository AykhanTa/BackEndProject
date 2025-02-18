﻿namespace BackEndProject.Extensions
{
    public static partial class Extension
    {
        public static bool IsImage(this IFormFile file) 
            => file.ContentType.Contains("image");

        public static bool IsCorrectSize(this IFormFile file, int size)
            => file.Length / 1024 > size;

        public async static Task<string> SaveFile(this IFormFile file,string folderName)
        {
            var filename = Guid.NewGuid() + Path.GetExtension(file.FileName);
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "img", $"{folderName}", filename);
            using FileStream fileStream = new(path, FileMode.Create);
            await file.CopyToAsync(fileStream);
            return filename;
        }
    }
}
