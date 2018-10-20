/*
@license
dhtmlxScheduler.Net v.3.4.1 Professional Evaluation

This software is covered by DHTMLX Evaluation License. Contact sales@dhtmlx.com to get Commercial or Enterprise license. Usage without proper license is prohibited.

(c) Dinamenta, UAB.
*/
Scheduler.plugin(function (e) {
    !function () {
        e.config.container_autoresize = !0, e.config.month_day_min_height = 90, e.config.min_grid_size = 25, e.config.min_map_size = 400; var t = e._pre_render_events, i = !0, a = 0, n = 0; e._pre_render_events = function (r, s) {
            if (!e.config.container_autoresize || !i) return t.apply(this, arguments); var d = this.xy.bar_height, o = this._colsS.heights, l = this._colsS.heights = [0, 0, 0, 0, 0, 0, 0], h = this._els.dhx_cal_data[0]; if (r = this._table_view ? this._pre_render_events_table(r, s) : this._pre_render_events_line(r, s),
                this._table_view) if (s) this._colsS.heights = o; else {
                    var _ = h.firstChild; if (_.rows) {
                        for (var c = 0; c < _.rows.length; c++) {
                            if (l[c]++ , l[c] * d > this._colsS.height - this.xy.month_head_height) {
                                var u = _.rows[c].cells, g = this._colsS.height - this.xy.month_head_height; 1 * this.config.max_month_events !== this.config.max_month_events || l[c] <= this.config.max_month_events ? g = l[c] * d : (this.config.max_month_events + 1) * d > this._colsS.height - this.xy.month_head_height && (g = (this.config.max_month_events + 1) * d); for (var f = 0; f < u.length; f++)u[f].childNodes[1].style.height = g + "px";
                                l[c] = (l[c - 1] || 0) + u[0].offsetHeight
                            } l[c] = (l[c - 1] || 0) + _.rows[c].cells[0].offsetHeight
                        } l.unshift(0), _.parentNode.offsetHeight < _.parentNode.scrollHeight && !_._h_fix
                    } else if (r.length || "visible" != this._els.dhx_multi_day[0].style.visibility || (l[0] = -1), r.length || -1 == l[0]) {
                        var v = (_.parentNode.childNodes, (l[0] + 1) * d + 1); n != v + 1 && (this._obj.style.height = a - n + v - 1 + "px"), v += "px", h.style.top = this._els.dhx_cal_navline[0].offsetHeight + this._els.dhx_cal_header[0].offsetHeight + parseInt(v, 10) + "px", h.style.height = this._obj.offsetHeight - parseInt(h.style.top, 10) - (this.xy.margin_top || 0) + "px";
                        var m = this._els.dhx_multi_day[0]; m.style.height = v, m.style.visibility = -1 == l[0] ? "hidden" : "visible", m = this._els.dhx_multi_day[1], m.style.height = v, m.style.visibility = -1 == l[0] ? "hidden" : "visible", m.className = l[0] ? "dhx_multi_day_icon" : "dhx_multi_day_icon_small", this._dy_shift = (l[0] + 1) * d, l[0] = 0
                    }
                } return r
        }; var r = ["dhx_cal_navline", "dhx_cal_header", "dhx_multi_day", "dhx_cal_data"], s = function (t) {
            a = 0; for (var i = 0; i < r.length; i++) {
                var s = r[i], d = e._els[s] ? e._els[s][0] : null, o = 0; switch (s) {
                    case "dhx_cal_navline": case "dhx_cal_header":
                        o = parseInt(d.style.height, 10); break; case "dhx_multi_day": o = d ? d.offsetHeight - 1 : 0, n = o; break; case "dhx_cal_data": var l = e.getState().mode; if (o = d.childNodes[1] && "month" != l ? d.childNodes[1].offsetHeight : Math.max(d.offsetHeight - 1, d.scrollHeight), "month" == l) { if (e.config.month_day_min_height && !t) { var h = d.getElementsByTagName("tr").length; o = h * e.config.month_day_min_height } t && (d.style.height = o + "px") } else if ("year" == l) o = 190 * e.config.year_y; else if ("agenda" == l) {
                            if (o = 0, d.childNodes && d.childNodes.length) for (var _ = 0; _ < d.childNodes.length; _++)o += d.childNodes[_].offsetHeight;
                            o + 2 < e.config.min_grid_size ? o = e.config.min_grid_size : o += 2
                        } else if ("week_agenda" == l) { for (var c, u, g = e.xy.week_agenda_scale_height + e.config.min_grid_size, f = 0; f < d.childNodes.length; f++) { u = d.childNodes[f]; for (var _ = 0; _ < u.childNodes.length; _++) { for (var v = 0, m = u.childNodes[_].childNodes[1], p = 0; p < m.childNodes.length; p++)v += m.childNodes[p].offsetHeight; c = v + e.xy.week_agenda_scale_height, c = 1 != f || 2 != _ && 3 != _ ? c : 2 * c, c > g && (g = c) } } o = 3 * g } else if ("map" == l) {
                            o = 0; for (var b = d.querySelectorAll(".dhx_map_line"), _ = 0; _ < b.length; _++)o += b[_].offsetHeight;
                            o + 2 < e.config.min_map_size ? o = e.config.min_map_size : o += 2
                        } else if (e._gridView) if (o = 0, d.childNodes[1].childNodes[0].childNodes && d.childNodes[1].childNodes[0].childNodes.length) { for (var b = d.childNodes[1].childNodes[0].childNodes[0].childNodes, _ = 0; _ < b.length; _++)o += b[_].offsetHeight; o += 2, o < e.config.min_grid_size && (o = e.config.min_grid_size) } else o = e.config.min_grid_size; if (e.matrix && e.matrix[l]) if (t) o += 2, d.style.height = o + "px"; else {
                            o = 2; for (var x = e.matrix[l], y = x.y_unit, w = 0; w < y.length; w++)o += y[w].children ? x.folder_dy || x.dy : x.dy;
                        } ("day" == l || "week" == l || e._props && e._props[l]) && (o += 2)
                }a += o
            } e._obj.style.height = a + "px", t || e.updateView()
        }, d = function () { if (!e.config.container_autoresize || !i) return !0; var t = e.getState().mode; s(), (e.matrix && e.matrix[t] || "month" == t) && window.setTimeout(function () { s(!0) }, 1) }; e.attachEvent("onViewChange", d), e.attachEvent("onXLE", d), e.attachEvent("onEventChanged", d), e.attachEvent("onEventCreated", d), e.attachEvent("onEventAdded", d), e.attachEvent("onEventDeleted", d), e.attachEvent("onAfterSchedulerResize", d),
            e.attachEvent("onClearAll", d), e.attachEvent("onBeforeExpand", function () { return i = !1, !0 }), e.attachEvent("onBeforeCollapse", function () { return i = !0, !0 })
    }()
});