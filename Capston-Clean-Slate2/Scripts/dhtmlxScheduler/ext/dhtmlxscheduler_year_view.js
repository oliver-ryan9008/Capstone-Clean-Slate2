/*
@license
dhtmlxScheduler.Net v.3.4.1 Professional Evaluation

This software is covered by DHTMLX Evaluation License. Contact sales@dhtmlx.com to get Commercial or Enterprise license. Usage without proper license is prohibited.

(c) Dinamenta, UAB.
*/
Scheduler.plugin(function (e) {
    e.config.year_x = 4, e.config.year_y = 3, e.xy.year_top = 0, e.templates.year_date = function (t) { return e.date.date_to_str(e.locale.labels.year_tab + " %Y")(t) }, e.templates.year_month = e.date.date_to_str("%F"), e.templates.year_scale_date = e.date.date_to_str("%D"), e.templates.year_tooltip = function (e, t, a) { return a.text }, function () {
        var t = function () { return "year" == e._mode }; e.dblclick_dhx_month_head = function (a) {
            if (t()) {
                var n = a.target || a.srcElement, i = e._getClassName(n.parentNode); if (-1 != i.indexOf("dhx_before") || -1 != i.indexOf("dhx_after")) return !1;
                for (var r = n; r && (!r.hasAttribute || !r.hasAttribute("date"));)r = r.parentNode; if (r) { var o = this.templates.xml_date(r.getAttribute("date")); o.setDate(parseInt(n.innerHTML, 10)); var d = this.date.add(o, 1, "day"); !this.config.readonly && this.config.dblclick_create && this.addEventNow(o.valueOf(), d.valueOf(), a) }
            }
        }; var a = e.changeEventId; e.changeEventId = function () { a.apply(this, arguments), t() && this.year_view(!0) }; var n = e.render_data, i = e.date.date_to_str("%Y/%m/%d"), r = e.date.str_to_date("%Y/%m/%d"); e.render_data = function (e) {
            if (!t()) return n.apply(this, arguments); for (var a = 0; a < e.length; a++)this._year_render_event(e[a])
        }; var o = e.clear_view; e.clear_view = function () { if (!t()) return o.apply(this, arguments); var a = e._year_marked_cells, n = null; for (var i in a) a.hasOwnProperty(i) && (n = a[i], n.className = "dhx_month_head", n.setAttribute("date", "")); e._year_marked_cells = {} }, e._hideToolTip = function () { this._tooltip && (this._tooltip.style.display = "none", this._tooltip.date = new Date(9999, 1, 1)) }, e._showToolTip = function (t, a, n, i) {
            if (this._tooltip) {
                if (this._tooltip.date.valueOf() == t.valueOf()) return; this._tooltip.innerHTML = ""
            } else { var r = this._tooltip = document.createElement("DIV"); r.className = "dhx_year_tooltip", document.body.appendChild(r), r.onclick = e._click.dhx_cal_data } for (var o = this.getEvents(t, this.date.add(t, 1, "day")), d = "", l = 0; l < o.length; l++) {
                var s = o[l]; if (this.filter_event(s.id, s)) {
                    var _ = s.color ? "background:" + s.color + ";" : "", c = s.textColor ? "color:" + s.textColor + ";" : ""; d += "<div class='dhx_tooltip_line' style='" + _ + c + "' event_id='" + o[l].id + "'>",
                        d += "<div class='dhx_tooltip_date' style='" + _ + c + "'>" + (o[l]._timed ? this.templates.event_date(o[l].start_date) : "") + "</div>", d += "<div class='dhx_event_icon icon_details'>&nbsp;</div>", d += this.templates.year_tooltip(o[l].start_date, o[l].end_date, o[l]) + "</div>"
                }
            } this._tooltip.style.display = "", this._tooltip.style.top = "0px", document.body.offsetWidth - a.left - this._tooltip.offsetWidth < 0 ? this._tooltip.style.left = a.left - this._tooltip.offsetWidth + "px" : this._tooltip.style.left = a.left + i.offsetWidth + "px", this._tooltip.date = t,
                this._tooltip.innerHTML = d, document.body.offsetHeight - a.top - this._tooltip.offsetHeight < 0 ? this._tooltip.style.top = a.top - this._tooltip.offsetHeight + i.offsetHeight + "px" : this._tooltip.style.top = a.top + "px"
        }, e._year_view_tooltip_handler = function (a) { if (t()) { var a = a || event, n = a.target || a.srcElement; "a" == n.tagName.toLowerCase() && (n = n.parentNode), -1 != e._getClassName(n).indexOf("dhx_year_event") ? e._showToolTip(r(n.getAttribute("date")), getOffset(n), a, n) : e._hideToolTip() } }, e._init_year_tooltip = function () {
            e._detachDomEvent(e._els.dhx_cal_data[0], "mouseover", e._year_view_tooltip_handler),
            dhtmlxEvent(e._els.dhx_cal_data[0], "mouseover", e._year_view_tooltip_handler)
        }, e.attachEvent("onSchedulerResize", function () { return t() ? (this.year_view(!0), !1) : !0 }), e._get_year_cell = function (e) { var t = e.getMonth() + 12 * (e.getFullYear() - this._min_date.getFullYear()) - this.week_starts._month, a = this._els.dhx_cal_data[0].childNodes[t], e = this.week_starts[t] + e.getDate() - 1; return a.querySelector(".dhx_year_body").firstChild.rows[Math.floor(e / 7)].cells[e % 7].firstChild }, e._year_marked_cells = {}, e._mark_year_date = function (t, a) {
            var n = i(t), r = this._get_year_cell(t), o = this.templates.event_class(a.start_date, a.end_date, a); e._year_marked_cells[n] || (r.className = "dhx_month_head dhx_year_event", r.setAttribute("date", n), e._year_marked_cells[n] = r), r.className += o ? " " + o : ""
        }, e._unmark_year_date = function (e) { this._get_year_cell(e).className = "dhx_month_head" }, e._year_render_event = function (e) {
            var t = e.start_date; for (t = t.valueOf() < this._min_date.valueOf() ? this._min_date : this.date.date_part(new Date(t)); t < e.end_date;)if (this._mark_year_date(t, e),
                t = this.date.add(t, 1, "day"), t.valueOf() >= this._max_date.valueOf()) return
        }, e.year_view = function (t) { var a; if (t && (a = e.xy.scale_height, e.xy.scale_height = -1), e._els.dhx_cal_header[0].style.display = t ? "none" : "", e.set_sizes(), t && (e.xy.scale_height = a), e._table_view = t, !this._load_mode || !this._load()) if (t) { if (e._init_year_tooltip(), e._reset_year_scale(), e._load_mode && e._load()) return void (e._render_wait = !0); e.render_view_data() } else e._hideToolTip() }, e._reset_year_scale = function () {
        this._cols = [], this._colsS = {};
            var t = [], a = this._els.dhx_cal_data[0], n = this.config; a.scrollTop = 0, a.innerHTML = ""; var i = Math.floor(parseInt(a.style.width) / n.year_x), r = Math.floor((parseInt(a.style.height) - e.xy.year_top) / n.year_y); 190 > r && (r = 190, i = Math.floor((parseInt(a.style.width) - e.xy.scroll_width) / n.year_x)); var o = i - 11, d = 0, l = document.createElement("div"), s = this.date.week_start(e._currentDate()); this._process_ignores(s, 7, "day", 1); for (var _ = 7 - (this._ignores_detected || 0), c = 0, u = 0; 7 > u; u++)this._ignores && this._ignores[u] || (this._cols[u] = Math.floor(o / (_ - c)),
                this._render_x_header(u, d, s, l), o -= this._cols[u], d += this._cols[u], c++), s = this.date.add(s, 1, "day"); l.lastChild.className += " dhx_scale_bar_last"; for (var u = 0; u < l.childNodes.length; u++)this._waiAria.yearHeadCell(l.childNodes[u]); for (var h = this.date[this._mode + "_start"](this.date.copy(this._date)), p = h, v = null, u = 0; u < n.year_y; u++)for (var m = 0; m < n.year_x; m++) {
                v = document.createElement("DIV"), v.style.cssText = "position:absolute;", v.setAttribute("date", this.templates.xml_format(h)), v.innerHTML = "<div class='dhx_year_month'></div><div class='dhx_year_grid'><div class='dhx_year_week'>" + l.innerHTML + "</div><div class='dhx_year_body'></div></div>";
                    var g = v.querySelector(".dhx_year_month"), f = v.querySelector(".dhx_year_grid"), y = v.querySelector(".dhx_year_week"), b = v.querySelector(".dhx_year_body"), x = e.uid(); this._waiAria.yearHeader(g, x), this._waiAria.yearGrid(f, x), g.innerHTML = this.templates.year_month(h); for (var k = this.date.week_start(h), w = (this._reset_month_scale(b, h, k, 6), b.querySelectorAll("td")), D = 0; D < w.length; D++)this._waiAria.yearDayCell(w[D]); a.appendChild(v), y.style.height = y.childNodes[0].offsetHeight + "px"; var E = Math.round((r - 190) / 2); v.style.marginTop = E + "px",
                        this.set_xy(v, i - 10, r - E - 10, i * m + 5, r * u + 5 + e.xy.year_top), t[u * n.year_x + m] = (h.getDay() - (this.config.start_on_monday ? 1 : 0) + 7) % 7, h = this.date.add(h, 1, "month")
                } this._els.dhx_cal_date[0].innerHTML = this.templates[this._mode + "_date"](p, h, this._mode), this.week_starts = t, t._month = p.getMonth(), this._min_date = p, this._max_date = h
        }; var d = e.getActionData; e.getActionData = function (a) {
            if (!t()) return d.apply(e, arguments); var n = a ? a.target : event.srcElement, i = e._get_year_month_date(n), r = e._get_year_month_cell(n), o = e._get_year_day_indexes(r);
            return o && i ? (i = e.date.add(i, o.week, "week"), i = e.date.add(i, o.day, "day")) : i = null, { date: i, section: null }
        }, e._get_year_day_indexes = function (t) { var a = e._get_year_el_node(t, this._locate_year_month_table); if (!a) return null; for (var n = 0, i = 0, n = 0, r = a.rows.length; r > n; n++) { for (var o = a.rows[n].getElementsByTagName("td"), i = 0, d = o.length; d > i && o[i] != t; i++); if (d > i) break } return r > n ? { day: i, week: n } : null }, e._get_year_month_date = function (t) {
            var t = e._get_year_el_node(t, e._locate_year_month_root); if (!t) return null; var a = t.getAttribute("date");
            return a ? e.date.week_start(e.date.month_start(r(a))) : null
        }, e._locate_year_month_day = function (t) { return -1 != e._getClassName(t).indexOf("dhx_year_event") && t.hasAttribute && t.hasAttribute("date") }; var l = e._locate_event; e._locate_event = function (t) { var a = l.apply(e, arguments); if (!a) { var n = e._get_year_el_node(t, e._locate_year_month_day); if (!n || !n.hasAttribute("date")) return null; var i = r(n.getAttribute("date")), o = e.getEvents(i, e.date.add(i, 1, "day")); if (!o.length) return null; a = o[0].id } return a }, e._locate_year_month_cell = function (e) {
            return "td" == e.nodeName.toLowerCase()
        }, e._locate_year_month_table = function (e) { return "table" == e.nodeName.toLowerCase() }, e._locate_year_month_root = function (e) { return e.hasAttribute && e.hasAttribute("date") }, e._get_year_month_cell = function (e) { return this._get_year_el_node(e, this._locate_year_month_cell) }, e._get_year_month_table = function (e) { return this._get_year_el_node(e, this._locate_year_month_table) }, e._get_year_month_root = function (e) {
            return this._get_year_el_node(this._get_year_month_table(e), this._locate_year_month_root);
        }, e._get_year_el_node = function (e, t) { for (; e && !t(e);)e = e.parentNode; return e }
    }()
});