using Agape.Lib.DBService;
using ERPW.Lib.Authentication;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace ServiceWeb.Service
{
    public class UserImageService
    {
        private static UserImageService _instance;
        public static UserImageService getInstance()
        {
            if (_instance == null)
            {
                _instance = new UserImageService();
            }
            return _instance;
        }

        public static string PATH_IMAGE = "~/images/";
        public static string PATH_IMAGE_ICON = "~/images/icon/";
        public static string PATH_IMAGE_USER = "~/images/profile/";
        public static string SIZE64 = "64";
        public static string SIZE92 = "92";
        public static string SIZE128 = "128";
        public static string SIZE256 = "256";
        public static string SIZEOriginal = "Original";
        public static string PATH_IMAGE_USER64 = "~/images/profile/64/";
        public static string PATH_IMAGE_USER92 = "~/images/profile/92/";
        public static string PATH_IMAGE_USER128 = "~/images/profile/128/";
        public static string PATH_IMAGE_USER256 = "~/images/profile/256/";
        public static string PATH_IMAGE_USEROriginal = "~/images/profile/Original/";
        public static string PATH_IMAGE_USER_EMPTY = "~/images/user.png";

        public class UserImage
        {
            DBService db = new DBService();

            private string ImagePath64 { get; set; }
            private string ImagePath92 { get; set; }
            private string ImagePath128 { get; set; }
            private string ImagePath256 { get; set; }
            private string ImagePathOriginal { get; set; }

            private string _ImageVersion = "";

            private string GetImageVersion(string SID, string CompanyCode, string EmployeeCode)
            {
                string sql = @"select UPDATED_ON from master_employee
                                where sid= '" + SID + @"'
                                and companycode = '" + CompanyCode + @"'
                                and EmployeeCode = '" + EmployeeCode + "'";
                DataTable dt = db.selectDataFocusone(sql);
                try
                {
                    return dt.Rows[0]["UPDATED_ON"].ToString();
                }
                catch
                {
                    return "";
                }
            }
            private string ImageVersion
            {
                get
                {
                    return string.IsNullOrEmpty(_ImageVersion) ? "" : "?vs=" + _ImageVersion;
                }
                set
                {
                    _ImageVersion = value;
                }
            }
            
            public UserImage(string CompanyCode, string SID, string EmployeeCode, string imgVersion)
            {
                ImagePath64 = PATH_IMAGE_USER64 + SID + "_" + CompanyCode + "_" + EmployeeCode + ".png";
                ImagePath92 = PATH_IMAGE_USER92 + SID + "_" + CompanyCode + "_" + EmployeeCode + ".png";
                ImagePath128 = PATH_IMAGE_USER128 + SID + "_" + CompanyCode + "_" + EmployeeCode + ".png";
                ImagePath256 = PATH_IMAGE_USER256 + SID + "_" + CompanyCode + "_" + EmployeeCode + ".png";
                ImagePathOriginal = PATH_IMAGE_USEROriginal + SID + "_" + CompanyCode + "_" + EmployeeCode + ".png";
                if (string.IsNullOrEmpty(imgVersion))
                {
                    imgVersion = GetImageVersion(SID, CompanyCode, EmployeeCode);
                }
                ImageVersion = imgVersion;
            }

            public static string NotFoundImage
            {
                get
                {
                    return PATH_IMAGE_USER_EMPTY;
                }
            }

            public string Image_64
            {
                get
                {
                    return CheckUserImagePath(ImagePath64).Replace("~", "") + ImageVersion;
                }
            }

            public string Image_92
            {
                get
                {
                    return CheckUserImagePath(ImagePath92).Replace("~", "") + ImageVersion;
                }
            }

            public string Image_128
            {
                get
                {
                    return CheckUserImagePath(ImagePath128).Replace("~", "") + ImageVersion;
                }
            }
            public string Image_256
            {
                get
                {
                    return CheckUserImagePath(ImagePath256).Replace("~", "") + ImageVersion;
                }
            }
            public string Image_Original
            {
                get
                {
                    return CheckUserImagePath(ImagePathOriginal).Replace("~", "") + ImageVersion;
                }
            }
            public string Image_64_WithOutCheckFile
            {
                get
                {
                    return ImagePath64.Replace("~", "");
                }
            }

            public string Image_92_WithOutCheckFile
            {
                get
                {
                    return ImagePath92.Replace("~", "");
                }
            }

            public string Image_128_WithOutCheckFile
            {
                get
                {
                    return ImagePath128.Replace("~", "");
                }
            }
            public string Image_256_WithOutCheckFile
            {
                get
                {
                    return ImagePath256.Replace("~", "");
                }
            }
            public string Image_Original_WithOutCheckFile
            {
                get
                {
                    return ImagePathOriginal.Replace("~", "");
                }
            }

            #region version and with out checkfile

            public string Image_64_WithOutCheckFile_WithVersion
            {
                get
                {
                    return ImagePath64.Replace("~", "") + ImageVersion;
                }
            }

            public string Image_92_WithOutCheckFile_WithVersion
            {
                get
                {
                    return ImagePath92.Replace("~", "") + ImageVersion;
                }
            }

            public string Image_128_WithOutCheckFile_WithVersion
            {
                get
                {
                    return ImagePath128.Replace("~", "") + ImageVersion;
                }
            }
            public string Image_256_WithOutCheckFile_WithVersion
            {
                get
                {
                    return ImagePath256.Replace("~", "") + ImageVersion;
                }
            }
            public string Image_Original_WithOutCheckFile_WithVersion
            {
                get
                {
                    return ImagePathOriginal.Replace("~", "") + ImageVersion;
                }
            }

            #endregion
            public string Image_64_Tilde
            {
                get
                {
                    return CheckUserImagePath(ImagePath64) + ImageVersion;
                }
            }

            public string Image_92_Tilde
            {
                get
                {
                    return CheckUserImagePath(ImagePath92) + ImageVersion;
                }
            }

            public string Image_128_Tilde
            {
                get
                {
                    return CheckUserImagePath(ImagePath128) + ImageVersion;
                }
            }
            public string Image_256_Tilde
            {
                get
                {
                    return CheckUserImagePath(ImagePath256) + ImageVersion;
                }
            }
            public string Image_Original_Tilde
            {
                get
                {
                    return CheckUserImagePath(ImagePathOriginal) + ImageVersion;
                }
            }

            private string CheckUserImagePath(string path)
            {
                if (HttpContext.Current == null)
                {
                    return NotFoundImage;
                }

                if (!File.Exists(HttpContext.Current.Server.MapPath(path)))
                {
                    return NotFoundImage;
                }
                return path;
            }
        }

        public static UserImgModel getImgProfile(string EmployeeCode)
        {
            var imgProfile = new UserImage(
                 PublicAuthentication.CompanyCode,
                 PublicAuthentication.SID,
                 EmployeeCode,
                 ""
             );

            UserImgModel img = new UserImgModel();
            img.Image_64 = imgProfile.Image_64;
            img.Image_92 = imgProfile.Image_92;
            img.Image_128 = imgProfile.Image_128;
            img.Image_256 = imgProfile.Image_256;
            img.Image_Original = imgProfile.Image_Original;
            return img;
        }

        [Serializable]
        public class UserImgModel
        {
            public string Image_64 { get; set; }
            public string Image_92 { get; set; }
            public string Image_128 { get; set; }
            public string Image_256 { get; set; }
            public string Image_Original { get; set; }
        }
    }
}