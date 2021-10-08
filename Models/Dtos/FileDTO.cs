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
            public class ResponseDTO
            {

            }

            public UploadDTO()
            {
                Request = new RequestDTO();
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
            public class ResponseDTO
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
            public class ResponseDTO
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
            public class ResponseDTO
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
            public class ResponseDTO
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
    }
}
