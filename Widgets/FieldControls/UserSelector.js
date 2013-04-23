
Type.registerNamespace("SitefinityWebApp.Widgets.FieldControls");

SitefinityWebApp.Widgets.FieldControls.UserSelector = function (element) {

    SitefinityWebApp.Widgets.FieldControls.UserSelector.initializeBase(this, [element]);
    this._selectButton = null;
    this._doneButton = null;
    this._cancelButton = null;
    this._itemsSelector = null;
    this._selectorWrapper = null;
    this._selectedItemsList = null;
    this._selectDialog = null;
    this._dynamicModulesDataServicePath = null;

    /*empty kendo observable array*/
    this._selectedItems = kendo.observable({items:[]});

    this._selectButtonClickedDelegate = null;
    this._getSelectedItemsSuccessDelegate = null;
    this._removeSelectedItemDelegate = null;
    this._doneButtonClickedDelegate = null;
    this._cancelButtonClickedDelegate = null;

}

SitefinityWebApp.Widgets.FieldControls.UserSelector.prototype =
{
    initialize: function () {
        SitefinityWebApp.Widgets.FieldControls.UserSelector.callBaseMethod(this, "initialize");

        if (this._selectButton) {
            this._selectButtonClickedDelegate = Function.createDelegate(this, this._selectButtonClicked);
            $addHandler(this._selectButton, "click", this._selectButtonClickedDelegate);
        }

        if (this._doneButton) {
            this._doneButtonClickedDelegate = Function.createDelegate(this, this._doneButtonClicked);
            $addHandler(this._doneButton, "click", this._doneButtonClickedDelegate);
        }

        if (this._cancelButton) {
            this._cancelButtonClickedDelegate = Function.createDelegate(this, this._cancelButtonClicked);
            $addHandler(this._cancelButton, "click", this._cancelButtonClickedDelegate);
        }

        this._getSelectedItemsSuccessDelegate = Function.createDelegate(this, this._getSelectedItemsSuccess);

        this._removeSelectedItemDelegate = Function.createDelegate(this, this._removeSelectedItem);
        jQuery(this.get_element()).find(this.get_selectedItemsList()).delegate('.remove', 'click', this._removeSelectedItemDelegate);

        if (this.get_itemsSelector()) {
            this._selectDialog = jQuery(this.get_selectorWrapper()).dialog({
                autoOpen: false,
                modal: true,
                width: 410,
                height: "auto",
                closeOnEscape: true,
                resizable: false,
                draggable: false,
                zIndex: 5000,
                dialogClass: "sfSelectorDialog"
            });
        }
        /*bind the empty observable array to the list which is going to display it*/
        
        kendo.bind($(this.get_selectedItemsList()), this._selectedItems);
    },

    dispose: function () {
        SitefinityWebApp.Widgets.FieldControls.UserSelector.callBaseMethod(this, "dispose");
        if (this._selectButton) {
            $removeHandler(this._selectButton, "click", this._selectButtonClickDelegate);
        }
        if (this._selectButtonClickedDelegate) {
            delete this._selectButtonClickedDelegate;
        }
        if (this._getSelectedItemsSuccessDelegate) {
            delete this._getSelectedItemsSuccessDelegate;
        }
        if (this._removeSelectedItemDelegate) {
            jQuery(this.get_element()).find(this.get_selectedItemsList()).undelegate('.remove', 'click', this._removeSelectedItemDelegate);
            delete this._removeSelectedItemDelegate;
        }
        if (this._doneButtonClickedDelegate) {
            delete this._doneButtonClickedDelegate;
        }
        if (this._cancelButtonClickedDelegate) {
            delete this._cancelButtonClickedDelegate;
        }

        this._selectedItems.items.splice(0, this._selectedItems.items.length);
    },

    /* --------------------  public methods ----------- */

    /*Gets the value of the field control.*/
    get_value: function () {
        /*on publish if we have items in the kendo observable array 
        we get their ids in a aray of Guids so that they can be persisted*/
        var selectedKeysArray = new Array();
        var data = this._selectedItems.toJSON();
        for (var i = 0; i < data.items.length; i++) {
            selectedKeysArray.push(data.items[i].UserID);
        }
        if (selectedKeysArray.length > 0)
            return selectedKeysArray;
        else
            return null;
    },

    /*Sets the value of the text field control.*/
    set_value: function (value) {
        /*clears the observable array*/
        this._selectedItems.items.splice(0, this._selectedItems.items.length);
        var surrogateItemType = "Telerik.Sitefinity.Security.Web.Services.WcfMembershipUser"

        /*if there are related items get them through the dynamic modules' data service*/
        if (value != null && value != "") {
            var filterExpression = "";
            for (var i = 0; i < value.length; i++) {
                if (i == 0)
                    filterExpression = filterExpression + 'Id = ' + value[i].toString();
                else
                    filterExpression = filterExpression + ' OR Id = ' + value[i].toString();
            }
            var data = {

                "filter": filterExpression
                
            };
            $.ajax({
                url: this.get_dynamicModulesDataServicePath(),
                type: "GET",
                dataType: "json",
                data: data,
                contentType: "application/json; charset=utf-8",
                /*on success add them to the kendo observable array*/
                success: this._getSelectedItemsSuccessDelegate
            });
        }

        this.raisePropertyChanged("value");
        this._valueChangedHandler();
    },


    /* -------------------- events -------------------- */

    /* -------------------- event handlers ------------ */

    _selectButtonClicked: function (sender, args) {
        this.get_itemsSelector().dataBind();
        this._selectDialog.dialog("open");
        this._selectDialog.dialog().parent().css("min-width", "525px");
        dialogBase.resizeToContent();
        /*Check already selected related items*/
        var selected = this._selectedItems.items.toJSON();
        var toBeChecked = new Array();
        for (var i = 0; i < selected.length; i++) {
            toBeChecked.push(selected[i].UserID);
        }
        this._itemsSelector.set_selectedKeys(toBeChecked);
        return false;
    },

    _doneButtonClicked: function (keys) {
        if (keys != null) {
            /*push newly selected items in the observable array*/
            var selectedItems = this.get_itemsSelector().getSelectedItems();
            this._selectedItems.items.splice(0, this._selectedItems.items.length);
            for (var i = 0; i < selectedItems.length; i++) {
                this._selectedItems.items.push(selectedItems[i]);
            }
        }
        this._selectDialog.dialog("close");
        dialogBase.resizeToContent();
    },

    _cancelButtonClicked: function (sender, args) {
        this._selectDialog.dialog("close");
        dialogBase.resizeToContent();
    },

    _getSelectedItemsSuccess: function (result) {
        /*push existing related items in the kendo observable array*/
        this._selectedItems.items.splice(0, this._selectedItems.items.length);
        for (var i = 0; i < result.Items.length; i++) {
            this._selectedItems.items.push(result.Items[i]);
        }
    },

    _removeSelectedItem: function (value) {
        var itemToRemove = $(value.target).siblings().first();
        var itemName = itemToRemove.html();
        var data = this._selectedItems.toJSON();
        /*find the index of the selected item and delete it*/
        for (var i = 0; i < data.items.length; i++) {
         
            if (data.items[i].DisplayName == itemName) {

                this._selectedItems.items.splice(i, 1);
                break;
            }
        }
    },

    /* -------------------- private methods ----------- */

    /* -------------------- properties ---------------- */

    get_selectButton: function () {
        return this._selectButton;
    },
    set_selectButton: function (value) {
        this._selectButton = value;
    },

    get_itemsSelector: function () {
        return this._itemsSelector;
    },
    set_itemsSelector: function (value) {
        this._itemsSelector = value;
    },

    get_selectorWrapper: function () {
        return this._selectorWrapper;
    },
    set_selectorWrapper: function (value) {
        this._selectorWrapper = value;
    },

    get_selectedItemsList: function () {
        return this._selectedItemsList;
    },
    set_selectedItemsList: function (value) {
        this._selectedItemsList = value;
    },

    get_doneButton: function () {
        return this._doneButton;
    },
    set_doneButton: function (value) {
        this._doneButton = value;
    },

    get_cancelButton: function () {
        return this._cancelButton;
    },
    set_cancelButton: function (value) {
        this._cancelButton = value;
    },

    get_dynamicModulesDataServicePath: function () {
        return this._dynamicModulesDataServicePath;
    },
    set_dynamicModulesDataServicePath: function (value) {
        this._dynamicModulesDataServicePath = value;
    },

    get_dynamicModuleType: function () {
        return this._dynamicModuleType;
    },
    set_dynamicModuleType: function (value) {
        this._dynamicModuleType = value;
    }
};

SitefinityWebApp.Widgets.FieldControls.UserSelector.registerClass("SitefinityWebApp.Widgets.FieldControls.UserSelector", Telerik.Sitefinity.Web.UI.Fields.FieldControl);
