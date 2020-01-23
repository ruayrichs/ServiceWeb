using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Agape.Lib.DBService;
using System.Data;

namespace ServiceWeb.Services
{
    public class CompanyProfileService
    {
        public class CompanyProfileDetail{
            private DBService dbService = new DBService();

            private CompanyProfileDetail()
            {
            }
            
            private DataTable DTCompanyProfile = null;

            public CompanyProfileDetail(string sid)
            {
                string sql = "select * from SNA_LINK_COMPANY_PROFILE " +
                         "where sid='" + sid + "'";

                DTCompanyProfile = dbService.selectDataFocusone(sql);
            }

            #region property
            public String custBgHeader 
            {
                get
                {
                    string strResult = getProfileValue("Image_Customer_Top");
                    return string.IsNullOrEmpty(strResult) ? "/images/no-img-bg.png" : strResult;
                }
            }
            public String custBgCenter1
            {
                get
                {
                    string strResult = getProfileValue("Image_Customer_Center1");
                    return string.IsNullOrEmpty(strResult) ? "/images/no-img-bg.png" : strResult;
                }
            }
            public String custBgCenter2
            {
                get
                {
                    string strResult = getProfileValue("Image_Customer_Center2");
                    return string.IsNullOrEmpty(strResult) ? "/images/no-img-bg.png" : strResult;
                }
            }
            public String custBgCenter3
            {
                get
                {
                    string strResult = getProfileValue("Image_Customer_Center3");
                    return string.IsNullOrEmpty(strResult) ? "/images/no-img-bg.png" : strResult;
                }
            }
            public String custBgFooter
            {
                get
                {
                    string strResult = getProfileValue("Image_Customer_Bottom");
                    return string.IsNullOrEmpty(strResult) ? "/images/no-img-bg.png" : strResult;
                }
            }
            public string custProfileImage
            {
                get
                {
                    string strResult = getProfileValue("Image_Profile");
                    return string.IsNullOrEmpty(strResult) ? "/images/no-img-profile.png" : strResult;
                }
            }
            public string LoadingBackground
            {
                get
                {
                    string strResult = getProfileValue("Image_Background");
                    return string.IsNullOrEmpty(strResult) ? "/images/no-img-bg.png" : strResult;
                }
            }
            public string LoadingBackgroundMobile
            {
                get
                {
                    string strResult = getProfileValue("Image_Background_Mobile");
                    return string.IsNullOrEmpty(strResult) ? "/images/no-img-bg.png" : strResult;
                }
                
            }
            public string custProfileName
            {
                get
                {
                    return getProfileValue("Name");
                }

            }
            public string custProfileDetail
            {
                get
                {
                    return getProfileValue("Detail");
                }

            }

            private String getProfileValue(String colName)
            {
                if (DTCompanyProfile == null || DTCompanyProfile.Rows.Count == 0)
                {
                    return "";
                }
                return DTCompanyProfile.Rows[0][colName].ToString();
            }
            #endregion
        }

        private DBService dbService = new DBService();
        private static CompanyProfileService _instance = null;

        public static CompanyProfileService getInstance()
        {
            if (_instance == null)
                _instance = new CompanyProfileService();
            return _instance;
        }

        public DataTable getComanyProfile(string sid)
        {
            string sql = "select * from SNA_LINK_COMPANY_PROFILE " +
                         "where sid='" + sid + "'";

            DataTable dt = dbService.selectDataFocusone(sql);
            return dt;
        }

        public void addComanyProfile(string sid, string name, string detail, string imageprofile, string imagebackground, string imagebackgroundmobile, string imagecover,
            string facebookID, string twitterID, string instragramID, string lineID, string googleplusID, string SEOtitle, string SEOdescription, string SEOkeyword,
            string SEOauthor, string SEOimage, string imagecustomertop, string imagecustomercenter1, string imagecustomercenter2, string imagecustomercenter3, string imagecustomerbottom, string createdon, string createdby)
        {
            string sql = "insert into SNA_LINK_COMPANY_PROFILE(SID, Name, Detail, Image_Profile, Image_Background, Image_Background_Mobile, Image_Cover, Social_FacebookID, Social_TwitterID," +
                         "Social_InstragramID, Social_LineID, Social_GooglePlusID, SEO_Title, SEO_Description, SEO_Keyword, SEO_Author, SEO_Image, Image_Customer_Top, Image_Customer_Center1," +
                         "Image_Customer_Center2, Image_Customer_Center3, Image_Customer_Bottom, created_on, created_by, updated_on, updated_by) " +
                         "VALUES ('" + sid + "','" + name + "','" + detail + "','" + imageprofile + "','" + imagebackground + "','" + imagebackgroundmobile + "','" + imagecover + "'," +
                         "'" + facebookID + "','" + twitterID + "','" + instragramID + "','" + lineID + "','" + googleplusID + "','" + SEOtitle + "','" + SEOdescription + "','" + SEOkeyword + "'," +
                         "'" + SEOauthor + "','" + SEOimage + "','" + imagecustomertop + "','" + imagecustomercenter1 + "','" + imagecustomercenter2 + "','" + imagecustomercenter3 + "','" + imagecustomerbottom + "','" + createdon + "','" + createdby + "','','')";

            dbService.executeSQLForFocusone(sql);
        }

        public void editComanyProfile(string sid, string name, string detail, string imageprofile, string imagebackground, string imagebackgroundmobile, string imagecover,
            string facebookID, string twitterID, string instragramID, string lineID, string googleplusID, string SEOtitle, string SEOdescription, string SEOkeyword,
            string SEOauthor, string SEOimage, string imagecustomertop, string imagecustomercenter1, string imagecustomercenter2, string imagecustomercenter3, 
            string imagecustomerbottom, string updatedon, string updatedby)
        {
            string sql = "UPDATE [SNA_LINK_COMPANY_PROFILE] SET " +
                         "[Name] = '" + name + "' " +
                         ",[Detail] = '" + detail + "' " +
                         ",[Social_FacebookID] = '" + facebookID + "' " +
                         ",[Social_TwitterID] = '" + twitterID + "' " +
                         ",[Social_InstragramID] = '" + instragramID + "' " +
                         ",[Social_LineID] = '" + lineID + "' " +
                         ",[Social_GooglePlusID] = '" + googleplusID + "' " +
                         ",[SEO_Title] = '" + SEOtitle + "' " +
                         ",[SEO_Description] = '" + SEOdescription + "' " +
                         ",[SEO_Keyword] = '" + SEOkeyword + "' " +
                         ",[SEO_Author] = '" + SEOauthor + "' " +
                         ",[SEO_Image] = '" + SEOimage + "' " +
                         ",[updated_on] = '" + updatedon + "' " +
                         ",[updated_by] = '" + updatedby + "' ";
                        if (!string.IsNullOrEmpty(imageprofile))
                        {
                            sql += ",[Image_Profile] = '" + imageprofile + "'";
                        }
                        if (!string.IsNullOrEmpty(imagebackground))
                        {
                            sql += ",[Image_Background] = '" + imagebackground + "'";
                        }
                        if (!string.IsNullOrEmpty(imagebackgroundmobile))
                        {
                            sql += ",[Image_Background_Mobile] = '" + imagebackgroundmobile + "'";
                        }
                        if (!string.IsNullOrEmpty(imagecover))
                        {
                            sql += ",[Image_Cover] = '" + imagecover + "'";
                        }
                        if (!string.IsNullOrEmpty(imagecustomertop))
                        {
                            sql += ",[Image_Customer_Top] = '" + imagecustomertop + "'";
                        }
                        if (!string.IsNullOrEmpty(imagecustomercenter1))
                        {
                            sql += ",[Image_Customer_Center1] = '" + imagecustomercenter1 + "'";
                        }
                        if (!string.IsNullOrEmpty(imagecustomercenter2))
                        {
                            sql += ",[Image_Customer_Center2] = '" + imagecustomercenter2 + "'";
                        }
                        if (!string.IsNullOrEmpty(imagecustomercenter3))
                        {
                            sql += ",[Image_Customer_Center3] = '" + imagecustomercenter3 + "'";
                        }
                        if (!string.IsNullOrEmpty(imagecustomerbottom))
                        {
                            sql += ",[Image_Customer_Bottom] = '" + imagecustomerbottom + "'";
                        }

                        sql += "WHERE SID = '" + sid + "'";
                         
            dbService.executeSQLForFocusone(sql);
        }
    }
}