using agape.lib.ag.commonutils.convert;
using agape.lib.web.ICMV2;
using Agape.FocusOne.Utilities;
using Agape.Lib.DBService;
using Focusone.ICMWcfService;
using SNA.Lib.crm.utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace ServiceWeb.Service
{
    public class materialService
    {

        LookupICMService lookupICMService = new LookupICMService();
        DBService dbService = new DBService();
        private static materialService inStance;

        public static materialService getInSatnce()
        {
            if (inStance == null)
            {
                inStance = new materialService();
            }
            return inStance;
        }


        public DataTable getMaterialGroupNR(string sid)
        {
            string sql = "select A.MaterialGroupCode as ItmsGrpCod, A.MaterialGroupCode + ' - ' +A.Description as ItmsGrpNam  " +
                "from master_config_material_doctype as A " +
                "inner join master_config_materrial_doctype_docdetail as B " +
                "on A.SID = B.SID and A.Companycode = B.companyCode and A.MaterialGroupCode = B.MaterialGroupCode ";
            sql += "  where A.SID='" + sid + "'  and (A.CompanyCode = '*') and B.PostingType='MAMASTER' order by A.MaterialGroupCode ";

            DataTable dt = dbService.selectDataFocusone(sql);
            return dt;
        }

        public DataTable getMaterialGeneral_V2(String sid, String companycode, String ItemCode, String ItemDesc, String ItemGroup, String active, String ItemReference)
        {
            string where = "";
            if (companycode != "")
            {
                where += " and C.CompanyCode='" + companycode + "'";
            }
            if (ItemCode != "")
            {
                where += " and A.ItmNumber like '%" + ItemCode + "%'";
            }
            if (ItemDesc != "")
            {
                where += F1StringUtils.createLikeString("A.ItmDescription", ItemDesc);
            }
            if (ItemGroup != "")
            {
                where += " and A.ItmGroup='" + ItemGroup + "'";
            }
            if (active != "")
            {
                where += " and B.Valid='" + active + "'";
            }
            if (ItemReference != "")
            {
                where += " and A.Reference='" + ItemReference + "'";
            }


            string sql = @"SELECT A.ItmNumber AS ItmNumber,A.ItmDescription AS ItmDescription,A.ItmType AS ItmType,A.CurrCode AS CurrCode,A.ControlStock AS ControlStock
                            ,A.ItemCat AS ItemCat,A.ItmGroup AS ItmGroup,A.CREATED_BY AS CREATED_BY,A.CREATED_ON AS CREATED_ON,B.BaseUOM AS BaseUOM
                            ,C.ItmsGrpNam AS ItmsGrpNam,D.ItemTypeName AS ItemTypeName,E.Description AS Description,C.CompanyCode AS CompanyCode,F.UDESC AS UDESC 
                            FROM master_mm_items AS A 
                            INNER JOIN master_mm_item_general AS B
                            ON A.SID = B.SID AND A.ItmNumber = B.ItmNumber  
                            INNER JOIN master_mm_itemgroup AS C
                            ON A.SID = C.SID AND A.ItmGroup = C.ItmsGrpCod  
                            INNER JOIN master_mm_itemtype AS D
                            ON A.SID = D.SID AND A.ItmType = D.ItemTypeID  
                            INNER JOIN sd_master_item_cate AS E
                            ON A.SID = E.SID AND A.ItemCat = E.ItmCateCode  
                            LEFT OUTER JOIN master_mm_weight_setup AS F
                            ON B.BaseUOM = F.UCODE AND B.SID = F.SID 
                            where A.SID='" + sid + "'";

            DataTable dt = dbService.selectDataFocusone(sql + where);
            return dt;
        }



        public string getPathImage(string sid, string itemnumber)
        {
            string path = "";
            string sql = "SELECT * FROM [master_mm_item_picture_for_link] WHERE SID='" + sid + "' and ItmNumber='" + itemnumber + "' order by ItemNo asc";

            DataTable dt = dbService.selectDataFocusone(sql);

            if (dt.Rows.Count > 0)
            {
                path = dt.Rows[0]["file_path"].ToString();
            }

            return path;
        }

        public DataTable getSearchMaterailTypeV2(string sid)
        {
            string sql = "select * from master_mm_itemtype where SID ='" + sid + "' and ItemTypeID in ('FG','NT','RM','SE')  ";

            return dbService.selectDataFocusone(sql);

        }

        public DataTable getSearchUnit(string sid)
        {
            //master_mm_weight_setup
            return lookupICMService.GetSearchData(sid, "SHM000057", "#where A.SID='" + sid + "' ");
        }


        public DataTable getSearchMaterailCategory(string sid)
        {
            return lookupICMService.GetSearchData(sid, "SHM000346", "#where A.SID='" + sid + "' ");
        }


        public DataTable getHierarchySearch(string sid)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("select * from ep_master_conf_hierarchy");
            sb.AppendLine("where HierarchyType='MATERIAL' and HierarchyParent='CAR' and SID = '" + sid + "' ");
            return dbService.selectDataFocusone(sb.ToString());
        }


        public DataTable getMatPrice(string dtable, string sid, string material)
        {
            string sql = "select * from " + dtable + " " +
                         "where SID='" + sid + "' and MeterialCode='" + material + "'";

            return dbService.selectDataFocusone(sql);
        }

        public DataTable getMatPriceByScale(string dtable, string sid, string material)
        {
            string sql = @" select * from 
                            (select MeterialCode
                            ,UOM,validfrom,validto 
                            ,'scale' as amount
                             ,count(UOM) as number
                            from " + dtable + @" 
                            where MeterialCode='" + material + @"'
                             and Qty > 0
                            group by MeterialCode,UOM,validfrom,validto

                            Union

                            select MeterialCode
                            ,UOM,validfrom,validto 
                            ,convert(varchar,amount)
                             ,count(UOM) as number
                            from " + dtable + @" 
                            where MeterialCode='" + material + @"'
                             and Qty <= 0.00000
                            group by MeterialCode,UOM,validfrom,validto,amount

                            ) matprice
                            order by  validfrom ASC ";
            return dbService.selectDataFocusone(sql);

        }

        public DataTable getMatPriceForUpdateScale(string dtable, string sid, string matCode
            , string uom, string valideForm, string valideTo)
        {
            string sql = @" SELECT [SID],[MeterialCode],[xVersion],[refBOM] 
                           ,[refPack],[xLineNo],[percentage],[amount] 
                           ,[CurrencyCode],[per],[UOM],[validfrom],[validto] 
                           ,[xTime] ,[xDate],[Qty],[ScaleUOM],[CREATED_BY],[CREATED_ON] 
                           ,[UPDATE_BY],[UPDATE_ON]
                            FROM " + dtable + @"                              
                            where SID = '" + sid + @"'
                            and MeterialCode='" + matCode + @"'   
                            and UOM = '" + uom + @"' 
                            and Qty > 0 
                            and validfrom='" + valideForm + @"'  
                            and validto='" + valideTo + @"' ";
            return dbService.selectDataFocusone(sql);
        }


        public DataTable getMatPriceSortValidFromDESC(string dtable, string sid,
            string material, string unitPrice, string conditionKey, string validform, string validto)
        {
            string sql = "select * from " + dtable + " " +
                         "where SID='" + sid + "' and MeterialCode='" + material + "' and UOM = '" + unitPrice + "' ";
            if (!String.IsNullOrEmpty(conditionKey))
            {
                string[] strCondi = conditionKey.Split('|');
                for (int i = 0; i < strCondi.Length; i++)
                {
                    string[] con = strCondi[i].Trim().Split('=');
                    sql += " and " + con[0] + "='" + con[1] + "' ";
                }
            }
            if (!String.IsNullOrEmpty(validform) && !String.IsNullOrEmpty(validto))
            {
                sql += " and validto >= '" + validform + "' and validfrom <= '" + validto + "'  ";
            }
            sql += " order by validfrom DESC ";

            return dbService.selectDataFocusone(sql);
        }

        public string getMaterialPreviewImage(string sid, string materialCode)
        {
            string sql = @"SELECT a.* FROM master_mm_item_picture_for_link a 
                           INNER JOIN (SELECT SID,ItmNumber,MIN(ItemNo) as minItemNo 
                           FROM master_mm_item_picture_for_link GROUP BY SID,ItmNumber) b
                           ON  a.SID = b.SID and a.ItmNumber = b.ItmNumber and a.ItemNo = b.minItemNo";

            sql += " WHERE A.SID='" + sid + "' AND A.ItmNumber='" + materialCode + "'";

            DataTable dt = dbService.selectDataFocusone(sql);

            if (dt.Rows.Count > 0)
            {
                string protocol = HttpContext.Current.Request.Url.Scheme + Uri.SchemeDelimiter;
                string host = HttpContext.Current.Request.Url.Host;
                string port = (HttpContext.Current.Request.Url.Port == 80 || HttpContext.Current.Request.Url.Port.ToString() == "") ? "" : ":" + HttpContext.Current.Request.Url.Port;

                string path = protocol + host + port + "\\managefile\\" + sid + dt.Rows[0]["file_path"].ToString();

                return path;
            }

            return "";
        }

        public DataTable getPlant(string sid)
        {
            DataTable dt = lookupICMService.GetSearchData(sid, icmconstants.ICM_CONST_SH_PLANT, "#Where A.SID='" + sid + "' ");
            return dt;
        }

        public DataTable getStorageLocation(string sid, string PLANTCODE)
        {
            string sql = "select STORAGELOCCODE, StoreName, PLANTCODE from dbo.mm_conf_define_storagelocation where 1=1 and SID='" + sid + "'";

            if (!string.IsNullOrEmpty(PLANTCODE))
            {
                sql += " and PLANTCODE='" + PLANTCODE + "'";
            }
            DataTable dt = dbService.selectDataFocusone(sql);
            return dt;
        }
        public string getStorageName(string sid, string _plantcode, string _storage)
        {
            string _result = "";

            string sql = "select * from mm_conf_define_storagelocation " +
                        "where sid='" + sid + "' and PLANTCODE='" + _plantcode + "' and STORAGELOCCODE='" + _storage + "'";

            DataTable dt = dbService.selectDataFocusone(sql);
            if (dt.Rows.Count > 0)
            {
                _result = dt.Rows[0]["StoreName"].ToString();
            }
            return _result;
        }


        public DataTable getSearchMat(string sid, string matCode)
        {
            string where = "";
            if (matCode != "")
            {
                where += " and A.ItmNumber='" + matCode + "'";
            }
            DataTable dt = lookupICMService.GetSearchData(sid,
                    icmconstants.ICM_CONST_MM_GETMATERIAL, "#where A.SID='"
                        + sid
                        + "' " + where);
            return dt;
        }
        public DataTable getSearchMat(string sid, string companyCode, string matGroup, string matCode)
        {
            string where = "";
            if (companyCode != "")
            {
                where += " and C.companyCode='" + companyCode + "'";
            }
            if (matGroup != "")
            {
                where += " and A.ItmGroup='" + matGroup + "'";
            }
            if (matCode != "")
            {
                where += " and A.ItmNumber='" + matCode + "'";
            }

            DataTable dt = lookupICMService.GetSearchData(sid,
                    "SHJTH003", "#where 1=1 " + where);
            return dt;
        }

        public DataTable getSearchMaterailType(string sid)
        {
            DataTable dt;
            dt = lookupICMService.GetSearchData(sid,
                    "SHM000061", "#where A.SID='"
                        + sid
                        + "' ");
            return dt;
        }


        public DataTable getSearchCurrency(string sid)
        {
            DataTable dt;
            dt = lookupICMService.GetSearchData(sid,
                icmconstants.ICM_CONST_OPP_SEARCHCURRENCY, "#Where A.SID='"
                + sid + "' ");
            return dt;
        }

        public DataTable getSearchIssueMethod()
        {
            return getSearchIssueMethod("");
        }

        public DataTable getSearchIssueMethod(string sid)
        {
            string sql = "select * from master_issuemethod";

            if (!String.IsNullOrEmpty(sid))
            {
                sql += " WHERE SID='" + sid + "'";
            }

            DataTable dt = dbService.selectDataFocusone(sql);
            return dt;
        }

        public string getconfigIssueMethod(string sid)
        {
            string IssueCode = "";
            string sql = "select * from master_mm_item_default_value_for_link";

            if (!String.IsNullOrEmpty(sid))
            {
                sql += " WHERE SID='" + sid + "'";
            }

            DataTable dt = dbService.selectDataFocusone(sql);
            if (dt.Rows.Count > 0)
            {
                IssueCode = dt.Rows[0]["Issue_Code"].ToString();
            }

            return IssueCode;
        }

        public DataTable getSearchsareacode(string sid, string sareacode, string companycode)
        {
            string where = "";
            if (!string.IsNullOrEmpty(sareacode))
                where += " and A.SAREACODE='" + sareacode + "'";
            if (!string.IsNullOrEmpty(companycode))
                where += " and E.COMPANYCODE='" + companycode + "'";

            DataTable dt = lookupICMService.GetSearchData(sid,
                    "SHM000304", "#where A.SID='"
                        + sid
                        + "' " + where);
            foreach (DataRow dr in dt.Rows)
            {
                dr["SAREADESC"] = dr["SAREACODE"].ToString() + " - " + dr["SAREADESC"].ToString();
            }
            return dt;
        }
        public DataTable getSearchOrg(string sid, string companycode)
        {
            DataTable dt = lookupICMService.GetSearchData(sid,
                icmconstants.ICM_CONST_SH_SALES_ORG, "#Where A.SID='"
                + sid + "' and B.COMPANYCODE='" + companycode + "'");
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.NewRow();
                dr["SORGCODE"] = "*";
                dr["SORGNAME"] = "*";
                dt.Rows.Add(dr);
            }
            return dt;
        }
        public DataTable getSearchDivision(string sid)
        {
            DataTable dt = lookupICMService.GetSearchData(sid,
                icmconstants.ICM_CONST_SH_DIVISION, "#Where A.SID='"
                + sid + "' ");
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.NewRow();
                dr["SDIVCODE"] = "*";
                dr["SDIVNAME"] = "*";
                dt.Rows.Add(dr);
            }
            return dt;
        }
        public DataTable getSearchSCHANNELCODE(string sid, string SCHANNELCODE)
        {
            string where = "";
            if (!string.IsNullOrEmpty(SCHANNELCODE))
            {
                where += " and A.SCHANNELCODE='" + SCHANNELCODE + "'";
            }
            DataTable dt = lookupICMService.GetSearchData(sid,
                    "SHM000298", "#where A.SID='"
                        + sid
                        + "' " + where);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.NewRow();
                dr["SCHANNELCODE"] = "*";
                dr["SHCANNELNAME"] = "*";
                dt.Rows.Add(dr);
            }
            return dt;
        }
        public DataTable getSearchPurOrg(string sid, string companycode)
        {
            string sql = "select a.PURORGCODE,a.NAME1 from mm_conf_define_purchasing_org as a " +
                "inner join mm_conf_assign_porg_to_company as b on a.SID = b.SID and a.PURORGCODE = b.PURORGCODE " +
                "where a.SID = '" + sid + "'";

            if (!string.IsNullOrEmpty(companycode))
                sql += " and b.COMPANYCODE = '" + companycode + "'";

            DataTable dt = dbService.selectDataFocusone(sql);

            DataRow dr = dt.NewRow();
            dr["PURORGCODE"] = "*";
            dr["NAME1"] = "*";
            dt.Rows.Add(dr);

            return dt;
        }
        public DataTable getSearchPurGroup(string sid)
        {

            string sql = "select PURGROUPCODE,NAME1 from mm_conf_define_purchasing_group where SID = '" + sid + "'";

            DataTable dt = dbService.selectDataFocusone(sql);

            DataRow dr = dt.NewRow();
            dr["PURGROUPCODE"] = "*";
            dr["NAME1"] = "*";
            dt.Rows.Add(dr);

            return dt;
        }
        public DataTable getMaterial_Vendor(string sid, string ItmNumber)
        {
            string sql = "select * from master_mm_item_purchasingdata  " +
                            "where sid='" + sid + "' ";

            if (!string.IsNullOrEmpty(ItmNumber))
                sql += " and ItmNumber='" + ItmNumber + "'";
            DataTable dt = dbService.selectDataFocusone(sql);
            return dt;
        }
        public DataTable getMaterial_purchOrgdata(string sid, string ItmNumber)
        {
            string sql = "select distinct a.purorgcode,b.NAME1 as org_name from dbo.material_purchasdata a " +
                            "left join mm_conf_define_purchasing_org b on a.purorgcode = b.PURORGCODE " +
                            "where a.sid='" + sid + "'";

            if (!string.IsNullOrEmpty(ItmNumber))
                sql += " and a.ItmNumber='" + ItmNumber + "'";
            sql += " order by a.purorgcode";
            DataTable dt = dbService.selectDataFocusone(sql);
            return dt;
        }
        public DataTable getMaterial_purchGroupdata(string sid, string ItmNumber)
        {
            string sql = "select distinct a.purgroupcode,c.NAME1 as group_name from dbo.material_purchasdata a " +
                            "left join mm_conf_define_purchasing_group c on a.purgroupcode = c.PURGROUPCODE " +
                            "where a.sid='" + sid + "'";

            if (!string.IsNullOrEmpty(ItmNumber))
                sql += " and a.ItmNumber='" + ItmNumber + "'";
            sql += " order by a.purgroupcode";
            DataTable dt = dbService.selectDataFocusone(sql);
            return dt;
        }
        public DataTable getUom(string sid, string ItmNumber)
        {
            string sql = "select a.ItmNumber,a.PurchUoM as UCODE,b.UDESC as UDESC  from dbo.master_mm_item_purchasingdata a " +
                            "left join master_mm_weight_setup b " +
                            "on a.SID=b.SID and a.PurchUoM=b.UCODE where a.SID='" + sid + "'";

            if (!string.IsNullOrEmpty(ItmNumber))
                sql += " and a.ItmNumber='" + ItmNumber + "'";

            DataTable dt = dbService.selectDataFocusone(sql);
            return dt;
        }
        public string getUomDesc(string sid, string uom)
        {
            string sql = " select * from master_mm_weight_setup "
                       + " where SID='" + sid + "' and UCODE='" + uom + "'";
            string uomdesc = "";
            DataTable dt = dbService.selectDataFocusone(sql);
            if (dt.Rows.Count > 0)
            {
                uomdesc = dt.Rows[0]["UDESC"].ToString();
            }
            else
            {
                uomdesc = "";
            }

            return uomdesc;
        }
        public DataTable getSearchMatGroup(string sid, string companyCode, string matGroup)
        {
            string where = "";
            if (companyCode != "")
            {
                where += " and A.companyCode='" + companyCode + "'";
            }
            if (matGroup != "")
            {
                where += " and A.ItmsGrpCod='" + matGroup + "'";
            }


            DataTable dt = lookupICMService.GetSearchData(sid,
                    "SHM000049", "#where A.SID='"
                        + sid
                        + "' " + where);
            return dt;
        }
        public DataTable getSearchMatSaleData(string sid, string matCode, string saleOrg, string saleDivision, string saleChannel)
        {
            string where = "";
            if (matCode != "")
            {
                where += " and A.ItmNumber='" + matCode + "'";
            }
            if (saleOrg != "")
            {
                where += " and (A.SaleOrg='" + saleOrg + "' or A.SaleOrg='*') ";
            }
            if (saleDivision != "")
            {
                where += " and (A.saleDivision='" + saleDivision + "' or A.SaleDivision='*') ";
            }
            if (saleChannel != "")
            {
                where += " and (A.saleChannel='" + saleChannel + "' or A.saleChannel='*') ";
            }
            DataTable dt = lookupICMService.GetSearchData(sid,
                    "SHM000417", "#where A.SID='"
                        + sid
                        + "' " + where);
            return dt;
        }
        public DataTable GetMaterialProperties(string sid, string itemnumber, string username)
        {

            //GET Properties Master
            string sql = " select '' as SID, '' as ItmNumber, '' as PropertiesCode, '' as xValue, '' as CREATED_BY, '' as CREATED_ON" +
                        " , '' as UPDATED_BY, '' as UPDATED_ON, '' as SelectedValue" +
                        " , PropertiesCode as Code, Description, SelectedCode" +
                        " from master_conf_properties " +
                        " where xType='MAMASTER' order by PropertiesCode";

            DataTable dtProperties = dbService.selectDataFocusone(sql);

            sql = " select itm.SID , itm.ItmNumber , itm.PropertiesCode , itm.xValue , itm.CREATED_BY , itm.CREATED_ON" +
                " , itm.UPDATED_BY , itm.UPDATED_ON , itm.SelectedValue" +
                " from master_mm_properties itm" +
                " where itm.SID='" + sid + "' and itm.ItmNumber='" + itemnumber + "'" +
                " order by itm.PropertiesCode";
            DataTable dtMaterial = dbService.selectDataFocusone(sql);

            foreach (DataRow drProperties in dtProperties.Rows)
            {
                drProperties["SID"] = sid;
                drProperties["ItmNumber"] = itemnumber;
                drProperties["PropertiesCode"] = drProperties["Code"];
                DataRow[] drMaterial = dtMaterial.Select("PropertiesCode='" + drProperties["Code"].ToString() + "'");
                if (drMaterial.Length > 0)
                {
                    foreach (DataRow dr in drMaterial)
                    {
                        drProperties["xValue"] = dr["xValue"];
                        drProperties["SelectedValue"] = dr["SelectedValue"];
                        drProperties["CREATED_BY"] = dr["CREATED_BY"];
                        drProperties["CREATED_ON"] = dr["CREATED_ON"];
                        drProperties["UPDATED_BY"] = dr["UPDATED_BY"];
                        drProperties["UPDATED_ON"] = dr["UPDATED_ON"];
                    }
                }
                else
                {
                    drProperties["CREATED_BY"] = username;
                    drProperties["CREATED_ON"] = Validation.getCurrentServerStringDateTime();
                    drProperties["UPDATED_BY"] = username;
                    drProperties["UPDATED_ON"] = Validation.getCurrentServerStringDateTime();
                }
            }

            dtProperties.TableName = "master_mm_properties";
            dtProperties.PrimaryKey = new DataColumn[] { dtProperties.Columns["SID"]
                , dtProperties.Columns["ItmNumber"]
                , dtProperties.Columns["PropertiesCode"] };

            return dtProperties;
        }
        public void updateGiftVoucher(string sid, string companycode, string vouchercode, string memberid)
        {
            if (string.IsNullOrEmpty(companycode))
                throw new Exception("กรุณาระบุ CompanyCode");
            if (string.IsNullOrEmpty(vouchercode))
                throw new Exception("กรุณาระบุ VoucherCode");
            if (string.IsNullOrEmpty(memberid))
                throw new Exception("กรุณาระบุ MemberCode");
            StringBuilder sql = new StringBuilder();
            DataTable dt = dbService.selectDataFocusone("select * from LT_GIFT_VOUCHER_MASTER where SID='" + sid + "' and CompanyCode='" + companycode +
                "' and VOUCHERCODE='" + vouchercode + "' and MEMBERCODE='" + memberid + "'");
            int act;
            if (dt.Rows.Count > 0)
            {
                int.TryParse(dt.Rows[0]["ACTUALNTIME"].ToString(), out act);
                act++;
                dbService.executeSQLForFocusone("update LT_GIFT_VOUCHER_MASTER set ACTUALNTIME='" + act + "'where SID='" + sid +
                    "' and CompanyCode='" + companycode + "' and VOUCHERCODE='" + vouchercode + "' and MEMBERCODE='" + memberid + "'");
            }
            else
                throw new Exception("ไม่พบ Voucher ที่ต้องการอัพเดท");
        }
        public void UpdateMaterialName(string sid, string material_code, string material_name)
        {
            if (string.IsNullOrEmpty(material_code))
                return;


            dbService.executeSQLForFocusone("update master_mm_items set ItmDescription='" + material_name + "' where SID='" + sid
                + "' and ItmNumber='" + material_code + "' ");
        }
        public DataTable getMaterialByMatCode(string sid, string matcode)
        {

            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select * from dbo.master_mm_items where SID='" + sid + "' ");
            if (!string.IsNullOrEmpty(matcode))
            {
                sql.AppendLine("  and ItmNumber='" + matcode + "'");
            }
            DataTable dt = dbService.selectDataFocusone(sql.ToString());
            return dt;
        }

        public DataTable smartSearchMaterial(string sid, string searchText)
        {
            string sql = "SELECT TOP(100) A.ItmNumber, A.ItmDescription, A.ItmGroup, B.ItmsGrpNam FROM master_mm_items A " +
                " LEFT JOIN master_mm_itemgroup B " +
                " ON A.SID = B.SID and A.ItmGroup = B.ItmsGrpCod " +
                " WHERE A.SID='" + sid + "'";

            if (!string.IsNullOrEmpty(searchText))
            {
                sql += " AND (A.ItmNumber LIKE '%" + searchText + "%'";
                sql += " OR A.ItmDescription LIKE '%" + searchText + "%'";
                sql += " OR B.ItmsGrpNam LIKE '%" + searchText + "%')";
            }

            DataTable dt = dbService.selectDataFocusone(sql);

            return dt;
        }

        public DataTable smartSearchMaterialByValue(string sid, string searchText)
        {
            string sql = "SELECT TOP(100) A.ItmNumber, A.ItmDescription, A.ItmGroup, B.ItmsGrpNam FROM master_mm_items A " +
                " LEFT JOIN master_mm_itemgroup B " +
                " ON A.SID = B.SID and A.ItmGroup = B.ItmsGrpCod " +
                " WHERE A.SID='" + sid + "'";

            if (!string.IsNullOrEmpty(searchText))
            {
                sql += " AND (A.ItmNumber LIKE '%" + searchText + "%'";
                sql += " OR A.ItmDescription LIKE '%" + searchText + "%'";
                sql += " OR B.ItmsGrpNam LIKE '%" + searchText + "%'";
                sql += " OR A.ItmGroup LIKE '%" + searchText + "%')";
            }

            DataTable dt = dbService.selectDataFocusone(sql);

            return dt;
        }

        public DataTable smartSearchMaterialGroup(string sid, string searchText)
        {
            string sql = @"SELECT mmGroup.ItmsGrpCod,mmGroup.ItmsGrpNam,mmGroup.ItmsGrpCod + ' : ' + mmGroup.ItmsGrpNam as CodeWithName
                           FROM master_mm_itemgroup as mmGroup
                           WHERE mmGroup.SID='" + sid + "'";

            if (!string.IsNullOrEmpty(searchText))
            {
                sql += " AND (mmGroup.ItmsGrpCod LIKE '%" + searchText + "%' OR mmGroup.ItmsGrpNam LIKE '%" + searchText + "%')";
            }

            sql += " ORDER BY mmGroup.ItmsGrpCod";

            DataTable dt = dbService.selectDataFocusone(sql);

            return dt;
        }

        public DataTable smartSearchMaterialByPlantAndStorage(string sid, string plant, string storage, string searchText)
        {
            string sql = @"SELECT TOP(100) 
                           A.ItmNumber, A.ItmDescription, A.ItmGroup, B.ItmsGrpNam ,
                           ISNULL(C.PLANTCODE,'') as PLANTCODE,ISNULL(C.STORAGELOCCODE,'') as STORAGELOCCODE
                           FROM master_mm_items A
                           LEFT JOIN master_mm_itemgroup B
                           ON A.SID = B.SID 
                           AND A.ItmGroup = B.ItmsGrpCod
                           INNER JOIN mm_master_material_org C
                           ON A.SID = B.SID 
                           AND A.ItmNumber = C.MATNR
                           WHERE A.SID='" + sid + @"' AND C.PLANTCODE = '" + plant + @"' AND C.STORAGELOCCODE = '" + storage + "'";


            if (!string.IsNullOrEmpty(searchText))
            {
                sql += " AND (A.ItmNumber LIKE '%" + searchText + "%'";
                sql += " OR A.ItmDescription LIKE '%" + searchText + "%'";
                sql += " OR B.ItmsGrpNam LIKE '%" + searchText + "%'";
                sql += " OR A.ItmGroup LIKE '%" + searchText + "%')";
            }

            DataTable dt = dbService.selectDataFocusone(sql);

            return dt;
        }

        public DataTable getMaterialHashTag(string sid, string materialCode)
        {
            string sql = "SELECT * FROM [master_hashtag_material] WHERE SID='" + sid + "'";

            if (!string.IsNullOrEmpty(materialCode))
            {
                sql += " AND ObjectID='" + materialCode + "'";
            }

            DataTable dt = dbService.selectDataFocusone(sql);

            return dt;
        }

        public bool showStandardPricing(string sid, string companyCode)
        {
            bool result = false;

            try
            {
                string sqlGetTablePrice = "select * from sd_master_mapping_table_std_price where sid='" + sid + "' and companyCode='" + companyCode + "'";

                DataTable dtTable = dbService.selectDataFocusone(sqlGetTablePrice);

                if (dtTable.Rows.Count > 0)
                {
                    result = Convert.ToBoolean(dtTable.Rows[0]["show_price"]);
                }
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }

        public string getCompanyCurrency(string sid, string companyCode)
        {
            string sql = "select CurrencyCode from master_company_detail_basic_initial where SID='" + sid + "' and CompanyCode='" + companyCode + "'";

            DataTable dt = dbService.selectDataFocusone(sql);

            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["CurrencyCode"].ToString();
            }

            return "";
        }



        public string checkMaintain(string sid, string companycode)
        {
            string Dtable = "";
            string sql = "select * from sd_master_mapping_table_std_price  " +
                         "where sid='" + sid + "' and companyCode='" + companycode + "' ";

            DataTable dt = dbService.selectDataFocusone(sql);
            if (dt.Rows.Count > 0)
            {
                Dtable = dt.Rows[0]["table_price"].ToString();
            }

            return Dtable;
        }


        public void saveMatPrice(string dtable, string sid, string material, decimal amount, string currencycode, string uom, string validfrom, string validto, decimal Qty, string createdby, string createdon)
        {
            string sql = "insert into " + dtable + " (SID, MeterialCode, xVersion, refBOM, refPack, xLineNo, percentage, amount, CurrencyCode, per, UOM, ScaleUOM, validfrom, validto, " +
                         "xTime, xDate, Qty, CREATED_BY, CREATED_ON, UPDATE_BY, UPDATE_ON) " +
                         "VALUES ('" + sid + "','" + material + "','','','','00001',0.00,'" + amount + "','" + currencycode + "',1.00,'" + uom + "','PC','" + validfrom + "', " +
                         "'" + validto + "','000000','19000101'," + Qty + ",'" + createdby + "','" + createdon + "','','');";
            dbService.executeSQLForFocusone(sql);
        }

        public void editMatPrice(string dtable, string sid, string material, decimal amount, string validfrom, string validto, string updatedby, string updateon)
        {
            string sql = "UPDATE " + dtable + " SET " +
                         "[amount] = '" + amount + "' " +

                         "WHERE SID = '" + sid + "' and MeterialCode = '" + material + "' and validfrom = '" + validfrom + "' and validto = '" + validto + "'";


            dbService.executeSQLForFocusone(sql);
        }

        public void deleteMatPrice(string dtable, string sid, string material, decimal amount,
            string validfrom, string validto, string uom, string conditionKeyDiferrence)
        {
            string sql = "Delete " + dtable + " " +

                         "WHERE SID = '" + sid + "' and MeterialCode = '" + material + "' and amount = '" + amount + "' and validfrom = '" + validfrom + "' and validto = '" + validto + "' ";
            sql += " and UOM ='" + uom + "' and Qty <= 0 ";
            string condi = "";
            if (!String.IsNullOrEmpty(conditionKeyDiferrence))
            {
                string[] strcon = conditionKeyDiferrence.Split('|');
                for (int i = 0; i < strcon.Length; i++)
                {
                    string[] key = strcon[i].Split('=');
                    condi += " and " + key[0].Trim() + "='" + key[1].Trim() + "' ";
                }
            }

            sql += " " + condi;
            dbService.executeSQLForFocusone(sql.ToString());
        }

        public DataTable getVendor(string sid, string materialcode)
        {
            string sql = "select sup.*,items.ItmDescription,vendor.VendorName from material_mapping_supplier sup " +
                         "left outer join master_vendor vendor on sup.SID = vendor.SID and sup.VendorCode = vendor.VendorCode " +
                         "left outer join master_mm_items items on sup.SID = items.SID and sup.MaterialCode = items.ItmNumber " +
                         "where sup.SID = '" + sid + "' and sup.MaterialCode = '" + materialcode + "'";

            DataTable dt = dbService.selectDataFocusone(sql);
            return dt;
        }

        public void saveVendor(string sid, string materialcode, string vendorcode, string createdby, string createdon)
        {
            string sql = "insert into material_mapping_supplier (SID, MaterialCode, VendorCode, CREATED_BY, CREATED_ON) " +
                         "VALUES ('" + sid + "','" + materialcode + "','" + vendorcode + "','" + createdby + "','" + createdon + "');";

            dbService.executeSQLForFocusone(sql);
        }

        public void deleteVendor(string sid, string materialcode, string vendorcode)
        {
            string sql = "Delete material_mapping_supplier " +
                         "WHERE SID = '" + sid + "' and MaterialCode = '" + materialcode + "' and VendorCode = '" + vendorcode + "'";

            dbService.executeSQLForFocusone(sql.ToString());
        }

        public DataTable getSearchEmployeeCode(string sid, string employeecode, string companycode)
        {
            DataTable dt = new DataTable();
            string where = "";
            if (employeecode != "")
            {
                where += " and A.EmployeeCode='" + employeecode + "'";
            }
            if (companycode != "")
            {
                where += " and A.CompanyCode='" + companycode + "'";
            }
            //where += " and A.ActiveOn = 'True' ";
            dt = lookupICMService.GetSearchData(sid,
                    "SHM000728", "#where A.SID='"
                        + sid
                        + "' " + where);
            return dt;
        }

        public DataTable getMaterialSaleUnits(string sid, string materialCode, string saleOrg, string saleDivision, string saleChannel)
        {
            string where = " WHERE A.SID='" + sid + "'";

            if (materialCode != "")
            {
                where += " AND A.ItmNumber='" + materialCode + "'";
            }
            if (saleOrg != "")
            {
                where += " AND (A.SaleOrg='" + saleOrg + "' or A.SaleOrg='*') ";
            }
            if (saleDivision != "")
            {
                where += " AND (A.saleDivision='" + saleDivision + "' or A.SaleDivision='*') ";
            }
            if (saleChannel != "")
            {
                where += " AND (A.saleChannel='" + saleChannel + "' or A.saleChannel='*') ";
            }

            string sql = "SELECT DISTINCT A.SalesUoM, B.UDESC AS SalesUoMDesc " +
                " FROM master_mm_item_saledata A " +
                " INNER JOIN master_mm_weight_setup B" +
                " ON A.SID = B.SID AND A.SalesUoM = B.UCODE" + where;

            DataTable dt = dbService.selectDataFocusone(sql);

            return dt;
        }


        #region Material price Access Sequence

        public DataTable SearchListAccessSequenceForMaterial(string sid, string AccSeqCode)
        {
            string sql = @"
                  SELECT item.AccSeqCode,seq.AccSeqDesc
                     
                FROM [sd_conf_access_sequence_item] item

                left join sd_conf_access_sequence seq
				on item.AccSeqCode = seq.AccSeqCode and item.sid = seq.SID

                 where item.sid='" + sid + @"'

                 and item.Pcondition = 'MeterialCode'
            
            ";

            if (!String.IsNullOrEmpty(AccSeqCode))
            {
                sql += " and item.AccSeqCode = '" + AccSeqCode + "' ";
            }
            sql += " group by item.AccSeqCode,seq.AccSeqDesc,item.Pcondition ";

            return dbService.selectDataFocusone(sql);
        }

        public DataTable SearchDataAccessSequence(string sid)
        {
            string sql = @"
                        select  AccSeqCode,AccSeqDesc 
                        ,AccSeqCode+' : '+AccSeqDesc as AccSeqCodeDesc  
                        from dbo.sd_conf_access_sequence 
                        where SID='" + sid + @"'
                        order by AccSeqCode ASC ";
            return dbService.selectDataFocusone(sql);
        }


        public DataTable SearchDataAccessSequenceItem(string sid, string AccSeqCode)
        {
            string sql = @" 
                        select item.steporder as Code

                         ,STUFF((
                            SELECT ' / ' + Pcondition  
                            FROM sd_conf_access_sequence_item
                            WHERE (steporder = item.steporder) and (AccSeqCode = item.AccSeqCode) 
                            FOR XML PATH(''),TYPE).value('(./text())[1]','VARCHAR(MAX)')
                            ,1,2,'') AS Descripttion

                         ,item.steporder+' : '+STUFF((
                                SELECT ' / ' + Pcondition  
                                FROM sd_conf_access_sequence_item
                                WHERE (steporder = item.steporder) and (AccSeqCode = item.AccSeqCode) 
                                FOR XML PATH(''),TYPE).value('(./text())[1]','VARCHAR(MAX)')
                              ,1,2,'') AS CodeDescripttion

                        from dbo.sd_conf_access_sequence_item item
                        where item.SID='" + sid + @"' and item.Pcondition = 'MeterialCode'
                        and item.AccSeqCode='" + AccSeqCode + @"'
                        Group by item.AccSeqCode,item.steporder ";
            return dbService.selectDataFocusone(sql);
        }

        public DataTable searchDataAccessSequenceItemForSearchBindDataScreen(string sid, string AccSeqCode, string AccSeqCodeItem)
        {
            string sql = @"
                        select item.AccSeqCode
                              ,item.steporder as AccSeqItem
                                ,item.sid 
                             ,STUFF((
                                SELECT ', ' + Pcondition  
                                FROM sd_conf_access_sequence_item
                                WHERE  (item.SID=SID ) and (steporder = item.steporder) and (AccSeqCode = item.AccSeqCode) 
                                FOR XML PATH(''),TYPE).value('(./text())[1]','VARCHAR(MAX)')
                              ,1,2,'') AS PrimaryAcc

                            ,'F_'+'" + sid + @"'+'_'+item.AccSeqCode+'_'+item.steporder as tablename
                        from dbo.sd_conf_access_sequence_item item
                        where item.SID='" + sid + @"' and item.Pcondition = 'MeterialCode'
                        and item.AccSeqCode='" + AccSeqCode + "' ";
            if (!string.IsNullOrEmpty(AccSeqCodeItem))
            {
                sql += "  and item.steporder = '" + AccSeqCodeItem + "' ";
            }
            sql += "  Group by item.AccSeqCode,item.steporder,item.sid   ";

            return dbService.selectDataFocusone(sql);
        }

        public DataTable searchDataAccessSequenceItemTable(string sid, string AcSeq, string AcSeqItem
            , string ItemPrimaryKey, string materialCode, string CurrDate, string UOM)
        {
            string tableName = "F_" + sid + "_" + AcSeq + "_" + AcSeqItem;
            string sql = @"Select " + ItemPrimaryKey.Replace(" ", "") + @"
                    m.percentage,
                    m.amount,
                    m.CurrencyCode,
                    m.per,
                    m.UOM,
                    m.validfrom,
                    m.validto,
                    m.xVersion,
                    m.refBOM
                    from " + tableName + @" m 

                    left join master_mm_weight_setup UD
                    on m.UOM=UD.UCODE 
                    and m.SID = UD.SID
            ";

            sql += " where m.sid='" + sid + "' and m.MeterialCode = '" + materialCode + "'  ";
            if (!String.IsNullOrEmpty(CurrDate))
            {
                sql += " and m.validfrom <= '" + CurrDate + "'  and  m.validto >= '" + CurrDate + "'";
            }
            if (!String.IsNullOrEmpty(UOM))
            {
                sql += " and  m.UOM = '" + UOM + @"' ";
            }
            sql += " order by UD.UDESC ASC ";
            DataTable dt = dbService.selectDataFocusone(sql);

            dt.TableName = tableName;
            int nprimary = ItemPrimaryKey.Split(',').Length - 1;
            string[] strprimary = ItemPrimaryKey.Split(',');
            for (int i = 0; i < nprimary; i++)
            {
                dt.Columns[strprimary[i].Trim()].ExtendedProperties.Add("isKey", "True");
            }
            return dt;
        }


        public DataTable searchTableForsetPrimarykey(string sid, string tableName)
        {
            string sql = @"
                    select top 0 * from " + tableName + @"
                    where SID ='" + sid + @"'
                    ";
            DataTable dtTableSelect = dbService.selectDataFocusone(sql);

            string pSql = @" 
                select b.Table_Name,b.Column_name from INFORMATION_SCHEMA.TABLE_CONSTRAINTS a inner join INFORMATION_SCHEMA.KEY_COLUMN_USAGE b
                on a.Table_name = b.Table_name and a.CONSTRAINT_NAME = b.CONSTRAINT_NAME  where a.Table_name = (
                '" + tableName + @"') order by b.table_name
            ";
            DataTable dtPrimary = dbService.selectDataFocusone(pSql);
            if (dtPrimary.Rows.Count > 0)
            {
                DataColumn[] xcolumn = new DataColumn[dtPrimary.Rows.Count];
                for (int i = 0; i < dtPrimary.Rows.Count; i++)
                {
                    xcolumn[i] = dtTableSelect.Columns[dtPrimary.Rows[i]["Column_name"].ToString()];
                }
                dtTableSelect.PrimaryKey = xcolumn;
            }
            dtTableSelect.TableName = tableName;
            return dtTableSelect;
        }

        public DataTable SearchMatPriceHistory(string sid, string CompanyCode, string tablename, string materialCode, string keyDifference)
        {
            string key = "";
            if (!string.IsNullOrEmpty(keyDifference))
            {
                key = keyDifference.Replace(",", "+','+");
            }
            else
            {
                key = "''";
            }

            string sql = @"Select (" + key + @") as KeyDiferrence_data,
                    '" + keyDifference + @"' as KeyDiferrence,
                    m.percentage,
                    m.amount,
                    m.CurrencyCode,
                    m.per,
                    m.UOM,
                    UD.UDESC as UOM_Des,
                    m.validfrom,
                    CONVERT(VARCHAR(10), CAST(m.validfrom AS DATETIME), 103) AS validfromDes,
                    m.validto,
                    CONVERT(VARCHAR(10), CAST(m.validto AS DATETIME), 103) AS validtoDes,
                    m.CREATED_BY,
                    emC.[FirstName_TH]+' '+emC.[LastName_TH] as CREATED_BY_Des,
                    m.CREATED_ON,

                    case ISNULL(m.CREATED_ON,'')
					    when '' then ''
					    else
                        CONVERT(VARCHAR(10), CAST(SUBSTRING(m.CREATED_ON,1,8) AS DATETIME), 103)
                        +' '+CONVERT(VARCHAR(8), Stuff(Stuff(SUBSTRING(m.CREATED_ON,9,6), 3, 0, ':'), 6, 0, ':'), 8) 
					end  CREATED_ON_Des,

                    m.UPDATE_BY,
                    emU.[FirstName_TH]+' '+emU.[LastName_TH] as UPDATE_BY_Des,
                    m.UPDATE_ON,

                   case ISNULL(m.UPDATE_ON,'')
					    when '' then ''
					    else
                        CONVERT(VARCHAR(10), CAST(SUBSTRING(m.UPDATE_ON,1,8) AS DATETIME), 103)
                        +' '+CONVERT(VARCHAR(8), Stuff(Stuff(SUBSTRING(m.UPDATE_ON,9,6), 3, 0, ':'), 6, 0, ':'), 8) 

					end  UPDATE_ON_Des
                    
                    from " + tablename + @" m 

                    left join master_mm_weight_setup UD
                    on m.UOM=UD.UCODE 
                    and m.SID = UD.SID

                    left join [master_employee] emC
					on emC.SID='" + sid + @"' and emC.[CompanyCode] = '" + CompanyCode + @"'
					and  m.CREATED_BY = emC.[EmployeeCode]  

					left join [master_employee] emU
					on emU.SID='" + sid + @"' and emU.[CompanyCode] = '" + CompanyCode + @"'
					and m.UPDATE_BY = emU.[EmployeeCode]  

                    where m.MeterialCode = '" + materialCode + @"' and m.SID = '" + sid + @"'";

            sql += @" order by validfrom DESC  ";
            //case ISNULL(m.UPDATE_ON,'') 
            //when  '' then CONVERT(decimal, ISNUll(m.CREATED_ON,'00000000000000'))
            //else CONVERT(decimal, m.UPDATE_ON) 
            //end DESC

            return dbService.selectDataFocusone(sql);
        }

        public DataTable SearchUOMForMatPriceCurr(string sid, string tablename, string materialCode, string currDate)
        {
            string sql = @" 
                    select m.UOM as UCODE , u.UDESC 
                    from " + tablename + @" m 
                    left join [master_mm_weight_setup] u 
                    on u.SID = m.SID and m.UOM = u.UCODE 
                    where m.SID='" + sid + @"' 
                     and m.MeterialCode='" + materialCode + @"' 
                     and m.validfrom <= '" + currDate + @"' 
                     and m.validto >= '" + currDate + @"' 
                     group by m.UOM , u.UDESC
                     order by u.UDESC ASC
                ";
            return dbService.selectDataFocusone(sql);
        }

        #endregion 


        #region Config MaterialType and MaterialCategory
        public string getMaterialTypeConfig(string SID, string CompanyCode)
        {
            string MaterialType = "";
            string sql = @"select * from LINK_CRM_CONF_CREATE_MATERIAL where SID='" + SID + "' and CompanyCode='" + CompanyCode + "'";

            DataTable dt = dbService.selectDataFocusone(sql);

            if (dt.Rows.Count > 0)
            {
                MaterialType = dt.Rows[0]["MaterialType"].ToString();
            }

            return MaterialType;
        }

        public string getMaterialCategoryConfig(string SID, string CompanyCode)
        {
            string MaterialCategory = "";
            string sql = @"select * from LINK_CRM_CONF_CREATE_MATERIAL where SID='" + SID + "' and CompanyCode='" + CompanyCode + "'";

            DataTable dt = dbService.selectDataFocusone(sql);

            if (dt.Rows.Count > 0)
            {
                MaterialCategory = dt.Rows[0]["MaterialCategory"].ToString();
            }

            return MaterialCategory;
        }

        public string getCurrencyConfig(string SID, string CompanyCode)
        {
            string Currency = "";
            string sql = @"select * from LINK_CRM_CONF_CREATE_MATERIAL where SID='" + SID + "' and CompanyCode='" + CompanyCode + "'";

            DataTable dt = dbService.selectDataFocusone(sql);

            if (dt.Rows.Count > 0)
            {
                Currency = dt.Rows[0]["Currency"].ToString();
            }

            return Currency;
        }
        #endregion

        public bool IsChackTextboxMatEnable(string SID, string MaterialGroupCode)
        {
            bool IsMatEnable = false;
            string sql = @"select b.ExternalOrNot,a.* 
                            from dbo.master_config_materrial_doctype_docdetail a
                            inner join master_config_material_nr b
                              on a.SID = b.SID 
                              and a.NumberRangeCode = b.NumberRangeCode 
                            where a.SID='" + SID + @"'
                              and a.MaterialGroupCode='" + MaterialGroupCode + @"'";

            foreach (DataRow dr in dbService.selectDataFocusone(sql).Rows)
            {
                IsMatEnable = Convert.ToBoolean(dr["ExternalOrNot"]);
            }

            return IsMatEnable;
        }


        #region updatePrice material

        public void updateMaterialPrice(string SID, string CompanyCode, string MaterialCode, decimal MaterialPrice
           , string CurrencyCode, string UOM, string validfrom, string validto, string conditionkey
           , string UpdatedBy, string UpdatedOn)
        {
            string sqlGetTablePrice = "select * from sd_master_mapping_table_std_price where sid='" + SID + "' and companyCode='" + CompanyCode + "'";

            DataTable dtTable = dbService.selectDataFocusone(sqlGetTablePrice);


            string tableNameForDefault = "";

            if (dtTable.Rows.Count > 0)
            {
                tableNameForDefault = dtTable.Rows[0]["table_price"].ToString();
            }

            string sqlchek = @"
                        SELECT *
                          FROM " + tableNameForDefault + @"
                          where SID='" + SID + @"' and MeterialCode='" + MaterialCode + @"' 
                          and validfrom = '" + validfrom + @"' and validto='" + validto + @"'
                          and [UOM] = '" + UOM + @"' ";
            DataTable dtTableCheckUpdate = dbService.selectDataFocusone(sqlchek);

            if (dtTableCheckUpdate.Rows.Count <= 0)
            {
                MaterialUpdatePriceForSave(
                    SID,
                    tableNameForDefault,
                    MaterialCode,
                    MaterialPrice.ToString(),
                    CurrencyCode,
                    UOM,
                    validfrom,
                    validto,
                   conditionkey,
                   UpdatedBy
                    );
            }
            else
            {
                string sql = @"update " + tableNameForDefault + @" set 
                            amount='" + MaterialPrice + @"'
                            ,UPDATE_BY='" + UpdatedBy + @"'
                            ,UPDATE_ON='" + UpdatedOn + @"'
                            where SID='" + SID + "' and MeterialCode='" + MaterialCode + @"'
                            and validfrom = '" + validfrom + @"' and validto='" + validto + @"'
                            and [UOM] = '" + UOM + @"' ";
                dbService.executeSQLForFocusone(sql);
            }
        }



        public void MaterialUpdatePriceForSave(string sid, string tablename, string materialCode, string MatPrice, String currencyCode, String UOM
            , string validefrom, string valideto, string ConditionKey, string EmpolyeeCode)
        {
            try
            {
                if (MatPrice == "" || validefrom == "" || valideto == "")
                {
                    throw new Exception("กรุณากรอกข้อมูลให้ครบถ้วน");
                }
                decimal matPrice = 0;
                decimal.TryParse(MatPrice, out matPrice);
                int ValidFrom = Convert.ToInt32(validefrom);
                int validTo = Convert.ToInt32(valideto);

                string CreatedOn = Validation.getCurrentServerStringDateTime();

                DataTable dtNew = ckeckAdd_UpdateMatPrive(
                                        sid,
                                        materialCode,
                                        UOM,
                                        ValidFrom, validTo,
                                        tablename,
                                        ConditionKey,
                                        EmpolyeeCode,
                                        CreatedOn);

                //set for save data cuurent key
                DataRow drNew = dtNew.NewRow();
                //checkCurrTableKeyDifference(drNewMin, drPriceCurr, conditionKey);

                if (!string.IsNullOrEmpty(ConditionKey))
                {
                    string[] ArrCondi = ConditionKey.Split('|');
                    for (int i = 0; i < ArrCondi.Length; i++)
                    {
                        string[] key = ArrCondi[i].Split('=');
                        drNew[key[0]] = key[1];
                    }
                }

                drNew["SID"] = sid;
                drNew["MeterialCode"] = materialCode;
                drNew["xVersion"] = "";
                drNew["refBOM"] = "";
                drNew["refPack"] = "";
                drNew["xLineNo"] = "00001";
                drNew["percentage"] = (decimal)0.00;
                drNew["amount"] = matPrice;
                drNew["CurrencyCode"] = currencyCode;
                drNew["per"] = (decimal)1.00;
                drNew["UOM"] = UOM;
                drNew["validfrom"] = validefrom; // new convert
                drNew["validto"] = valideto;
                drNew["xTime"] = "000000";
                drNew["xDate"] = "19000101";
                drNew["Qty"] = 0;
                if (dtNew.Columns.Contains("ScaleUOM"))
                {
                    drNew["ScaleUOM"] = UOM;
                }
                drNew["CREATED_BY"] = EmpolyeeCode;
                drNew["CREATED_ON"] = CreatedOn;
                drNew["UPDATE_BY"] = "";
                drNew["UPDATE_ON"] = "";
                dtNew.Rows.Add(drNew);
                dbService.SaveTransactionForFocusone(dtNew);

            }
            catch (Exception ex)
            {
                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
        }

        private DataTable ckeckAdd_UpdateMatPrive(string sid, string materialcode, string UnitPrice, int validForm,
            int validTo, string tablename, string conditionKey, string CreatedBy, string CreatedOn)
        {
            //string Dtable = matService.checkMaintain(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode);
            DataTable dtMatPriceAll = getMatPriceSortValidFromDESC(
                                        tablename,
                                        sid,
                                        materialcode,
                                        UnitPrice,
                                        conditionKey,
                                        validForm.ToString(),
                                        validTo.ToString());

            DataTable dtNewData = searchTableForsetPrimarykey(sid, tablename);

            DataView dvMat = new DataView(dtMatPriceAll);
            dvMat.RowFilter = "validto > " + validTo.ToString();
            // เซ็ตขอบบน
            string newValideFrom = validTo.ToString();
            if (dvMat.Count > 0)
            {
                newValideFrom = ((
                                    DateTime.ParseExact(validTo.ToString(),
                                    "yyyyMMdd",
                                    new CultureInfo("en-US"),
                                    DateTimeStyles.None)
                                    ).AddDays(+1)).ToString("yyyyMMdd");
            }
            // เซ็ตขอบล่าง
            string newValideTo = ((
                                       DateTime.ParseExact(validForm.ToString(),
                                       "yyyyMMdd",
                                       new CultureInfo("en-US"),
                                       DateTimeStyles.None)
                                     ).AddDays(-1)).ToString("yyyyMMdd");

            foreach (DataRow drPrice in dtMatPriceAll.Rows)
            {
                if ((Convert.ToInt32(drPrice["validfrom"]) >= validForm)
                            && (Convert.ToInt32(drPrice["validfrom"]) <= validTo)
                            && (Convert.ToInt32(drPrice["validto"]) > validTo))
                {

                    setDataToStateDelete(dtNewData, drPrice, conditionKey);
                    setDataToStateAddDataValideFromMax(dtNewData, drPrice, newValideFrom, conditionKey, CreatedBy, CreatedOn);
                }
                else if ((Convert.ToInt32(drPrice["validfrom"]) >= validForm)
                            && (Convert.ToInt32(drPrice["validfrom"]) <= validTo)
                            && (Convert.ToInt32(drPrice["validto"]) <= validTo))
                {
                    setDataToStateDelete(dtNewData, drPrice, conditionKey);
                }

                else if ((Convert.ToInt32(drPrice["validfrom"]) <= validForm)
                            && (Convert.ToInt32(drPrice["validto"]) >= validForm)
                            && (Convert.ToInt32(drPrice["validto"]) <= validTo))
                {
                    setDataToStateDelete(dtNewData, drPrice, conditionKey);
                    setDataToStateAddDataValideToMin(dtNewData, drPrice, newValideTo, conditionKey, CreatedBy, CreatedOn);
                }
                else if ((Convert.ToInt32(drPrice["validfrom"]) <= validForm)
                            && (Convert.ToInt32(drPrice["validto"]) >= validTo))
                {

                    setDataToStateDelete(dtNewData, drPrice, conditionKey);
                    setDataToStateAddDataValideToMin(dtNewData, drPrice, newValideTo, conditionKey, CreatedBy, CreatedOn);
                    setDataToStateAddDataValideFromMax(dtNewData, drPrice, newValideFrom, conditionKey, CreatedBy, CreatedOn);

                }

                else if ((Convert.ToInt32(drPrice["validfrom"]) <= validForm)
                            && (Convert.ToInt32(drPrice["validto"]) <= validTo))
                {
                    return dtNewData;
                }
            }

            return dtNewData;
        }


        //check sub date valideform and validTo
        private void setDataToStateDelete(DataTable dtNewDataForSave, DataRow drPriceCurr, string conditionKey)
        {
            DataRow drAddDelete = dtNewDataForSave.NewRow();
            checkCurrTableKeyDifference(dtNewDataForSave, drAddDelete, drPriceCurr, conditionKey);

            drAddDelete["SID"] = drPriceCurr["SID"].ToString();
            drAddDelete["MeterialCode"] = drPriceCurr["MeterialCode"].ToString();
            drAddDelete["xVersion"] = drPriceCurr["xVersion"].ToString();
            drAddDelete["refBOM"] = drPriceCurr["refBOM"].ToString();
            drAddDelete["refPack"] = drPriceCurr["refPack"].ToString();
            drAddDelete["xLineNo"] = drPriceCurr["xLineNo"].ToString();
            drAddDelete["percentage"] = drPriceCurr["percentage"].ToString();
            drAddDelete["amount"] = drPriceCurr["amount"].ToString();
            drAddDelete["CurrencyCode"] = drPriceCurr["CurrencyCode"].ToString();
            drAddDelete["per"] = drPriceCurr["per"].ToString();
            drAddDelete["UOM"] = drPriceCurr["UOM"].ToString();
            drAddDelete["validfrom"] = drPriceCurr["validfrom"].ToString(); // add data To delete row in data base
            drAddDelete["validto"] = drPriceCurr["validto"].ToString();
            drAddDelete["xTime"] = drPriceCurr["xTime"].ToString();
            drAddDelete["xDate"] = drPriceCurr["xDate"].ToString();
            drAddDelete["Qty"] = drPriceCurr["Qty"].ToString();
            drAddDelete["CREATED_BY"] = drPriceCurr["CREATED_BY"].ToString();
            drAddDelete["CREATED_ON"] = drPriceCurr["CREATED_ON"].ToString();
            drAddDelete["UPDATE_BY"] = drPriceCurr["UPDATE_BY"].ToString();
            drAddDelete["UPDATE_ON"] = drPriceCurr["UPDATE_ON"].ToString();
            dtNewDataForSave.Rows.Add(drAddDelete);
            dtNewDataForSave.Rows[dtNewDataForSave.Rows.Count - 1].AcceptChanges();
            dtNewDataForSave.Rows[dtNewDataForSave.Rows.Count - 1].Delete();
        }

        private void setDataToStateAddDataValideFromMax(DataTable dtNewDataForSave, DataRow drPriceCurr,
            string newValideFrom, string conditionKey, string CreatedBy, string CreatedOn)
        {
            DataRow drNewMax = dtNewDataForSave.NewRow();
            checkCurrTableKeyDifference(dtNewDataForSave, drNewMax, drPriceCurr, conditionKey);

            drNewMax["SID"] = drPriceCurr["SID"].ToString();
            drNewMax["MeterialCode"] = drPriceCurr["MeterialCode"].ToString();
            drNewMax["xVersion"] = drPriceCurr["xVersion"].ToString();
            drNewMax["refBOM"] = drPriceCurr["refBOM"].ToString();
            drNewMax["refPack"] = drPriceCurr["refPack"].ToString();
            drNewMax["xLineNo"] = drPriceCurr["xLineNo"].ToString();
            drNewMax["percentage"] = drPriceCurr["percentage"].ToString();
            drNewMax["amount"] = drPriceCurr["amount"].ToString();
            drNewMax["CurrencyCode"] = drPriceCurr["CurrencyCode"].ToString();
            drNewMax["per"] = drPriceCurr["per"].ToString();
            drNewMax["UOM"] = drPriceCurr["UOM"].ToString();
            drNewMax["validfrom"] = newValideFrom; // new convert
            drNewMax["validto"] = drPriceCurr["validto"].ToString();
            drNewMax["xTime"] = drPriceCurr["xTime"].ToString();
            drNewMax["xDate"] = drPriceCurr["xDate"].ToString();
            drNewMax["Qty"] = drPriceCurr["Qty"].ToString();
            drNewMax["CREATED_BY"] = drPriceCurr["CREATED_BY"].ToString();
            drNewMax["CREATED_ON"] = drPriceCurr["CREATED_ON"].ToString();
            drNewMax["UPDATE_BY"] = CreatedBy;
            drNewMax["UPDATE_ON"] = CreatedOn;
            dtNewDataForSave.Rows.Add(drNewMax);

        }

        private void setDataToStateAddDataValideToMin(DataTable dtNewDataForSave, DataRow drPriceCurr,
            string newValideTo, string conditionKey, string CreatedBy, string CreatedOn)
        {
            DataRow drNewMin = dtNewDataForSave.NewRow();
            checkCurrTableKeyDifference(dtNewDataForSave, drNewMin, drPriceCurr, conditionKey);

            drNewMin["SID"] = drPriceCurr["SID"].ToString();
            drNewMin["MeterialCode"] = drPriceCurr["MeterialCode"].ToString();
            drNewMin["xVersion"] = drPriceCurr["xVersion"].ToString();
            drNewMin["refBOM"] = drPriceCurr["refBOM"].ToString();
            drNewMin["refPack"] = drPriceCurr["refPack"].ToString();
            drNewMin["xLineNo"] = drPriceCurr["xLineNo"].ToString();
            drNewMin["percentage"] = drPriceCurr["percentage"].ToString();
            drNewMin["amount"] = drPriceCurr["amount"].ToString();
            drNewMin["CurrencyCode"] = drPriceCurr["CurrencyCode"].ToString();
            drNewMin["per"] = drPriceCurr["per"].ToString();
            drNewMin["UOM"] = drPriceCurr["UOM"].ToString();
            drNewMin["validfrom"] = drPriceCurr["validfrom"].ToString(); // new convert
            drNewMin["validto"] = newValideTo;
            drNewMin["xTime"] = drPriceCurr["xTime"].ToString();
            drNewMin["xDate"] = drPriceCurr["xDate"].ToString();
            drNewMin["Qty"] = drPriceCurr["Qty"].ToString();
            drNewMin["CREATED_BY"] = drPriceCurr["CREATED_BY"].ToString();
            drNewMin["CREATED_ON"] = drPriceCurr["CREATED_ON"].ToString();
            drNewMin["UPDATE_BY"] = CreatedBy;
            drNewMin["UPDATE_ON"] = CreatedOn;
            dtNewDataForSave.Rows.Add(drNewMin);

        }

        //check key isnull in table difference
        private void checkCurrTableKeyDifference(DataTable dtNewForSave, DataRow drNewDataForSave, DataRow drPriceCurr, string conditionKey)
        {
            if (!string.IsNullOrEmpty(conditionKey))
            {
                string[] ArrCondi = conditionKey.Split('|');
                for (int i = 0; i < ArrCondi.Length; i++)
                {
                    string key = ArrCondi[i].Split('=')[0];
                    drNewDataForSave[key] = drPriceCurr[key];
                }
            }

            if (dtNewForSave.Columns.Contains("ScaleUOM"))
            {
                drNewDataForSave["ScaleUOM"] = drPriceCurr["ScaleUOM"];
            }
        }
        #endregion

        public DataTable getMaterialInventory(string sid, string companyCode, string materialCode, string plantCode)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select a.ItmNumber as MaterialCode, a.ItmDescription as MaterialDesc, b.PLANTCODE as Plant, b.STORAGELOCCODE as Storage,");
            sql.AppendLine("c.BaseUOM, d.PLANTNAME1 as PlantDesc, e.StoreName as StorageDesc, isnull(f.qty, 0) as QOH, g.UDESC as UomDesc");
            sql.AppendLine("from master_mm_items a");
            sql.AppendLine("inner join mm_master_material_org b");
            sql.AppendLine("on a.SID = b.SID and a.ItmNumber = b.MATNR and b.STATUS_PLANT='A'");
            sql.AppendLine("inner join master_mm_item_general c");
            sql.AppendLine("on a.SID = c.SID and a.ItmNumber = c.ItmNumber");
            sql.AppendLine("left join mm_conf_define_plant d");
            sql.AppendLine("on b.SID = d.SID and b.PLANTCODE = d.PLANTCODE");
            sql.AppendLine("left join mm_conf_define_storagelocation e");
            sql.AppendLine("on b.SID = e.SID and b.PLANTCODE = e.PLANTCODE and b.STORAGELOCCODE = e.STORAGELOCCODE");
            sql.AppendLine("left join stockByMaterial f");
            sql.AppendLine("on b.SID = f.SID and b.MATNR = f.ITMNUMBER and b.PLANTCODE = f.PLANT and b.STORAGELOCCODE = f.STORAGELOCATION and b.RACKBIN = f.RACKBIN and f.COMPANYCODE='" + companyCode + "'");
            sql.AppendLine("left join master_mm_weight_setup g");
            sql.AppendLine("on c.SID = g.SID and c.BaseUOM = g.UCODE");
            sql.AppendLine("where a.SID='" + sid + "' and a.ItmNumber='" + materialCode + "'");
            sql.AppendLine(Validation.CreateString(plantCode, "b.PLANTCODE"));
            sql.AppendLine("order by Plant asc, Storage asc");

            DataTable dt = dbService.selectDataFocusone(sql.ToString());

            return dt;
        }

        public DataTable GetMaterialSalesUnit(string sid, string materialCode)
        {
            // Get Base UOM
            string sql = @"SELECT a.BaseUOM AS UCODE, b.UDESC FROM master_mm_item_general a
                           INNER JOIN master_mm_weight_setup b
                           ON a.SID = b.SID AND a.BaseUOM = b.UCODE
                           WHERE a.SID = '" + sid + "' AND a.ItmNumber = '" + materialCode + "'";

            DataTable dtBase = dbService.selectDataFocusone(sql);

            DataTable dtResult = dtBase.Copy();

            // Get Sale Data
            sql = @"SELECT a.SalesUoM AS UCODE, b.UDESC FROM master_mm_item_saledata a
                    INNER JOIN master_mm_weight_setup b
                    ON a.SID = b.SID AND a.SalesUoM = b.UCODE
                    WHERE a.SID = '" + sid + "' AND a.ItmNumber = '" + materialCode + "'";

            DataTable dtSales = dbService.selectDataFocusone(sql);

            foreach (DataRow dr in dtSales.Rows)
            {
                DataRow[] drr = dtResult.Select("UCODE = '" + dr["UCODE"] + "'");

                if (drr.Length == 0)
                {
                    dtResult.ImportRow(dr);
                }
            }

            dtResult.Columns.Add("xDisplay", typeof(System.String));

            foreach (DataRow dr in dtResult.Rows)
            {
                dr["xDisplay"] = ObjectUtil.PrepareCodeAndDescription(dr["UCODE"].ToString(), dr["UDESC"].ToString());
            }

            return dtResult;
        }
    }
}