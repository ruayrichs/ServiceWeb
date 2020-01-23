<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true" CodeBehind="EmployeeMaster.aspx.cs" Inherits="ServiceWeb.crm.Master.EmployeeMaster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .nav-tabs--vertical {
            border-bottom: none;
            border-right: 1px solid #ddd;
            display: flex;
            flex-flow: column nowrap;
            }
        .nav-tabs--left {
            margin: 0 15px;
            }
        .nav-tabs--left .nav-item + .nav-item {
            margin-top: .25rem;
            }
        .nav-tabs--left .nav-link {
            transition: border-color .125s ease-in;
            white-space: nowrap;
            }
        .nav-tabs--left .nav-link:hover {
            background-color: #f7f7f7;
            border-color: transparent;
            }
        .nav-tabs--left .nav-link.active {
            border-bottom-color: #ddd;
            border-right-color: #fff;
            border-bottom-left-radius: 0.25rem;
            border-top-right-radius: 0;
            margin-right: -1px;
            }
        .nav-tabs--left .nav-link.active:hover {
            background-color: #fff;
            border-color: #0275d8 #fff #0275d8 #0275d8;
            }
        </style>
    <div>
        <div class="form-row" style="margin-bottom:8px;margin-top:-8px;">
            <div class="col float-right" style="text-align:left;">
                <a href="#"><button class="btn btn-warning btn-sm mb-1">Back</button></a>
                <asp:Button runat="server" CssClass="btn btn-primary btn-sm mb-1" Text="ยืนยัน" ID="btnSaveCreate" />
            </div>
        </div>
        <div class="card">
            <div class="card card-header">
                <div class="form-row">
                    <div class="col-sm-6" style="text-align:left;">
                        <h4>MANAGE EMPLOYEE</h4>
                    </div>
                    <div class="col-sm-6" style="text-align:right;">
                        <a href="#">BACK TO EMPLOYEE MASTER</a>
                    </div>
                </div>
            </div>
            <div class="card card-body">
                <div>
                    <div class="row">
                        <div class="col-3">
                            <div class="nav nav-tabs nav-tabs--vertical nav-tabs--left" id="v-pills-tab" role="tablist" aria-orientation="vertical">
                                <a class="nav-link active show" id="v-pills-personal-tab" data-toggle="pill" href="#v-pills-personal" role="tab" aria-controls="v-pills-personal" aria-selected="true">Personal</a>
                                <a class="nav-link" id="v-pills-profile-tab" data-toggle="pill" href="#v-pills-profile" role="tab" aria-controls="v-pills-profile" aria-selected="false">Profile</a>
                                <a class="nav-link" id="v-pills-messages-tab" data-toggle="pill" href="#v-pills-address" role="tab" aria-controls="v-pills-address" aria-selected="false">Address</a>
                                <a class="nav-link" id="v-pills-position-tab" data-toggle="pill" href="#v-pills-position" role="tab" aria-controls="v-pills-position" aria-selected="false">Position</a>
                                <a class="nav-link" id="v-pills-role-tab" data-toggle="pill" href="#v-pills-role" role="tab" aria-controls="v-pills-role" aria-selected="false">Role</a>
                            </div>
                        </div>
                        <div class="col-9">
                            <div class="tab-content" id="v-pills-tabContent">
                                <div class="tab-pane fade active show" id="v-pills-personal" role="tabpanel" aria-labelledby="v-pills-personal-tab">
                                    <div>
                                        <div class="form-row">
                                            <div class="col-lg-12">
                                                <iframe></iframe>
                                            </div>
                                        </div>
                                        <div class="form-row">
                                            <div class="col-xl-12">
                                                <div class="form-row">
                                                    <div class="col-sm-2">
                                                        <asp:Label runat="server" Text="รหัสบริษัท" id="labelCompanyCode"/>
                                                    </div>
                                                    <div class="col-sm-4">
                                                        <asp:Label runat="server" Text="" ID="lblCompanyCode" />
                                                    </div>
                                                    <div class="col-sm-2">
                                                        <asp:Label runat="server" Text="ชื่อบริษัท" ID="labelCompanyName" />
                                                    </div>
                                                    <div class="col-sm-4">
                                                        <asp:Label runat="server" Text="" ID="lblCompanyName" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-row">
                                            <div class="col-xl-12">
                                                <div class="form-row">
                                                    <div class="col-sm-2">
                                                        <asp:Label runat="server" Text="กลุ่มพนักงาน" ID="labelEmployeeGroup" />
                                                    </div>
                                                    <div class="col-sm-4">
                                                        <asp:DropDownList runat="server" ID="ddlEmployeeGroup" AutoPostBack="true"
                                                            OnChange="agroLoading(true);"
                                                            CssClass="form-control" meta:resourcekey="ddlEmployeeGroupResource1">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-sm-2"><asp:Label runat="server" Text="รหัสพนักงาน" ID="labelEmployeeCode" /></div>
                                                    <div class="col-sm-4">
                                                        <asp:TextBox ID="txtEmployeeCode" CssClass="form-control" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-row">
                                            <div class="col-xl-12">
                                                <div class="form-row">
                                                    <div class="col-sm-2">
                                                        <asp:Label runat="server" ID="labelTitleName" Text="คำนำหน้าชื่อ"></asp:Label>
                                                    </div>
                                                    <div class="col-sm-4">
                                                        <asp:DropDownList runat="server" ID="ddlTitleName" CssClass="form-control">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-sm-2">
                                                        <asp:Label runat="server" ID="labelActive" Text="มีการใช้งาน"></asp:Label>
                                                    </div>
                                                    <div class="col-sm-4">
                                                        <asp:CheckBox runat="server" ID="chkActive" Checked="false" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-row">
                                            <div class="col-xl-12">
                                                <div class="form-row">
                                                    <div class="col-sm-2">
                                                        <asp:Label runat="server" ID="labelNameTH" Text="ชื่อ(ไทย)"></asp:Label>
                                                    </div>
                                                    <div class="col-sm-4">
                                                        <asp:TextBox id="txtNameTH" type="text" class="form-control" runat="server" clientidmode="Static" />
                                                    </div>
                                                    <div class="col-sm-2">
                                                        <asp:Label runat="server" ID="labelSurnameTH" Text="นามสกุล(ไทย)"></asp:Label>
                                                    </div>
                                                    <div class="col-sm-4">
                                                        <asp:TextBox id="txtSurnameTH" type="text" class="form-control" runat="server" clientidmode="Static" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-row">
                                            <div class="col-xl-12">
                                                <div class="form-row">
                                                    <div class="col-sm-2">
                                                        <asp:Label runat="server" ID="labelNameEN" Text="ชื่อ(อังกฤษ)"></asp:Label>
                                                    </div>
                                                    <div class="col-sm-4">
                                                        <asp:TextBox id="txtNameEN" type="text" class="form-control" runat="server" clientidmode="Static" />
                                                    </div>
                                                    <div class="col-sm-2">
                                                        <asp:Label runat="server" ID="labelSurnameEN" Text="นามสกุล(อังกฤษ)"></asp:Label>
                                                    </div>
                                                    <div class="col-sm-4">
                                                        <asp:TextBox id="txtSurnameEN" type="text" class="form-control" runat="server" clientidmode="Static" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-row">
                                            <div class="col-xl-12">
                                                <div class="form-row">
                                                    <div class="col-sm-2">
                                                        <asp:Label runat="server" ID="labelEmail" Text="Email"></asp:Label>
                                                    </div>
                                                    <div class="col-sm-4">
                                                        <asp:TextBox ID="txtEmail" CssClass="form-control" runat="server" ClientIDMode="Static" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="tab-pane fade" id="v-pills-profile" role="tabpanel" aria-labelledby="v-pills-profile-tab">
                                    <div class="form-row">
                                        <div class="col-xl-12">
                                            <div class="form-row">
                                                <div class="col-sm-2">
                                                    <asp:Label runat="server" ID="labelNationalityCode" Text="เลขที่บัตรประชาชน"></asp:Label>
                                                </div>
                                                <div class="col-sm-4">
                                                    <asp:TextBox ID="txtNatinalityCode" MaxLength="13" onkeydown="return (!(event.keyCode>=65));"
                                                    CssClass="form-control" runat="server" ClientIDMode="Static"></asp:TextBox>
                                                </div>
                                                <div class="col-sm-2">
                                                    <asp:Label runat="server" ID="labelNational" Text="เชื้อชาติ"></asp:Label>
                                                </div>
                                                <div class="col-sm-4">
                                                    <asp:DropDownList runat="server" ID="ddlNational" CssClass="form-control">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-row">
                                        <div class="col-xl-12">
                                            <div class="form-row">
                                                <div class="col-sm-2">
                                                    <asp:Label runat="server" ID="labelGender" Text="เพศ"></asp:Label>
                                                </div>
                                                <div class="col-sm-4">
                                                    <select class="form-control" id="ddlGender" runat="server">
                                                        <option value="">ไม่ระบุ
                                                        </option>
                                                        <option value="M">ชาย
                                                        </option>
                                                        <option value="F">หญิง
                                                        </option>
                                                    </select>
                                                </div>
                                                <div class="col-sm-2">
                                                    <asp:Label runat="server" ID="labelNational2" Text="เชื้อชาติที่ 2"></asp:Label>
                                                </div>
                                                <div class="col-sm-4">
                                                    <asp:DropDownList runat="server" ID="ddlNational2" CssClass="form-control">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-row">
                                        <div class="col-xl-12">
                                            <div class="form-row">
                                                <div class="col-sm-2">
                                                    <asp:Label runat="server" ID="labelBirthDay" Text="วันเกิด"></asp:Label>
                                                </div>
                                                <div class="col-sm-4">
                                                    <asp:TextBox ID="txtBirthDay" Style="background-color: White;" CssClass="form-control date-picker"
                                                        runat="server" ClientIDMode="Static"/>
                                                </div>
                                                <div class="col-sm-2">
                                                    <asp:Label runat="server" ID="labelLanguage" Text="ภาษาที่ใช้"></asp:Label>
                                                </div>
                                                <div class="col-sm-4">
                                                    <asp:DropDownList runat="server" ID="ddlLanguane" CssClass="form-control">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-row">
                                        <div class="col-xl-12">
                                            <div class="form-row">
                                                <div class="col-sm-2">
                                                    <asp:Label runat="server" ID="labelBirthPlace" Text="สถานที่เกิด"></asp:Label>
                                                </div>
                                                <div class="col-sm-4">
                                                    <asp:TextBox id="txtBirthPlace" type="text" class="form-control" runat="server" clientidmode="Static" />
                                                </div>
                                                <div class="col-sm-2">
                                                    <asp:Label runat="server" ID="labelMarriageStatus" Text="สถานภาพสมรส"></asp:Label>
                                                </div>
                                                <div class="col-sm-4">
                                                    <asp:DropDownList runat="server" ID="ddlMarriageStatus" CssClass="form-control">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-row">
                                        <div class="col-xl-12">
                                            <div class="form-row">
                                                <div class="col-sm-2">
                                                    <asp:Label runat="server" ID="labelBirthProvince" Text="เมืองที่เกิด"></asp:Label>
                                                </div>
                                                <div class="col-sm-4">
                                                    <asp:TextBox id="txtBirthProvinvce" type="text" class="form-control" runat="server" clientidmode="Static" />
                                                </div>
                                                <div class="col-sm-2">
                                                    <asp:Label runat="server" ID="labelRegion" Text="ศาสนา"></asp:Label>
                                                </div>
                                                <div class="col-sm-4">
                                                    <asp:DropDownList runat="server" ID="ddlRegion" CssClass="form-control">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-row">
                                        <div class="col-xl-12">
                                            <div class="form-row">
                                                <div class="col-sm-2">
                                                    <asp:Label runat="server" ID="labelCountryBirthPlace" Text="ประเทศที่เกิด"></asp:Label>
                                                </div>
                                                <div class="col-sm-4">
                                                    <asp:DropDownList runat="server" ID="ddlCountryBirthPlace" CssClass="form-control">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-sm-2">
                                                    <asp:Label runat="server" ID="labelCountOfChild" Text="จำนวนบุตร"></asp:Label>
                                                </div>
                                                <div class="col-sm-4">
                                                    <asp:TextBox runat="server" ID="txtCountOfChild" CssClass="form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="tab-pane fade" id="v-pills-address" role="tabpanel" aria-labelledby="v-pills-address-tab">
                                    <div class="form-row">
                                    <div class="col-xl-12">
                                        <div class="form-row">
                                            <div class="col-sm-2">
                                                <asp:Label runat="server" ID="labelHouseNo" Text="บ้านเลขที่"></asp:Label>
                                            </div>
                                            <div class="col-sm-4">
                                                <asp:TextBox id="txtHouseNo" type="text" class="form-control" runat="server" clientidmode="Static" />
                                            </div>
                                            <div class="col-sm-2">
                                                <asp:Label runat="server" ID="labelRoad" Text="ถนน"></asp:Label>
                                            </div>
                                            <div class="col-sm-4">
                                                <asp:TextBox id="txtRoad" type="text" class="form-control" runat="server" clientidmode="Static" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                    <div class="form-row">
                                        <div class="col-xl-12">
                                            <div class="form-row">
                                                <div class="col-sm-2">
                                                    <asp:Label runat="server" ID="labelDistinct" Text="แขวง/ตำบล"></asp:Label>
                                                </div>
                                                <div class="col-sm-4">
                                                    <asp:TextBox id="txtDistinct" type="text" class="form-control" runat="server" clientidmode="Static" />
                                                </div>
                                                <div class="col-sm-2">
                                                    <asp:Label runat="server" ID="labelAmphur" Text="เขต/อำเภอ"></asp:Label>
                                                </div>
                                                <div class="col-sm-4">
                                                    <asp:TextBox id="txtAmpher" type="text" class="form-control" runat="server" clientidmode="Static" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-row">
                                        <div class="col-xl-12">
                                            <div class="form-row">
                                                <div class="col-sm-2">
                                                    <asp:Label runat="server" ID="labelProvince" Text="จังหวัด"></asp:Label>
                                                </div>
                                                <div class="col-sm-4">
                                                    <asp:TextBox id="txtProvice" type="text" class="form-control" runat="server" clientidmode="Static" />
                                                </div>
                                                <div class="col-sm-2">
                                                    <asp:Label runat="server" ID="labelCountry" Text="ประเทศ"></asp:Label>
                                                </div>
                                                <div class="col-sm-4">
                                                    <asp:DropDownList runat="server" ID="ddlCountry" CssClass="form-control" meta:resourcekey="ddlCountryResource1">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-row">
                                        <div class="col-xl-12">
                                            <div class="form-row">
                                                <div class="col-sm-2">
                                                    <asp:Label runat="server" ID="labelPostCode" Text="รหัสไปรษณีย์"></asp:Label>
                                                </div>
                                                <div class="col-sm-4">
                                                    <asp:TextBox id="txtPostCode" type="text" onkeydown="return (!(event.keyCode>=65));" class="form-control"
                                                    runat="server" clientidmode="Static" />
                                                </div>
                                                <div class="col-sm-2">
                                                    <asp:Label runat="server" ID="labelMobilePhone" Text="โทรศัพท์มือถือ"></asp:Label>
                                                </div>
                                                <div class="col-sm-4">
                                                    <asp:TextBox id="txtMobilePhone" type="text" onkeydown="return (!(event.keyCode>=65));"
                                                    class="form-control" runat="server" clientidmode="Static" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-row">
                                        <div class="col-xl-12">
                                            <div class="form-row">
                                                <div class="col-sm-2">
                                                    <asp:Label runat="server" ID="labelTelephone" Text="โทรศัพท์บ้าน"></asp:Label>
                                                </div>
                                                <div class="col-sm-4">
                                                    <asp:TextBox id="txtTelphone" type="text" onkeydown="return (!(event.keyCode>=65));" class="form-control"
                                                    runat="server" clientidmode="Static" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="tab-pane fade" id="v-pills-position" role="tabpanel" aria-labelledby="v-pills-position-tab">
                                    <div class="form-row">
                                        <div class="col-xl-12">
                                            <div class="form-row">
                                                <div class="col-sm-2">
                                                    <asp:Label runat="server" ID="labelBranch" Text="สาขา"></asp:Label>
                                                </div>
                                                <div class="col-sm-4">
                                                    <asp:DropDownList runat="server" ID="ddlBranch" CssClass="form-control">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-sm-2">
                                                    <asp:Label runat="server" ID="labelCostCenter" Text="Cost Center"></asp:Label>
                                                </div>
                                                <div class="col-sm-4">
                                                    <asp:DropDownList runat="server" ID="ddlCostCenter" CssClass="form-control">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-row">
                                        <div class="col-xl-12">
                                            <div class="form-row">
                                                <div class="col-sm-2">
                                                    <asp:Label runat="server" ID="labelPosition2" Text="ตำแหน่ง"></asp:Label>
                                                </div>
                                                <div class="col-sm-4">
                                                    <asp:DropDownList runat="server" ID="ddlPosition" CssClass="form-control">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-sm-2">
                                                    <asp:Label runat="server" ID="labelDepartment" Text="แผนก"></asp:Label>
                                                </div>
                                                <div class="col-sm-4">
                                                    <asp:DropDownList runat="server" ID="ddlDepartment" CssClass="form-control">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="tab-pane fade" id="v-pills-role" role="tabpanel" aria-labelledby="v-pills-role-tab">
                                    <div class="form-row">
                                    <asp:Button runat="server" CssClass="btn btn-info" Text="Create" />
                                </div>
                                    <div>
                                    <table style="width:100%">
                                        <tr>
                                            <th>Select</th>
                                            <th>Role ID</th> 
                                            <th>Role Name</th>
                                            <th>Role Description</th>
                                        </tr>
                                    </table>
                                </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    
</asp:Content>
