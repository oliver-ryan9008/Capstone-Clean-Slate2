/*
@license
dhtmlxScheduler.Net v.3.4.1 Professional Evaluation

This software is covered by DHTMLX Evaluation License. Contact sales@dhtmlx.com to get Commercial or Enterprise license. Usage without proper license is prohibited.

(c) Dinamenta, UAB.
*/
Scheduler.plugin(function (e) {
    e.config.limit_start = null, e.config.limit_end = null, e.config.limit_view = !1, e.config.check_limits = !0, e.config.mark_now = !0, e.config.display_marked_timespans = !0, e._temp_limit_scope = function () {
        function t(t, a, n, i, r) {
            var o = e, d = [], l = { _props: "map_to", matrix: "y_property" }; for (var s in l) { var _ = l[s]; if (o[s]) for (var c in o[s]) { var u = o[s][c], h = u[_]; t[h] && (d = o._add_timespan_zones(d, e._get_blocked_zones(a[c], t[h], n, i, r))) } } return d = o._add_timespan_zones(d, e._get_blocked_zones(a, "global", n, i, r));
        } var a = null, n = "dhx_time_block", i = "default", r = function (e, t, a) { return t instanceof Date && a instanceof Date ? (e.start_date = t, e.end_date = a) : (e.days = t, e.zones = a), e }, o = function (e, t, a) { var i = "object" == typeof e ? e : { days: e }; return i.type = n, i.css = "", t && (a && (i.sections = a), i = r(i, e, t)), i }; e.blockTime = function (t, a, n) { var i = o(t, a, n); return e.addMarkedTimespan(i) }, e.unblockTime = function (t, a, n) { a = a || "fullday"; var i = o(t, a, n); return e.deleteMarkedTimespan(i) }, e.attachEvent("onBeforeViewChange", function (t, a, n, i) {
            function r(t, a) {
                var n = e.config.limit_start, i = e.config.limit_end, r = e.date.add(t, 1, a); return t.valueOf() > i.valueOf() || r <= n.valueOf()
            } return e.config.limit_view && (i = i || a, n = n || t, r(i, n) && a.valueOf() != i.valueOf()) ? (setTimeout(function () { var t = r(a, n) ? e.config.limit_start : a; e.setCurrentView(r(t, n) ? null : t, n) }, 1), !1) : !0
        }), e.checkInMarkedTimespan = function (a, n, r) {
            n = n || i; for (var o = !0, d = new Date(a.start_date.valueOf()), l = e.date.add(d, 1, "day"), s = e._marked_timespans; d < a.end_date; d = e.date.date_part(l), l = e.date.add(d, 1, "day")) {
                var _ = +e.date.date_part(new Date(d)), c = d.getDay(), u = t(a, s, c, _, n);
                if (u) for (var h = 0; h < u.length; h += 2) { var p = e._get_zone_minutes(d), v = a.end_date > l || a.end_date.getDate() != d.getDate() ? 1440 : e._get_zone_minutes(a.end_date), m = u[h], g = u[h + 1]; if (v > m && g > p && (o = "function" == typeof r ? r(a, p, v, m, g) : !1, !o)) break }
            } return !o
        }; var d = e.checkLimitViolation = function (t) {
            if (!t) return !0; if (!e.config.check_limits) return !0; var a = e, i = a.config, r = []; if (t.rec_type) for (var o = e.getRecDates(t), d = 0; d < o.length; d++) { var l = e._copy_event(t); e._lame_copy(l, o[d]), r.push(l) } else r = [t]; for (var s = !0, _ = 0; _ < r.length; _++) {
                var c = !0, l = r[_]; l._timed = e.isOneDayEvent(l), c = i.limit_start && i.limit_end ? l.start_date.valueOf() >= i.limit_start.valueOf() && l.end_date.valueOf() <= i.limit_end.valueOf() : !0, c && (c = !e.checkInMarkedTimespan(l, n, function (e, t, n, i, r) {
                    var o = !0; return r >= t && t >= i && ((1440 == r || r > n) && (o = !1), e._timed && a._drag_id && "new-size" == a._drag_mode ? (e.start_date.setHours(0), e.start_date.setMinutes(r)) : o = !1), (n >= i && r > n || i > t && n > r) && (e._timed && a._drag_id && "new-size" == a._drag_mode ? (e.end_date.setHours(0), e.end_date.setMinutes(i)) : o = !1),
                        o
                })), c || (c = a.checkEvent("onLimitViolation") ? a.callEvent("onLimitViolation", [l.id, l]) : c), s = s && c
            } return s || (a._drag_id = null, a._drag_mode = null), s
        }; e._get_blocked_zones = function (e, t, a, n, i) { var r = []; if (e && e[t]) for (var o = e[t], d = this._get_relevant_blocked_zones(a, n, o, i), l = 0; l < d.length; l++)r = this._add_timespan_zones(r, d[l].zones); return r }, e._get_relevant_blocked_zones = function (e, t, a, n) { var i = a[t] && a[t][n] ? a[t][n] : a[e] && a[e][n] ? a[e][n] : []; return i }, e.attachEvent("onMouseDown", function (e) { return !(e == n) }),
            e.attachEvent("onBeforeDrag", function (t) { return t ? d(e.getEvent(t)) : !0 }), e.attachEvent("onClick", function (t, a) { return d(e.getEvent(t)) }), e.attachEvent("onBeforeLightbox", function (t) { var n = e.getEvent(t); return a = [n.start_date, n.end_date], d(n) }), e.attachEvent("onEventSave", function (t, a, n) { if (!a.start_date || !a.end_date) { var i = e.getEvent(t); a.start_date = new Date(i.start_date), a.end_date = new Date(i.end_date) } if (a.rec_type) { var r = e._lame_clone(a); return e._roll_back_dates(r), d(r) } return d(a) }), e.attachEvent("onEventAdded", function (t) {
                if (!t) return !0; var a = e.getEvent(t); return !d(a) && e.config.limit_start && e.config.limit_end && (a.start_date < e.config.limit_start && (a.start_date = new Date(e.config.limit_start)), a.start_date.valueOf() >= e.config.limit_end.valueOf() && (a.start_date = this.date.add(e.config.limit_end, -1, "day")), a.end_date < e.config.limit_start && (a.end_date = new Date(e.config.limit_start)), a.end_date.valueOf() >= e.config.limit_end.valueOf() && (a.end_date = this.date.add(e.config.limit_end, -1, "day")), a.start_date.valueOf() >= a.end_date.valueOf() && (a.end_date = this.date.add(a.start_date, this.config.event_duration || this.config.time_step, "minute")),
                    a._timed = this.isOneDayEvent(a)), !0
            }), e.attachEvent("onEventChanged", function (t) { if (!t) return !0; var n = e.getEvent(t); if (!d(n)) { if (!a) return !1; n.start_date = a[0], n.end_date = a[1], n._timed = this.isOneDayEvent(n) } return !0 }), e.attachEvent("onBeforeEventChanged", function (e, t, a) { return d(e) }), e.attachEvent("onBeforeEventCreated", function (t) { var a = e.getActionData(t).date, n = { _timed: !0, start_date: a, end_date: e.date.add(a, e.config.time_step, "minute") }; return d(n) }), e.attachEvent("onViewChange", function () {
                e._mark_now();
            }), e.attachEvent("onSchedulerResize", function () { return window.setTimeout(function () { e._mark_now() }, 1), !0 }), e.attachEvent("onTemplatesReady", function () { e._mark_now_timer = window.setInterval(function () { e._is_initialized() && e._mark_now() }, 6e4) }), e._mark_now = function (t) {
                var a = "dhx_now_time"; this._els[a] || (this._els[a] = []); var n = e._currentDate(), i = this.config; if (e._remove_mark_now(), !t && i.mark_now && n < this._max_date && n > this._min_date && n.getHours() >= i.first_hour && n.getHours() < i.last_hour) {
                    var r = this.locate_holder_day(n);
                    this._els[a] = e._append_mark_now(r, n)
                }
            }, e._append_mark_now = function (t, a) {
                var n = "dhx_now_time", i = e._get_zone_minutes(a), r = { zones: [i, i + 1], css: n, type: n }; if (!this._table_view) { if (this._props && this._props[this._mode]) { var o, d, l = this._props[this._mode], s = l.size || l.options.length; l.days > 1 ? (o = t, d = t + s) : (o = 0, d = o + s); for (var _ = [], c = o; d > c; c++) { var u = c; r.days = u; var h = e._render_marked_timespan(r, null, u)[0]; _.push(h) } return _ } return r.days = t, e._render_marked_timespan(r, null, t) } return "month" == this._mode ? (r.days = +e.date.date_part(a),
                    e._render_marked_timespan(r, null, null)) : void 0
            }, e._remove_mark_now = function () { for (var e = "dhx_now_time", t = this._els[e], a = 0; a < t.length; a++) { var n = t[a], i = n.parentNode; i && i.removeChild(n) } this._els[e] = [] }, e._marked_timespans = { global: {} }, e._get_zone_minutes = function (e) { return 60 * e.getHours() + e.getMinutes() }, e._prepare_timespan_options = function (t) {
                var a = [], n = []; if ("fullweek" == t.days && (t.days = [0, 1, 2, 3, 4, 5, 6]), t.days instanceof Array) {
                    for (var r = t.days.slice(), o = 0; o < r.length; o++) {
                        var d = e._lame_clone(t); d.days = r[o],
                            a.push.apply(a, e._prepare_timespan_options(d))
                    } return a
                } if (!t || !(t.start_date && t.end_date && t.end_date > t.start_date || void 0 !== t.days && t.zones) && !t.type) return a; var l = 0, s = 1440; "fullday" == t.zones && (t.zones = [l, s]), t.zones && t.invert_zones && (t.zones = e.invertZones(t.zones)), t.id = e.uid(), t.css = t.css || "", t.type = t.type || i; var _ = t.sections; if (_) {
                    for (var c in _) if (_.hasOwnProperty(c)) {
                        var u = _[c]; u instanceof Array || (u = [u]); for (var o = 0; o < u.length; o++) {
                            var h = e._lame_copy({}, t); h.sections = {}, h.sections[c] = u[o],
                                n.push(h)
                        }
                    }
                } else n.push(t); for (var p = 0; p < n.length; p++) {
                    var v = n[p], m = v.start_date, g = v.end_date; if (m && g) for (var f = e.date.date_part(new Date(m)), b = e.date.add(f, 1, "day"); g > f;) { var h = e._lame_copy({}, v); delete h.start_date, delete h.end_date, h.days = f.valueOf(); var y = m > f ? e._get_zone_minutes(m) : l, x = g > b || g.getDate() != f.getDate() ? s : e._get_zone_minutes(g); h.zones = [y, x], a.push(h), f = b, b = e.date.add(b, 1, "day") } else v.days instanceof Date && (v.days = e.date.date_part(v.days).valueOf()), v.zones = t.zones.slice(), a.push(v);
                } return a
            }, e._get_dates_by_index = function (t, a, n) { var i = []; a = e.date.date_part(new Date(a || e._min_date)), n = new Date(n || e._max_date); for (var r = a.getDay(), o = t - r >= 0 ? t - r : 7 - a.getDay() + t, d = e.date.add(a, o, "day"); n > d; d = e.date.add(d, 1, "week"))i.push(d); return i }, e._get_css_classes_by_config = function (e) { var t = []; return e.type == n && (t.push(n), e.css && t.push(n + "_reset")), t.push("dhx_marked_timespan", e.css), t.join(" ") }, e._get_block_by_config = function (e) {
                var t = document.createElement("DIV"); return e.html && ("string" == typeof e.html ? t.innerHTML = e.html : t.appendChild(e.html)),
                    t
            }, e._render_marked_timespan = function (t, a, n) {
                var i = [], r = e.config, o = this._min_date, d = this._max_date, l = !1; if (!r.display_marked_timespans) return i; if (!n && 0 !== n) { if (t.days < 7) n = t.days; else { var s = new Date(t.days); if (l = +s, !(+d > +s && +s >= +o)) return i; n = s.getDay() } var _ = o.getDay(); _ > n ? n = 7 - (_ - n) : n -= _ } var c = t.zones, u = e._get_css_classes_by_config(t); if (e._table_view && "month" == e._mode) {
                    var h = [], p = []; if (a) h.push(a), p.push(n); else {
                        p = l ? [l] : e._get_dates_by_index(n); for (var v = 0; v < p.length; v++)h.push(this._scales[p[v]]);
                    } for (var v = 0; v < h.length; v++) {
                    a = h[v], n = p[v]; var m = Math.floor((this._correct_shift(n, 1) - o.valueOf()) / (864e5 * this._cols.length)), g = this.locate_holder_day(n, !1) % this._cols.length; if (!this._ignores[g]) {
                        var f = e._get_block_by_config(t), b = Math.max(a.offsetHeight - 1, 0), y = Math.max(a.offsetWidth - 1, 0), x = this._colsS[g], k = this._colsS.heights[m] + (this._colsS.height ? this.xy.month_scale_height + 2 : 2) - 1; f.className = u, f.style.top = k + "px", f.style.lineHeight = f.style.height = b + "px"; for (var w = 0; w < c.length; w += 2) {
                            var D = c[v], S = c[v + 1];
                            if (D >= S) return []; var N = f.cloneNode(!0); N.style.left = x + Math.round(D / 1440 * y) + "px", N.style.width = Math.round((S - D) / 1440 * y) + "px", a.appendChild(N), i.push(N)
                        }
                    }
                    }
                } else {
                    var E = n; if (this._ignores[this.locate_holder_day(n, !1)]) return i; if (this._props && this._props[this._mode] && t.sections && t.sections[this._mode]) { var M = this._props[this._mode]; E = M.order[t.sections[this._mode]]; var A = M.order[t.sections[this._mode]]; if (M.days > 1) { var O = M.size || M.options.length; E = E * O + A } else E = A, M.size && E > M.position + M.size && (E = 0) } a = a ? a : e.locate_holder(E);
                    for (var v = 0; v < c.length; v += 2) { var D = Math.max(c[v], 60 * r.first_hour), S = Math.min(c[v + 1], 60 * r.last_hour); if (D >= S) { if (v + 2 < c.length) continue; return [] } var N = e._get_block_by_config(t); N.className = u; var C = 24 * this.config.hour_size_px + 1, T = 36e5; N.style.top = Math.round((60 * D * 1e3 - this.config.first_hour * T) * this.config.hour_size_px / T) % C + "px", N.style.lineHeight = N.style.height = Math.max(Math.round(60 * (S - D) * 1e3 * this.config.hour_size_px / T) % C, 1) + "px", a.appendChild(N), i.push(N) }
                } return i
            }, e._mark_timespans = function () {
                var t = this._els.dhx_cal_data[0], a = [];
                if (e._table_view && "month" == e._mode) for (var n in this._scales) { var i = new Date(+n); a.push.apply(a, e._on_scale_add_marker(this._scales[n], i)) } else for (var i = new Date(e._min_date), r = 0, o = t.childNodes.length; o > r; r++) { var d = t.childNodes[r]; d.firstChild && e._getClassName(d.firstChild).indexOf("dhx_scale_hour") > -1 || (a.push.apply(a, e._on_scale_add_marker(d, i)), i = e.date.add(i, 1, "day")) } return a
            }, e.markTimespan = function (t) {
                var a = !1; this._els.dhx_cal_data || (e.get_elements(), a = !0); var n = e._marked_timespans_ids, i = e._marked_timespans_types, r = e._marked_timespans;
                e.deleteMarkedTimespan(), e.addMarkedTimespan(t); var o = e._mark_timespans(); return a && (e._els = []), e._marked_timespans_ids = n, e._marked_timespans_types = i, e._marked_timespans = r, o
            }, e.unmarkTimespan = function (e) { if (e) for (var t = 0; t < e.length; t++) { var a = e[t]; a.parentNode && a.parentNode.removeChild(a) } }, e._addMarkerTimespanConfig = function (t) {
                var a = "global", n = e._marked_timespans, i = t.id, r = e._marked_timespans_ids; r[i] || (r[i] = []); var o = t.days, d = t.sections, l = t.type; if (t.id = i, d) {
                    for (var s in d) if (d.hasOwnProperty(s)) {
                        n[s] || (n[s] = {}); var _ = d[s], c = n[s]; c[_] || (c[_] = {}), c[_][o] || (c[_][o] = {}), c[_][o][l] || (c[_][o][l] = [], e._marked_timespans_types || (e._marked_timespans_types = {}), e._marked_timespans_types[l] || (e._marked_timespans_types[l] = !0)); var u = c[_][o][l]; t._array = u, u.push(t), r[i].push(t)
                    }
                } else {
                n[a][o] || (n[a][o] = {}), n[a][o][l] || (n[a][o][l] = []), e._marked_timespans_types || (e._marked_timespans_types = {}), e._marked_timespans_types[l] || (e._marked_timespans_types[l] = !0); var u = n[a][o][l]; t._array = u, u.push(t), r[i].push(t);
                }
            }, e._marked_timespans_ids = {}, e.addMarkedTimespan = function (t) { var a = e._prepare_timespan_options(t); if (a.length) { for (var n = a[0].id, i = 0; i < a.length; i++)e._addMarkerTimespanConfig(a[i]); return n } }, e._add_timespan_zones = function (e, t) {
                var a = e.slice(); if (t = t.slice(), !a.length) return t; for (var n = 0; n < a.length; n += 2)for (var i = a[n], r = a[n + 1], o = n + 2 == a.length, d = 0; d < t.length; d += 2) {
                    var l = t[d], s = t[d + 1]; if (s > r && r >= l || i > l && s >= i) a[n] = Math.min(i, l), a[n + 1] = Math.max(r, s), n -= 2; else {
                        if (!o) continue; var _ = i > l ? 0 : 2; a.splice(n + _, 0, l, s);
                    } t.splice(d--, 2); break
                } return a
            }, e._subtract_timespan_zones = function (e, t) { for (var a = e.slice(), n = 0; n < a.length; n += 2)for (var i = a[n], r = a[n + 1], o = 0; o < t.length; o += 2) { var d = t[o], l = t[o + 1]; if (l > i && r > d) { var s = !1; i >= d && l >= r && a.splice(n, 2), d > i && (a.splice(n, 2, i, d), s = !0), r > l && a.splice(s ? n + 2 : n, s ? 0 : 2, l, r), n -= 2; break } } return a }, e.invertZones = function (t) { return e._subtract_timespan_zones([0, 1440], t.slice()) }, e._delete_marked_timespan_by_id = function (t) {
                var a = e._marked_timespans_ids[t]; if (a) for (var n = 0; n < a.length; n++)for (var i = a[n], r = i._array, o = 0; o < r.length; o++)if (r[o] == i) {
                    r.splice(o, 1); break
                }
            }, e._delete_marked_timespan_by_config = function (t) { var a, n = e._marked_timespans, r = t.sections, o = t.days, d = t.type || i; if (r) { for (var l in r) if (r.hasOwnProperty(l) && n[l]) { var s = r[l]; n[l][s] && (a = n[l][s]) } } else a = n.global; if (a) if (void 0 !== o) a[o] && a[o][d] && (e._addMarkerTimespanConfig(t), e._delete_marked_timespans_list(a[o][d], t)); else for (var _ in a) if (a[_][d]) { var c = e._lame_clone(t); t.days = _, e._addMarkerTimespanConfig(c), e._delete_marked_timespans_list(a[_][d], t) } }, e._delete_marked_timespans_list = function (t, a) {
                for (var n = 0; n < t.length; n++) { var i = t[n], r = e._subtract_timespan_zones(i.zones, a.zones); if (r.length) i.zones = r; else { t.splice(n, 1), n--; for (var o = e._marked_timespans_ids[i.id], d = 0; d < o.length; d++)if (o[d] == i) { o.splice(d, 1); break } } }
            }, e.deleteMarkedTimespan = function (t) {
                if (arguments.length || (e._marked_timespans = { global: {} }, e._marked_timespans_ids = {}, e._marked_timespans_types = {}), "object" != typeof t) e._delete_marked_timespan_by_id(t); else {
                t.start_date && t.end_date || (void 0 !== t.days || t.type || (t.days = "fullweek"),
                    t.zones || (t.zones = "fullday")); var a = []; if (t.type) a.push(t.type); else for (var n in e._marked_timespans_types) a.push(n); for (var i = e._prepare_timespan_options(t), r = 0; r < i.length; r++)for (var o = i[r], d = 0; d < a.length; d++) { var l = e._lame_clone(o); l.type = a[d], e._delete_marked_timespan_by_config(l) }
                }
            }, e._get_types_to_render = function (t, a) { var n = t ? e._lame_copy({}, t) : {}; for (var i in a || {}) a.hasOwnProperty(i) && (n[i] = a[i]); return n }, e._get_configs_to_render = function (e) {
                var t = []; for (var a in e) e.hasOwnProperty(a) && t.push.apply(t, e[a]);
                return t
            }, e._on_scale_add_marker = function (t, a) {
                if (!e._table_view || "month" == e._mode) {
                    var n = a.getDay(), i = a.valueOf(), r = this._mode, o = e._marked_timespans, d = [], l = []; if (this._props && this._props[r]) {
                        var s = this._props[r], _ = s.options, c = e._get_unit_index(s, a), u = _[c]; if (s.days > 1) { var h = 864e5, p = Math.round((a - e._min_date) / h); a = e.date.add(e._min_date, Math.floor(p / _.length), "day"), a = e.date.date_part(a) } else a = e.date.date_part(new Date(this._date)); if (n = a.getDay(), i = a.valueOf(), o[r] && o[r][u.key]) {
                            var v = o[r][u.key], m = e._get_types_to_render(v[n], v[i]);
                            d.push.apply(d, e._get_configs_to_render(m))
                        }
                    } var g = o.global, f = g[i] || g[n]; d.push.apply(d, e._get_configs_to_render(f)); for (var b = 0; b < d.length; b++)l.push.apply(l, e._render_marked_timespan(d[b], t, a)); return l
                }
            }, e.attachEvent("onScaleAdd", function () { e._on_scale_add_marker.apply(e, arguments) }), e.dblclick_dhx_marked_timespan = function (t, a) { e.callEvent("onScaleDblClick", [e.getActionData(t).date, a, t]), e.config.dblclick_create && e.addEventNow(e.getActionData(t).date, null, t) }
    }, e._temp_limit_scope()
});