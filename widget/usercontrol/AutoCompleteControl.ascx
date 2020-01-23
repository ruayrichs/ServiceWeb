<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AutoCompleteControl.ascx.cs" Inherits="ServiceWeb.widget.usercontrol.AutoCompleteControl" %>

<style>
    .ui-autocomplete {
        z-index: 10000 !important;
        max-height: 250px;
        overflow-y: scroll;
        overflow-x: hidden;
    }
    .autocomplete-set-rename {
            position: absolute;
            padding: 10px;
            /*padding-top: 10px;*/
            border-radius: 5px;
            border: 1px solid #ccc;
            background: #fff;
            z-index: 1;
            box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.29);
            width: 350px;
            z-index: 10001 !important
    }
    .autocomplete-set-rename-background {
        position: fixed;
        z-index: 1000;
        left: 0;
        top: 0;
        right: 0;
        bottom: 0;
        background: rgba(0, 0, 0, 0.63);
    }
    .body-scroll {
        overflow: hidden;
    }

    #<%= textAutoComplete.ClientID %>[disabled] + .showpanelRename<%= ClientID %> {
      cursor : no-drop;
    }
</style>

<div class="panel-autocomplete-box" id="<%= "control-" + ClientID %>">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpBindAutoComplete">
        <ContentTemplate>

            <div class="divinputgroup<%= ClientID %>">
                <asp:TextBox runat="server" CssClass="form-control" ID="textAutoComplete" />
                <span class="hide d-none input-group-addon hand showpanelRename<%= ClientID %>">
                    <i class="fa fa-pencil" aria-hidden="true"></i>
                </span>
            </div>

            <asp:HiddenField runat="server" ID="hddDataSource" Value="[]" />
            <asp:HiddenField runat="server" ID="hddDataValue" />
            <asp:HiddenField runat="server" ID="hddDataText" />

			<div class="hide d-none autocomplete-control-data<%= ClientID %>"><%= ControlData %></div>
            <div id="strJson<%= ClientID %>" class="hide d-none"><%= strJson %></div>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <div class="autocomplete-set-rename-background" style="display: none;">
        <div class="autocomplete-set-rename">
            <div class="row">
                <div class="col-md-12">
                    <b><i class="fa fa-pencil" aria-hidden="true"></i>Re Name</b>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <asp:TextBox ID="txtRenameData" CssClass="form-control input-sm" runat="server" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <button class="btn btn-success btn-sm setRenameAutoCompleteData<%= ClientID %>" type="button">
                        <i class="fa fa-check-square-o"></i>
                        บันทึกการแก้ไข
                    </button>
                    <button class="btn btn-default btn-sm" onclick="$(this).closest('.autocomplete-set-rename-background').hide();$('body').removeClass('body-scroll');" type="button">
                        <i class="fa fa-remove"></i>
                        ยกเลิกการแก้ไข
                    </button>
                </div>
            </div>
        </div>
    </div>
</div>