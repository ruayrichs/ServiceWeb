<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MasterDataEquipmentControl.ascx.cs" Inherits="ServiceWeb.crm.Master.mastertier.usercontrol.MasterDataEquipmentControl" %>
<style>
    .title-box-tab {
        border-bottom: solid 1px #ccc; 
        display: block; 
        color: #00b9ff;
    }
    .penel-inbox {
        margin-bottom:15px;
    }
    .pad-column {
        padding-bottom:5px;
    }
</style>
<div class="row">
    <div class="col-lg-12">
         <ul class="nav nav-tabs" id="tab-pos">
            <li class="active" id="tab-pos-General-Equipment">
                <a data-toggle="tab" href="#General-Equipment">
                    <i class="fa fa-user" style="color: gold;"></i>&nbsp;General
                </a>
            </li>
            <li id="tab-pos-Location-Equipment">
                <a data-toggle="tab" href="#Location-Equipment">
                    <i class="fa fa-map-marker" style="color: blue;"></i>&nbsp;Location
                </a>
            </li> 
            <li id="tab-pos-Organization-Equipment">
                <a data-toggle="tab" href="#Organization-Equipment">
                    <i class="fa fa-credit-card" style="color: blue;"></i>&nbsp;Organization
                </a>
            </li> 
            <li id="tab-pos-OwnerAssignment-Equipment">
                <a data-toggle="tab" href="#OwnerAssignment-Equipment">
                    <i class="fa fa-users" style="color: blue;"></i>&nbsp;Owner Assignment
                </a>
            </li>
             <li id="tab-pos-Sale-Equipment">
                <a data-toggle="tab" href="#Sale-Equipment">
                    <i class="fa fa-money" style="color: blue;"></i>&nbsp;Sale
                </a>
            </li>
             <li id="tab-pos-SerialData-Equipment">
                <a data-toggle="tab" href="#SerialData-Equipment">
                    <i class="fa fa-credit-card" style="color: blue;"></i>&nbsp;Serial Data
                </a>
            </li>
             <li id="tab-pos-Warranty-Equipment">
                <a data-toggle="tab" href="#Warranty-Equipment">
                    <i class="fa fa-th-large" style="color: blue;"></i>&nbsp;Warranty
                </a>
            </li>
             <li id="tab-pos-AdditionalDat-Equipment">
                <a data-toggle="tab" href="#AdditionalDat-Equipment">
                    <i class="fa fa-info" style="color: blue;"></i>&nbsp;Additional Dat
                </a>
            </li>
             <li id="tab-pos-Picture-Equipment">
                <a data-toggle="tab" href="#Picture-Equipment">
                    <i class="fa fa-file-image-o" style="color: blue;"></i>&nbsp;Picture
                </a>
            </li> 

         </ul>
    </div>
</div>
<div class="tab-content" style="background-color: white; border: solid 1px #ddd; border-top: none; padding: 20px;">
    <div id="General-Equipment" class="tab-pane fade in active">
        <div class="penel-inbox GeneralBOX">
            <label class="title-box-tab">
                General BOX
            </label>
            <div class="row">
                <div class="col-lg-2 pad-column">
                    Weight
                </div>
                <div class="col-lg-2 pad-column">
                    <asp:TextBox ID="txtGeneralboxWeight" CssClass="form-control" runat="server" />
                </div>
                <div class="col-lg-2 pad-column">
                    <div class="input-group">
                        <asp:TextBox ID="txtGeneralboxWeightCode" CssClass="form-control" runat="server" />
                        <span class="input-group-addon hand">
                            <i class="fa fa-search"></i>
                        </span>
                    </div>
                </div>
                <div class="col-lg-2 pad-column">
                    Size/Dimension
                </div>
                <div class="col-lg-4">
                    <asp:TextBox ID="txtGeneralboxSize" CssClass="form-control" runat="server" />
                </div>
            </div>
            <div class="row">
                <div class="col-lg-2 pad-column">
                    Material NO
                </div>
                <div class="col-lg-4 pad-column">
                    <asp:TextBox ID="txtGeneralboxMaterialNo" CssClass="form-control" runat="server" />
                </div>
                <div class="col-lg-2 pad-column">
                    Start-up Date
                </div>
                <div class="col-lg-4 pad-column">
                    <div class="input-group">
                        <asp:TextBox ID="txtGeneralboxStartUpDate" CssClass="form-control date-picker" runat="server" />
                        <span class="input-group-addon hand">
                            <i class="fa fa-calendar"></i>
                        </span>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-2 pad-column">
                    Active By
                </div>
                <div class="col-lg-4 pad-column">
                    <asp:TextBox ID="txtGeneralboxActiveBy" Enabled="false" CssClass="form-control" runat="server" />
                </div>
                <div class="col-lg-2 pad-column">
                    Active Time
                </div>
                <div class="col-lg-4 pad-column">
                    <asp:TextBox ID="txtGeneralboxActiveTime" Enabled="false" CssClass="form-control" runat="server" />
                </div>
            </div>
            <div class="row">
                <div class="col-lg-2 pad-column">
                    Active Date
                </div>
                <div class="col-lg-4 pad-column">
                    <asp:TextBox ID="txtGeneralboxActiveDate" Enabled="false" CssClass="form-control" runat="server" />
                </div>
            </div>
        </div>

        <div class="penel-inbox ReferenceDataBox">
            <label class="title-box-tab">
                Reference Data Box
            </label>
             <div class="row">
                <div class="col-lg-2 pad-column">
                    Acquistion Value
                </div>
                <div class="col-lg-4 pad-column">
                    <asp:TextBox ID="txtAcquistionValue" CssClass="form-control" runat="server" />
                </div>
                <div class="col-lg-2 pad-column">
                    Acquistion Date
                </div>
                <div class="col-lg-4 pad-column">
                    <div class="input-group">
                        <asp:TextBox ID="txtAcquistionDate" CssClass="form-control date-picker" runat="server" />
                        <span class="input-group-addon hand">
                            <i class="fa fa-calendar"></i>
                        </span>
                    </div>
                </div>
            </div>
        </div>

        <div class="penel-inbox ManufacturerDataBox">
            <label class="title-box-tab">
                Manufacturer Data Box
            </label>
            <div class="row">
                <div class="col-lg-2 pad-column">
                    Manufacturer
                </div>
                <div class="col-lg-4 pad-column">
                    <asp:TextBox ID="txtManufacturer" CssClass="form-control" runat="server" />
                </div>
                <div class="col-lg-2 pad-column">
                    Manufacturer Country
                </div>
                <div class="col-lg-4 pad-column">
                    <asp:TextBox ID="txtManufacturerCountry" CssClass="form-control" runat="server" />
                </div>
            </div>
             <div class="row">
                <div class="col-lg-2 pad-column">
                    Model NO
                </div>
                <div class="col-lg-4 pad-column">
                    <asp:TextBox ID="txtManufacturerModelNO" CssClass="form-control" runat="server" />
                </div>
                <div class="col-lg-2">
                    Constr.Yr/Mn
                </div>
                <div class="col-lg-2 pad-column">
                    <div class="row">
                        <div class="col-lg-10 pad-column">
                             <asp:TextBox ID="txtManufacturerConstrYr" CssClass="form-control" runat="server" />
                       </div>
                        <div class="col-lg-2">
                            <span>/</span>
                        </div>
                    </div>
                </div>
                 <div class="col-lg-2 pad-column">
                    <asp:TextBox ID="txtManufacturerConstrMn" CssClass="form-control" runat="server" />
                </div>
            </div>
        </div>
    </div>
    <div id="Location-Equipment" class="tab-pane fade in">
        <div class="penel-inbox LocationDataBox">
            <label class="title-box-tab">
                Location Data Box
            </label>
            <div class="row">
                <div class="col-lg-2 pad-column">
                    Plant
                </div>
                <div class="col-lg-2 pad-column">
                    <div class="input-group">
                    <asp:TextBox ID="txtLocationBoxPlant" CssClass="form-control" runat="server" />
                      <span class="input-group-addon hand">
                            <i class="fa fa-search"></i>
                        </span>
                    </div>
                </div>
                <div class="col-lg-4 pad-column">
                    <asp:TextBox ID="txtLocationBoxPlantDescript" Enabled="false" CssClass="form-control" runat="server" />
                </div>
            </div>
            <div class="row">
               <div class="col-lg-2 pad-column">
                    Location Country
                </div>
                <div class="col-lg-2 pad-column">
                    <div class="input-group">
                    <asp:TextBox ID="txtLocationBoxLocation" CssClass="form-control" runat="server" />

                        <span class="input-group-addon hand">
                            <i class="fa fa-search"></i>
                        </span>
                    </div>
                </div>
                 <div class="col-lg-4 pad-column">
                    <asp:TextBox ID="txtLocationBoxLocationDescript" Enabled="false" CssClass="form-control" runat="server" />
                </div>
            </div>
             <div class="row">
                <div class="col-lg-2 pad-column">
                    Room
                </div>
                <div class="col-lg-4 pad-column">
                    <asp:TextBox ID="txtLocationBoxRoom" CssClass="form-control" runat="server" />
                </div>
            </div>
            <div class="row">
                <div class="col-lg-2 pad-column">
                    Shelf
                </div>
                <div class="col-lg-4 pad-column">
                    <asp:TextBox ID="txtLocationBoxShelf" CssClass="form-control" runat="server" />
                </div>
            </div>
            <div class="row">
                <div class="col-lg-2 pad-column">
                    Work Center
                </div>
                <div class="col-lg-2 pad-column">
                    <div class="input-group">
                       <asp:TextBox ID="txtLocationBoxWorkCenter" CssClass="form-control" runat="server" />
                        <span class="input-group-addon hand">
                            <i class="fa fa-search"></i>
                        </span>
                    </div>
                </div>
                <div class="col-lg-4 pad-column">
                    <asp:TextBox ID="txtLocationBoxWorkCenterDescript" Enabled="false" CssClass="form-control" runat="server" />
                </div>
            </div>

        </div>

         <div class="penel-inbox LocationDataBox">
            <label class="title-box-tab">
                Address Box
            </label>
             <div class="row">
                 <div class="col-lg-2 pad-column">
                     Name
                 </div>
                 <div class="col-lg-10 pad-column">
                     <asp:TextBox ID="txtLocationDataBoxName1" CssClass="form-control" runat="server" />
                 </div>
             </div>
             <div class="row">
                 <div class="col-lg-2 pad-column">
                 </div>
                 <div class="col-lg-10 pad-column">
                     <asp:TextBox ID="txtLocationDataBoxName2" CssClass="form-control" runat="server" />
                 </div>
             </div>
             <div class="row">
                 <div class="col-lg-2 pad-column">
                     Address
                 </div>
                 <div class="col-lg-2 pad-column">
                     <asp:TextBox ID="txtAddress1" CssClass="form-control" runat="server" />
                 </div>
                 <div class="col-lg-4 pad-column">
                     <asp:TextBox ID="txtAddress2" CssClass="form-control" runat="server" />
                 </div>
                 <div class="col-lg-2 pad-column">
                     <asp:TextBox ID="txtAddress3" CssClass="form-control" runat="server" />
                 </div>
                 <div class="col-lg-2 pad-column">
                     <asp:TextBox ID="txtAddress4" CssClass="form-control" runat="server" />
                 </div>
             </div>
             <div class="row">
                 <div class="col-lg-2 pad-column">
                     Street
                 </div>
                 <div class="col-lg-4 pad-column">
                      <asp:TextBox ID="txtLocationDataBoxStreet" CssClass="form-control" runat="server" />
                 </div>
             </div>
             <div class="row">
                 <div class="col-lg-2 pad-column">
                     Telephone
                 </div>
                 <div class="col-lg-4 pad-column">
                     <asp:TextBox ID="txtLocationDataBoxTelephone" CssClass="form-control"  runat="server" />
                 </div>
                  <div class="col-lg-2 pad-column">
                     Fax
                 </div>
                 <div class="col-lg-4 pad-column">
                     <asp:TextBox ID="txtLocationDataBoxFax" CssClass="form-control"  runat="server" />
                 </div>
             </div>

         </div>

    </div>
    <div id="Organization-Equipment" class="tab-pane fade in">
         <div class="penel-inbox Oganization">
            <label class="title-box-tab">
                Account Assignment Box
            </label>
             <div class="row">
                 <div class="col-lg-2 pad-column">
                     Company
                 </div>
                 <div class="col-lg-2 pad-column">
                     <asp:TextBox ID="txtCompanyCode" CssClass="form-control" runat="server" />
                 </div>
                  <div class="col-lg-6 pad-column">
                     <asp:TextBox ID="txtCompanyFullName" Enabled="false" CssClass="form-control" runat="server" />
                 </div>
             </div>
             <div class="row">
                 <div class="col-lg-2 pad-column">
                     Business Area
                 </div>
                 <div class="col-lg-2 pad-column">
                   <div class="input-group">
                    <asp:TextBox ID="txtBusinessArea" CssClass="form-control" runat="server" />
                      <span class="input-group-addon hand">
                            <i class="fa fa-search"></i>
                        </span>
                    </div>
                 </div>
                  <div class="col-lg-6 pad-column">
                     <asp:TextBox ID="txtBusinessAreaFullName" Enabled="false" CssClass="form-control" runat="server" />
                 </div>
             </div>
             <div class="row">
                 <div class="col-lg-2 pad-column">
                     Asset
                 </div>
                 <div class="col-lg-2 pad-column">
                     <div class="row">
                         <div class="col-lg-11">
                             <asp:TextBox ID="txtAsset1" CssClass="form-control" Enabled="false" runat="server" />
                         </div>
                         <div class="col-lg-1">
                             <label>/</label>
                         </div>
                     </div> 
                 </div>
                  <div class="col-lg-2 pad-column">
                    <div class="input-group">
                    <asp:TextBox ID="txtAsset2" Enabled="false" CssClass="form-control" runat="server" />
                      <span class="input-group-addon hand">
                            <i class="fa fa-search"></i>
                        </span>
                    </div>
                 </div>
             </div>
              <div class="row">
                 <div class="col-lg-2 pad-column">
                     Cost Center
                 </div>
                  <div class="col-lg-4 pad-column">
                      <div class="input-group">
                          <asp:TextBox ID="txtCostCenter" CssClass="form-control" runat="server" />
                          <span class="input-group-addon hand">
                              <i class="fa fa-search"></i>
                          </span>
                      </div>
                  </div>
              </div>
         </div>
    </div>
    <div id="OwnerAssignment-Equipment" class="tab-pane fade in">
        <div class="penel-inbox OwnerAssignment">
            <asp:Repeater ID="rptOwnerAssignment" runat="server">
                <HeaderTemplate>
                    <table class="table table-bordered">
                        <tr>
                             <th>Item No</th>
                             <th>OwnerType</th>
                             <th>OwnerCode</th>
                             <th>OwnerDesc</th>
                            <th>BeginDate</th>
                             <th>EndDate</th>
                             <th>ActiveStatus</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td><%# Eval("ItemNo") %></td>
                        <td><%# Eval("OwnerType") %></td>
                        <td><%# Eval("OwnerCode") %></td>
                        <td><%# Eval("OwnerDesc") %></td>
                        <td><%# Eval("BeginDate") %></td>
                        <td><%# Eval("EndDate") %></td>
                        <td><%# Eval("ActiveStatus") %></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>  
    </div>
    <div id="Sale-Equipment" class="tab-pane fade in">
         <div class="penel-inbox SalesandDistribution">
             <label class="title-box-tab">
                 Sales and Distribution
             </label>
             <div class="row">
                 <div class="col-lg-2 pad-column">
                     Billing DocType
                 </div>
                 <div class="col-lg-4 pad-column">
                     <div class="input-group">
                         <asp:TextBox ID="txtBillingDocType" CssClass="form-control" runat="server" />
                         <span class="input-group-addon hand">
                             <i class="fa fa-search"></i>
                         </span>
                     </div>
                 </div>
                 <div class="col-lg-6">
                      <asp:TextBox ID="txttxtBillingDocTypeDescript" Enabled="false" CssClass="form-control" runat="server" />
                 </div>
             </div>
             <div class="row">
                 <div class="col-lg-2 pad-column">
                     Billing Doc Year
                 </div>
                 <div class="col-lg-4 pad-column">
                     <asp:TextBox ID="txtBillingDocYear" CssClass="form-control" runat="server" />
                 </div>
             </div>
             <div class="row">
                 <div class="col-lg-2 pad-column">
                     Billing Docnumber
                 </div>
                 <div class="col-lg-4 pad-column">
                     <div class="input-group">
                         <asp:TextBox ID="txtBillingDocnumber" CssClass="form-control" runat="server" />
                         <span class="input-group-addon hand">
                             <i class="fa fa-search"></i>
                         </span>
                     </div>
                 </div>
             </div>

             <div class="row">
                 <div class="col-lg-2 pad-column">
                    Sale Organization
                 </div>
                 <div class="col-lg-4 pad-column">
                     <div class="input-group">
                         <asp:TextBox ID="txtSaleOrganization" CssClass="form-control" runat="server" />
                         <span class="input-group-addon hand">
                             <i class="fa fa-search"></i>
                         </span>
                     </div>
                 </div>
                 <div class="col-lg-6 pad-column">
                      <asp:TextBox ID="txttxtSaleOrganizationDescript" CssClass="form-control" runat="server" />
                 </div>
             </div>
             <div class="row">
                 <div class="col-lg-2 pad-column">
                    Dist Chan
                 </div>
                 <div class="col-lg-4 pad-column">
                     <div class="input-group">
                         <asp:TextBox ID="txtDistChan" CssClass="form-control" runat="server" />
                         <span class="input-group-addon hand">
                             <i class="fa fa-search"></i>
                         </span>
                     </div>
                 </div>
                 <div class="col-lg-6 pad-column">
                      <asp:TextBox ID="txtDistChanDescript" CssClass="form-control" runat="server" />
                 </div>
             </div>
             <div class="row">
                 <div class="col-lg-2 pad-column">
                    Division
                 </div>
                 <div class="col-lg-4 pad-column">
                     <div class="input-group">
                         <asp:TextBox ID="txtDivision" CssClass="form-control" runat="server" />
                         <span class="input-group-addon hand">
                             <i class="fa fa-search"></i>
                         </span>
                     </div>
                 </div>
                 <div class="col-lg-6 pad-column">
                      <asp:TextBox ID="txtDivisionDescript" CssClass="form-control" runat="server" />
                 </div>
             </div>
             <div class="row">
                 <div class="col-lg-2 pad-column">
                    Sale Office
                 </div>
                 <div class="col-lg-4 pad-column">
                     <div class="input-group">
                         <asp:TextBox ID="txtSaleOffice" CssClass="form-control" runat="server" />
                         <span class="input-group-addon hand">
                             <i class="fa fa-search"></i>
                         </span>
                     </div>
                 </div>
                 <div class="col-lg-6 pad-column">
                      <asp:TextBox ID="txttxtSaleOfficeDescript" CssClass="form-control" runat="server" />
                 </div>
             </div>
             <div class="row">
                 <div class="col-lg-2 pad-column">
                    Sale Group
                 </div>
                 <div class="col-lg-4 pad-column">
                     <div class="input-group">
                         <asp:TextBox ID="txtSaleGroup" CssClass="form-control" runat="server" />
                         <span class="input-group-addon hand">
                             <i class="fa fa-search"></i>
                         </span>
                     </div>
                 </div>
                 <div class="col-lg-6 pad-column">
                      <asp:TextBox ID="txtSaleGroupDescript" CssClass="form-control" runat="server" />
                 </div>
             </div>
         </div>
          <div class="penel-inbox LicenseBox">
             <label class="title-box-tab">
                 License Box
             </label>
              <div class="row">
                  <div class="row">
                      <div class="col-lg-2 pad-column">
                          License Number
                      </div>
                      <div class="col-lg-4 pad-column">
                          <asp:TextBox ID="txtLicenseNumber" CssClass="form-control" runat="server" />
                      </div>
                  </div>
              </div>
           </div>
         <div class="penel-inbox Oganization">
             <label class="title-box-tab">
                 Partner Data Box
             </label>
             <div class="row">
                  <div class="row">
                      <div class="col-lg-2 pad-column">
                          Sold-To Party
                      </div>
                      <div class="col-lg-4 pad-column">
                          <asp:TextBox ID="txtSoldToParty" CssClass="form-control" runat="server" />
                      </div>
                  </div>
              </div>
             <div class="row">
                  <div class="row">
                      <div class="col-lg-2 pad-column">
                          Ship-To Party
                      </div>
                      <div class="col-lg-4 pad-column">
                          <asp:TextBox ID="txtShipToParty" CssClass="form-control" runat="server" />
                      </div>
                  </div>
              </div>
          </div>
    </div>
    <div id="SerialData-Equipment" class="tab-pane fade in">
         <div class="penel-inbox SerialData">
             <label class="title-box-tab">
                 General Box
             </label>
             <div class="row">
                 <div class="col-lg-12">
                     <asp:Repeater ID="rptSerialData" runat="server">
                         <HeaderTemplate>
                             <table class="table table-bordered">
                                 <tr>
                                     <th>Item No</th>
                                     <th>MaterialCode</th>
                                     <th>MaterialName</th>
                                     <th>SerialNo</th>
                                     <th>EffectiveForm</th>
                                     <th>EffectiveTo</th>
                                     <th>ActiveStatus</th>
                                 </tr>
                         </HeaderTemplate>
                         <ItemTemplate>
                             <tr>
                                 <td><%# Eval("ItemNo") %></td>
                                 <td><%# Eval("MaterialCode") %></td>
                                 <td><%# Eval("MaterialName") %></td>
                                 <td><%# Eval("SerialNo") %></td>
                                 <td><%# Eval("EffectiveForm") %></td>
                                 <td><%# Eval("EffectiveTo") %></td>
                                 <td><%# Eval("ActiveStatus") %></td>
                             </tr>
                         </ItemTemplate>
                         <FooterTemplate>
                             </table>
                         </FooterTemplate>
                     </asp:Repeater>
                 </div>
             </div>

          </div>
    </div>
    <div id="Warranty-Equipment" class="tab-pane fade in">
         <div class="penel-inbox Warranty">
             <asp:Repeater ID="rptWarranty" runat="server">
                         <HeaderTemplate>
                             <table class="table table-bordered">
                                 <tr class="info">
                                     <th>Configuration Item No.</th>
                                     <th>Configuration Item Desc</th>
                                     <th>MatCode</th>
                                     <th>Mat.Decription</th>
                                     <th>ContractNo</th>
                                     <th>Waranetee No.</th>
                                     <th>Start Date</th>
                                     <th>End Date</th>
                                     <th>Termination Date</th>
                                 </tr>
                         </HeaderTemplate>
                         <ItemTemplate>
                             <tr>
                                 <td><%# Eval("EquipmentNo") %></td>
                                 <td><%# Eval("EquipmentDesc") %></td>
                                 <td><%# Eval("MatCode") %></td>
                                 <td><%# Eval("MatDecription") %></td>
                                 <td><%# Eval("ContractNo") %></td>
                                 <td><%# Eval("WaraneteeNo") %></td>
                                 <td><%# Eval("StartDate") %></td>
                                 <td><%# Eval("EndDate") %></td>
                                 <td><%# Eval("TerminationDate") %></td>
                             </tr>
                         </ItemTemplate>
                         <FooterTemplate>
                             </table>
                         </FooterTemplate>
                     </asp:Repeater>
         </div>
    </div>
    <div id="AdditionalDat-Equipment" class="tab-pane fade in">
         <div class="penel-inbox AdditionalDat">
             <label class="title-box-tab">
                 Properties
             </label>
             <div class="row">
                 <div class="col-lg-12">
                     <asp:Repeater ID="rptAdditionalDat" runat="server">
                         <HeaderTemplate>
                             <table class="table table-bordered">
                                 <tr class="info">
                                     <th>Properties</th>
                                     <th>Description</th>
                                     <th>Value</th>
                                     <th>Selected Value</th>
                                 </tr>
                         </HeaderTemplate>
                         <ItemTemplate>
                             <tr>
                                 <td><%# Eval("Properties") %></td>
                                 <td><%# Eval("Description") %></td>
                                 <td><%# Eval("Value") %></td>
                                 <td><%# Eval("Selected Value") %></td>
                             </tr>
                         </ItemTemplate>
                         <FooterTemplate>
                             </table>
                         </FooterTemplate>
                     </asp:Repeater>
                 </div>
             </div>
          </div>
    </div>
    <div id="Picture-Equipment" class="tab-pane fade in">
         <div class="penel-inbox AdditionalDat">
             <div class="row">
                 <div class="col-lg-12 pad-column">
                     <span class="btn btn-primary"><i class="fa fa-plus-circle"></i>&nbsp;Add Picture</span>
                 </div>
             </div>
         </div>
    </div>
</div>