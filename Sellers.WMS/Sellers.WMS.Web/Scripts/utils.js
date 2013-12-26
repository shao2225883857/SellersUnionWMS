var com = com || {};
var utils = utils || {};

utils.formatString = function(str, args) {
    var result = str;
    if (arguments.length > 0) {
        if (arguments.length == 1 && typeof(args) == "object") {
            for (var key in args) {
                if (args[key] != undefined) {
                    var reg = new RegExp("({" + key + "})", "g");
                    result = result.replace(reg, args[key]);
                }
            }
        } else {
            for (var i = 0; i < arguments.length; i++) {
                if (arguments[i] != undefined) {

                    var reg = new RegExp("({)" + i + "(})", "g");
                    result = result.replace(reg, arguments[i]);
                }
            }
        }
    }
    return result;
};

com.createFrame= function(url) {
    var s = '<iframe scrolling="auto" frameborder="0"  src="' + url + '" style="width:100%;height:99.5%"></iframe>';
    return s;
}
com.exporter = function (opt) {
    var self = this;

    var defaultOptions = {
        action: "/home/download",
        dataGetter: "api",
        dataAction: "",
        dataParams: {},
        titles: [[]],
        fileType: 'xls',
        compressType: 'none'
    };

    this.paging = function (page, rows) {
        self.params.dataParams.page = page;
        self.params.dataParams.rows = rows;
        return self;
    };

    this.compress = function () {
        self.params.compressType = 'zip';
        return self;
    };

    this.title = function (filed, title) {
        self.params.titles[0][filed] = title;
        return self;
    };

    this.download = function (suffix) {
        self.params.fileType = suffix || "xls";
        self.params.dataParams = JSON.stringify(self.params.dataParams);
        self.params.titles = JSON.stringify(self.params.titles);
        var downloadHelper = $('<iframe style="display:none;" id="downloadHelper"></iframe>').appendTo('body')[0];
        var doc = downloadHelper.contentWindow.document;
        if (doc) {
            doc.open();
            doc.write('')//微软为doc.clear()有时会出bug
            doc.writeln(utils.formatString("<html><body><form id='downloadForm' name='downloadForm' method='post' action='{0}'>"
, self.params.action));
            for (var key in self.params)
                doc.writeln(utils.formatString("<input type='hidden' name='{0}' value='{1}'>", key, self.params[key]));
            doc.writeln('<\/form><\/body><\/html>');
            doc.close();
            var form = doc.forms[0];
            if (form) {
                form.submit();
            }
        }
    };

    initFromGrid = function (grid) {
        var options = grid.$element().datagrid('options');
        if (grid.treegrid)
            options.url = options.url || grid.treegrid('options').url;

        var titles = [[]], length = Math.max(options.frozenColumns.length, options.columns.length);
        for (var i = 0; i < length; i++)
            titles[i] = (options.frozenColumns[i] || []).concat(options.columns[i] || []);
        self.params = $.extend(true, {}, defaultOptions, {
            dataAction: options.url,
            dataParams: options.queryParams,
            titles: titles
        });
    };

    if (opt.$element)
        initFromGrid(opt);
    else
        self.params = $.extend(true, {}, defaultOptions, opt);
    return self;
};