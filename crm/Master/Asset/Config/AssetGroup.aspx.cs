using Agape.FocusOne.Utilities;
using Agape.Lib.DBService;
using ServiceWeb.auth;
using ERPW.Lib.Authentication;
using SNA.Lib.Transaction;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.crm.Master.Asset.Config
{
    public partial class AssetGroup : AbstractsSANWebpage //System.Web.UI.Page
    {

        #region Service
        DBService db = new DBService();
        AssetService assetService = new AssetService();

        #endregion

        #region DataTable

        protected DataTable DTGridView
        {
            get
            {
                if (Session["AssetGroup.DTGridView"] == null)
                {
                    Session["AssetGroup.DTGridView"] = new DataTable();
                }
                return (DataTable)Session["AssetGroup.DTGridView"];
            }
            set { Session["AssetGroup.DTGridView"] = value; }
        }
        #endregion


        protected void getDataAssetGroup()
        {
            string sql = "";
            sql += " SELECT A.* ";
            sql += " , D.xStart ,D.xEnd ,D.Year , D.PrefixCode, D.SuffixCode ,D.ExternalOrNot ";
            sql += " FROM am_define_assetgroup AS A ";
            sql += " INNER JOIN master_config_asset_doctype AS B ";
            sql += " ON A.SID = B.SID AND  A.AssetGroup = B.AssetClass ";

            sql += " INNER JOIN master_config_asset_doctype_docdetail AS C ";
            sql += " ON A.SID = C.SID  AND B.AssetClass = C.AssetClass ";

            sql += " INNER JOIN master_config_asset_nr AS D ";
            sql += " ON B.SID = D.SID AND C.AssetClass = D.NumberRangeCode ";

            sql += " INNER JOIN master_config_asset_nr_mapping AS E ";
            sql += " ON A.SID = E.SID  AND D.NumberRangeCode = E.AssetClass ";

            sql += " WHERE A.SID='" + ERPWAuthentication.SID + "' ";
            sql += " AND A.CompanyCode='" + ERPWAuthentication.CompanyCode + "' ";

            //if (!String.IsNullOrEmpty(txtAssetGroup.Text))
            //{
            //    sql += " AND A.AssetGroup = '" + txtAssetGroup.Text + "'";
            //}


            DTGridView = db.selectDataFocusone(sql);
            gvData.DataSource = DTGridView;
            gvData.DataBind();
            updatePanel.Update();
            clearData();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                getDataAssetGroup();

            }
        }



        protected void gvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvData.PageIndex = e.NewPageIndex;
                gvData.DataSource = DTGridView;
                gvData.DataBind();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(updatePanel, updatePanel.GetType(), "msgbox", "alertMessage('" + ObjectUtil.Err(ex.Message) + "');", true);
            }
        }

        protected void GridgvData_EditItem(object sender, EventArgs e)
        {
            string code = Convert.ToString(((LinkButton)sender).CommandName);
            string[] str = code.Split(',');

            DataRow[] drr = DTGridView.Select("SID = '" + str[0] + "' and CompanyCode = '" + str[1] + "' and AssetGroup = '" + str[2] + "'");
            if (drr.Length > 0)
            {
                DataRow dr = drr[0];
                txtAssetGroup.Text = Convert.ToString(dr["AssetGroup"]);
                txtAssetGroup.Enabled = false;
                txtAssetGroupDescription.Text = Convert.ToString(dr["Description"]);
                popupNumberRange.Text = Convert.ToString(dr["NumberRangeCode"]);
                popupPrefix.Text = Convert.ToString(dr["PrefixCode"]);
                txtAssetAccountCode.Text = Convert.ToString(dr["AssetAccountCode"]);
                txtDepreciationAccCode.Text = Convert.ToString(dr["DepreciationAccCode"]);
                txtAccumlateDepreAccCode.Text = Convert.ToString(dr["AccumlateDepreAccCode"]);
                txtAssetClearingAccCode.Text = Convert.ToString(dr["AssetClearingAccCode"]);
                ChkxActive.Checked = Convert.ToBoolean(dr["xActive"]);
                popupStart.Text = Convert.ToString(dr["xStart"]);
                popupEnd.Text = Convert.ToString(dr["xEnd"]);
                popupSuffix.Text = Convert.ToString(dr["SuffixCode"]);
                popupYear.Text = Convert.ToString(dr["Year"]);
                popupYear.Enabled = false;
                popupExternal.Checked = Convert.ToBoolean(dr["ExternalOrNot"]);
                popupRequrst.Checked = Convert.ToBoolean(dr["RequestLocation"]);

            }
            btnCreateModal.Visible = false;
            btnEdit.Visible = true;
            udpnPopup.Update();
            ClientService.DoJavascript("$('#formgvData').modal('show');");
        }


        protected void GridgvData_DeleteItem(object sender, EventArgs e)
        {
            string code = Convert.ToString(((LinkButton)sender).CommandName);
            string[] str = code.Split(',');

            string sql = "Delete FROM am_define_assetgroup ";
            sql += " Where SID = '" + str[0] + "' and CompanyCode ='" + str[1] + "' ";
            sql += " AND AssetGroup ='" + str[2] + "'; ";

            sql += "Delete FROM master_config_asset_doctype ";
            sql += " Where SID = '" + str[0] + "' and AssetClass ='" + str[2] + "' ;";


            sql += "Delete FROM master_config_asset_doctype_docdetail ";
            sql += " Where SID = '" + str[0] + "' and AssetClass = '" + str[2] + "';";


            sql += "Delete FROM master_config_asset_nr ";
            sql += " Where SID = '" + str[0] + "' and NumberRangeCode = '" + str[2] + "';";


            sql += "Delete FROM master_config_asset_nr_mapping ";
            sql += " Where SID = '" + str[0] + "' and AssetClass = '" + str[2] + "';";

            db.selectDataFocusone(sql);
            ClientService.DoJavascript("AGSuccess('Delete success.')");
            updatePanel.Update();
            getDataAssetGroup();

        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {

            clearData();
            btnCreateModal.Visible = true;
            btnEdit.Visible = false;
            udpnPopup.Update();
            ClientService.DoJavascript("$('#formgvData').modal('show');");
        }

        protected void btnCreateModal_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(txtAssetGroup.Text))
                {
                    throw new Exception("กรุณากรอก Asset Group.");
                }
                if (String.IsNullOrEmpty(txtAssetGroupDescription.Text))
                {
                    throw new Exception("กรุณากรอก Asset Group Description.");
                }
                if (String.IsNullOrEmpty(popupPrefix.Text))
                {
                    throw new Exception("กรุณากรอก Prefix.");
                }
                if (String.IsNullOrEmpty(popupStart.Text))
                {
                    throw new Exception("กรุณากรอก Start.");
                }
                if (String.IsNullOrEmpty(popupEnd.Text))
                {
                    throw new Exception("กรุณากรอก End.");
                }

                List<String> queryList = new List<string>();

                #region am_define_assetgroup

                string sql_ssetgroup = "insert into am_define_assetgroup ";
                sql_ssetgroup += "(SID,CompanyCode,AssetGroup,Description,Created_By,Created_On,NumberRangeCode,AssetAccountCode ";
                sql_ssetgroup += ",DepreciationAccCode,AccumlateDepreAccCode,AssetClearingAccCode,xActive,RequestLocation,UPDATED_ON)";
                sql_ssetgroup += " Values ";
                sql_ssetgroup += "('" + ERPWAuthentication.SID + "'";
                sql_ssetgroup += ",'" + ERPWAuthentication.CompanyCode + "'";
                sql_ssetgroup += ",'" + txtAssetGroup.Text + "'";
                sql_ssetgroup += ",'" + txtAssetGroupDescription.Text + "'";
                sql_ssetgroup += ",'" + ERPWAuthentication.EmployeeCode + "'";
                sql_ssetgroup += ",'" + Validation.getCurrentServerStringDateTime() + "'";
                sql_ssetgroup += ",'" + txtAssetGroup.Text + "'";
                sql_ssetgroup += ",'" + txtAssetAccountCode.Text + "'";
                sql_ssetgroup += ",'" + txtDepreciationAccCode.Text + "'";
                sql_ssetgroup += ",'" + txtAccumlateDepreAccCode.Text + "'";
                sql_ssetgroup += ",'" + txtAssetClearingAccCode.Text + "'";
                sql_ssetgroup += ",'" + Convert.ToString(ChkxActive.Checked) + "'";
                sql_ssetgroup += ",'" + Convert.ToString(popupRequrst.Checked) + "'";
                sql_ssetgroup += ",'')";




                queryList.Add(sql_ssetgroup);
                #endregion


                #region master_config_asset_doctype

                string master_config_asset_doctype = "insert into master_config_asset_doctype ";
                master_config_asset_doctype += "(SID,Companycode,AssetClass,Description,CREATED_BY,CREATED_ON,xActive,PrefixCode,UPDATED_ON)";
                master_config_asset_doctype += " Values ";
                master_config_asset_doctype += "('" + ERPWAuthentication.SID + "'";
                master_config_asset_doctype += ",'*'";
                master_config_asset_doctype += ",'" + txtAssetGroup.Text + "'";
                master_config_asset_doctype += ",'" + txtAssetGroupDescription.Text + "'";
                master_config_asset_doctype += ",'" + ERPWAuthentication.EmployeeCode + "'";
                master_config_asset_doctype += ",'" + Validation.getCurrentServerStringDateTime() + "'";
                master_config_asset_doctype += ",'" + Convert.ToString(ChkxActive.Checked) + "'";
                master_config_asset_doctype += ",'" + popupPrefix.Text + "'";
                master_config_asset_doctype += ",'')";

                queryList.Add(master_config_asset_doctype);
                #endregion

                #region master_config_asset_doctype_docdetail
                string master_config_asset_doctype_docdetail = "insert into master_config_asset_doctype_docdetail ";
                master_config_asset_doctype_docdetail += "(AssetClass,SID,NumberRangeCode,ReverseDocumentType,AuthorizationGroup,Asset,Customer,Vendor,Meterial,GlAccount ";
                master_config_asset_doctype_docdetail += " ,NetDocumentType,CustVendCheck,NegativePosting,InterCompany,EnterTradingPartner,ReferenceNumber,DocumentHeader,BtchInputOnly,ExRateType,DebitRecInd ";
                master_config_asset_doctype_docdetail += " ,RecIndCredit,CREATED_BY,PostingType,companyCode,AccountDP,CREATED_ON,FinancialDocType,FIControl ";
                master_config_asset_doctype_docdetail += " ,ProfitTran,LostProfitTran,ARDocType,AMIDocType,AMODocType,AutoPost,UPDATED_ON )";
                master_config_asset_doctype_docdetail += " Values ";
                master_config_asset_doctype_docdetail += "('" + txtAssetGroup.Text + "'";
                master_config_asset_doctype_docdetail += ",'" + ERPWAuthentication.SID + "'";
                master_config_asset_doctype_docdetail += ",'" + txtAssetGroup.Text + "'";
                master_config_asset_doctype_docdetail += ",''";
                master_config_asset_doctype_docdetail += ",''";
                master_config_asset_doctype_docdetail += ",'False'";
                master_config_asset_doctype_docdetail += ",'False'";
                master_config_asset_doctype_docdetail += ",'False'";
                master_config_asset_doctype_docdetail += ",'False'";
                master_config_asset_doctype_docdetail += ",'False'";

                master_config_asset_doctype_docdetail += ",'False'";
                master_config_asset_doctype_docdetail += ",'False'";
                master_config_asset_doctype_docdetail += ",'False'";
                master_config_asset_doctype_docdetail += ",'False'";
                master_config_asset_doctype_docdetail += ",'False'";
                master_config_asset_doctype_docdetail += ",'False'";
                master_config_asset_doctype_docdetail += ",''";
                master_config_asset_doctype_docdetail += ",'False'";
                master_config_asset_doctype_docdetail += ",''";
                master_config_asset_doctype_docdetail += ",''";

                master_config_asset_doctype_docdetail += ",''";
                master_config_asset_doctype_docdetail += ",'" + ERPWAuthentication.EmployeeCode + "'";
                master_config_asset_doctype_docdetail += ",'AM'";
                master_config_asset_doctype_docdetail += ",'*'";
                master_config_asset_doctype_docdetail += ",''";
                master_config_asset_doctype_docdetail += ",'" + Validation.getCurrentServerStringDateTime() + "'";
                master_config_asset_doctype_docdetail += ",''";
                master_config_asset_doctype_docdetail += ",'False'";

                master_config_asset_doctype_docdetail += ",''";
                master_config_asset_doctype_docdetail += ",''";
                master_config_asset_doctype_docdetail += ",''";
                master_config_asset_doctype_docdetail += ",''";
                master_config_asset_doctype_docdetail += ",''";
                master_config_asset_doctype_docdetail += ",'False'";
                master_config_asset_doctype_docdetail += ",'')";


                queryList.Add(master_config_asset_doctype_docdetail);

                #endregion

                #region master_config_asset_nr
                string master_config_asset_nr = "insert into master_config_asset_nr ";
                master_config_asset_nr += "(SID,NumberRangeCode,CompanyCode,Year,xStart,xEnd,xCurrent,ExternalOrNot,CREATED_BY,CREATED_ON,UPDATED_ON ";
                master_config_asset_nr += ",FreeDefine,PrefixCode,SuffixCode)";
                master_config_asset_nr += " Values ";
                master_config_asset_nr += "('" + ERPWAuthentication.SID + "'";
                master_config_asset_nr += ",'" + txtAssetGroup.Text + "'";
                master_config_asset_nr += ",'*'";
                master_config_asset_nr += ",'*'";
                master_config_asset_nr += ",'" + popupStart.Text + "'";
                master_config_asset_nr += ",'" + popupEnd.Text + "'";
                master_config_asset_nr += ",''";
                master_config_asset_nr += ",'" + Convert.ToString(popupExternal.Checked) + "'";
                master_config_asset_nr += ",'" + ERPWAuthentication.EmployeeCode + "'";
                master_config_asset_nr += ",'" + Validation.getCurrentServerStringDateTime() + "'";
                master_config_asset_nr += ",''";
                master_config_asset_nr += ",'False'";
                master_config_asset_nr += ",'" + popupPrefix.Text + "'";
                master_config_asset_nr += ",'" + popupSuffix.Text + "')";


                queryList.Add(master_config_asset_nr);
                #endregion

                #region master_config_asset_nr_mapping

                string master_config_asset_nr_mapping = "insert into master_config_asset_nr_mapping ";
                master_config_asset_nr_mapping += "(SID,AssetClass,NumberRangeCode,CompanyCode,CREATED_BY,CREATED_ON,UPDATED_ON)";
                master_config_asset_nr_mapping += " Values ";
                master_config_asset_nr_mapping += "('" + ERPWAuthentication.SID + "'";
                master_config_asset_nr_mapping += ",'" + txtAssetGroup.Text + "'";
                master_config_asset_nr_mapping += ",'" + txtAssetGroup.Text + "'";
                master_config_asset_nr_mapping += ",'*'";
                master_config_asset_nr_mapping += ",'" + ERPWAuthentication.EmployeeCode + "'";
                master_config_asset_nr_mapping += ",'" + Validation.getCurrentServerStringDateTime() + "'";
                master_config_asset_nr_mapping += ",'')";

                queryList.Add(master_config_asset_nr_mapping);
                #endregion

                db.executeSQLForFocusone(queryList);
                ClientService.DoJavascript("$('.modal').modal('hide');");
                ClientService.DoJavascript("AGSuccess('Create success.')");
                getDataAssetGroup();
            }
            catch (Exception ex)
            {

                ClientService.DoJavascript("AGError('" + ObjectUtil.Err(ex.Message) + "')");
            }



        }
        protected void btnEditModal_Click(object sender, EventArgs e)
        {

            string sql = "Update am_define_assetgroup SET ";
            sql += "Description ='" + txtAssetGroupDescription.Text + "' ";
            sql += ",Updated_By ='" + ERPWAuthentication.EmployeeCode + "' ";
            sql += ",Updated_On = '" + Validation.getCurrentServerStringDateTime() + "'";
            sql += ",NumberRangeCode  ='" + popupNumberRange.Text + "' ";
            sql += ",AssetAccountCode  ='" + txtAssetAccountCode.Text + "' ";
            sql += ",DepreciationAccCode  ='" + txtDepreciationAccCode.Text + "' ";
            sql += ",AccumlateDepreAccCode  ='" + txtAccumlateDepreAccCode.Text + "' ";
            sql += ",AssetClearingAccCode  ='" + txtAssetClearingAccCode.Text + "' ";
            sql += ",xActive = '" + Convert.ToString(ChkxActive.Checked) + "' ";
            sql += ",RequestLocation = '" + Convert.ToString(popupRequrst.Checked) + "' ";
            sql += " Where SID = '" + ERPWAuthentication.SID + "' and CompanyCode = '" + ERPWAuthentication.CompanyCode + "' ";
            sql += " and AssetGroup ='" + txtAssetGroup.Text + "' ;";


            ////////////////////////
            sql += "Update master_config_asset_doctype SET ";
            sql += "Description ='" + txtAssetGroupDescription.Text + "'";
            sql += ",UPDATED_BY ='" + ERPWAuthentication.EmployeeCode + "' ";
            sql += " ,UPDATED_ON ='" + Validation.getCurrentServerStringDateTime() + "'";
            sql += " ,xActive ='" + Convert.ToString(ChkxActive.Checked) + "'";
            sql += ",PrefixCode = '" + popupPrefix.Text + "'";
            sql += " Where SID = '" + ERPWAuthentication.SID + "' and AssetClass = '" + txtAssetGroup.Text + "' ;";


            ///////////////////////////////////////
            sql += "Update master_config_asset_nr SET ";
            sql += " Year ='" + popupYear.Text + "'";
            sql += " ,xStart ='" + popupStart.Text + "' ";
            sql += " ,xEnd ='" + popupEnd.Text + "'";
            sql += ",ExternalOrNot ='" + Convert.ToString(popupExternal.Checked) + "'";
            sql += ",UPDATED_BY ='" + ERPWAuthentication.EmployeeCode + "'";
            sql += ",UPDATED_ON ='" + Validation.getCurrentServerStringDateTime() + "'";
            sql += ",PrefixCode ='" + popupPrefix.Text + "'";
            sql += ",SuffixCode ='" + popupSuffix.Text + "'";
            sql += " Where NumberRangeCode = '" + txtAssetGroup.Text + "' and SID = '" + ERPWAuthentication.SID + "';";

            db.executeSQLForFocusone(sql);
            ClientService.DoJavascript("$('.modal').modal('hide');");
            ClientService.DoJavascript("AGSuccess('Change success.')");
            getDataAssetGroup();
        }

        private void clearData()
        {

            txtAssetGroup.Text = "";
            txtAssetGroupDescription.Text = "";
            popupNumberRange.Text = "";
            popupPrefix.Text = "";
            txtAssetAccountCode.Text = "";
            txtDepreciationAccCode.Text = "";
            txtAccumlateDepreAccCode.Text = "";
            txtAssetClearingAccCode.Text = "";
            ChkxActive.Checked = true;
            popupStart.Text = "";
            popupEnd.Text = "";
            popupSuffix.Text = "";
            //popupYear.Text = "";
            popupExternal.Checked = true;
            popupRequrst.Checked = true;


        }

    }
}