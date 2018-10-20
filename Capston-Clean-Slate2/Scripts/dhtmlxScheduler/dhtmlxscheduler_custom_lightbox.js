/*
@license
dhtmlxScheduler.Net v.3.4.1 Professional Evaluation

This software is covered by DHTMLX Evaluation License. Contact sales@dhtmlx.com to get Commercial or Enterprise license. Usage without proper license is prohibited.

(c) Dinamenta, UAB.
*/
Scheduler.plugin(function (e) {
e.initCustomLightbox = function (t, i, r) {
    function n() { if (!i._lightbox) { var e = i._getLightbox(), r = e.childNodes[1]; e.className.indexOf("dhx_cal_light_wide") >= 0 ? r.lastChild.firstChild.style.display = "none" : r.firstChild.style.display = "none", r.style.height = r.style.height.replace("px", "") - 25 + "px", e.style.height = e.style.height.replace("px", "") - 50 + "px", e.style.width = +t.width + 10 + "px", r.style.width = t.width + "px" } return i._lightbox } var a = { initial: 0, load_data: 1, save_data: 2 }, s = r + "_here_iframe_";
    i.config.buttons_left = [], i.config.buttons_right = [], i._getLightbox = i.getLightbox, i.config.lightbox.sections = [{ type: "frame", name: "box" }], i._cust_string_to_date = function (e) { return i.templates.xml_date(e) }, i._cust_date_to_str = function (e) { return i.templates.xml_format(e) }, i._deep_copy = function (t) {
        if ("object" == typeof t) {
            if ("[object Date]" == Object.prototype.toString.call(t)) var i = new Date(t); else if ("[object Array]" == Object.prototype.toString.call(t)) var i = new Array; else var i = new Object; for (var r in t) i[r] = e._deep_copy(t[r]);
        } else var i = t; return i
    }, "external" == t.type ? (i.attachEvent("onBeforeLightbox", function () { return i._custom_box_stage = 0, !0 }), i.getLightbox = n, i._setLightboxValues = function (e, r) {
        var e = document.getElementById(s); try {
            switch (i._custom_box_stage) {
                case a.initial: if (!i.dataProcessor) { var n = i.dataProcessor = new DataProcessor; n.init(i) } var n = i.dataProcessor || n, d = n._getRowData(r), o = -1 === (t.view || "").indexOf("?") ? "?" : "&", l = "<form action='/" + t.view + o + "id=" + encodeURIComponent(r) + "' method='POST'>"; for (var h in d) l += "<input type='hidden' name='" + h + "'/>";
                    if (l += "</form>", e.Document) var _ = e.Document.body; else var _ = e.contentDocument.body; _.innerHTML = l; var c = 0; for (var h in d) _.firstChild.childNodes[c++].value = d[h]; _.firstChild.submit(); break; case a.load_data: if (!e.contentWindow.lightbox) { e.contentWindow; e.contentWindow.lightbox = { close: function () { i._remove_customBox() } } } i.callEvent("onLightbox", [r]); break; case a.save_data: if (!e) return; var u = e.contentWindow; if (!u || !u.response_data) return; i._doLAction(u.response_data), i._remove_customBox()
            }
        } catch (g) {
            i._remove_customBox(),
            window.console && console.log(g)
        } i._custom_box_stage++
    }, i._remove_customBox = function () { i._lightbox ? i.endLightbox(!1, i._lightbox) : i.endLightbox(!1), i.callEvent("onAfterLightbox", []) }, i._doLAction = function (e) {
        try {
            if (!e) return; var t = e.data; t.start_date && t.end_date && (t.start_date = i._cust_string_to_date(e.data.start_date), t.end_date = i._cust_string_to_date(e.data.end_date)); var r = e.action; switch (e.action) {
                case "insert": i._loading = !0, i.addEvent(t), i._loading = !1, r = "inserted"; break; case "update": var n = i.getEvent(e.sid);
                    for (var a in t) n[a] = t[a]; i.event_updated(n), i.updateEvent(e.sid), r = "updated"; break; case "delete": i.deleteEvent(e.sid, !0), r = "deleted"
            }i.dataProcessor.callEvent("onAfterUpdate", [e.sid, r, e.tid, e])
        } catch (s) { }
    }, i.form_blocks.frame = {
        onload: function (e, t, i) { i._setLightboxValues(e, t) }, render: function (e) { return "<div style='display:inline-block; height:" + t.height + "px'></div>" }, set_value: function (e, n, a, d) {
        i._last_id = a.id; var o = '<iframe id="' + s + '" frameborder="0" onload="' + r + ".form_blocks.frame.onload(this, " + r + "._last_id, " + r + ');" src=""';
            return (t.width || t.height) && (o += " style='"), t.width && (o += "width:" + t.width + "px;", e.style.width = t.width + "px"), t.height && (o += "height:" + t.height + "px;", e.style.height = t.height + "px"), (t.width || t.height) && (o += " '"), o += "><html></html></iframe>", e.innerHTML = o, t.className && (e.className = t.className), !0
        }, get_value: function (e, t) { return !0 }, focus: function (e) { return !0 }
    }) : (i.form_blocks.frame = {
        render: function (e) {
            var i = '<iframe  id="' + r + "_here_iframe_\" onload='" + r + "._addLightboxInterface(this)' frameborder='0' src='" + t.view + "'";
            return (t.width || t.height) && (i += " style='"), t.width && (i += "width:" + t.width + "px;"), t.height && (i += "height:" + t.height + "px;"), (t.width || t.height) && (i += " '"), i += " ></iframe>"
        }, set_value: function (e, t, i) { if (e.contentWindow && e.contentWindow.setValues) { if (1 == e.contentWindow.document.getElementsByTagName("form").length) e.contentWindow.document.getElementsByTagName("form")[0].reset(); else { var i = e.contentWindow.getValues(); for (var r in i) i[r] = ""; e.contentWindow.setValues(i) } e.contentWindow.setValues(i) } }, get_value: function (e, t) {
            return i._deep_copy(e.contentWindow.getValues())
        }, focus: function (e) { return !0 }
    }, i.getLightbox = n, i._addLightboxInterface = function (e) {
        if (e.contentWindow.lightbox || (e.contentWindow.lightbox = {}), e.contentWindow.lightbox.save = function () { var t = i.getEvent(i.getState().lightbox_id), r = e.contentWindow.getValues(); for (var n in r) t[n] = r[n]; i.endLightbox(!0, i._lightbox), i.callEvent("onAfterLightbox", []) }, e.contentWindow.lightbox.close = function (e) { i.endLightbox(!1, i._lightbox), i.callEvent("onAfterLightbox", []) },
            e.contentWindow.lightbox.remove = function () { var e = i.locale.labels.confirm_deleting; (!e || confirm(e)) && (i.deleteEvent(i._lightbox_id), i._new_event = null), i.endLightbox(!0, i._lightbox) }, 1 == e.contentWindow.document.getElementsByTagName("form").length) e.contentWindow.document.getElementsByTagName("form")[0].reset(); else if (e.contentWindow.getValues && e.contentWindow.setValues) { var t = e.contentWindow.getValues(); for (var r in t) t[r] = ""; e.contentWindow.setValues(t) } e.contentWindow.setValues && e.contentWindow.setValues(i.getEvent(i._lightbox_id)),
                i.callEvent("onLightbox", [i._lightbox_id])
    })
}
});