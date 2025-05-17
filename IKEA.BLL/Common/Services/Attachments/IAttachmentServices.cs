using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.BLL.Common.Services.Attachments
{
    public interface IAttachmentServices
    {
        public string UploadImage(IFormFile File, string FolderName);

        public bool DeleteImage(string FilePath);
    }
}
