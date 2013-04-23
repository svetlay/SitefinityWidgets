<%@ Control Language="C#" AutoEventWireup="true" %>
<%@ Register Assembly="Telerik.Sitefinity" Namespace="Telerik.Sitefinity.Web.UI"
    TagPrefix="sf" %>
<%@ Register Assembly="Telerik.Sitefinity" Namespace="Telerik.Sitefinity.Web.UI.Backend.Security.Principals"
    TagPrefix="sf" %>
<%@ Register Assembly="Telerik.Sitefinity" TagPrefix="designers" Namespace="Telerik.Sitefinity.Web.UI.ControlDesign" %>
<sf:ResourceLinks ID="resourcesLinks" runat="server">
    <sf:ResourceFile JavaScriptLibrary="JQuery" />
    <sf:ResourceFile Name="Scripts/jquery-ui-1.8.8.custom.min.js" />
    <sf:ResourceFile Name="Styles/jQuery/jquery.ui.core.css" />
    <sf:ResourceFile Name="Styles/jQuery/jquery.ui.dialog.css" />
    <sf:ResourceFile Name="Styles/jQuery/jquery.ui.theme.sitefinity.css" />
</sf:ResourceLinks>

<sf:SitefinityLabel ID="titleLabel" runat="server" CssClass="sfTxtLbl" />
<div id="selectorWrapper" runat="server">
<%--    <sf:UserSelector ID="itemsSelector" runat="server" ItemType="Telerik.Sitefinity.Security.Model.User" ItemSurrogateType="Telerik.Sitefinity.Security.Web.Services.WcfMembershipUser, Telerik.Sitefinity, Version=5.3.3900.0, Culture=neutral, PublicKeyToken=b28c218413bdf563"
        DataKeyNames="UserID" ShowSelectedFilter="true" AllowPaging="false" PageSize="10" AllowMultipleSelection="true"
        AllowSearching="true" ShowProvidersList="true" InclueAllProvidersOption="true"
        ServiceUrl="~/Sitefinity/Services/Security/Users.svc/"
        SearchBoxTitleText="Filter by username" ShowHeader="true">
        <DataMembers>
            <sf:DataMemberInfo runat="server" Name="DisplayName" IsExtendedSearchField="true" HeaderText='User Name'>
            <img sys:src="{{AvatarThumbnailUrl}}" width="48px" />
                <strong>{{DisplayName}}</strong>
            </sf:DataMemberInfo>
            <sf:DataMemberInfo runat="server" Name="PublicationDate" HeaderText='Date'>
                <span>{{Email}}</span>
            </sf:DataMemberInfo>
        </DataMembers>
    </sf:UserSelector>--%>
    <sf:UserSelector id="itemsSelector" ServiceUrl="~/Sitefinity/Services/Security/Users.svc" runat="server">
       <DataMembers>
            <sf:DataMemberInfo ID="DataMemberInfo1" runat="server" Name="UserName" IsExtendedSearchField="false" HeaderText='User Name'>
            <img sys:src="{{AvatarThumbnailUrl}}" width="48px" />
                
            </sf:DataMemberInfo>
               <sf:DataMemberInfo ID="DataMemberInfo3" runat="server" Name="DisplayName" IsExtendedSearchField="false" HeaderText='Full Name'>
                <span>{{DisplayName}}</span>
            </sf:DataMemberInfo>
        </DataMembers>
    </sf:UserSelector>
    <asp:Panel runat="server" ID="buttonAreaPanel" class="sfButtonArea sfSelectorBtns">
        <asp:LinkButton ID="doneButton" runat="server" OnClientClick="return false;" CssClass="sfLinkBtn sfSave">
            <strong class="sfLinkBtnIn">
            <asp:Literal runat="server" Text="<%$Resources:Labels, Done %>" />
            </strong>
        </asp:LinkButton>
        <asp:Literal ID="literalOr" runat="server" Text="<%$Resources:Labels, or%>" />
            <asp:LinkButton ID="cancelButton" runat="server" CssClass="sfCancel" OnClientClick="return false;">
            <asp:Literal runat="server" Text="<%$Resources:Labels, Cancel %>" />
            </asp:LinkButton>
    </asp:Panel>
</div>

<ul id="selectedItemsList" data-template="ul-template" data-bind="source: items" runat="server" class="sfCategoriesList">
</ul>
<script id="ul-template" type="text/x-kendo-template">
    <li>
        <span data-bind="text: DisplayName"> </span>
        <a class="remove sfRemoveBtn">Remove</a>                            
    </li>
</script>
<asp:HyperLink ID="selectButton" runat="server" NavigateUrl="javascript:void(0);" CssClass="sfLinkBtn sfChange">
    <strong class="sfLinkBtnIn">Select...</strong>
</asp:HyperLink>
<sf:SitefinityLabel ID="descriptionLabel" runat="server" WrapperTagName="div" HideIfNoText="true"
    CssClass="sfDescription" />
<sf:SitefinityLabel ID="exampleLabel" runat="server" WrapperTagName="div" HideIfNoText="true" CssClass="sfExample" />
