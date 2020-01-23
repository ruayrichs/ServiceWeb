using ERPW.Lib.Authentication;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ServiceWeb.Service
{
    public class PublicAuthentication
    {
        public static string SID
        {
            get
            {
                return ERPWAuthentication.SID;
            }
        }

        public static string CompanyCode
        {
            get
            {
                return ERPWAuthentication.CompanyCode;
            }
        }


        public static string FocusOneLinkProfileImage
        {
            get
            {
                string DisplayImage = "";

                if (string.IsNullOrEmpty(ERPWAuthentication.EmployeeCode))
                {
                    DisplayImage = "/images/user_avatar.png";
                }
                else
                {
                    DisplayImage += new UserImageService.UserImage(
                        ERPWAuthentication.CompanyCode,
                        ERPWAuthentication.SID,
                        ERPWAuthentication.EmployeeCode,
                        ERPWAuthentication.LatestUpdatedOn
                    ).Image_128_WithOutCheckFile;
                    if (DisplayImage.IndexOf("?") > 0)
                    {
                        DisplayImage = DisplayImage.Substring(0, DisplayImage.IndexOf("?"));
                    }
                    string ServerPath = HttpContext.Current.Server.MapPath("~" + DisplayImage);
                    if (!File.Exists(ServerPath))
                    {
                        DisplayImage = "/images/user_avatar.png";
                    }
                }

                return DisplayImage;
            }
        }

        public static string FocusOneLinkProfileImageByEmployeeCode(string employeeCode)
        {
            string DisplayImage = "";

            if (string.IsNullOrEmpty(employeeCode))
            {
                DisplayImage = "/images/user_avatar.png";
            }
            else
            {
                DisplayImage += new UserImageService.UserImage(
                    PublicAuthentication.CompanyCode,
                    PublicAuthentication.SID,
                    employeeCode,
                    ""
                ).Image_128_WithOutCheckFile;

                string ServerPath = HttpContext.Current.Server.MapPath("~" + DisplayImage);
                if (!File.Exists(ServerPath))
                {
                    DisplayImage = "/images/user_avatar.png";
                }
            }

            return DisplayImage;
        }
    }
}