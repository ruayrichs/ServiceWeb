using agape.lib.web.configuration.utils;
using Agape.FocusOne.Utilities;
using Agape.Lib.DBService;
using SNA.Lib.Master;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace ServiceWeb.Service
{
    public class AssetService
    {
        private DBService dbService = new DBService();
        private static AssetService _instance;
        public static AssetService getInstance()
        {
            if (_instance == null)
            {
                _instance = new AssetService();
            }
            return _instance;
        }

        public DataTable getEmployee(string sid, string companyCode)
        {
            string sql = "SELECT * FROM master_employee WHERE SID='" + sid + "' AND CompanyCode='" + companyCode + "'";

            DataTable dt = dbService.selectDataFocusone(sql);

            foreach (DataRow dr in dt.Rows)
            {
                if (string.IsNullOrEmpty(dr["FirstName_TH"].ToString()))
                {
                    dr["FirstName_TH"] = dr["FirstName"];
                }
                if (string.IsNullOrEmpty(dr["LastName_TH"].ToString()))
                {
                    dr["LastName_TH"] = dr["LastName"];
                }
            }

            return dt;
        }



        public DataTable SearchEmployee(string sid, string companyCode, string searchText)
        {
            string sql = "SELECT TOP(50) * FROM master_employee WHERE SID='" + sid + "' AND CompanyCode='" + companyCode + "'";

            if (!string.IsNullOrEmpty(searchText))
            {
                sql += " AND (FirstName LIKE '%" + searchText + "%'";
                sql += " OR LastName LIKE '%" + searchText + "%' ";
                sql += " OR LastName LIKE '%" + searchText + "%' ";
                sql += " OR FirstName_TH LIKE '%" + searchText + "%'";
                sql += " OR LastName_TH LIKE '%" + searchText + "%')";

            }

            DataTable dt = dbService.selectDataFocusone(sql);

            foreach (DataRow dr in dt.Rows)
            {
                if (string.IsNullOrEmpty(dr["FirstName_TH"].ToString()))
                {
                    dr["FirstName_TH"] = dr["FirstName"];
                }
                if (string.IsNullOrEmpty(dr["LastName_TH"].ToString()))
                {
                    dr["LastName_TH"] = dr["LastName"];
                }
            }

            return dt;
        }

        public DataTable getBUArea(string sid)
        {
            string sql = "SELECT * FROM master_buarea WHERE SID='" + sid + "'";

            DataTable dt = dbService.selectDataFocusone(sql);

            return dt;
        }

        public DataTable getAssetGroup(string sid, string companyCode, string assetGroupCode)
        {
            string sql = "SELECT * FROM am_define_assetgroup WHERE SID='" + sid + "' AND CompanyCode='" + companyCode + "'";

            sql += Validation.CreateString(assetGroupCode, "AssetGroup");

            sql += " ORDER BY AssetGroup";

            DataTable dt = dbService.selectDataFocusone(sql);

            return dt;
        }

        public DataTable getAssetType(string sid, string companyCode, string assetGroupCode, string assetTypeCode)
        {
            string sql = "SELECT * FROM am_define_assettype WHERE SID='" + sid + "' AND CompanyCode='" + companyCode + "'";

            sql += Validation.CreateString(assetGroupCode, "AssetGroup");
            sql += Validation.CreateString(assetTypeCode, "GroupCode");

            sql += " ORDER BY GroupCode";

            DataTable dt = dbService.selectDataFocusone(sql);

            return dt;
        }

        public DataTable getAssetCategory1(string sid, string companyCode, string assetGroupCode, string assetTypeCode, string assetCategory)
        {
            string sql = "SELECT * FROM am_define_assetcategory1 WHERE SID='" + sid + "' AND CompanyCode='" + companyCode + "'";

            sql += Validation.CreateString(assetGroupCode, "AssetGroup");
            sql += Validation.CreateString(assetTypeCode, "AssetType");
            sql += Validation.CreateString(assetCategory, "AssetCategory");

            sql += " ORDER BY AssetCategory";

            DataTable dt = dbService.selectDataFocusone(sql);

            return dt;
        }

        public DataTable getAssetCategory2(string sid, string companyCode, string assetGroupCode, string assetTypeCode, string assetCategory1, string assetCategory2)
        {
            string sql = "SELECT * FROM am_define_assetcategory2 WHERE SID='" + sid + "' AND CompanyCode='" + companyCode + "'";

            sql += Validation.CreateString(assetGroupCode, "AssetGroup");
            sql += Validation.CreateString(assetTypeCode, "AssetType");
            sql += Validation.CreateString(assetCategory1, "AssetCategory1");
            sql += Validation.CreateString(assetCategory2, "AssetCategory");

            sql += " ORDER BY AssetCategory";

            DataTable dt = dbService.selectDataFocusone(sql);

            return dt;
        }

        public DataTable getAssetLocation1(string sid, string companyCode, string assetLocation)
        {
            string sql = "SELECT * FROM am_define_assetlocation WHERE SID='" + sid + "' AND CompanyCode='" + companyCode + "'";

            sql += Validation.CreateString(assetLocation, "AssetLocation");

            sql += " ORDER BY AssetLocation";

            DataTable dt = dbService.selectDataFocusone(sql);

            return dt;
        }

        public DataTable getAssetLocation2(string sid, string companyCode, string assetLocation1, string assetLocation2)
        {
            string sql = "SELECT * FROM am_define_assetlocation2 WHERE SID='" + sid + "' AND CompanyCode='" + companyCode + "'";

            sql += Validation.CreateString(assetLocation1, "AssetLocation1");
            sql += Validation.CreateString(assetLocation2, "AssetLocation2");

            sql += " ORDER BY AssetLocation2";

            DataTable dt = dbService.selectDataFocusone(sql);

            return dt;
        }

        public DataTable getAssetRoom(string sid, string companyCode, string assetLocation1, string assetLocation2, string assetRoom)
        {
            string sql = "SELECT * FROM am_define_assetroom WHERE SID='" + sid + "' AND CompanyCode='" + companyCode + "'";

            sql += Validation.CreateString(assetLocation1, "AssetLocation1");
            sql += Validation.CreateString(assetLocation2, "AssetLocation2");
            sql += Validation.CreateString(assetRoom, "AssetRoom");

            sql += " ORDER BY AssetRoom";

            DataTable dt = dbService.selectDataFocusone(sql);

            return dt;
        }

        public DataTable getDepreciationMethod(string sid)
        {
            string sql = "SELECT * FROM am_define_depreciation_method WHERE sid='" + sid + "'";

            DataTable dt = dbService.selectDataFocusone(sql);

            if (dt.Rows.Count == 0)
            {
                throw new Exception("กรุณาระบุค่า Depreciation Method ที่ Table : am_define_depreciation_method");
            }

            return dt;
        }

        public string getCompanyCurrency(string sid, string companyCode)
        {
            string sql = "SELECT CurrencyCode FROM master_company_detail_basic_initial WHERE SID='" + sid + "' AND CompanyCode='" + companyCode + "'";

            DataTable dt = dbService.selectDataFocusone(sql);

            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["CurrencyCode"].ToString();
            }

            return "";
        }

        public DataTable getCurrency(string sid)
        {
            string sql = "SELECT * FROM master_currencytype WHERE SID='" + sid + "'";

            DataTable dt = dbService.selectDataFocusone(sql);

            return dt;
        }

        public DataTable getDepartment(string sid, string companyCode)
        {
            string sql = "SELECT * FROM master_conf_department WHERE sid='" + sid + "' and companyCode='" + companyCode + "'";

            DataTable dt = dbService.selectDataFocusone(sql);

            return dt;
        }

        public DataTable getAssetMappingEmployee(string sid, string companyCode, string assetCode)
        {
            string _sql = "SELECT * FROM link_asset_mapping_employee WHERE SID='" + sid + "' AND CompanyCode='" + companyCode + "' AND AssetCode='" + assetCode + "'";

            DataTable dt = dbService.selectDataFocusone(_sql);
            dt.TableName = "link_asset_mapping_employee";
            dt.PrimaryKey = new DataColumn[] { dt.Columns["SID"], dt.Columns["CompanyCode"], dt.Columns["AssetCode"] };

            return dt;
        }

        public void saveAssetMappingEmployee(DataTable dt)
        {
            dbService.SaveTransactionForFocusone(dt);
        }

        public DataTable GetAssetMaster(string sid, string companyCode
             , string AssetGroup, string Branch, string AssetType, string AssetCategory1
               , string AssetCode, string AssetCategory2, string AssetName, string Location1
               , string Owner, string Location2, string Department, string Room, string status)
        {
            string _sql = @"select c.AssetGroup,e.Description as AssetGroupName,c.AssetType,f.GroupName as AssetTypeName,a.AssetCode,
                        b.AssetSubCodeDescription,b.NetValue,c.CURRENCYCODE,d.AssetOwner,g.FirstName_TH,g.LastName_TH,a.AssetStatus
                        from am_master_asset_header a
                        inner join am_master_asset_subcode b
                        on a.SID = b.SID and a.CompanyCode = b.CompanyCode and a.AssetCode = b.AssetCode
                        inner join am_master_asset_general1 c
                        on b.SID = c.SID and b.CompanyCode = c.CompanyCode and b.AssetCode = c.AssetCode and b.AssetSubCode = c.AssetSubCode
                        inner join am_master_asset_general2 d
                        on b.SID = d.SID and b.CompanyCode = d.CompanyCode and b.AssetCode = d.AssetCode and b.AssetSubCode = d.AssetSubCode
                        left join am_define_assetgroup e
                        on c.SID = e.SID and c.CompanyCode = e.CompanyCode and c.AssetGroup = e.AssetGroup
                        left join am_define_assettype f
                        on c.SID = f.SID and c.CompanyCode = f.CompanyCode and c.AssetGroup = f.AssetGroup and c.AssetType = f.GroupCode
                        left join master_employee g
                        on d.SID = g.SID and d.CompanyCode = g.CompanyCode and d.AssetOwner = g.EmployeeCode
                        where a.SID='" + sid + "' and a.CompanyCode='" + companyCode + "'";

            _sql += Validation.CreateString(Branch, "a.BranchCode");
            _sql += Validation.CreateString(status, "a.AssetStatus");
            _sql += Validation.CreateString(AssetGroup, "c.AssetGroup");
            _sql += Validation.CreateString(AssetType, "c.AssetType");
            _sql += Validation.CreateString(AssetCategory1, "c.AssetCategory1");
            _sql += Validation.CreateString(AssetCategory2, "c.AssetCategory2");
            _sql += Validation.CreateString(Location1, "d.Location1");
            _sql += Validation.CreateString(Location2, "d.Location2");
            _sql += Validation.CreateString(Owner, "d.AssetOwner");
            _sql += Validation.CreateString(Department, "d.Department");
            _sql += Validation.CreateString(Room, "d.Room");

            if (!string.IsNullOrEmpty(AssetCode))
            {
                _sql += " and a.AssetCode like '%" + AssetCode + "%'";
            }
            if (!string.IsNullOrEmpty(AssetName))
            {
                _sql += " and b.AssetSubCodeDescription like '%" + AssetName + "%'";
            }

            _sql += " order by a.AssetCode";

            DataTable dt = dbService.selectDataFocusone(_sql);

            return dt;
        }

        public void DeleteAssetMaster(string sid, string companyCode, string KEY)
        {
            StringBuilder queryBuilder = new StringBuilder();
            //queryBuilder.AppendLine("DELETE FROM link_po_tracking_mapping_employee ");
            //queryBuilder.AppendLine("WHERE SID='" + sid + "'");
            //queryBuilder.AppendLine("AND CompanyCode='" + companyCode + "'");
            //queryBuilder.AppendLine("AND VendorCode='" + KEY + "'");

            //dbService.executeSQLForFocusone(queryBuilder.ToString());
        }

        public void CreateAssetMaster(string sid, string companyCode, AssetEntity assetForSaveCreate)
        {
            string AssestGroup = assetForSaveCreate.AssetGroup;
        }

        public void UpdateAssetMaster(string sid, string companyCode, AssetEntity assetForSaveEdit)
        {
            string AssestGroupp = assetForSaveEdit.AssetGroup;
        }

        public DataTable getAssetListForSmartSearch(string sid, string companyCode)
        {

            string sql = @"select a.*,b.AssetGroup,c.Description as AssetGroupName from am_master_asset_subcode a
left outer join am_master_asset_general1 b
on a.sid = b.sid and a.AssetCode = b.AssetCode and a.CompanyCode = b.CompanyCode
left outer join am_define_assetgroup c
on b.SID = c.SID and b.CompanyCode = c.CompanyCode and b.AssetGroup = c.AssetGroup
 where a.sid='" + sid + "' and a.CompanyCode='" + companyCode + "' ";

            //string sql = "select * from am_master_asset_subcode where sid='"+sid+"' and CompanyCode='"+companyCode+"' ";
            return dbService.selectDataFocusone(sql);
        }

        public DataTable AssetSearch(string sid, string companyCode)
        {

            return AssetSearch(sid, companyCode, "");
        }

        public DataTable AssetSearchLogistic(string sid, string companyCode)
        {

            return AssetSearch(sid, companyCode, "LO");
        }

        public DataTable AssetSearch(string sid, string companyCode, string assetGroup)
        {

            string sql = @"select a.*,b.AssetGroup,c.Description as AssetGroupName from am_master_asset_subcode a
                 inner join [am_master_asset_header] y on a.sid = y.sid
                        and a.companycode = y.companycode and a.assetcode = y.assetcode
                left outer join am_master_asset_general1 b
                on a.sid = b.sid and a.AssetCode = b.AssetCode and a.CompanyCode = b.CompanyCode
                left outer join am_define_assetgroup c
                on b.SID = c.SID and b.CompanyCode = c.CompanyCode and b.AssetGroup = c.AssetGroup
                 where a.sid='" + sid + "' and a.CompanyCode='" + companyCode + "'  AND y.AssetStatus='True'  ";

            if (!string.IsNullOrEmpty(assetGroup))
            {
                sql += " and  c.AssetGroup = '" + assetGroup + "' ";
            }

            //string sql = "select * from am_master_asset_subcode where sid='"+sid+"' and CompanyCode='"+companyCode+"' ";
            DataTable dt = dbService.selectDataFocusone(sql);

            return dt;
        }


        public DataTable AssetForSmartSearch(string sid, string companyCode, string searchText)
        {

            string sql = @"SELECT TOP(50) a.*,b.AssetGroup,c.Description as AssetGroupName from am_master_asset_subcode a
        inner join  [am_master_asset_header] y on a.sid = y.sid
            and a.companycode = y.companycode and a.assetcode = y.assetcode
        left outer join am_master_asset_general1 b
        on a.sid = b.sid and a.AssetCode = b.AssetCode and a.CompanyCode = b.CompanyCode
        left outer join am_define_assetgroup c
        on b.SID = c.SID and b.CompanyCode = c.CompanyCode and b.AssetGroup = c.AssetGroup
         where a.sid='" + sid + "' and a.CompanyCode='" + companyCode + "' AND y.AssetStatus='True'  ";

            if (!string.IsNullOrEmpty(searchText))
            {
                sql += " AND (b.AssetGroup LIKE '%" + searchText + "%'";
                sql += " OR c.Description LIKE '%" + searchText + "%' ";
                sql += " OR a.AssetSubCodeDescription LIKE '%" + searchText + "%' ";
                sql += " OR a.AssetCode LIKE '%" + searchText + "%')";
            }

            //string sql = "select * from am_master_asset_subcode where sid='"+sid+"' and CompanyCode='"+companyCode+"' ";
            DataTable dt = dbService.selectDataFocusone(sql);

            return dt;
        }

        public DataTable getReportAssetUsed(string sid, string companyCode, string snaid, string p_assetcode, string p_assetname, string p_assetDepartment, string p_assetOwner
            , string mainPerson, string mainWorkGroup, string mainSubWork, string p_planDateFrom, string p_planDateTo)
        {
            StringBuilder str = new StringBuilder();
            str.AppendLine("select usedAsset.AssetCode,assetMaster.AssetSubCodeDescription as AssetName ");
            str.AppendLine(",assetDep.Department as AssetDepartment ");
            str.AppendLine(",depMaster.departmentName as AssetDepartmentName ");
            str.AppendLine(",assetDep.AssetOwner ");
            str.AppendLine(",assetOwner.FirstName_TH + ' ' + assetOwner.LastName_TH as AssetOwnerName ");
            str.AppendLine(",timeAttend.PROJECTCODE ");
            str.AppendLine(",timeAttend.SUBPROJECT ");
            str.AppendLine(",projMaster.ProjectName + '/'+subproj.Name as DisplayWorkGroup ");
            str.AppendLine(",timeAttend.JOBDESCRIPTION as ActivitySubject ");
            str.AppendLine(",employee.FullName_TH as MainResPerson ");
            str.AppendLine(",timeAttend.DATEIN as PlanStartDate ");
            str.AppendLine(",timeAttend.TIMEIN as PlanStartTime ");
            str.AppendLine(",timeAttend.DATEOUT as PlanEndDate ");
            str.AppendLine(",timeAttend.TIMEOUT as PlanEndTime ");
            str.AppendLine(",'' as TotalPlan  ");
            str.AppendLine(", case when timeAttend.XSTATUS='' THEN 'ยังไม่รับทราบ'  ");
            str.AppendLine("  when timeAttend.XSTATUS='READED' and timeAttend.checkOut_Date='' THEN 'รับทราบแล้ว'  ");
            str.AppendLine("  when timeAttend.XSTATUS='SUCCESS'  THEN 'เสร็จสิ้นแล้ว'  ");
            str.AppendLine("  when timeAttend.checkOut_Date<>'' THEN 'ปิดงาน'  ");
            str.AppendLine("ELSE 'ไม่ระบุ' end  as ActivityStatus ");
            str.AppendLine(",SUBSTRING (timeAttend.CompleteDate,1,8) as ActualEndDate ");
            str.AppendLine(",SUBSTRING (timeAttend.CompleteDate,9,6) as ActualEndTime ");
            str.AppendLine(",'' as TotalActual ");
            str.AppendLine(" from " + WebConfigHelper.getDatabaseSNAName() + ".dbo.SNA_ACTIVITY_ASSET usedAsset ");
            str.AppendLine("inner join " + WebConfigHelper.getDatabaseSNAName() + ".dbo.SNA_" + snaid + "_TIMEATTENDANCE timeAttend ");
            str.AppendLine("on usedAsset.SNAID = timeAttend.COMPANYCODE and usedAsset.AOBJECTLINK = timeAttend.AOBJECTLINK ");
            str.AppendLine("left outer join am_master_asset_subcode assetMaster ");
            str.AppendLine("on usedAsset.sid = assetMaster.SID  and usedAsset.AssetCode = assetMaster.AssetCode  ");
            str.AppendLine("and assetMaster.CompanyCode='" + companyCode + "' ");
            str.AppendLine("left outer join am_master_asset_general2 assetDep ");
            str.AppendLine("on assetMaster.SID = assetDep.SID and assetMaster.CompanyCode = assetDep.CompanyCode  ");
            str.AppendLine("and assetMaster.AssetCode = assetDep.AssetCode ");
            str.AppendLine("left outer join  ");
            str.AppendLine("( ");
            str.AppendLine("select a.SID,a.COMPANYCODE,a.LINKID,b.EmployeeCode ");
            str.AppendLine(",b.FirstName +' '+ b.LastName as FullName ");
            str.AppendLine(",b.FirstName_TH +' '+ b.LastName_TH as FullName_TH ");
            str.AppendLine(" from SNA_LINK_REFERENCE a ");
            str.AppendLine("inner join master_employee b ");
            str.AppendLine("on a.SID = b.SID and a.COMPANYCODE = b.COMPANYCODE ");
            str.AppendLine("and a.REFOBJ = b.EmployeeCode ");
            str.AppendLine(") employee ");
            str.AppendLine("on timeAttend.COMPANYCODE='" + snaid + "' and timeAttend.EMPCODE = employee.LINKID  and employee.SID='" + sid + "' ");
            str.AppendLine("left outer join dbo.LINK_PROJECT_MASTER projMaster  ");
            str.AppendLine("on usedAsset.SID = projMaster.SID and projMaster.CompanyCode='" + companyCode + "' ");
            str.AppendLine(" and timeAttend.PROJECTCODE = projMaster.ProjectCode ");
            str.AppendLine("left outer join dbo.LINK_PROJECT_MASTER_SUB_PROJECT subproj ");
            str.AppendLine("on usedAsset.SID = subproj.SID and subproj.CompanyCode=projMaster.CompanyCode ");
            str.AppendLine("and timeAttend.PROJECTCODE = subproj.ProjectCode and  timeAttend.SUBPROJECT = subproj.SubProjectCode  ");
            str.AppendLine("left outer join master_conf_department depMaster ");
            str.AppendLine("on depMaster.sid = usedAsset.sid and depMaster.companyCode='" + companyCode + "' ");
            str.AppendLine("and depMaster.departmentCode = assetDep.Department ");

            str.AppendLine("left outer join master_employee assetOwner ");
            str.AppendLine("on assetDep.AssetOwner = assetOwner.EmployeeCode and assetDep.sid = assetOwner.SID ");
            str.AppendLine("and assetDep.CompanyCode = assetOwner.CompanyCode");
            str.AppendLine("where usedAsset.SID='" + sid + "' and usedAsset.SNAID='" + snaid + "' ");

            if (!String.IsNullOrEmpty(p_assetcode))
            {
                str.AppendLine(" and  usedAsset.AssetCode like '%" + p_assetcode + "%' ");
            }
            if (!String.IsNullOrEmpty(p_assetname))
            {
                str.AppendLine(" and  assetMaster.AssetSubCodeDescription like '%" + p_assetname + "%' ");
            }
            if (!String.IsNullOrEmpty(p_assetDepartment))
            {
                str.AppendLine(" and ( assetDep.Department like '%" + p_assetDepartment + "%' or depMaster.departmentName  like'%" + p_assetDepartment + "%') ");
            }
            if (!String.IsNullOrEmpty(p_assetOwner))
            {
                str.AppendLine(" and  (assetDep.AssetOwner like'%" + p_assetOwner
                    + "%' or assetOwner.FirstName_TH + ' ' + LastName_TH like'%" + p_assetOwner
                    + "%' or assetOwner.FirstName+ ' ' + LastName like'%" + p_assetOwner + "%') ");

            }
            if (!String.IsNullOrEmpty(mainPerson))
            {
                str.AppendLine(" and  timeAttend.EMPCODE='" + mainPerson + "' ");
            }
            if (!String.IsNullOrEmpty(mainWorkGroup) && mainWorkGroup != "*")
            {
                str.AppendLine(" and  timeAttend.PROJECTCODE='" + mainWorkGroup + "' ");
            }
            if (!String.IsNullOrEmpty(mainSubWork))
            {
                str.AppendLine(" and  timeAttend.SUBPROJECT='" + mainSubWork + "' ");
            }
            str.AppendLine(Validation.CreateStringFromTo("", p_planDateFrom, p_planDateTo, "timeAttend.DATEIN"));
            DataTable dt = dbService.selectDataFocusone(str.ToString());

            foreach (DataRow dr in dt.Rows)
            {
                string planStartDateTime = dr["PlanStartDate"].ToString() + dr["PlanStartTime"].ToString();
                string planEndDateTime = dr["PlanEndDate"].ToString() + dr["PlanEndTime"].ToString();
                DateTime PlanStart = DateTime.ParseExact(planStartDateTime, "yyyyMMddHHmmss", CultureInfo.CreateSpecificCulture("en-US"));
                DateTime PlanEnd = DateTime.ParseExact(planEndDateTime, "yyyyMMddHHmmss", CultureInfo.CreateSpecificCulture("en-US"));

                int totalPlan = PlanEnd.Subtract(PlanStart).Hours;
                dr["TotalPlan"] = totalPlan.ToString();

                if (dr["ActualEndDate"].ToString() != "")
                {
                    string ActualEndDateTime = dr["ActualEndDate"].ToString() + dr["ActualEndTime"].ToString();
                    DateTime ActualEnd = DateTime.ParseExact(ActualEndDateTime, "yyyyMMddHHmmss", CultureInfo.CreateSpecificCulture("en-US"));

                    int totalActual = ActualEnd.Subtract(PlanStart).Hours;
                    dr["TotalActual"] = totalActual.ToString();
                }
            }

            return dt;
        }

        public DataTable getAssetHistory(string sid, string snaId, string companyCode, string assetCode)
        {
            string tableActivity = EnumConstantTableName.GetEnumTableName(EnumConstantTableName.TimeAttendance, snaId);

            string _sql = @"SELECT A.AssetCode,A.SNAID,B.AOBJECTLINK,B.EMPCODE,B.PROJECTCODE,B.SUBPROJECT,B.JOBDESCRIPTION,B.DATEIN,C.ProjectName,D.Name
                            FROM " + WebConfigHelper.getDatabaseSNAName() + @".[dbo].[SNA_ACTIVITY_ASSET] A
                            INNER JOIN " + WebConfigHelper.getDatabaseSNAName() + @".[dbo].[" + tableActivity + @"] B
                            ON A.SNAID = B.COMPANYCODE AND A.AOBJECTLINK = B.AOBJECTLINK
                            LEFT JOIN " + WebConfigHelper.getDatabaseF1Name() + @".[dbo].[LINK_PROJECT_MASTER] C
                            ON A.SID = C.SID AND B.PROJECTCODE = C.ProjectCode AND C.CompanyCode='" + companyCode + @"'
                            LEFT JOIN " + WebConfigHelper.getDatabaseF1Name() + @".[dbo].[LINK_PROJECT_MASTER_SUB_PROJECT] D
                            ON A.SID = D.SID AND B.PROJECTCODE = D.ProjectCode AND B.SUBPROJECT = D.SubProjectCode AND D.CompanyCode='" + companyCode + @"'
                            WHERE A.SID='" + sid + "' AND A.SNAID='" + snaId + "' AND A.AssetCode='" + assetCode + "' AND B.ItemType NOT IN ('003','005') AND B.XSTATUS <> 'CANCELED'";

            _sql += " ORDER BY B.DATEIN";

            DataTable dt = dbService.selectData(_sql);

            return dt;
        }

        public DataTable getAssetAgenda(string sid, string snaId, string companyCode, string assetCode)
        {
            string sql = @"select a.CREATED_BY, b.SID, b.AssetCode ,c.AssetSubCodeDescription as AssetName, a.AOBJECTLINK,a.COMPANYCODE, a.JOBDESCRIPTION as Title,a.DATEIN,a.TIMEIN
,a.DATEOUT,a.TIMEOUT,a.ItemType from " + WebConfigHelper.getDatabaseSNAName() + @".dbo.SNA_" + snaId + @"_TIMEATTENDANCE a  inner join " + WebConfigHelper.getDatabaseSNAName() + @".dbo.SNA_ACTIVITY_ASSET b
on a.AOBJECTLINK = b.AOBJECTLINK and a.COMPANYCODE = b.SNAID
left outer join  am_master_asset_subcode c
on b.AssetCode = c.AssetCode and b.SID = c.SID and c.CompanyCode='" + companyCode + "' ";


            string where = " where b.sid='" + sid + "' and b.snaid='" + snaId + "' and a.ItemType <> '000'  and a.ItemType <> '100'  and a.ItemType <> '200'  and a.ItemType <> '301'  and a.ItemType <> '302'  and a.ItemType <> '303'  and a.ItemType <> '304' and a.XSTATUS <> 'CANCELED' and (a.checkOut_Time ='' or a.checkOut_Time is null) ";
            if (!string.IsNullOrEmpty(assetCode))
            {
                where += " and b.AssetCode='" + assetCode + "' ";
            }
            DataTable dt = dbService.selectDataFocusone(sql + where);

            return dt;
        }

        [Serializable]
        public class AssetEntity
        {
            public string AssetGroup { get; set; }
            public string Branch { get; set; }
            public string AssetType { get; set; }
            public string AssetCategory1 { get; set; }
            public string AssetCode { get; set; }
            public string AssetCategory2 { get; set; }
            public string AssetName { get; set; }
            public string Location1 { get; set; }
            public string Owner { get; set; }
            public string Location2 { get; set; }
            public string Department { get; set; }
            public string Room { get; set; }

            public string AssetValue { get; set; }
            public string Currency { get; set; }
            public string AssetReceiveDate { get; set; }



        }
    }
}