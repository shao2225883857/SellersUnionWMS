/**
* 模块名：mms viewModel
* 程序名: wms.viewModel.search.js
* Copyright(c) 2013-2015 liuhuisheng [ liuhuisheng.xm@gmail.com ] 
**/
var wms = wms || {};
wms.viewModel = wms.viewModel || {};

com.editGridViewModel = function (grid) {
    var self = this;
    this.begin = function (index, row) {
        if (index == undefined || typeof index === 'object') {
            row = grid.datagrid('getSelected');
            index = grid.datagrid('getRowIndex', row);
        }
        self.editIndex = self.ended() ? index : self.editIndex;
        grid.datagrid('selectRow', self.editIndex).datagrid('beginEdit', self.editIndex);
    };
    this.ended = function () {
        if (self.editIndex == undefined) return true;
        if (grid.datagrid('validateRow', self.editIndex)) {
            grid.datagrid('endEdit', self.editIndex);
            self.editIndex = undefined;
            return true;
        }
        grid.datagrid('selectRow', self.editIndex);
        return false;
    };
    this.addnew = function (rowData) {
        if (self.ended()) {
            if (Object.prototype.toString.call(rowData) != '[object Object]') rowData = {};
            rowData = $.extend({ _isnew: true }, rowData);
            grid.datagrid('appendRow', rowData);
            self.editIndex = grid.datagrid('getRows').length - 1;
            grid.datagrid('selectRow', self.editIndex);
            self.begin(self.editIndex, rowData);
        }
    };
    this.deleterow = function () {
        var selectRow = grid.datagrid('getSelected');
        if (selectRow) {
            var selectIndex = grid.datagrid('getRowIndex', selectRow);
            if (selectIndex == self.editIndex) {
                grid.datagrid('cancelEdit', self.editIndex);
                self.editIndex = undefined;
            }
            grid.datagrid('deleteRow', selectIndex);
        }
    };
    this.reject = function () {
        grid.datagrid('rejectChanges');
    };
    this.accept = function () {
        grid.datagrid('acceptChanges');
        var rows = grid.datagrid('getRows');
        for (var i in rows) delete rows[i]._isnew;
    };
    this.getChanges = function (include, ignore) {
        if (!include) include = [], ignore = true;
        var deleted = utils.filterProperties(grid.datagrid('getChanges', "deleted"), include, ignore),
            updated = utils.filterProperties(grid.datagrid('getChanges', "updated"), include, ignore),
            inserted = utils.filterProperties(grid.datagrid('getChanges', "inserted"), include, ignore);

        var changes = { deleted: deleted, inserted: utils.minusArray(inserted, deleted), updated: utils.minusArray(updated, deleted) };
        changes._changed = (changes.deleted.length + changes.updated.length + changes.inserted.length) > 0;

        return changes;
    };
    this.isChangedAndValid = function () {
        return self.ended() && self.getChanges()._changed;
    };
};

//通用查询页面
wms.viewModel.search = function (data) {
    var self = this;
    this.idField = data.idField || "Id";
    this.urls = data.urls;
    this.resx = data.resx;
    this.dataSource = data.dataSource;
    this.form = ko.mapping.fromJS(data.form);
    delete this.form.__ko_mapping__;

    this.grid = {
        size: { w: 4, h: 94 },
        url: self.urls.query,
        queryParams: ko.observable(),
        pagination: true,
        customLoad: false
    };
    this.grid.queryParams(data.form);
    this.searchClick = function () {
        var param = ko.toJS(this.form);
        this.grid.queryParams(param);
    };
    this.clearClick = function () {
        $.each(self.form, function () { this(''); });
        this.searchClick();
    };
    this.refreshClick = function () {
        window.location.reload();
    };
    this.addClick = function () {
        com.ajax({
            type: 'GET',
            url: self.urls.billno,
            success: function (d) {
                com.openTab(self.resx.detailTitle, self.urls.edit + d);
            }
        });
    };
    this.deleteClick = function () {
        var row = self.grid.datagrid('getSelected');
        if (!row) return com.message('warning', self.resx.noneSelect);
        com.message('confirm', self.resx.deleteConfirm, function (b) {
            if (b) {
                com.ajax({
                    type: 'DELETE', url: self.urls.remove + row[self.idField], success: function () {
                        com.message('success', self.resx.deleteSuccess);
                        self.searchClick();
                    }
                });
            }
        });
    };
    this.editClick = function () {
        var row = self.grid.datagrid('getSelected');
        if (!row) return com.message('warning', self.resx.noneSelect);
        com.openTab(self.resx.detailTitle, self.urls.edit + row[self.idField]);
    };
    this.grid.onDblClickRow = this.editClick;
    this.auditClick = function () {
        var row = self.grid.datagrid('getSelected');
        if (!row) return com.message('warning', self.resx.noneSelect);
        com.auditDialog(function (d) {
            com.ajax({
                type: 'POST',
                url: self.urls.audit + row[self.idField],
                data: JSON.stringify(d),
                success: function () {
                    com.message('success', self.resx.auditSuccess);
                }
            });
        });
    };
    this.downloadClick = function (vm, event) {
        com.exportFile(self.grid).download($(event.currentTarget).attr("suffix"));
    };
};

//通用编辑页面
wms.viewModel.edit = function (data) {
    var self = this;
    this.dataSource = data.dataSource;                          //下拉框的数据源
    this.urls = data.urls;                                      //api服务地址
    this.resx = data.resx;                                      //中文资源
    this.scrollKeys = ko.mapping.fromJS(data.scrollKeys);       //数据滚动按钮（上一条下一条）
    this.form = ko.mapping.fromJS(data.form || data.defaultForm); //表单数据
    this.setting = data.setting;
    this.defaultRow = data.defaultRow;                          //默认grid行的值
    this.defaultForm = data.defaultForm;                        //主表的默认值

    this.grid = {
        size: { w: 6, h: 177 },
        pagination: false,
        remoteSort: false,
        url: ko.observable(self.urls.getdetail + self.scrollKeys.current())
    };
    this.gridEdit = new com.editGridViewModel(self.grid);
    this.grid.onDblClickRow = self.gridEdit.begin;
    this.grid.onClickRow = self.gridEdit.ended;
    this.grid.toolbar = [{
        text: '选择在库材料',
        iconCls: 'icon-search',
        handler: function () {
            wms.com.selectMaterial(self, { _xml: 'wms.material_dict' });
        }
    }, '-', {
        text: '删除材料',
        iconCls: 'icon-remove',
        handler: self.gridEdit.deleterow
    }];

    this.rejectClick = function () {
        ko.mapping.fromJS(data.form, {}, self.form);
        self.gridEdit.reject();
        com.message('success', self.resx.rejected);
    };
    this.firstClick = function () {
        self.scrollTo(self.scrollKeys.first());
    };
    this.previousClick = function () {
        self.scrollTo(self.scrollKeys.previous());
    };
    this.nextClick = function () {
        self.scrollTo(self.scrollKeys.next());
    };
    this.lastClick = function () {
        self.scrollTo(self.scrollKeys.last());
    };
    this.scrollTo = function (id) {
        if (id == self.scrollKeys.current()) return;
        com.setLocationHashId(id);
        com.ajax({
            type: 'GET',
            url: self.urls.getmaster + id,
            success: function (d) {
                ko.mapping.fromJS(d, {}, self);
                ko.mapping.fromJS(d, {}, data);
            }
        });
        self.grid.url(self.urls.getdetail + id);
        self.grid.datagrid('loaded');
    };
    this.saveClick = function () {
        self.gridEdit.ended(); //结束grid编辑状态
        var post = {           //传递到后台的数据
            form: com.formChanges(self.form, data.form, self.setting.postFormKeys),
            list: self.gridEdit.getChanges(self.setting.postListFields)
        };
        if ((self.gridEdit.ended() && com.formValidate()) && (post.form._changed || post.list._changed)) {
            com.ajax({
                url: self.urls.edit,
                data: ko.toJSON(post),
                success: function (d) {
                    com.message('success', self.resx.editSuccess);
                    ko.mapping.fromJS(post.form, {}, data.form); //更新旧值
                    self.gridEdit.accept();
                }
            });
        }
    };
    this.auditClick = function () {
        var updateArray = ['ApproveState', 'ApproveRemark'];
        wms.com.auditDialog(this.form, function (d) {
            com.ajax({
                type: 'POST',
                url: self.urls.audit + self.scrollKeys.current(),
                data: JSON.stringify(d),
                success: function () {
                    com.message('success', d.status == "passed" ? self.resx.auditPassed : self.resx.auditReject);
                    if (data.form)
                        for (var i in updateArray) data.form[updateArray[i]] = self.form[updateArray[i]]();
                }
            });
        });
    };
    this.approveButton = {
        iconCls: ko.computed(function () { return self.form.ApproveState() == "passed" ? "icon-user-reject" : "icon-user-accept"; }),
        text: ko.computed(function () { return self.form.ApproveState() == "passed" ? "取消审核" : "审核"; })
    };
    this.printClick = function () {
        com.openTab('打印报表', '/report?p1=0002&p2=2012-1-1', 'icon-printer_color');
    };
};


var com = com || {};
var wms = wms || {};
wms.viewModel = wms.viewModel || {};

com.editGridViewModel = function (grid) {
    var self = this;
    this.begin = function (index, row) {
        if (index == undefined || typeof index === 'object') {
            row = grid.datagrid('getSelected');
            index = grid.datagrid('getRowIndex', row);
        }
        self.editIndex = self.ended() ? index : self.editIndex;
        grid.datagrid('selectRow', self.editIndex).datagrid('beginEdit', self.editIndex);
    };
    this.ended = function () {
        if (self.editIndex == undefined) return true;
        if (grid.datagrid('validateRow', self.editIndex)) {
            grid.datagrid('endEdit', self.editIndex);
            self.editIndex = undefined;
            return true;
        }
        grid.datagrid('selectRow', self.editIndex);
        return false;
    };
    this.addnew = function (rowData) {
        if (self.ended()) {
            if (Object.prototype.toString.call(rowData) != '[object Object]') rowData = {};
            rowData = $.extend({ _isnew: true }, rowData);
            grid.datagrid('appendRow', rowData);
            self.editIndex = grid.datagrid('getRows').length - 1;
            grid.datagrid('selectRow', self.editIndex);
            self.begin(self.editIndex, rowData);
        }
    };
    this.deleterow = function () {
        var selectRow = grid.datagrid('getSelected');
        if (selectRow) {
            var selectIndex = grid.datagrid('getRowIndex', selectRow);
            if (selectIndex == self.editIndex) {
                grid.datagrid('cancelEdit', self.editIndex);
                self.editIndex = undefined;
            }
            grid.datagrid('deleteRow', selectIndex);
        }
    };
    this.reject = function () {
        grid.datagrid('rejectChanges');
    };
    this.accept = function () {
        grid.datagrid('acceptChanges');
        var rows = grid.datagrid('getRows');
        for (var i in rows) delete rows[i]._isnew;
    };
    this.getChanges = function (include, ignore) {
        if (!include) include = [], ignore = true;
        var deleted = utils.filterProperties(grid.datagrid('getChanges', "deleted"), include, ignore),
            updated = utils.filterProperties(grid.datagrid('getChanges', "updated"), include, ignore),
            inserted = utils.filterProperties(grid.datagrid('getChanges', "inserted"), include, ignore);

        var changes = { deleted: deleted, inserted: utils.minusArray(inserted, deleted), updated: utils.minusArray(updated, deleted) };
        changes._changed = (changes.deleted.length + changes.updated.length + changes.inserted.length) > 0;

        return changes;
    };
    this.isChangedAndValid = function () {
        return self.ended() && self.getChanges()._changed;
    };
};

