using agape.lib.web.focusonemenu;
using Agape.FocusOne.Utilities;
using Agape.Lib.DBService;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ServiceWeb.Service
{
    public class MenuService
    {
        private DBService dbservice = new DBService();
        private FocusoneMenuDataService service = new FocusoneMenuDataService();
        private static MenuService _inStance;
        private string sql;
        private DataTable dtData;

        #region structure canAccess page
        private string IsAuthorizationConfig = System.Configuration.ConfigurationSettings.AppSettings["USE_AUTH"];
        public bool CanAll = false;
        public bool CanCreate = false;
        public bool CanChange = false;
        public bool CanDisplay = false;
        public bool CanCopy = false;
        public bool CanDelete = false;
        public bool CanPrint = false;
        public bool CanCancel = false;
        public bool CanReverse = false;


        #endregion

        public static MenuService getInStance()
        {
            if (_inStance == null)
            {
                _inStance = new MenuService();
            }
            return _inStance;
        }

        #region menu service POSWeb
        public string getMenuInFomationString(string sid,string EmpCode)
        {
            return JsonConvert.SerializeObject(getMenuInfomation(sid, EmpCode)).ToString();
        }

        private List<enDataResource> getMenuInfomation(string sid, string EmpCode)
        {
            dtData = getMenuData(sid, EmpCode);
            List<enDataResource> listmenu = new List<enDataResource>();
            enDataResource enMenu;
            enXtype mEnType;
            enMenuData mEnItem;

            foreach (DataRow dr in dtData.Rows)
            {
                enMenu = listmenu.Find(a => a.HeaderID == dr["HeaderID"].ToString());
                if (enMenu == null)
                {
                    enMenu = new enDataResource();
                    enMenu.HeaderID = dr["HeaderID"].ToString();
                    enMenu.HeaderName = dr["HeaderName"].ToString();
                    enMenu.listxType = new List<enXtype>();
                    listmenu.Add(enMenu);
                }

                //list group => Admin,report
                mEnType = enMenu.listxType.Find(a => a.xType == dr["xType"].ToString());
                if (mEnType == null)
                {
                    mEnType = new enXtype();
                    mEnType.xType = dr["xType"].ToString();
                    mEnType.listMenu = new List<enMenuData>();
                    enMenu.listxType.Add(mEnType);
                }

                //menu item 
                mEnItem = new enMenuData();
                mEnItem.ID = dr["MenuID"].ToString();
                mEnItem.Title = dr["MenuTitle"].ToString();
                mEnItem.Subtitle = dr["MenuSubtitle"].ToString();
                mEnItem.url = dr["NAVIGATEURL"].ToString();
                mEnItem.img = dr["NAVIGATEIMAGE"].ToString();
                mEnType.listMenu.Add(mEnItem);

            }


            dtData = null;
            sql = "";
            GC.Collect();
            return listmenu;
        }


        public string getMenuFavoriteString(string sid, string EmpCode)
        {
            return JsonConvert.SerializeObject(getMenuFavorite(sid, EmpCode)).ToString();
        }
        private List<enMenuData> getMenuFavorite(string sid, string EmpCode)
        {
            DataTable dtDataFavorite = getDataTableFavoriteMenu(sid, EmpCode);
            List<enMenuData> listmenu = new List<enMenuData>();
            //enDataResource enMenu;
            //enXtype mEnType;
            enMenuData mEnItem;

            foreach (DataRow dr in dtDataFavorite.Rows)
            {
                //menu item 
                mEnItem = new enMenuData();
                mEnItem.ID = dr["MenuID"].ToString();
                mEnItem.Title = dr["MenuTitle"].ToString();
                mEnItem.Subtitle = dr["MenuSubtitle"].ToString();
                mEnItem.url = dr["NAVIGATEURL"].ToString();
                mEnItem.img = dr["NAVIGATEIMAGE"].ToString();
                listmenu.Add(mEnItem);
            }
            sql = "";
            GC.Collect();
            return listmenu;
        }

        private DataTable getDataTableFavoriteMenu(string sid, string empCode)
        {
            string sql = @"SELECT head.[HeaderID]
                          ,head.[HeaderName]
	                      ,item.xType
	                      ,item.MenuID
	                      ,item.MenuSubtitle
	                      ,item.MenuTitle
	                      ,item.NAVIGATEIMAGE
	                      ,item.NAVIGATEURL
                      FROM [CRM_MENU_HEADER] head

                      inner join CRM_MENU_ITEM item
                      on item.SID = head.SID and item.HeaderID = head.HeaderID
					  inner join CRM_MENU_EMPLOYEE_MAPPING emp
					  on item.sid = emp.sid and item.MenuID = emp.Menuid
 
                      where head.SID='" + sid + "' and emp.EmployeeCode='" + empCode + @"'

                      group by head.[HeaderID] ,head.[HeaderName] ,item.xType ,item.MenuID
                               ,item.MenuSubtitle,item.MenuTitle,item.NAVIGATEIMAGE,item.NAVIGATEURL
                               ,head.xIndex,item.xIndex,emp.SEQ 
                      order by emp.SEQ asc
					    ,item.xIndex asc ";
            return dbservice.selectDataFocusone(sql);
        }

        private DataTable getMenuData(string sid, string EmpCode)
        {
            bool IsAuth = false;
            string strCheckAuth = "";
            bool.TryParse(IsAuthorizationConfig, out IsAuth);
            if (IsAuth)
            {
                strCheckAuth = @"
		                        inner join [auth_profileobject] mapObject
		                        on mapObject.sid = item.SID and mapObject.parentobjectid = item.MenuID
                                and mapObject.objecttype='S'
		                        and mapObject.objectvalue like '%03%'

		                        inner join auth_userprofilemapping mapUser
			                        on mapUser.sid = mapObject.SID 
			                        and mapUser.userid='" + EmpCode + @"' 
			                        and mapUser.profileid = mapObject.profileid
                                ";
            }
           
           sql = @"SELECT head.[HeaderID]
                          ,head.[HeaderName]
	                      ,item.xType
	                      ,item.MenuID
	                      ,item.MenuSubtitle
	                      ,item.MenuTitle
	                      ,item.NAVIGATEIMAGE
	                      ,item.NAVIGATEURL
                      FROM [SERVICE_MENU_HEADER] head

                      left join SERVICE_MENU_ITEM item
                      on item.SID = head.SID and item.HeaderID = head.HeaderID

                     left join SERVICE_MENU_TYPE_INDEX sorttype
                     on sorttype.SID = head.SID and sorttype.xType = item.xType
                     " + strCheckAuth+@"
                      where head.SID='" + sid+ @"'

                      group by head.[HeaderID] ,head.[HeaderName] ,item.xType ,item.MenuID
                               ,item.MenuSubtitle,item.MenuTitle,item.NAVIGATEIMAGE,item.NAVIGATEURL
                               ,head.xIndex,sorttype.xIndex,item.xIndex
                      order by head.xIndex asc,
						  case ISNULL(sorttype.xIndex,'')
							  when '' then 99999
							  else sorttype.xIndex
						  end asc
					    ,item.xIndex asc ";
           return dbservice.selectDataFocusone(sql);
        }


        public DataTable getSearchProgram(string sid)
        {
            string sql = @"
                        SELECT
                             item.MenuID as LINK_OBJID
	                        ,item.MenuTitle as PROG_NAME
	                        ,item.MenuID as F1_OBJID
	                        ,item.MenuTitle as ROOTOBJECT
                            ,ISNULl(descp.Description,'') as Description
                            ,item.NAVIGATEURL
                        FROM [SERVICE_MENU_HEADER] head

                            left join SERVICE_MENU_ITEM item
                            on item.SID = head.SID and item.HeaderID = head.HeaderID

                            left join SERVICE_MENU_TYPE_INDEX sorttype
                            on sorttype.SID = head.SID and sorttype.xType = item.xType

                            left join F1_MENU_DESCRIPTION descp
							on descp.SID=item.SID and descp.ProgramID = item.MenuID

                            where head.SID='" + sid+@"'
                            order by head.xIndex asc,
		                        case ISNULL(sorttype.xIndex,'')
			                        when '' then 99999
			                        else sorttype.xIndex
		                        end asc
	                        ,item.xIndex asc ";
            return dbservice.selectDataFocusone(sql);
        }

        public DataTable getHeadRoleType(string sid,string xSystemCode)
        {
            string sql = @"
                        SELECT SID
                              ,roleid
                              ,xsystem
                          FROM SNA_LINK_ROLE_TYPE
                          where SID='" + sid + "' and xsystem = '" + xSystemCode + @"'
                        ";
            return dbservice.selectDataFocusone(sql);
        }

        public void SaveHeadRoleType(string sid,string roleid,string xsystem,string CreateBy)
        {
            string sql = @"INSERT INTO [SNA_LINK_ROLE_TYPE]
                               ([SID]
                               ,[roleid]
                               ,[xsystem]
                               ,[created_on]
                               ,[created_by]
                               ,[updated_on]
                               ,[update_by])
                         VALUES
                               ('"+sid+@"'
                               ,'"+roleid+@"'
                               ,'"+xsystem+@"'
                               ,'"+Validation.getCurrentServerStringDateTime()+@"'
                               ,'"+CreateBy+@"'
                               ,''
                               ,'')";
            dbservice.executeSQLForFocusone(sql);
        }

        public void UpdateHeadRoleType(string sid,string roleid,string xsystem,string UpdateBy)
        {
            string sql = @" Update SNA_LINK_ROLE_TYPE
                               set roleid = '" + roleid + @"'
                               ,updated_on = '"+Validation.getCurrentServerStringDateTime()+ @"'
                               ,update_by = '"+UpdateBy+@"'
                               where SID='" + sid+"'  and roleid = '" + roleid + "' and xsystem='" + xsystem + "' ";
            dbservice.executeSQLForFocusone(sql);
        }

        public MenuService getAccessPageByProgramID(string sid, string UserName, string ProgramID)
        {
            if (_inStance == null)
            {
                _inStance = new MenuService();
            }


            bool IsAuth = false;
            bool.TryParse(IsAuthorizationConfig, out IsAuth);
            if (!IsAuth)
            {
                setDefaultAccessPage(true);
                return _inStance;
            }

            if (ProgramID == null)
            {
                setDefaultAccessPage(true);
                return _inStance;
            }

            string sqlOpenPage = @"select distinct po.parentobjectid, po.objectvalue
                        from auth_userprofilemapping pm
                        inner join auth_profileobject po
	                        on po.sid = pm.sid
	                        and po.profileid = pm.profileid
	                        and po.objecttype = 'm'

                        where pm.sid = '" + sid + @"'
	                        and pm.userid = '" + UserName + @"'
                        	and po.parentobjectid = '" + ProgramID + @"'";
            DataTable dtOpenPage = dbservice.selectDataFocusone(sqlOpenPage);

            if (dtOpenPage.Rows.Count == 0)
            {
                setDefaultAccessPage(false);
                return _inStance;
            }

            string sqlAccessPage = @"select distinct po.parentobjectid, po.objectvalue
                        from auth_userprofilemapping pm
                        inner join auth_profileobject po
	                        on po.sid = pm.sid
	                        and po.profileid = pm.profileid
	                        and po.objecttype = 's'

                        where pm.sid = '" + sid + @"'
	                        and pm.userid = '" + UserName + @"'
                        	and po.parentobjectid = '" + ProgramID + @"' ";

            DataTable dtAccessPage = dbservice.selectDataFocusone(sqlAccessPage);

            setDefaultAccessPage(false);
            string[] _action;
            DataRow[] drr = dtAccessPage.Select(String.Format("parentobjectid = '{0}'", ProgramID));

            for (int i = 0; i < drr.Length; i++)
            {
                _action = drr[i][1].ToString().Split(',');
                for (int j = 0; j < _action.Length; j++)
                {
                    switch ((string)_action[j])
                    {
                        case "*": setDefaultAccessPage(true);
                            break;
                        case "01": CanCreate = true;
                            break;
                        case "02": CanChange = true;
                            break;
                        case "03": CanDisplay = true;
                            break;
                        case "04": CanCopy = true;
                            break;
                        case "05": CanDelete = true;
                            break;
                        case "06": CanPrint = true;
                            break;
                        case "07": CanCancel = true;
                            break;
                        case "10": CanReverse = true;
                            break;
                    }
                }
            }
            return _inStance;
        }

        private void setDefaultAccessPage(bool IsAccess)
        {
            CanAll = IsAccess;
            CanCreate = IsAccess;
            CanChange = IsAccess;
            CanDisplay = IsAccess;
            CanCopy = IsAccess;
            CanDelete = IsAccess;
            CanPrint = IsAccess;
            CanCancel = IsAccess;
            CanReverse = IsAccess;
        }

        public void insertSelectedMenu(string sid, string CompanyCode, string EmployeeCode, string MenuID)
        {
            string sqlemp = " SELECT * FROM [CRM_MENU_EMPLOYEE_MAPPING] " +
                        " where EmployeeCode = '" + EmployeeCode + "'";
            int valueIndex;
            DataTable dtemp = dbservice.selectDataFocusone(sqlemp);
            try
            {
                if (dtemp.Rows.Count != 0)
                {
                    valueIndex = Convert.ToInt32(dtemp.Compute("max(SEQ)", string.Empty)) + 1;
                }
                else
                {
                    valueIndex = 0;
                }
            }
            catch
            {
                valueIndex = 0;
            }

            string sql = "insert into CRM_MENU_EMPLOYEE_MAPPING (SID,CompanyCode,EmployeeCode,MenuID,SEQ,created_on) values (";
            sql += "'" + sid + "'";
            sql += ",'" + CompanyCode + "'";
            sql += ",'" + EmployeeCode + "'";
            sql += ",'" + MenuID + "'";
            sql += ",'" + valueIndex + "'";
            sql += ",'" + Validation.getCurrentServerStringDateTime() + "'";
            sql += ");";

            dbservice.executeSQLForFocusone(sql);
        }

        public void deleteSelectedMenu(string sid, string CompanyCode, string EmployeeCode, string MenuID)
        {
            string sql = "delete from CRM_MENU_EMPLOYEE_MAPPING where SID = '" + sid + "' and CompanyCode = '" + CompanyCode + "' and EmployeeCode = '" + EmployeeCode + "' and MenuID = '" + MenuID + "'";
            dbservice.executeSQLForFocusone(sql);
        }

        #endregion
    }


    #region class entities structure

    [Serializable]
    public class enDataResource
    {
        public string HeaderID { get; set; }
        public string HeaderName { get; set; }
        public List<enXtype> listxType { get; set; }
    }

    [Serializable]
    public class enXtype
    {
        public string xType { get; set; }
        public List<enMenuData> listMenu { get; set; }
    }

    [Serializable]
    public class enMenuData
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string url { get; set; }
        public string img { get; set; }
    }


    #endregion

}