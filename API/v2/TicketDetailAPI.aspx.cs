using Agape.Lib.DBService;
using Newtonsoft.Json;
using ServiceWeb.API.Model.Common;
using ServiceWeb.API.Model.Respond;
using ServiceWeb.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.API.v2
{
    public partial class TicketDetailAPI : AbstractWebAPI
    {
        DBService db = new DBService();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                TicketDetailResponseModel_V2 response = new TicketDetailResponseModel_V2();
                if (!checkPermission())
                {
                    response.resultCode = "Error";
                    response.message = "No Permission.";
                }
                else
                {
                    response = getDataTicketDetail();
                }
                Response.Write(JsonConvert.SerializeObject(response, Formatting.Indented));
            }
            catch(Exception ex)
            {
                BadRequest(ex.Message);
            }
        }

        private TicketDetailResponseModel_V2 getDataTicketDetail()
        {
            TicketDetailResponseModel_V2 response = new TicketDetailResponseModel_V2();
            try
            {
                inputAPI model = GetInputDataModel<inputAPI>(Request);
                #region //Convert TicketNumber to DB Format & 
                string TicketNumber = AfterSaleService.getInstance().ConvertToTicketDB(model.TicketNumber);

                string ticketType = "";
                string ticketYear = "";
                string ticketNo = "";
                //Get Key For Update
                DataTable dt = AfterSaleService.getInstance().GetTicketDetailByTicketNumber(_SID, _CompanyCode, TicketNumber);
                if (dt.Rows.Count <= 0)
                {
                    throw new Exception("ไม่พบหมายเลข ticket no : " + model.TicketNumber + " ในระบบ!");
                }
                ticketType = Convert.ToString(dt.Rows[0]["Doctype"]);
                ticketYear = Convert.ToString(dt.Rows[0]["Fiscalyear"]);
                ticketNo = Convert.ToString(dt.Rows[0]["CallerID"]);

                string TicketNoDisplay = AfterSaleService.getInstance().GetTicketNoForDisplay(
                        _SID, _CompanyCode, ticketType, ticketNo
                    );

                #endregion

                response.data = GetTicketDetail_V2(ticketNo, TicketNoDisplay);
                //TicketDetailModel_V2 TicketDetail = new TicketDetailModel_V2();
                string aobjectlink = AfterSaleService.getInstance().getAobjectLinkByTicketNumber(
                    ticketNo
                );
                response.data.AssignTo = new List<AssignToModel>();
                response.data.AssignTo.AddRange(AfterSaleService.getInstance().getAssignTo_MainDelegate_ByAobjectLink(
                    _SID, _CompanyCode, aobjectlink
                ));
                response.data.AssignTo.AddRange(AfterSaleService.getInstance().getAssignTo_Participants_ByAobjectLink(
                    _SID, _CompanyCode, aobjectlink
                ));


                response.resultCode = "Success";
                response.message = "Search success.";

            }
            catch (Exception ex)
            {
                response.resultCode = "Error";
                response.message = ex.Message;
            }

            return response;
        }

        private TicketDetailModel_V2 GetTicketDetail_V2(string TicketNumber, string TicketNoDisplay)
        {
            string sql = @"SELECT a.Doctype, a.CallerID, a.Docstatus, b.TicketStatusDesc, a.DOCDATE
                            , a.CallbackDate, a.CallbackTime, a.ReferenceDocument, a.AffectSLA
                            , c.FirstName + ' ' + c.LastName as FullName
                            , a.CREATED_ON, e.EquipmentNo, n.Description as EquipmentDesc, a.CustomerCode
                            , d.CustomerName, a.ContractPersonName
                            , a.ContractPersonName, a.Email, a.ContractPersonTel
                            , a.Impact, a.Urgency, a.Priority, a.HeaderText
                            , e.QueueOption, e.ProblemGroup, e.ProblemTypeCode, e.OriginCode, e.Remark
                            , n.CategoryCode, e.SerialNo, e.CallTypeCode, e.SummaryProblem
                            , e.SummaryCause, e.SummaryResolution, f.ImpactName, g.UrgencyName
                            , h.Description, i.OwnerGroupName, j.GROUPNAME, k.TypeName
                            , l.SourceName, m.AREANAME, p.REMARK1
                            , n.EquipmentType+':'+ s.Description as EquipmentTypeDesc
                            , r.EquipmentClass+':'+ q.ClassName as EquipmentClassDesc
                            , CASE n.CategoryCode 
		                        WHEN '00' THEN 'Main Equipment'
		                        WHEN '01' THEN 'Sub Equipment'
		                        ELSE 'Virtual Equipment'
		                        END AS CategoryName

                            FROM cs_servicecall_header a WITH (NOLOCK)

                            LEFT JOIN ERPW_TICKET_STATUS b WITH (NOLOCK)
                            on a.SID = b.SID and a.CompanyCode = b.CompanyCode
                            and a.Docstatus = b.TicketStatusCode

                            LEFT JOIN master_employee c WITH (NOLOCK)
                            on a.SID = c.SID and a.CompanyCode = c.CompanyCode
                            and a.CREATED_BY = c.EmployeeCode

                            LEFT JOIN master_customer d WITH (NOLOCK)
                            on a.SID = d.SID and a.CompanyCode = d.CompanyCode
                            and a.CustomerCode = d.CustomerCode

                            LEFT JOIN cs_servicecall_item e WITH (NOLOCK)
                            on a.SID = d.SID and a.CompanyCode = d.CompanyCode
                            and a.ObjectID = e.ObjectID

                            LEFT JOIN master_config_impact f WITH (NOLOCK)
                            on a.SID = f.SID and a.Impact = f.ImpactCode

                            LEFT JOIN master_config_urgency g WITH (NOLOCK)
                            on a.SID = g.SID and a.Urgency = g.UrgencyCode

                            LEFT JOIN ERPW_BUSINESSOBJECT_MAPPING_TICKET_TYPE business WITH (NOLOCK) 
                            on business.SID = a.SID
                            AND business.TicketType = a.Doctype

                            LEFT JOIN master_config_Priority h WITH (NOLOCK)
                            on a.SID = g.SID and a.Priority = h.PriorityCode

                            LEFT JOIN ERPW_OWNER_GROUP i WITH (NOLOCK)
                            on a.SID = i.SID and e.QueueOption = i.OwnerGroupCode

                            LEFT JOIN ERPW_TICKET_AREA_PROBLEM_GROUP_MASTER j WITH (NOLOCK)
                            on a.SID = j.SID and j.BUSINESSOBJECT = business.BusinessObject
                            and e.ProblemGroup = j.GROUPCODE

                            LEFT JOIN ERPW_TICKET_AREA_PROBLEM_TYPE_MASTER k WITH (NOLOCK)
                            on a.SID = k.SID and k.BUSINESSOBJECT = business.BusinessObject
                            and e.ProblemGroup = k.GROUPCODE
                            and e.QueueOption = k.OwnerGroupCode and e.ProblemTypeCode = k.TYPECODE

                            LEFT JOIN ERPW_TICKET_AREA_PROBLEM_SOURCE_MASTER l WITH (NOLOCK)
                            on a.SID = l.SID and l.BUSINESSOBJECT = business.BusinessObject
                            and e.ProblemGroup = l.GROUPCODE and e.ProblemTypeCode = l.TypeCode 
							and e.OriginCode = l.SourceCode and e.QueueOption = l.OwnerGroupCode

                            LEFT JOIN ERPW_TICKET_AREA_CONTACT_SOURCE_MASTER m WITH (NOLOCK)
                            on a.SID = m.SID and m.BUSINESSOBJECT = business.BusinessObject
                            and e.ProblemGroup = m.GROUPCODE and e.ProblemTypeCode = m.TYPECODE 
							and e.QueueOption = m.OwnerGroupCode and e.OriginCode = m.SourceCode
							and e.CallTypeCode = m.AREACODE

                            LEFT JOIN master_equipment n WITH (NOLOCK)
                            on a.SID = n.SID and a.CompanyCode = n.CompanyCode
                            and e.EquipmentNo = n.EquipmentCode

                            LEFT JOIN CONTACT_MASTER o WITH (NOLOCK)
                            on a.SID = o.SID and a.CompanyCode = o.CompanyCode
                            and d.CustomerCode = o.BPCODE

                            LEFT JOIN CONTACT_DETAILS p WITH (NOLOCK)
                            on a.SID = p.SID and o.AOBJECTLINK = p.AOBJECTLINK
                            and p.NAME1 = a.ContractPersonName

                            LEFT JOIN master_equipment_general r WITH (NOLOCK)
                            ON a.SID = r.SID AND a.CompanyCode = r.CompanyCode
	                        AND e.EquipmentNo = r.EquipmentCode

                            LEFT JOIN master_config_material_doctype s WITH (NOLOCK)
                            ON a.SID = s.SID AND a.CompanyCode = s.CompanyCode
	                        AND n.EquipmentType = s.MaterialGroupCode

                            LEFT JOIN master_equipment_class q WITH (NOLOCK)
                            ON a.SID = q.SID AND r.EquipmentClass = q.ClassCode


                    where a.sid='" + _SID + "' AND a.CompanyCode = '" + _CompanyCode + "'";
            if (!string.IsNullOrEmpty(TicketNumber))
            {
                sql += " AND a.CallerID='" + TicketNumber + "'";
            }

            DataTable dt = db.selectDataFocusone(sql);
            TicketDetailModel_V2 list = GetToListDetail(dt, TicketNoDisplay);
            //TicketDetailModel_V2 list = JsonConvert.DeserializeObject<TicketDetailModel_V2>(JsonConvert.SerializeObject(dt));
            return list;
        }

        private TicketDetailModel_V2 GetToListDetail(DataTable dt, string TicketNoDisplay)
        {
            TicketDetailModel_V2 Detail = new TicketDetailModel_V2();
            foreach (DataRow dr in dt.Rows)
            {

                Detail.TicketType = Convert.ToString(dr["Doctype"]);
                Detail.TicketNumber = TicketNoDisplay;
                Detail.TicketNumberDisplay = Convert.ToString(dr["CallerID"]);
                Detail.TicketStatusCode = Convert.ToString(dr["Docstatus"]);
                Detail.TicketStatusDesc = Convert.ToString(dr["TicketStatusDesc"]);
                Detail.TicketDate = Convert.ToString(dr["DOCDATE"]);
                Detail.TicketCallBackDate = Convert.ToString(dr["CallbackDate"]);
                Detail.TicketCallBackTime = Convert.ToString(dr["CallbackTime"]);
                Detail.ReferenceExternalTicket = Convert.ToString(dr["ReferenceDocument"]);
                Detail.TicketCreatedBy = Convert.ToString(dr["FullName"]);
                Detail.TicketCreatedDateTime = Convert.ToString(dr["CREATED_ON"]);

                Detail.CustomerCode = Convert.ToString(dr["CustomerCode"]);
                Detail.CustomerName = Convert.ToString(dr["CustomerName"]);
                Detail.ContactCode = Convert.ToString(dr["ContractPersonName"]);
                Detail.ContactName = Convert.ToString(dr["ContractPersonName"]);
                Detail.ContactEmail = Convert.ToString(dr["Email"]);
                Detail.ContactPhone = Convert.ToString(dr["ContractPersonTel"]);
                Detail.ContactRemark = Convert.ToString(dr["REMARK1"]);

                Detail.TicketImpact = Convert.ToString(dr["Impact"]);
                Detail.TicketUrgency = Convert.ToString(dr["Urgency"]);
                Detail.TicketPriority = Convert.ToString(dr["Priority"]);
                Detail.TicketSubject = Convert.ToString(dr["HeaderText"]);

                Detail.TicketDescription = Convert.ToString(dr["Remark"]);

                Detail.CICode = Convert.ToString(dr["EquipmentNo"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dr["EquipmentNo"])))
                {
                    Detail.CIName = Convert.ToString(dr["EquipmentDesc"]);
                }
                else
                {
                    Detail.CIName = "";
                }
                Detail.CIFamily = Convert.ToString(dr["EquipmentTypeDesc"]);
                Detail.CIClass = Convert.ToString(dr["EquipmentClassDesc"]);
                Detail.CICategory = Convert.ToString(dr["CategoryCode"])+":"+ Convert.ToString(dr["CategoryName"]);
                Detail.CISerialNo = Convert.ToString(dr["SerialNo"]);

                Detail.OwnerService = Convert.ToString(dr["QueueOption"]);
                Detail.IncidentGroup = Convert.ToString(dr["ProblemGroup"]);
                Detail.IncidentType = Convert.ToString(dr["ProblemTypeCode"]);
                Detail.IncidentSource = Convert.ToString(dr["OriginCode"]);
                Detail.ContactSource = Convert.ToString(dr["CallTypeCode"]);
                Detail.AffectSLA = Convert.ToString(dr["AffectSLA"]);
                Detail.SummaryProblem = Convert.ToString(dr["SummaryProblem"]);
                Detail.SummaryCause = Convert.ToString(dr["SummaryCause"]);
                Detail.SummaryResolution = Convert.ToString(dr["SummaryResolution"]);

                Detail.TicketImpactDes = Convert.ToString(dr["ImpactName"]);
                Detail.TicketUrgencyDes = Convert.ToString(dr["UrgencyName"]);
                Detail.TicketPriorityDes = Convert.ToString(dr["Description"]);
                Detail.OwnerServiceDes = Convert.ToString(dr["OwnerGroupName"]);
                Detail.IncidentGroupDes = Convert.ToString(dr["GROUPNAME"]);
                Detail.IncidentTypeDes = Convert.ToString(dr["TypeName"]);
                Detail.IncidentSourceDes = Convert.ToString(dr["SourceName"]);
                Detail.ContactSourceDes = Convert.ToString(dr["AREANAME"]);
            }
            return Detail;
        }

        public class TicketDetailModel_V2 : TicketDetailModel
        {
            public string TicketImpactDes { get; set; }
            public string TicketUrgencyDes { get; set; }
            public string TicketPriorityDes { get; set; }
            public string OwnerServiceDes { get; set; }
            public string IncidentGroupDes { get; set; }
            public string IncidentTypeDes { get; set; }
            public string IncidentSourceDes { get; set; }
            public string ContactSourceDes { get; set; }
        }

        public class TicketDetailResponseModel_V2 : CommonResponseModel
        {
            public TicketDetailModel_V2 data { get; set; }
        }
        public class inputAPI
        {
            public string TicketNumber { get; set; }
        }
    }
}