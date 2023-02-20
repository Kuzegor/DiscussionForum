using DiscussionForum.Models;
using Microsoft.AspNetCore.Hosting;

namespace DiscussionForum.Helpers
{
    public static class ImageHelper
    {
        //TODO: Add the following methods to ImageHelper class: SaveImage, DeleteImage
        public static bool ImageIsValid(IFormFile imageFile)
        {
            string fileNameLower = imageFile.FileName.ToLower();
            string[] extensions = { "jpg", "png", "jfif", "jpeg", "gif" };
            bool fileIsValid = false;
            for (int i = 0; i < extensions.Length; i++)
            {
                if (fileNameLower.EndsWith(extensions[i]))
                {
                    fileIsValid = true;
                    break;
                }
            }
            return fileIsValid;
        }
    }
}
