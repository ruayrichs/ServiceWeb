<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true" CodeBehind="UserChangePassword.aspx.cs" Inherits="ServiceWeb.UserProfile.UserChangePassword" %>

<%@ Register Src="~/UserProfile/UserControl/ChangePass.ascx" TagPrefix="uc1" TagName="ChangePass" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <style type="text/css">
        #table-coverage > tbody > tr:nth-child(2) > td {
            padding-top: 1.25rem;
        }

        #table-coverage > tbody > tr > td {
            border-top: 0;
        }

            #table-coverage > tbody > tr > td.card-header {
                padding: .75rem .3rem;
            }

            #table-coverage > tbody > tr > td:first-child {
                padding-left: 1.25rem;
            }

            #table-coverage > tbody > tr > td:last-child {
                padding-right: 1.25rem;
            }
    </style>

    <div class="row">
        <div class="col">
            <div class="card shadow">
                <div class="card-header">
                    <h5 class="mb-0">Change Password</h5>
                </div>
                <div class="card-body">
                    <div class="form-row">
                        <div class="col-lg-12">
                            <uc1:ChangePass runat="server" id="ChangePass" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        $(document).ready(function () {
        });
    </script>

</asp:Content>
