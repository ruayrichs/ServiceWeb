using Agape.FocusOne.Utilities;
using ERPW.Lib.Service;
using ERPW.Lib.Authentication;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERPW.Lib.Master.Entity;
using ERPW.Lib.Master;

namespace ServiceWeb.widget.usercontrol
{
    public partial class TimeLineControl : System.Web.UI.UserControl
    {
        ActivityRemarkTimeline serTimeline = new ActivityRemarkTimeline();
        LogServiceLibrary libLog = new LogServiceLibrary();

        public Boolean IsHasFile
        {
            get { return Convert.ToBoolean(hddIsHasFile.Value); }
            set { hddIsHasFile.Value = value.ToString(); udpOption.Update(); } 
        }
        public Boolean IsHyperLink
        { 
            get { return Convert.ToBoolean(hddIsHyperLink.Value); }
            set { hddIsHyperLink.Value = value.ToString(); udpOption.Update(); } 
        }
        public Boolean IsDateTime
        {
            get { return Convert.ToBoolean(hddIsDateTime.Value); }
            set { hddIsDateTime.Value = value.ToString(); udpOption.Update(); }
        }
        public string KeyAobjectlink
        {
            get { return hddKeyAobjectlink.Value; }
            set { hddKeyAobjectlink.Value = value.ToString(); udpOption.Update(); }
        }
        public string ProgramID
        {
            get { return hddProgramID.Value; }
            set { hddProgramID.Value = value.ToString(); udpOption.Update(); }
        }
        
        List<ActivityRemarkTimeline.TimelineRemark> _listTimelineRemark;
        List<ActivityRemarkTimeline.TimelineRemark> listTimelineRemark 
        {
            get
            {
                if ( _listTimelineRemark == null)
                {
                    _listTimelineRemark = serTimeline.getTimelineAttachFile(
                        ERPWAuthentication.SID,
                        ERPWAuthentication.CompanyCode,
                        KeyAobjectlink
                    );
                }
                return _listTimelineRemark;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void bindDataTimeLine(DataTable dtDataSource, string ColumnDate, string ColumnTitle, string ColumnDescription)
        {
        }
        public void bindDataTimeLineHasFile()
        {
            if (listTimelineRemark.Count > 0)
            {
                panelTimeLine.Visible = true;
                panelEmpty.Visible = false;
            }
            else
            {
                panelTimeLine.Visible = false;
                panelEmpty.Visible = true;
            }

            rptTimeLine.DataSource = listTimelineRemark;
            rptTimeLine.DataBind();
            udpTimeLine.Update();

            ClientService.DoJavascript("getremarkservice('" + KeyAobjectlink + "');");
            ClientService.DoJavascript("afterBinderTimeline();");
        }

        protected void rptTimeLine_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rptListFile = e.Item.FindControl("rptListFile") as Repeater;
            HiddenField hddDataKey = e.Item.FindControl("hddDataKey") as HiddenField;

            //dtListFileAttachment.DefaultView.RowFilter = "DateKey = '" + hddDataKey.Value + "'";
            //DataTable dt = dtListFileAttachment.DefaultView.ToTable();

            List<ActivityRemarkTimeline.TimelineFileUpload> listFile = listTimelineRemark.Where(w =>
                w.Date_DB.Equals(hddDataKey.Value)
            ).First().ListFileUpload;

            rptListFile.DataSource = listFile;
            rptListFile.DataBind();
        }

        protected void btnReloadTimeLine_Click(object sender, EventArgs e)
        {
            try
            {
                bindDataTimeLineHasFile();
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

        protected void btnSaveLog_Click(object sender, EventArgs e)
        {
            try
            {
                string curDataTime = Validation.getCurrentServerStringDateTime();
                string curData = curDataTime.Substring(0, 8);
                string curTime = curDataTime.Substring(8);
                string SID = ERPWAuthentication.SID;
                string CompanyCode = ERPWAuthentication.CompanyCode;
                string UserName = ERPWAuthentication.UserName;

                if (!string.IsNullOrEmpty(ProgramID))
                {
                    List<Main_LogService> en = new List<Main_LogService>();
                    if (!string.IsNullOrEmpty(hdd_LogRemark.Value.Trim()))
                    {
                        List<Detail_LogService> listDetail = new List<Detail_LogService>();
                        listDetail.Add(new Detail_LogService
                        {
                            ItemNumber = "",
                            FieldName = "Message",
                            OldValue = "",
                            NewValue = hdd_LogRemark.Value
                        });

                        en.Add(new Main_LogService
                        {
                            LOGOBJCODE = ProgramID,
                            PROGOBJECT = ProgramID + "|link_timeline",
                            ACCESSCODE = LogServiceLibrary.AccessCode_Attachment,
                            OBJPKREC = SID + CompanyCode + KeyAobjectlink,
                            APPLTYPE = "M",
                            Access_By = UserName,
                            Access_Date = curData,
                            Access_Time = curTime,
                            listDetail = listDetail
                        });
                    }
                    if (!string.IsNullOrEmpty(hdd_LogListFile.Value.Trim()))
                    {
                        List<Detail_LogService> listDetail = new List<Detail_LogService>();

                        List<string> listFiles = hdd_LogListFile.Value.Trim().Split(',').ToList();
                        listFiles.ForEach(r_file =>
                        {
                            listDetail.Add(new Detail_LogService
                            {
                                ItemNumber = "",
                                FieldName = "ContentUrl",
                                OldValue = "",
                                NewValue = r_file
                            });
                        });

                        en.Add(new Main_LogService
                        {
                            LOGOBJCODE = ProgramID,
                            PROGOBJECT = ProgramID + "|Link_Timeline_Assets",
                            ACCESSCODE = LogServiceLibrary.AccessCode_Attachment,
                            OBJPKREC = SID + CompanyCode + KeyAobjectlink,
                            APPLTYPE = "M",
                            Access_By = UserName,
                            Access_Date = curData,
                            Access_Time = curTime,
                            listDetail = listDetail
                        });

                    }

                    if (en.Count > 0)
                    {
                        libLog.SaveLog(SID, ProgramID, "M", en);
                    }
                }

                bindDataTimeLineHasFile();
                ClientService.DoJavascript("ReLoadEquipmentLog();");
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
    }
}