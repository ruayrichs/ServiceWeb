<%@ Page Title="" Language="C#" MasterPageFile="~/MasterConfig/MasterPage/MasterPageConfig.master" AutoEventWireup="true" CodeBehind="UserManagementMaster.aspx.cs" Inherits="ServiceWeb.crm.Master.UserManagement.UserManagementMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderConfig" runat="server">
    <script src="<%= Page.ResolveUrl("~/UserControl/AutoComplete/General/autocomplete-ganeral.js?vs=20190113") %>"></script>
    <nav class="navbar nav-header-action sticky-top bg-white" style="margin-left: 1px; margin-right: 1px;">
        <div class="pull-left">
            <asp:UpdatePanel ID="udpnAction" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <a class="btn btn-warning mb-1" href="UserManagementCriteria.aspx"><i class="fa fa-arrow-circle-left"></i>&nbsp;&nbsp;Back</a>
                    <button type="button" class="btn btn-success mb-1 AUTH_MODIFY" onclick="validateSave();"><i class="fa fa-save"></i>&nbsp;&nbsp;Save</button>
                    <asp:Button ID="btnAddRole" runat="server" OnClientClick="AGLoading('true');" CssClass="btn btn-primary d-none" OnClick="btnAddRole_Click" Text="เพิ่ม" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </nav>
    <div class="d-none">
        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
            <contenttemplate>
                <asp:Button ID="btnValidateSave" runat="server" OnClick="btnValidateSave_Click" />
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" OnClientClick="return confirmSave(this);" />
            </contenttemplate>
        </asp:UpdatePanel>
    </div>
    <div class="row">
        <div class="col">
            <div class="card shadow">
                <div class="card-header">
                    <h5 class="mb-0">รหัสผู้ใช้งาน</h5>

                </div>
                <div class="card-body">
                    <nav>
                        <div class="nav nav-tabs" id="nav-tab" role="tablist">
                            <a class="nav-item nav-link active" id="nav-emp-data-tab" data-toggle="tab" href="#nav-emp-data" role="tab" aria-controls="nav-header" aria-selected="true">ข้อมูลพนักงาน</a>
                            <a class="nav-item nav-link" id="nav-emp-general-tab" data-toggle="tab" href="#nav-emp-general" role="tab" aria-controls="nav-item" aria-selected="false">ข้อมูลส่วนตัว</a>
                            <a class="nav-item nav-link" id="nav-emp-address-tab" data-toggle="tab" href="#nav-emp-address" role="tab" aria-controls="nav-item" aria-selected="false">ข้อมูลที่อยู่</a>
                            <%--add Employee OwnerService Tab--%>
                            <a class="nav-item nav-link" id="nav-emp-ownerservice-tab" data-toggle="tab" href="#nav-emp-ownerservice" role="tab" aria-controls="nav-item" aria-selected="false">OwnerService</a>
                        </div>
                    </nav>
                    <div class="tab-content p-3" id="nav-tabContent">

                        <div class="tab-pane fade show active" id="nav-emp-data" role="tabpanel" aria-labelledby="nav-emp-data-tab">
                            <asp:UpdatePanel ID="udpEmployeeData" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>

                                    <div class="form-row">
                                        <div class="form-group col-md-4">
                                            <label>ชื่อผู้ใช้งาน</label>
                                            <asp:TextBox ID="txtUsernameCreate" CssClass="form-control form-control-sm required" runat="server" data-title="โปรดกรอกชื่อผู้ใช้งาน" />
                                            <asp:TextBox ID="txtUsernameOld" CssClass="form-control form-control-sm" Style="display: none;" runat="server" />
                                            <asp:TextBox ID="txtEmployeeCode" CssClass="form-control form-control-sm" Style="display: none;" runat="server" />
                                        </div>
                                        <div class="form-group col-md-4">
                                            <label>รหัสผ่าน</label>
                                            <asp:TextBox ID="txtPassword" CssClass="form-control form-control-sm required" TextMode="Password" runat="server" data-title="โปรดกรอกรหัสผ่านอย่างน้อย 5 ตัวอักษร" autocomplete="new-password" />
                                        </div>
                                    </div>
                                    <div class="form-row">
                                        <div class="form-group col-md-4">
                                            <label>กลุ่มพนักงาน</label>
                                            <asp:DropDownList ID="ddlCustomerGroup" CssClass="form-control form-control-sm required" runat="server" Enabled="false">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="form-group col-md-4">
                                            <label>ชื่อ(ภาษาอังกฤษ)</label>
                                            <asp:TextBox ID="txtFirstname" CssClass="form-control form-control-sm required" runat="server" data-title="โปรดกรอกชื่อ ของผู้ใช้งาน" Enabled="false" />
                                        </div>
                                        <div class="form-group col-md-4">
                                            <label>นามสกุล(ภาษาอังกฤษ)</label>
                                            <asp:TextBox ID="txtLastName" CssClass="form-control form-control-sm" runat="server" data-title="โปรดกรอกชื่อ ของผู้ใช้งาน" Enabled="false" />
                                        </div>
                                    </div>
                                    <div class="form-row">
                                        <div class="form-group col-md-4">
                                            <label>คำนำหน้าชื่อ</label>
                                            <asp:DropDownList ID="ddlTitleName" CssClass="form-control form-control-sm" runat="server" Enabled="false">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="form-group col-md-4">
                                            <label>ชื่อ(ภาษาไทย) </label>
                                            <asp:TextBox ID="txtFirstname_TH" CssClass="form-control form-control-sm" runat="server" data-title="โปรดกรอกชื่อ ของผู้ใช้งาน" Enabled="false" />
                                        </div>
                                        <div class="form-group col-md-4">
                                            <label>นามสกุล(ภาษาไทย) </label>
                                            <asp:TextBox ID="txtLastName_TH" CssClass="form-control form-control-sm" runat="server" data-title="โปรดกรอกชื่อ ของผู้ใช้งาน" Enabled="false" />
                                        </div>
                                    </div>



                                    <div class="form-row">
                                        <div class="form-group col-md-4">
                                            <label>อีเมล์</label>
                                            <asp:TextBox ID="txtEmail" CssClass="form-control form-control-sm required" runat="server" Enabled="false" />
                                        </div>
                                        <div class="form-group col-md-4">
                                            <label>โทรศัพท์มือถือ</label>
                                            <asp:TextBox ID="txtMobilePhone" onkeypress="return isNumberKey(event);" MaxLength="10" CssClass="form-control form-control-sm" runat="server" Enabled="false" />
                                        </div>
                                        <div class=" form-group col-md-4">
                                            <label>โทรศัพท์บ้าน</label>
                                            <asp:TextBox ID="txtTelphone" onkeypress="return isNumberKey(event);" MaxLength="13" CssClass="form-control form-control-sm" runat="server" Enabled="false" />
                                        </div>
                                    </div>
                                    <div class="form-row">
                                        <div class="form-group col-md-4">
                                            <label>ตำแหน่ง</label>
                                            <asp:DropDownList runat="server" ID="ddlPosition" CssClass="form-control form-control-sm" Enabled="false"></asp:DropDownList>
                                        </div>
                                        <div class="form-group col-md-4">
                                            <label>แผนก</label>
                                            <asp:TextBox ID="txtDepartment" CssClass="form-control form-control-sm " runat="server" Enabled="false" />
                                        </div>
                                        <div class="form-group col-md-4" style="display:none"> <%--ซ่อนไว้เพื่อไปใช้ tab owner service listOwnerService--%>
                                            <label>Owner Service</label>
                                            <asp:DropDownList ID="ddl_Owner_Ser" CssClass="form-control form-control-sm" runat="server">
                                            </asp:DropDownList>
                                        </div>

                                    </div>
                                    <div class="form-row">
                                        <div class="form-group col-md-3">
                                            <label>บทบาท</label>
                                            <asp:DropDownList ID="ddl_role_code" runat="server" class="form-control form-control-sm">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="form-group col-md-1" style="padding-top: 35px;">
                                            <asp:CheckBox ID="chkStatus" Text="&nbsp;ใช้งาน" runat="server" Enabled="false" />
                                        </div>
                                        <div class="form-group col-md-4">
                                            <label>ระยะเวลาเริ่มต้น</label>
                                            <div class="input-group">
                                                <asp:TextBox ID="txtissueDate" runat="server" CssClass="form-control form-control-sm date-picker ticket-allow-editor" Enabled="false"></asp:TextBox>
                                                <div class="input-group-append">
                                                    <span class="input-group-text"><i class="fa fa-calendar"></i></span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group  col-md-4">
                                            <label>ระยะเวลาสิ้นสุด</label>
                                            <div class="input-group">
                                                <asp:TextBox ID="txtexpDate" runat="server" CssClass="form-control form-control-sm date-picker ticket-allow-editor" Enabled="false"></asp:TextBox>
                                                <div class="input-group-append">
                                                    <span class="input-group-text"><i class="fa fa-calendar"></i></span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <div style="display: none">
                                <asp:UpdatePanel ID="udpnBtnRole" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <br />
                                        <div class="form-row">
                                            <div class="form-group col-md-2">
                                                <label>เลือกบทบาท</label>
                                            </div>
                                            <div class="col-md-3">
                                                <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-control form-control-sm"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-3">
                                                <span class="btn btn-primary" onclick="$('#<%= btnAddRole.ClientID  %>').click();">เพิ่ม</span>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="udpnRoleList" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="form-row">
                                            <div class="col-md-12">
                                                <table class="table table-bordered">
                                                    <tr>
                                                        <th class="text-nowrap" style="width: 15px; max-height: 15px;">ลบ
                                                        </th>
                                                        <th class="text-nowrap">รหัสบทบาท
                                                        </th>
                                                        <th class="text-nowrap">ชื่อบทบาท
                                                        </th>
                                                    </tr>
                                                    <asp:Repeater ID="rptRoleList" runat="server">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td class="text-center" style="max-height: 15px;">
                                                                    <span class="fa fa-times-circle-o fa-lg text-danger mr-1 AUTH_MODIFY" onclick="$(this).next().click()" style="cursor: pointer;"></span>
                                                                    <asp:Button ID="btnDeleteRole" OnClick="btnDeleteRole_Click" OnClientClick="AGLoading(true);" CommandArgument='<%# Eval("profileid") %>' CssClass="d-none" Text="" runat="server" />
                                                                    <asp:Label ID="lblRoleDataJson" Text='<%# Newtonsoft.Json.JsonConvert.SerializeObject(Container.DataItem) %>' CssClass="d-none" runat="server" />
                                                                </td>
                                                                <td class="text-nowrap">
                                                                    <%# Eval("RoleID") %>
                                                                </td>
                                                                <td class="text-nowrap">
                                                                    <%# Eval("RoleName") %>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </table>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div class="tab-pane" id="nav-emp-general" role="tabpanel" aria-labelledby="nav-emp-general-tab">
                            <asp:UpdatePanel ID="udpEmployeeGeneral" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>

                                    <div class="form-row">
                                        <div class="form-group col-md-4">
                                            <label>เลขที่บัตรประชาชน</label>
                                            <asp:TextBox Enabled="false" ID="txtNatinalityCode" MaxLength="13" onkeydown="return (!(event.keyCode>=65));" CssClass="form-control form-control-sm" runat="server" ClientIDMode="Static" meta:resourcekey="txtNatinalityCodeResource1" data-title="โปรดกรอกเลขที่บัตรประชาชน 13 หลัก"></asp:TextBox>
                                        </div>
                                        <div class="form-group col-md-4">
                                            <label>เชื้อชาติ</label>
                                            <asp:DropDownList Enabled="false" runat="server" ID="ddlNational" CssClass="form-control form-control-sm"></asp:DropDownList>
                                        </div>
                                        <div class="form-group col-md-4">
                                            <label>เชื้อชาติที่ 2</label>
                                            <asp:DropDownList Enabled="false" runat="server" ID="ddlNational2" CssClass="form-control form-control-sm" meta:resourcekey="ddlNational2Resource1"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-row">
                                        <div class="form-group col-md-2">
                                            <label>เพศ</label>
                                            <asp:DropDownList Enabled="false" ID="ddlGender" CssClass="form-control form-control-sm" runat="server">
                                                <asp:ListItem Value="" Text="ไม่ระบุ" />
                                                <asp:ListItem Value="Male" Text="ชาย" />
                                                <asp:ListItem Value="Female" Text="หญิง" />
                                            </asp:DropDownList>
                                        </div>
                                        <div class="form-group col-md-2">
                                            <label>วันเกิด</label>
                                            <div class="input-group">
                                                <asp:TextBox Enabled="false" ID="txtBirthDay" runat="server" CssClass="form-control form-control-sm date-picker ticket-allow-editor"></asp:TextBox>
                                                <div class="input-group-append">
                                                    <span class="input-group-text"><i class="fa fa-calendar"></i></span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group col-md-4">
                                            <label>สถานที่เกิด</label>
                                            <asp:TextBox Enabled="false" ID="txtBirthPlace" CssClass="form-control form-control-sm" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="form-group col-md-4">
                                            <label>เมืองที่เกิด</label>
                                            <asp:TextBox ID="txtBirthProvinvce" CssClass="form-control form-control-sm" runat="server" Enabled="false"></asp:TextBox>
                                        <%--<asp:TextBox ID="txtBirthProvinvce" class="form-control form-control-sm" runat="server" Enabled="false"></asp:TextBox>--%>
                                        </div>
                                    </div>
                                    <div class="form-row">
                                        <div class="form-group col-md-4">
                                            <label>ประเทศที่เกิด</label>
                                            <asp:DropDownList Enabled="false" runat="server" ID="ddlCountryBirthPlace" CssClass="form-control form-control-sm"></asp:DropDownList>
                                        </div>
                                        <div class="form-group col-md-4">
                                            <label>ภาษาที่ใช้</label>
                                            <asp:DropDownList Enabled="false" runat="server" ID="ddlLanguane" CssClass="form-control form-control-sm"></asp:DropDownList>
                                        </div>
                                        <div class="form-group col-md-4">
                                            <label>ศาสนา</label>
                                            <asp:DropDownList Enabled="false" runat="server" ID="ddlRegion" CssClass="form-control form-control-sm"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-row">
                                        <div class="form-group col-md-4">
                                            <label>สถานภาพสมรส</label>
                                            <asp:DropDownList Enabled="false" runat="server" ID="ddlMarriageStatus" CssClass="form-control form-control-sm"></asp:DropDownList>
                                        </div>
                                        <div class="form-group col-md-4">
                                            <label>จำนวนบุตร</label>
                                            <asp:TextBox Enabled="false" runat="server" ID="txtCountOfChild" onkeypress="return isNumberKey(event);" CssClass="form-control form-control-sm"></asp:TextBox>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="tab-pane" id="nav-emp-address" role="tabpanel" aria-labelledby="nav-emp-address-tab">
                            <asp:UpdatePanel ID="udpEmployeeAddress" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>

                                    <div class="form-row">
                                        <div class="form-group col-md-4">
                                            <label>บ้านเลขที่</label>
                                            <asp:TextBox Enabled="false" ID="txtHouseNo" CssClass="form-control form-control-sm" runat="server" />
                                        </div>
                                        <div class="form-group col-md-4">
                                            <label>ถนน</label>
                                            <asp:TextBox Enabled="false" ID="txtRoad" CssClass="form-control form-control-sm" runat="server" />
                                        </div>
                                        <div class="form-group col-md-4">
                                            <label>แขวง/ตำบล</label>
                                            <asp:TextBox Enabled="false" ID="txtSubDistinct" CssClass="form-control form-control-sm" runat="server" />
                                        </div>
                                    </div>
                                    <div class="form-row">
                                        <div class="form-group col-md-4">
                                            <label>เขต/อำเภอ</label>
                                            <asp:TextBox Enabled="false" ID="txtDistinct" CssClass="form-control form-control-sm" runat="server" />
                                        </div>
                                        <div class="form-group col-lg-4">
                                            <label>จังหวัด</label>
                                            <asp:TextBox Enabled="false" ID="txtProvice" type="text" CssClass="form-control form-control-sm" runat="server" />
                                        </div>
                                        <div class="form-group col-lg-4">
                                            <label>ประเทศ</label>
                                            <asp:DropDownList Enabled="false" runat="server" ID="ddlCountry" CssClass="form-control form-control-sm"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-row">
                                        <div class="form-group col-lg-4">
                                            <label>รหัสไปรษณีย์</label>
                                            <asp:TextBox Enabled="false" ID="txtPostCode" MaxLength="5" onkeydown="return (!(event.keyCode>=65));" CssClass="form-control form-control-sm" runat="server"  />
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="tab-pane" id="nav-emp-ownerservice" role="tabpanel" aria-labelledby="nav-emp-ownerservice-tab">
                            <asp:UpdatePanel ID="udpShowSelected" runat="server" UpdateMode="Conditional" >
                                    <ContentTemplate>
                                        <div class="chip-box">
                                           <label> Current OwnerService <br></label>
                                           <div class="border-chip-box">
                                               <asp:Button ID="btninitChip" runat="server" CssClass="none" onclick="btninitChip_Click"/>
                                               <asp:Repeater ID="rptOwnerService" runat="server" >
                                                   <ItemTemplate>
                                                      <div id="chip" class="chip">
                                                            <div class="chip-content">   
                                                                <%# Container.DataItem.ToString() %> 
                                                            </div>
                                                      </div>        
                                                   </ItemTemplate>
                                               </asp:Repeater>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                            </asp:UpdatePanel>   
                            <asp:UpdatePanel ID="udpEmployeeOwnerService" runat="server" UpdateMode="Conditional" >
                                 <ContentTemplate>
                                    <%-- qiwzee --%>
                                         <asp:ListBox ID="lstOwnerService" runat="server" SelectionMode="Multiple" Width="100%" />
                                 </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script>
        function validateSave() {
            $(".required").prop('required', true);
            $("#<%= btnValidateSave.ClientID %>").click();
        }
        function saveEmailMessage() {
            $("#<%= btnSave.ClientID %>").click();
        }
        function confirmSave(obj) {
            if (AGConfirm(obj, "ยืนยันการบันทึก")) {
                AGLoading(true);
                return true;
            }
            return false;
        }
        //====================== Script List OwnerService =====================================
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(PageLoaded)
        });
        function PageLoaded(sender, args) {

            $('[id*=lstOwnerService]').multiselect({
                includeSelectAllOption: true,
                buttonWidth: '30%',
                enableFiltering: true,
                buttonClass: 'btn btn-outline-primary',
                maxHeight: 200,
                onDropdownHidden: function (option, event, select) {
                    $("#<%= btninitChip.ClientID %>").click();
                }
            });
        }
    </script>
    <style>
                .chip {
                    display: inline-flex;
                    flex-direction: row;
                    background-color: #28a745;
                    border: none;
                    cursor: default;
                    height: 36px;
                    outline: none;
                    padding: 0;
                    font-size: 14px;
                    color: white;
                    font-family: "Open Sans", sans-serif;
                    white-space: nowrap;
                    align-items: center;
                    border-radius: 16px;
                    vertical-align: middle;
                    text-decoration: none;
                    justify-content: center;
                }

                .chip-content {
                    cursor: inherit;
                    display: flex;
                    align-items: center;
                    user-select: none;
                    white-space: nowrap;
                    padding-left: 12px;
                    padding-right: 12px;
                }
      
                .chip-box {
                    margin-top: 10px;
                    margin-bottom:20px;
                }
                .none{
                    display: none;
                }

                .border-chip-box{
                    border-color: #007bff;
                    border-style: solid;
                    border-radius: 5px;
                    padding: 12px;
                    height: auto;
                    border-width: 1px;
                            
                            
                }
                .dropdown-menu {
                    min-width: 100%;
                }
                
      </style>
</asp:Content>
