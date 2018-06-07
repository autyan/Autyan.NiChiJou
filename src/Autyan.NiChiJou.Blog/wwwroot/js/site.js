// Write your JavaScript code.
var base = {};

$.ajaxSetup({
    beforeSend: function (xhr) {
        xhr.setRequestHeader('X-Requested-With', 'XMLHttpRequest');
    }
});

base.AjaxGet = function (ajaxUrl, params, callback) {
    ajaxUrl = ajaxUrl + '?t=' + Date.now();
    $.ajax(ajaxUrl,
        {
            type: "GET",
            data: params,
            success: function (xhr) {
                ajaxSuccess(xhr, params, callback);
            },
            error: function (xhr) {
                ajaxFailure(xhr, params);
            },
            complete: function (xhr) {
                ajaxComplete(xhr);
            }
        });
};

base.AjaxPost = function (ajaxUrl, params, callback) {
    ajaxUrl = ajaxUrl + '?t=' + Date.now();
    $.ajax(ajaxUrl,
        {
            type: "POST",
            data: params,
            success: function (xhr) {
                ajaxSuccess(xhr, params, callback);
            },
            error: function (xhr) {
                ajaxFailure(xhr, params);
            },
            complete: function (xhr) {
                ajaxComplete(xhr);
            }
        });
};

var trimStr = function (str) {
    var re = /^\s+|\s+$/;
    return !str ? "" : str.replace(re, "");
}

var IsNullOrEmpty = function (obj) {
    try {
        if (typeof obj === "function") {
            return false;
        }
        if (obj.length === 0) {
            return true;
        }

        return false;
    }
    catch (e) {
        if (!obj) {
            return true;
        }
        if (typeof obj === "string" && trimStr(obj) === "") {
            return true;
        }

        if (typeof obj === "number" && isNaN(obj))
            return true;



        return false;
    }
}

var IsFunction = function (obj) {
    return typeof obj === "function";
}

var hasOwnProperty = Object.prototype.hasOwnProperty;

function isEmpty(obj) {

    // null and undefined are "empty"
    if (obj == null) return true;

    // Assume if it has a length property with a non-zero value
    // that that property is correct.
    if (obj.length > 0) return false;
    if (obj.length === 0) return true;

    // Otherwise, does it have any properties of its own?
    // Note that this doesn't handle
    // toString and valueOf enumeration bugs in IE < 9
    for (var key in obj) {
        if (hasOwnProperty.call(obj, key)) return false;
    }

    return true;
}

var Msg = function (msg, option) {
    var baseModel = $('#baseModal');
    if (option.title !== null) {
        baseModel.find('.modal-title').empty();
        baseModel.find('.modal-title').append('<h3>' + option.title + '</h3>');
    }

    baseModel.find('.modal-body').empty();
    baseModel.find('.modal-body').append(msg);

    baseModel.find('.btn-default').empty();
    if (!IsNullOrEmpty(option.cancel)) {
        baseModel.find('.btn-default').append(option.cancel);
    } else {
        baseModel.find('.btn-default').append('关闭');
    }

    baseModel.find('.btn-main').hide();
    if (!IsNullOrEmpty(option.confirm)) {
        baseModel.find('.btn-main').show().innerHTML = option.confirm;
    }

    baseModel.modal({ show: true, keyboard: true });

    if (!IsNullOrEmpty(option.callback)) {
        $('#modal-confirm').off();
        $('#modal-confirm').on('click', function () {
            option.callback(option.param);
        });
    }

    setTimeout(function () { $('#modal-cancel').focus() }, 200);
};

function ajaxFailure(ret) {
    switch (ret.status) {
        case 404:
            Msg("未找到您选择的页面，请重试！", { title: '提示！' });
            return;
        case 0:
            Msg("请求错误，请检查网络连接！", { title: '提示！' });
            return;
    }
}

function ajaxSuccess(ret, params, callback) {
    if (!IsNullOrEmpty(ret.Data) && !IsNullOrEmpty(ret.Data.PostForm)) {
        $('#' + ret.Data.PostForm).submit();
        return;
    }
    if (!IsNullOrEmpty(callback) && IsFunction(callback) && ret.Data != null) {
        callback(ret.Data, params);
    }
}

function ajaxComplete(ret) {
    if (!IsNullOrEmpty(ret.Data.Messages)) {
        var message = ret.Data.Messages.join("\r\n");
        if (ret.Data.Exception !== null) {
            message += ('<br/>ExceptionInfo:<br/>' + ret.Data.Exception);
        }
        Msg(message, { title: '提示！' });
    }
}