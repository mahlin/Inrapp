using Inrapporteringsportal.DataAccess.Repositories;
using InrapporteringsPortal.ApplicationService.DTOModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using InrapporteringsPortal.DataAccess;
using InrapporteringsPortal.DomainModel;

namespace InrapporteringsPortal.ApplicationService.Helpers
{
    public class FilesHelper
    {
        private readonly IPortalRepository _portalRepository;
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        String DeleteURL = null;
        String DeleteType = null;
        String StorageRoot = null;
        String UrlBase = null;
        String tempPath = null;
        //ex:"~/Files/something/";
        String serverMapPath = null;

        public FilesHelper(String DeleteURL, String DeleteType, String StorageRoot, String UrlBase, String tempPath, String serverMapPath)
        {
            this.DeleteURL = DeleteURL;
            this.DeleteType = DeleteType;
            this.StorageRoot = StorageRoot;
            this.UrlBase = UrlBase;
            this.tempPath = tempPath;
            this.serverMapPath = serverMapPath;
            _portalRepository = new PortalRepository(db);
        }

        public FilesHelper(String StorageRoot)
        {
            this.StorageRoot = StorageRoot;
            _portalRepository = new PortalRepository(db);
        }

        public void DeleteFiles(String pathToDelete)
        {
         
            string path = HostingEnvironment.MapPath(pathToDelete);

            System.Diagnostics.Debug.WriteLine(path);
            if (Directory.Exists(path))
            {
                DirectoryInfo di = new DirectoryInfo(path);
                foreach (FileInfo fi in di.GetFiles())
                {
                    System.IO.File.Delete(fi.FullName);
                    System.Diagnostics.Debug.WriteLine(fi.Name);
                }

                di.Delete(true);
            }
        }

        public String DeleteFile(String file)
        {
            System.Diagnostics.Debug.WriteLine("DeleteFile");
            //    var req = HttpContext.Current;
            System.Diagnostics.Debug.WriteLine(file);
 
            String fullPath = Path.Combine(StorageRoot, file);
            System.Diagnostics.Debug.WriteLine(fullPath);
            System.Diagnostics.Debug.WriteLine(System.IO.File.Exists(fullPath));
            String thumbPath = "/" + file + "80x80.jpg";
            String partThumb1 = Path.Combine(StorageRoot, "thumbs");
            String partThumb2 = Path.Combine(partThumb1, file + "80x80.jpg");

            System.Diagnostics.Debug.WriteLine(partThumb2);
            System.Diagnostics.Debug.WriteLine(System.IO.File.Exists(partThumb2));
            if (System.IO.File.Exists(fullPath))
            {
                //delete thumb 
                if (System.IO.File.Exists(partThumb2))
                {
                    System.IO.File.Delete(partThumb2);
                }
                System.IO.File.Delete(fullPath);
                String succesMessage = "Ok";
                return succesMessage;
            }
            String failMessage = "Error Delete";
            return failMessage;
        }
        public JsonFiles GetFileList()
        {

            var r = new List<ViewDataUploadFilesResult>();
       
            String fullPath = Path.Combine(StorageRoot);
            if (Directory.Exists(fullPath))
            {
                DirectoryInfo dir = new DirectoryInfo(fullPath);
                foreach (FileInfo file in dir.GetFiles())
                {
                    int SizeInt = unchecked((int)file.Length);
                    r.Add(UploadResult(file.Name,SizeInt,file.FullName));
                }

            }
            JsonFiles files = new JsonFiles(r);

            return files;
        }

        public void UploadAndShowResults(HttpContextBase ContentBase, List<ViewDataUploadFilesResult> resultList, string userId, string userName, string kommunKod, int selectedRegisterId, List<RegisterInfo> registerList)
        {
            var httpRequest = ContentBase.Request;
            //System.Diagnostics.Debug.WriteLine(Directory.Exists(tempPath));

            //Kolla vilket register filen/filerna hör till och skapa mapp om det behövs
            var slussmapp = registerList.Where(x => x.Id == selectedRegisterId).Select(x => x.Slussmapp).Single();
            var forvantadLevId = registerList.Where(x => x.Id == selectedRegisterId).Select(x => x.ForvantadLevransId).Single();
            StorageRoot = StorageRoot + slussmapp + "\\";
            String fullPath = Path.Combine(StorageRoot);
            Directory.CreateDirectory(fullPath);

            //hämta ett leveransId och skapa hashAddOn till filnamnet
            var orgId = _portalRepository.GetUserOrganisation(userId);
            //TODO - skicka med forvantadleveransid
            var levId = _portalRepository.GetNewLeveransId(userId, userName, orgId, selectedRegisterId, forvantadLevId);
            var hash = GetHashAddOn(kommunKod, levId);
            var headers = httpRequest.Headers;

            if (string.IsNullOrEmpty(headers["X-File-Name"]))
            {
                UploadWholeFile(ContentBase, resultList, hash, levId);
            }

            //TODO - Test EncryptDecrypt
            //var krypteradUtfil = "C:\\Socialstyrelsen\\KrypteringTest\\krypteradUtfil.txt";
            //var dekrypteradUtfil = "C:\\Socialstyrelsen\\KrypteringTest\\dekrypteradUtfil.txt";
            //var storKrypteradUtfil = "C:\\Socialstyrelsen\\KrypteringTest\\storKrypteradUtfil.txt";
            //var storDekrypteradUtfil = "C:\\Socialstyrelsen\\KrypteringTest\\storDekrypteradUtfil.txt";

            //EncryptDecrypt.AES_Encrypt("C:\\Socialstyrelsen\\KrypteringTest\\testfil.txt", krypteradUtfil);
            //EncryptDecrypt.AES_Decrypt("C:\\Socialstyrelsen\\KrypteringTest\\krypteradUtfil.txt", dekrypteradUtfil);
            //EncryptDecrypt.AES_Encrypt("C:\\Socialstyrelsen\\KrypteringTest\\Ekb_0330_201707_20170815T1011.txt", storKrypteradUtfil);
            //EncryptDecrypt.AES_Decrypt("C:\\Socialstyrelsen\\KrypteringTest\\storKrypteradUtfil.txt", storDekrypteradUtfil);

        }


        private void UploadWholeFile(HttpContextBase requestContext, List<ViewDataUploadFilesResult> statuses, string hash, int levId)
        {
            var request = requestContext.Request;
            for (int i = 0; i < request.Files.Count; i++)
            {
                var file = request.Files[i];

                //TODO - check filename depending on chosen registertype
                if (file.ContentLength > 0 && (Path.GetExtension(file.FileName) == ".txt" || Path.GetExtension(file.FileName) == ".xls"))
                {
                    String pathOnServer = Path.Combine(StorageRoot);
                    //var fullPath = Path.Combine(pathOnServer, Path.GetFileName(file.FileName));
                    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);
                    var extension = Path.GetExtension(file.FileName);
                    var filOfFilesAddOn = "!" + (i + 1).ToString() + "!" + (request.Files.Count).ToString();
                    var timestamp = DateTime.Now.ToString("yyyyMMdd" + "T"+ "HHmmss"); 
                    var extendedFileName = fileNameWithoutExtension + hash + filOfFilesAddOn + "!" + timestamp + extension;
                    var fullPath = Path.Combine(pathOnServer, Path.GetFileName(extendedFileName));
                    file.SaveAs(fullPath);

                    statuses.Add(UploadResult(file.FileName, file.ContentLength, file.FileName, (extendedFileName), levId, i+1));
                }
            }
        }

        public IEnumerable<FilloggDetaljDTO> HamtaFillogg(int leveransId)
        {
            var filloggar = _portalRepository.GetFilerForLeveransId(leveransId);

            return null;

            //return filloggar.Select(FilloggDetaljDTO.)
        }


        private void UploadPartialFile(string fileName, HttpContextBase requestContext, List<ViewDataUploadFilesResult> statuses)
        {
            var request = requestContext.Request;
            if (request.Files.Count != 1) throw new HttpRequestValidationException("Attempt to upload chunked file containing more than one fragment per request");
            var file = request.Files[0];
            var inputStream = file.InputStream;
            String patchOnServer = Path.Combine(StorageRoot);
            var fullName = Path.Combine(patchOnServer, Path.GetFileName(file.FileName));
            var ThumbfullPath = Path.Combine(fullName, Path.GetFileName(file.FileName + "80x80.jpg"));
            //TODO - rensa imagehandler stuff
            //ImageHandler handler = new ImageHandler();

            //var ImageBit = ImageHandler.LoadImage(fullName);
            //handler.Save(ImageBit, 80, 80, 10, ThumbfullPath);
            using (var fs = new FileStream(fullName, FileMode.Append, FileAccess.Write))
            {
                var buffer = new byte[1024];

                var l = inputStream.Read(buffer, 0, 1024);
                while (l > 0)
                {
                    fs.Write(buffer, 0, l);
                    l = inputStream.Read(buffer, 0, 1024);
                }
                fs.Flush();
                fs.Close();
            }
            statuses.Add(UploadResult(file.FileName, file.ContentLength, file.FileName));
        }

        private string GetHashAddOn(string kommunKod, int levId)
        {
            var hashAddOn = String.Empty;

            hashAddOn = "#" + kommunKod + "!" + levId;

            return hashAddOn;
        }
        public ViewDataUploadFilesResult UploadResult(String FileName,int fileSize,String FileFullPath, String SosFileName, int LeveransId, int SequenceNumber)
        {
            String getType = System.Web.MimeMapping.GetMimeMapping(FileFullPath);
            var result = new ViewDataUploadFilesResult()
            {
                name = FileName,
                size = fileSize,
                type = getType,
                url = UrlBase + FileName,
                deleteUrl = DeleteURL + FileName,
                thumbnailUrl = CheckThumb(getType, FileName),
                deleteType = DeleteType,
                sosName = SosFileName,
                leveransId = LeveransId,
                sequenceNumber = SequenceNumber
            };
            return result;
        }

        public ViewDataUploadFilesResult UploadResult(String FileName, int fileSize, String FileFullPath)
        {
            String getType = System.Web.MimeMapping.GetMimeMapping(FileFullPath);
            var result = new ViewDataUploadFilesResult()
            {
                name = FileName,
                size = fileSize,
                type = getType,
                url = UrlBase + FileName,
                deleteUrl = DeleteURL + FileName,
                thumbnailUrl = CheckThumb(getType, FileName),
                deleteType = DeleteType
            };
            return result;
        }

        public String CheckThumb(String type,String FileName)
        {
            var splited = type.Split('/');
            if (splited.Length == 2)
            {
                string extansion = splited[1].ToLower();
                if(extansion.Equals("jpeg") || extansion.Equals("jpg") || extansion.Equals("png") || extansion.Equals("gif"))
                {
                    String thumbnailUrl = UrlBase + "thumbs/" + Path.GetFileNameWithoutExtension(FileName) + "80x80.jpg";
                    return thumbnailUrl;
                }
                else
                {
                    if (extansion.Equals("octet-stream")) //Fix for exe files
                    {
                        return "/Content/Free-file-icons/48px/exe.png";

                    }
                    if (extansion.Contains("zip")) //Fix for exe files
                    {
                        return "/Content/Free-file-icons/48px/zip.png";
                    }
                    String thumbnailUrl = "/Content/Free-file-icons/48px/"+ extansion +".png";
                    return thumbnailUrl;
                }
            }
            else
            {
                return UrlBase + "/thumbs/" + Path.GetFileNameWithoutExtension(FileName) + "80x80.jpg";
            }
           
        }
        public List<String> FilesList()
        {

            List<String> Filess = new List<String>();
            string path = HostingEnvironment.MapPath(serverMapPath);
            System.Diagnostics.Debug.WriteLine(path);
            if (Directory.Exists(path))
            {
                DirectoryInfo di = new DirectoryInfo(path);
                foreach (FileInfo fi in di.GetFiles())
                {
                    Filess.Add(fi.Name);
                    System.Diagnostics.Debug.WriteLine(fi.Name);
                }

            }
            return Filess;
        }
    }
    public class ViewDataUploadFilesResult
    {
        public string name { get; set; }
        public int size { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public string deleteUrl { get; set; }
        public string thumbnailUrl { get; set; }
        public string deleteType { get; set; }
        public string sosName { get; set; }
        public int leveransId { get; set; }
        public int sequenceNumber { get; set; }
    }
    public class JsonFiles
    {
        public ViewDataUploadFilesResult[] files;
        public string TempFolder { get; set; }
        public JsonFiles(List<ViewDataUploadFilesResult> filesList)
        {
            files = new ViewDataUploadFilesResult[filesList.Count];
            for (int i = 0; i < filesList.Count; i++)
            {
                files[i] = filesList.ElementAt(i);
            }

        }
    }
}

    