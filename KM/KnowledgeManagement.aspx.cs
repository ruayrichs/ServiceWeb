using Agape.FocusOne.Utilities;
using ERPW.Lib.Authentication;
using ERPW.Lib.Master;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServiceWeb.auth;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.KM
{
    public partial class KnowledgeManagement : AbstractsSANWebpage
    {
        private KMServiceLibrary kmservice = KMServiceLibrary.getInstance();

        protected override Boolean ProgramPermission_CanView()
        {
            return ERPWAuthentication.Permission.KM_View || ERPWAuthentication.Permission.AllPermission;
        }

        protected override Boolean ProgramPermission_CanModify()
        {
            return ERPWAuthentication.Permission.KM_Modify || ERPWAuthentication.Permission.AllPermission;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadKnowledgeDocType();
            }
        }

        private void loadKnowledgeDocType()
        {
            List<KMCriteriaEntity> listData = kmservice.GetKMGroup(ERPWAuthentication.SID);
            ddlGroup.DataValueField = "xKey";
            ddlGroup.DataTextField = "xValue";
            ddlGroup.DataSource = listData;
            ddlGroup.DataBind();
            ddlGroup.Items.Insert(0, new ListItem("Select", ""));

            ddlGroupModal.DataValueField = "xKey";
            ddlGroupModal.DataTextField = "xValue";
            ddlGroupModal.DataSource = listData;
            ddlGroupModal.DataBind();
            ddlGroupModal.Items.Insert(0, new ListItem("Select", ""));
        }

        private void loadKnowledgeList()
        {
            List<KMHeaderData> listKm = new List<KMHeaderData>();
            string _where = kmservice.getWhereCondition("", "", "", "", ddlGroup.SelectedValue, txtSearch.Text);
            listKm = kmservice.getKMHeaderCondition(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, _where);
            listKm.ForEach(r =>
            {
                r.CREATED_ON = Validation.Convert2DateTimeDisplay(r.CREATED_ON);
            });
            JArray data = JsonConvert.DeserializeObject<JArray>(JsonConvert.SerializeObject(listKm));
            upPanelProfileList.Update();
            ClientService.DoJavascript("afterSearch(" + data + ");");
        }

        protected void btnSearchData_Click(object sender, EventArgs e)
        {
            try
            {
                loadKnowledgeList();
                ClientService.DoJavascript("scrollToTable();");
            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
            finally
            {
                ClientService.AGLoading(false);
            }

        }

        protected void btnOpenModalCreate_Click(object sender, EventArgs e)
        {
            try
            {
                txtKeywordModal.Text = "";
                txtSubjectModal.Text = "";
                txtDetailModal.Text = "";
                txtSymtomModal.Text = "";
                txtCauseModal.Text = "";
                txtSolutionModal.Text = "";
                hhdKeyAobjectlink.Value = Guid.NewGuid().ToString();
                loadTimeLineFileAttachment();
                udpCreateDetail.Update();
                ClientService.DoJavascript("showInitiativeModal('modal-CreateData');");
            }
            catch (Exception ex)
            {
                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
            finally
            {
                ClientService.AGLoading(false);
            }
        }

        protected void btnCreateDetail_Click(object sender, EventArgs e)
        {
            try
            {
                string msg = "";
                if (string.IsNullOrEmpty(ddlGroupModal.SelectedValue))
                {
                    msg += "KM Group is empty!<br/>";
                }

                if (string.IsNullOrEmpty(txtSubjectModal.Text.Trim()))
                {
                    msg += "Subject is empty!<br/>";
                }

                if (!string.IsNullOrEmpty(msg))
                {
                    throw new Exception(msg);
                }

                KMHeaderData en = new KMHeaderData();
                en.KMGroup = ddlGroupModal.SelectedValue;
                en.KMGroupName = "";
                en.ObjectID = "";
                en.PrimaryKeyWord = txtKeywordModal.Text;
                en.Description = txtSubjectModal.Text;
                en.AttachmentID = hhdKeyAobjectlink.Value;

                en.ObjectItem = "001";
                en.Details = txtDetailModal.Text;
                en.Solution = txtSolutionModal.Text;
                en.Symptom = txtSymtomModal.Text;
                en.Cause = txtCauseModal.Text;

                string KMOBjectID = kmservice.saveKMManagement(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    ERPWAuthentication.EmployeeCode,
                    ERPWAuthentication.UserName,
                    en);
               
                ClientService.DoJavascript("closeInitiativeModal('modal-CreateData');");
                string id = setDataToRedirectRefGUID(KMOBjectID);
                ClientService.AGSuccessRedirect("Created knowledge " + KMOBjectID + "  success.", "KnowledgeManagementDetail.aspx?id=" + id);
                
            }
            catch (Exception ex)
            {
                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
            finally
            {
                ClientService.AGLoading(false);
            }
        }


        #region Panel file Attachment
        private void loadTimeLineFileAttachment()
        {
            TimeLineControl.ProgramID = "";// LogServiceLibrary.PROGRAM_ID_KNOWLEDGE_MANAGEMENT;
            TimeLineControl.KeyAobjectlink = hhdKeyAobjectlink.Value;
            TimeLineControl.bindDataTimeLineHasFile();
        }
        #endregion

        protected void btnSubmitRequire_Click(object sender, EventArgs e)
        {
            ClientService.DoJavascript("$('#btnCreateClient').click();");
        }
        protected void btnSubmitRedirect_Click(object sender, EventArgs e)
        {
            try
            {
                string id = setDataToRedirectRefGUID(hddKnowledgeID.Value);
                Response.Redirect("KnowledgeManagementDetail.aspx?id=" + id);
            }
            catch (Exception ex)
            {
                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
            finally
            {
                ClientService.AGLoading(false);
            }
        }
        private string setDataToRedirectRefGUID(string knowledgeID)
        {
            KnowledgeManagementDetail obj = new KnowledgeManagementDetail();
            string id = obj.GUID_RERUEST;
            Session[obj.SessionNameDefault + id] = knowledgeID;
            return id;
        }
    }
}