using DiscussionForum.Models;
using Microsoft.AspNetCore.Hosting;

namespace DiscussionForum.Helpers
{
    public static class ImageHelper
    {
        //TODO: MORE METHODS IN ImageHelper class
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
