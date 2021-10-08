using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIFicheros.Models.Dtos
{
    public class FileDTO
    {
        #region Upload
        public UploadDTO Upload { get; set; }
        public class UploadDTO
        {
            public RequestDTO Request { get; set; }
            public ResponseDTO Response { get; set; }
            public class RequestDTO
            {
                public string FileName { get; set; }
                public string Data { get; set; }
            }
            public class ResponseDTO : BaseDTO
            {

            }

            public UploadDTO()
            {
                Request = new RequestDTO();
                Response = new ResponseDTO();
            }

            public UploadDTO(string fileName, string data)
            {
                Request = new RequestDTO()
                {
                    FileName = fileName,
                    Data = data
                };
                Response = new ResponseDTO();
            }
        }

        #endregion

        #region Download
        public DownloadDTO Download { get; set; }
        public class DownloadDTO
        {
            public RequestDTO Request { get; set; }
            public ResponseDTO Response { get; set; }
            public class RequestDTO
            {
                public string FileName { get; set; }
            }
            public class ResponseDTO : BaseDTO
            {
                public byte[] Data { get; set; }
            }

            public DownloadDTO()
            {
                Request = new RequestDTO();
                Response = new ResponseDTO();
            }
        }

        #endregion

        #region Delete
        public DeleteDTO Delete { get; set; }
        public class DeleteDTO
        {
            public RequestDTO Request { get; set; }
            public ResponseDTO Response { get; set; }
            public class RequestDTO
            {
                public string FileName { get; set; }
            }
            public class ResponseDTO : BaseDTO
            {

            }

            public DeleteDTO()
            {
                Request = new RequestDTO();
                Response = new ResponseDTO();
            }
        }

        #endregion

        #region ModifyName
        public ModifyNameDTO ModifyName { get; set; }
        public class ModifyNameDTO
        {
            public RequestDTO Request { get; set; }
            public ResponseDTO Response { get; set; }
            public class RequestDTO
            {
                public string OldFileName { get; set; }
                public string NewFileName { get; set; }
            }
            public class ResponseDTO : BaseDTO
            {

            }

            public ModifyNameDTO()
            {
                Request = new RequestDTO();
                Response = new ResponseDTO();
            }
        }

        #endregion

        #region GetURL
        public GetURLDTO GetURL { get; set; }
        public class GetURLDTO
        {
            public RequestDTO Request { get; set; }
            public ResponseDTO Response { get; set; }
            public class RequestDTO
            {
                public string FileName { get; set; }
            }
            public class ResponseDTO : BaseDTO
            {
                public string FileURL { get; set; }
            }

            public GetURLDTO()
            {
                Request = new RequestDTO();
                Response = new ResponseDTO();
            }
        }

        #endregion

        #region GetImageFromExternalAPI
        public GetImageFromExternalAPIDTO GetImageFromExternalAPI { get; set; }
        public class GetImageFromExternalAPIDTO
        {
            public RequestDTO Request { get; set; }
            public ResponseDTO Response { get; set; }
            public class RequestDTO
            {
                public string SearchWord { get; set; }
            }
            public class ResponseDTO : BaseDTO
            {
                public List<PhotoDTO> Photos { get; set; }
            }

            public class PhotoDTO
            {
                public string Id { get; set; }
                public string Description { get; set; }
                public string URL{ get; set; }
            }

            public GetImageFromExternalAPIDTO()
            {
                Request = new RequestDTO();
                Response = new ResponseDTO();
            }
        }

        #endregion

        #region GetImageFromExternalAPIAndUpload
        public GetImageFromExternalAPIAndUploadDTO GetImageFromExternalAPIAndUpload { get; set; }
        public class GetImageFromExternalAPIAndUploadDTO
        {
            public RequestDTO Request { get; set; }
            public ResponseDTO Response { get; set; }
            public class RequestDTO
            {
                public string PhotoId { get; set; }
            }
            public class ResponseDTO : BaseDTO
            {
            }

            public GetImageFromExternalAPIAndUploadDTO()
            {
                Request = new RequestDTO();
                Response = new ResponseDTO();
            }
        }

        #endregion
    }
}
