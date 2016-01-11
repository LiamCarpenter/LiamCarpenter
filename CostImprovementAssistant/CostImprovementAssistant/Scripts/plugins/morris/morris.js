(function() {
    var $,
        morris,
        minutesSpecHelper,
        secondsSpecHelper,
        slice = [].slice,
        bind = function(fn, me) { return function() { return fn.apply(me, arguments); }; },
        hasProp = {}.hasOwnProperty,
        __extends = function(child, parent) {
            for (var key in parent) {
                if (hasProp.call(parent, key)) child[key] = parent[key];
            }

            function ctor() { this.constructor = child; }

            ctor.prototype = parent.prototype;
            child.prototype = new ctor();
            child.__super__ = parent.prototype;
            return child;
        },
        indexOf = [].indexOf || function(item) {
            for (var i = 0, l = this.length; i < l; i++) {
                if (i in this && this[i] === item) return i;
            }
            return -1;
        };

    morris = window.Morris = {};

    $ = jQuery;

    morris.EventEmitter = (function() {
        function eventEmitter() {}

        eventEmitter.prototype.on = function(name, handler) {
            if (this.handlers == null) {
                this.handlers = {};
            }
            if (this.handlers[name] == null) {
                this.handlers[name] = [];
            }
            this.handlers[name].push(handler);
            return this;
        };

        eventEmitter.prototype.fire = function() {
            var args, handler, name, i, len, ref, results;
            name = arguments[0], args = 2 <= arguments.length ? slice.call(arguments, 1) : [];
            if ((this.handlers != null) && (this.handlers[name] != null)) {
                ref = this.handlers[name];
                results = [];
                for (i = 0, len = ref.length; i < len; i++) {
                    handler = ref[i];
                    results.push(handler.apply(null, args));
                }
                return results;
            }
        };

        return eventEmitter;

    })();

    morris.commas = function(num) {
        var absnum, intnum, ret, strabsnum;
        if (num != null) {
            ret = num < 0 ? "-" : "";
            absnum = Math.abs(num);
            intnum = Math.floor(absnum).toFixed(0);
            ret += intnum.replace(/(?=(?:\d{3})+$)(?!^)/g, ',');
            strabsnum = absnum.toString();
            if (strabsnum.length > intnum.length) {
                ret += strabsnum.slice(intnum.length);
            }
            return ret;
        } else {
            return '-';
        }
    };

    morris.pad2 = function(number) {
        return (number < 10 ? '0' : '') + number;
    };

    morris.Grid = (function(_super) {
        __extends(grid, _super);

        function grid(options) {
            this.resizeHandler = bind(this.resizeHandler, this);
            var _this = this;
            if (typeof options.element === 'string') {
                this.el = $(document.getElementById(options.element));
            } else {
                this.el = $(options.element);
            }
            if ((this.el == null) || this.el.length === 0) {
                throw new Error("Graph container element not found");
            }
            if (this.el.css('position') === 'static') {
                this.el.css('position', 'relative');
            }
            this.options = $.extend({}, this.gridDefaults, this.defaults || {}, options);
            if (typeof this.options.units === 'string') {
                this.options.postUnits = options.units;
            }
            this.raphael = new Raphael(this.el[0]);
            this.elementWidth = null;
            this.elementHeight = null;
            this.dirty = false;
            this.selectFrom = null;
            if (this.init) {
                this.init();
            }
            this.setData(this.options.data);
            this.el.bind('mousemove', function(evt) {
                var left, offset, right, width, x;
                offset = _this.el.offset();
                x = evt.pageX - offset.left;
                if (_this.selectFrom) {
                    left = _this.data[_this.hitTest(Math.min(x, _this.selectFrom))]._x;
                    right = _this.data[_this.hitTest(Math.max(x, _this.selectFrom))]._x;
                    width = right - left;
                    return _this.selectionRect.attr({
                        x: left,
                        width: width
                    });
                } else {
                    return _this.fire('hovermove', x, evt.pageY - offset.top);
                }
            });
            this.el.bind('mouseleave', function(evt) {
                if (_this.selectFrom) {
                    _this.selectionRect.hide();
                    _this.selectFrom = null;
                }
                return _this.fire('hoverout');
            });
            this.el.bind('touchstart touchmove touchend', function(evt) {
                var offset, touch;
                touch = evt.originalEvent.touches[0] || evt.originalEvent.changedTouches[0];
                offset = _this.el.offset();
                _this.fire('hover', touch.pageX - offset.left, touch.pageY - offset.top);
                return touch;
            });
            this.el.bind('click', function(evt) {
                var offset;
                offset = _this.el.offset();
                return _this.fire('gridclick', evt.pageX - offset.left, evt.pageY - offset.top);
            });
            if (this.options.rangeSelect) {
                this.selectionRect = this.raphael.rect(0, 0, 0, this.el.innerHeight()).attr({
                    fill: this.options.rangeSelectColor,
                    stroke: false
                }).toBack().hide();
                this.el.bind('mousedown', function(evt) {
                    var offset;
                    offset = _this.el.offset();
                    return _this.startRange(evt.pageX - offset.left);
                });
                this.el.bind('mouseup', function(evt) {
                    var offset;
                    offset = _this.el.offset();
                    _this.endRange(evt.pageX - offset.left);
                    return _this.fire('hovermove', evt.pageX - offset.left, evt.pageY - offset.top);
                });
            }
            if (this.options.resize) {
                $(window).bind('resize', function(evt) {
                    if (_this.timeoutId != null) {
                        window.clearTimeout(_this.timeoutId);
                    }
                    return _this.timeoutId = window.setTimeout(_this.resizeHandler, 100);
                });
            }
            if (this.postInit) {
                this.postInit();
            }
        }

        grid.prototype.gridDefaults = {
            dateFormat: null,
            axes: true,
            grid: true,
            gridLineColor: '#aaa',
            gridStrokeWidth: 0.5,
            gridTextColor: '#888',
            gridTextSize: 12,
            gridTextFamily: 'sans-serif',
            gridTextWeight: 'normal',
            hideHover: false,
            yLabelFormat: null,
            xLabelAngle: 0,
            numLines: 5,
            padding: 25,
            parseTime: true,
            postUnits: '',
            preUnits: '',
            ymax: 'auto',
            ymin: 'auto 0',
            goals: [],
            goalStrokeWidth: 1.0,
            goalLineColors: ['#666633', '#999966', '#cc6666', '#663333'],
            events: [],
            eventStrokeWidth: 1.0,
            eventLineColors: ['#005a04', '#ccffbb', '#3a5f0b', '#005502'],
            rangeSelect: null,
            rangeSelectColor: '#eef',
            resize: false
        };

        grid.prototype.setData = function(data, redraw) {
            var e, idx, index, maxGoal, minGoal, ret, row, step, total, y, ykey, ymax, ymin, yval, ref;
            if (redraw == null) {
                redraw = true;
            }
            this.options.data = data;
            if ((data == null) || data.length === 0) {
                this.data = [];
                this.raphael.clear();
                if (this.hover != null) {
                    this.hover.hide();
                }
                return;
            }
            ymax = this.cumulative ? 0 : null;
            ymin = this.cumulative ? 0 : null;
            if (this.options.goals.length > 0) {
                minGoal = Math.min.apply(Math, this.options.goals);
                maxGoal = Math.max.apply(Math, this.options.goals);
                ymin = ymin != null ? Math.min(ymin, minGoal) : minGoal;
                ymax = ymax != null ? Math.max(ymax, maxGoal) : maxGoal;
            }
            this.data = (function() {
                var i, len, results;
                results = [];
                for (index = i = 0, len = data.length; i < len; index = ++i) {
                    row = data[index];
                    ret = {
                        src: row
                    };
                    ret.label = row[this.options.xkey];
                    if (this.options.parseTime) {
                        ret.x = morris.parseDate(ret.label);
                        if (this.options.dateFormat) {
                            ret.label = this.options.dateFormat(ret.x);
                        } else if (typeof ret.label === 'number') {
                            ret.label = new Date(ret.label).toString();
                        }
                    } else {
                        ret.x = index;
                        if (this.options.xLabelFormat) {
                            ret.label = this.options.xLabelFormat(ret);
                        }
                    }
                    total = 0;
                    ret.y = (function() {
                        var j, len1, ref, results1;
                        ref = this.options.ykeys;
                        results1 = [];
                        for (idx = j = 0, len1 = ref.length; j < len1; idx = ++j) {
                            ykey = ref[idx];
                            yval = row[ykey];
                            if (typeof yval === 'string') {
                                yval = parseFloat(yval);
                            }
                            if ((yval != null) && typeof yval !== 'number') {
                                yval = null;
                            }
                            if (yval != null) {
                                if (this.cumulative) {
                                    total += yval;
                                } else {
                                    if (ymax != null) {
                                        ymax = Math.max(yval, ymax);
                                        ymin = Math.min(yval, ymin);
                                    } else {
                                        ymax = ymin = yval;
                                    }
                                }
                            }
                            if (this.cumulative && (total != null)) {
                                ymax = Math.max(total, ymax);
                                ymin = Math.min(total, ymin);
                            }
                            results1.push(yval);
                        }
                        return results1;
                    }).call(this);
                    results.push(ret);
                }
                return results;
            }).call(this);
            if (this.options.parseTime) {
                this.data = this.data.sort(function(a, b) {
                    return (a.x > b.x) - (b.x > a.x);
                });
            }
            this.xmin = this.data[0].x;
            this.xmax = this.data[this.data.length - 1].x;
            this.events = [];
            if (this.options.events.length > 0) {
                if (this.options.parseTime) {
                    this.events = (function() {
                        var i, len, ref, results;
                        ref = this.options.events;
                        results = [];
                        for (i = 0, len = ref.length; i < len; i++) {
                            e = ref[i];
                            results.push(morris.parseDate(e));
                        }
                        return results;
                    }).call(this);
                } else {
                    this.events = this.options.events;
                }
                this.xmax = Math.max(this.xmax, Math.max.apply(Math, this.events));
                this.xmin = Math.min(this.xmin, Math.min.apply(Math, this.events));
            }
            if (this.xmin === this.xmax) {
                this.xmin -= 1;
                this.xmax += 1;
            }
            this.ymin = this.yboundary('min', ymin);
            this.ymax = this.yboundary('max', ymax);
            if (this.ymin === this.ymax) {
                if (ymin) {
                    this.ymin -= 1;
                }
                this.ymax += 1;
            }
            if (((ref = this.options.axes) === true || ref === 'both' || ref === 'y') || this.options.grid === true) {
                if (this.options.ymax === this.gridDefaults.ymax && this.options.ymin === this.gridDefaults.ymin) {
                    this.grid = this.autoGridLines(this.ymin, this.ymax, this.options.numLines);
                    this.ymin = Math.min(this.ymin, this.grid[0]);
                    this.ymax = Math.max(this.ymax, this.grid[this.grid.length - 1]);
                } else {
                    step = (this.ymax - this.ymin) / (this.options.numLines - 1);
                    this.grid = (function() {
                        var i, ref1, ref2, results;
                        results = [];
                        for (y = i = ref1 = this.ymin, ref2 = this.ymax; step > 0 ? i <= ref2 : i >= ref2; y = i += step) {
                            results.push(y);
                        }
                        return results;
                    }).call(this);
                }
            }
            this.dirty = true;
            if (redraw) {
                return this.redraw();
            }
        };

        grid.prototype.yboundary = function(boundaryType, currentValue) {
            var boundaryOption, suggestedValue;
            boundaryOption = this.options["y" + boundaryType];
            if (typeof boundaryOption === 'string') {
                if (boundaryOption.slice(0, 4) === 'auto') {
                    if (boundaryOption.length > 5) {
                        suggestedValue = parseInt(boundaryOption.slice(5), 10);
                        if (currentValue == null) {
                            return suggestedValue;
                        }
                        return Math[boundaryType](currentValue, suggestedValue);
                    } else {
                        if (currentValue != null) {
                            return currentValue;
                        } else {
                            return 0;
                        }
                    }
                } else {
                    return parseInt(boundaryOption, 10);
                }
            } else {
                return boundaryOption;
            }
        };

        grid.prototype.autoGridLines = function(ymin, ymax, nlines) {
            var gmax, gmin, grid, smag, span, step, unit, y, ymag;
            span = ymax - ymin;
            ymag = Math.floor(Math.log(span) / Math.log(10));
            unit = Math.pow(10, ymag);
            gmin = Math.floor(ymin / unit) * unit;
            gmax = Math.ceil(ymax / unit) * unit;
            step = (gmax - gmin) / (nlines - 1);
            if (unit === 1 && step > 1 && Math.ceil(step) !== step) {
                step = Math.ceil(step);
                gmax = gmin + step * (nlines - 1);
            }
            if (gmin < 0 && gmax > 0) {
                gmin = Math.floor(ymin / step) * step;
                gmax = Math.ceil(ymax / step) * step;
            }
            if (step < 1) {
                smag = Math.floor(Math.log(step) / Math.log(10));
                grid = (function() {
                    var i, results;
                    results = [];
                    for (y = i = gmin; step > 0 ? i <= gmax : i >= gmax; y = i += step) {
                        results.push(parseFloat(y.toFixed(1 - smag)));
                    }
                    return results;
                })();
            } else {
                grid = (function() {
                    var i, results;
                    results = [];
                    for (y = i = gmin; step > 0 ? i <= gmax : i >= gmax; y = i += step) {
                        results.push(y);
                    }
                    return results;
                })();
            }
            return grid;
        };

        grid.prototype._calc = function() {
            var bottomOffsets, gridLine, h, i, w, yLabelWidths, ref, ref1;
            w = this.el.width();
            h = this.el.height();
            if (this.elementWidth !== w || this.elementHeight !== h || this.dirty) {
                this.elementWidth = w;
                this.elementHeight = h;
                this.dirty = false;
                this.left = this.options.padding;
                this.right = this.elementWidth - this.options.padding;
                this.top = this.options.padding;
                this.bottom = this.elementHeight - this.options.padding;
                if ((ref = this.options.axes) === true || ref === 'both' || ref === 'y') {
                    yLabelWidths = (function() {
                        var _i, len, ref1, results;
                        ref1 = this.grid;
                        results = [];
                        for (_i = 0, len = ref1.length; _i < len; _i++) {
                            gridLine = ref1[_i];
                            results.push(this.measureText(this.yAxisFormat(gridLine)).width);
                        }
                        return results;
                    }).call(this);
                    this.left += Math.max.apply(Math, yLabelWidths);
                }
                if ((ref1 = this.options.axes) === true || ref1 === 'both' || ref1 === 'x') {
                    bottomOffsets = (function() {
                        var _i, ref2, results;
                        results = [];
                        for (i = _i = 0, ref2 = this.data.length; 0 <= ref2 ? _i < ref2 : _i > ref2; i = 0 <= ref2 ? ++_i : --_i) {
                            results.push(this.measureText(this.data[i].text, -this.options.xLabelAngle).height);
                        }
                        return results;
                    }).call(this);
                    this.bottom -= Math.max.apply(Math, bottomOffsets);
                }
                this.width = Math.max(1, this.right - this.left);
                this.height = Math.max(1, this.bottom - this.top);
                this.dx = this.width / (this.xmax - this.xmin);
                this.dy = this.height / (this.ymax - this.ymin);
                if (this.calc) {
                    return this.calc();
                }
            }
        };

        grid.prototype.transY = function(y) {
            return this.bottom - (y - this.ymin) * this.dy;
        };

        grid.prototype.transX = function(x) {
            if (this.data.length === 1) {
                return (this.left + this.right) / 2;
            } else {
                return this.left + (x - this.xmin) * this.dx;
            }
        };

        grid.prototype.redraw = function() {
            this.raphael.clear();
            this._calc();
            this.drawGrid();
            this.drawGoals();
            this.drawEvents();
            if (this.draw) {
                return this.draw();
            }
        };

        grid.prototype.measureText = function(text, angle) {
            var ret, tt;
            if (angle == null) {
                angle = 0;
            }
            tt = this.raphael.text(100, 100, text).attr('font-size', this.options.gridTextSize).attr('font-family', this.options.gridTextFamily).attr('font-weight', this.options.gridTextWeight).rotate(angle);
            ret = tt.getBBox();
            tt.remove();
            return ret;
        };

        grid.prototype.yAxisFormat = function(label) {
            return this.yLabelFormat(label);
        };

        grid.prototype.yLabelFormat = function(label) {
            if (typeof this.options.yLabelFormat === 'function') {
                return this.options.yLabelFormat(label);
            } else {
                return "" + this.options.preUnits + (morris.commas(label)) + this.options.postUnits;
            }
        };

        grid.prototype.drawGrid = function() {
            var lineY, y, i, len, ref, ref1, ref2, results;
            if (this.options.grid === false && ((ref = this.options.axes) !== true && ref !== 'both' && ref !== 'y')) {
                return;
            }
            ref1 = this.grid;
            results = [];
            for (i = 0, len = ref1.length; i < len; i++) {
                lineY = ref1[i];
                y = this.transY(lineY);
                if ((ref2 = this.options.axes) === true || ref2 === 'both' || ref2 === 'y') {
                    this.drawYAxisLabel(this.left - this.options.padding / 2, y, this.yAxisFormat(lineY));
                }
                if (this.options.grid) {
                    results.push(this.drawGridLine("M" + this.left + "," + y + "H" + (this.left + this.width)));
                } else {
                    results.push(void 0);
                }
            }
            return results;
        };

        grid.prototype.drawGoals = function() {
            var color, goal, i, _i, len, ref, results;
            ref = this.options.goals;
            results = [];
            for (i = _i = 0, len = ref.length; _i < len; i = ++_i) {
                goal = ref[i];
                color = this.options.goalLineColors[i % this.options.goalLineColors.length];
                results.push(this.drawGoal(goal, color));
            }
            return results;
        };

        grid.prototype.drawEvents = function() {
            var color, event, i, _i, len, ref, results;
            ref = this.events;
            results = [];
            for (i = _i = 0, len = ref.length; _i < len; i = ++_i) {
                event = ref[i];
                color = this.options.eventLineColors[i % this.options.eventLineColors.length];
                results.push(this.drawEvent(event, color));
            }
            return results;
        };

        grid.prototype.drawGoal = function(goal, color) {
            return this.raphael.path("M" + this.left + "," + (this.transY(goal)) + "H" + this.right).attr('stroke', color).attr('stroke-width', this.options.goalStrokeWidth);
        };

        grid.prototype.drawEvent = function(event, color) {
            return this.raphael.path("M" + (this.transX(event)) + "," + this.bottom + "V" + this.top).attr('stroke', color).attr('stroke-width', this.options.eventStrokeWidth);
        };

        grid.prototype.drawYAxisLabel = function(xPos, yPos, text) {
            return this.raphael.text(xPos, yPos, text).attr('font-size', this.options.gridTextSize).attr('font-family', this.options.gridTextFamily).attr('font-weight', this.options.gridTextWeight).attr('fill', this.options.gridTextColor).attr('text-anchor', 'end');
        };

        grid.prototype.drawGridLine = function(path) {
            return this.raphael.path(path).attr('stroke', this.options.gridLineColor).attr('stroke-width', this.options.gridStrokeWidth);
        };

        grid.prototype.startRange = function(x) {
            this.hover.hide();
            this.selectFrom = x;
            return this.selectionRect.attr({
                x: x,
                width: 0
            }).show();
        };

        grid.prototype.endRange = function(x) {
            var end, start;
            if (this.selectFrom) {
                start = Math.min(this.selectFrom, x);
                end = Math.max(this.selectFrom, x);
                this.options.rangeSelect.call(this.el, {
                    start: this.data[this.hitTest(start)].x,
                    end: this.data[this.hitTest(end)].x
                });
                return this.selectFrom = null;
            }
        };

        grid.prototype.resizeHandler = function() {
            this.timeoutId = null;
            this.raphael.setSize(this.el.width(), this.el.height());
            return this.redraw();
        };

        return grid;

    })(morris.EventEmitter);

    morris.parseDate = function(date) {
        var isecs, m, msecs, n, o, offsetmins, p, q, r, ret, secs;
        if (typeof date === 'number') {
            return date;
        }
        m = date.match(/^(\d+) Q(\d)$/);
        n = date.match(/^(\d+)-(\d+)$/);
        o = date.match(/^(\d+)-(\d+)-(\d+)$/);
        p = date.match(/^(\d+) W(\d+)$/);
        q = date.match(/^(\d+)-(\d+)-(\d+)[ T](\d+):(\d+)(Z|([+-])(\d\d):?(\d\d))?$/);
        r = date.match(/^(\d+)-(\d+)-(\d+)[ T](\d+):(\d+):(\d+(\.\d+)?)(Z|([+-])(\d\d):?(\d\d))?$/);
        if (m) {
            return new Date(parseInt(m[1], 10), parseInt(m[2], 10) * 3 - 1, 1).getTime();
        } else if (n) {
            return new Date(parseInt(n[1], 10), parseInt(n[2], 10) - 1, 1).getTime();
        } else if (o) {
            return new Date(parseInt(o[1], 10), parseInt(o[2], 10) - 1, parseInt(o[3], 10)).getTime();
        } else if (p) {
            ret = new Date(parseInt(p[1], 10), 0, 1);
            if (ret.getDay() !== 4) {
                ret.setMonth(0, 1 + ((4 - ret.getDay()) + 7) % 7);
            }
            return ret.getTime() + parseInt(p[2], 10) * 604800000;
        } else if (q) {
            if (!q[6]) {
                return new Date(parseInt(q[1], 10), parseInt(q[2], 10) - 1, parseInt(q[3], 10), parseInt(q[4], 10), parseInt(q[5], 10)).getTime();
            } else {
                offsetmins = 0;
                if (q[6] !== 'Z') {
                    offsetmins = parseInt(q[8], 10) * 60 + parseInt(q[9], 10);
                    if (q[7] === '+') {
                        offsetmins = 0 - offsetmins;
                    }
                }
                return Date.UTC(parseInt(q[1], 10), parseInt(q[2], 10) - 1, parseInt(q[3], 10), parseInt(q[4], 10), parseInt(q[5], 10) + offsetmins);
            }
        } else if (r) {
            secs = parseFloat(r[6]);
            isecs = Math.floor(secs);
            msecs = Math.round((secs - isecs) * 1000);
            if (!r[8]) {
                return new Date(parseInt(r[1], 10), parseInt(r[2], 10) - 1, parseInt(r[3], 10), parseInt(r[4], 10), parseInt(r[5], 10), isecs, msecs).getTime();
            } else {
                offsetmins = 0;
                if (r[8] !== 'Z') {
                    offsetmins = parseInt(r[10], 10) * 60 + parseInt(r[11], 10);
                    if (r[9] === '+') {
                        offsetmins = 0 - offsetmins;
                    }
                }
                return Date.UTC(parseInt(r[1], 10), parseInt(r[2], 10) - 1, parseInt(r[3], 10), parseInt(r[4], 10), parseInt(r[5], 10) + offsetmins, isecs, msecs);
            }
        } else {
            return new Date(parseInt(date, 10), 0, 1).getTime();
        }
    };

    morris.Hover = (function() {
        hover.defaults = {
            "class": 'morris-hover morris-default-style'
        };

        function hover(options) {
            if (options == null) {
                options = {};
            }
            this.options = $.extend({}, morris.Hover.defaults, options);
            this.el = $("<div class='" + this.options["class"] + "'></div>");
            this.el.hide();
            this.options.parent.append(this.el);
        }

        hover.prototype.update = function(html, x, y) {
            this.html(html);
            this.show();
            return this.moveTo(x, y);
        };

        hover.prototype.html = function(content) {
            return this.el.html(content);
        };

        hover.prototype.moveTo = function(x, y) {
            var hoverHeight, hoverWidth, left, parentHeight, parentWidth, top;
            parentWidth = this.options.parent.innerWidth();
            parentHeight = this.options.parent.innerHeight();
            hoverWidth = this.el.outerWidth();
            hoverHeight = this.el.outerHeight();
            left = Math.min(Math.max(0, x - hoverWidth / 2), parentWidth - hoverWidth);
            if (y != null) {
                top = y - hoverHeight - 10;
                if (top < 0) {
                    top = y + 10;
                    if (top + hoverHeight > parentHeight) {
                        top = parentHeight / 2 - hoverHeight / 2;
                    }
                }
            } else {
                top = parentHeight / 2 - hoverHeight / 2;
            }
            return this.el.css({
                left: left + "px",
                top: parseInt(top) + "px"
            });
        };

        hover.prototype.show = function() {
            return this.el.show();
        };

        hover.prototype.hide = function() {
            return this.el.hide();
        };

        return hover;

    })();

    morris.Line = (function(_super) {
        __extends(line, _super);

        function line(options) {
            this.hilight = bind(this.hilight, this);
            this.onHoverOut = bind(this.onHoverOut, this);
            this.onHoverMove = bind(this.onHoverMove, this);
            this.onGridClick = bind(this.onGridClick, this);
            if (!(this instanceof morris.Line)) {
                return new morris.Line(options);
            }
            line.__super__.constructor.call(this, options);
        }

        line.prototype.init = function() {
            if (this.options.hideHover !== 'always') {
                this.hover = new morris.Hover({
                    parent: this.el
                });
                this.on('hovermove', this.onHoverMove);
                this.on('hoverout', this.onHoverOut);
                return this.on('gridclick', this.onGridClick);
            }
        };

        line.prototype.defaults = {
            lineWidth: 3,
            pointSize: 4,
            lineColors: ['#0b62a4', '#7A92A3', '#4da74d', '#afd8f8', '#edc240', '#cb4b4b', '#9440ed'],
            pointStrokeWidths: [1],
            pointStrokeColors: ['#ffffff'],
            pointFillColors: [],
            smooth: true,
            xLabels: 'auto',
            xLabelFormat: null,
            xLabelMargin: 24,
            continuousLine: true,
            hideHover: false
        };

        line.prototype.calc = function() {
            this.calcPoints();
            return this.generatePaths();
        };

        line.prototype.calcPoints = function() {
            var row, y, i, len, ref, results;
            ref = this.data;
            results = [];
            for (i = 0, len = ref.length; i < len; i++) {
                row = ref[i];
                row._x = this.transX(row.x);
                row._y = (function() {
                    var j, len1, ref1, results1;
                    ref1 = row.y;
                    results1 = [];
                    for (j = 0, len1 = ref1.length; j < len1; j++) {
                        y = ref1[j];
                        if (y != null) {
                            results1.push(this.transY(y));
                        } else {
                            results1.push(y);
                        }
                    }
                    return results1;
                }).call(this);
                results.push(row._ymax = Math.min.apply(Math, [this.bottom].concat((function() {
                    var j, len1, ref1, results1;
                    ref1 = row._y;
                    results1 = [];
                    for (j = 0, len1 = ref1.length; j < len1; j++) {
                        y = ref1[j];
                        if (y != null) {
                            results1.push(y);
                        }
                    }
                    return results1;
                })())));
            }
            return results;
        };

        line.prototype.hitTest = function(x) {
            var index, r, i, len, ref;
            if (this.data.length === 0) {
                return null;
            }
            ref = this.data.slice(1);
            for (index = i = 0, len = ref.length; i < len; index = ++i) {
                r = ref[index];
                if (x < (r._x + this.data[index]._x) / 2) {
                    break;
                }
            }
            return index;
        };

        line.prototype.onGridClick = function(x, y) {
            var index;
            index = this.hitTest(x);
            return this.fire('click', index, this.data[index].src, x, y);
        };

        line.prototype.onHoverMove = function(x, y) {
            var index;
            index = this.hitTest(x);
            return this.displayHoverForRow(index);
        };

        line.prototype.onHoverOut = function() {
            if (this.options.hideHover !== false) {
                return this.displayHoverForRow(null);
            }
        };

        line.prototype.displayHoverForRow = function(index) {
            var ref;
            if (index != null) {
                (ref = this.hover).update.apply(ref, this.hoverContentForRow(index));
                return this.hilight(index);
            } else {
                this.hover.hide();
                return this.hilight();
            }
        };

        line.prototype.hoverContentForRow = function(index) {
            var content, j, row, y, i, len, ref;
            row = this.data[index];
            content = "<div class='morris-hover-row-label'>" + row.label + "</div>";
            ref = row.y;
            for (j = i = 0, len = ref.length; i < len; j = ++i) {
                y = ref[j];
                content += "<div class='morris-hover-point' style='color: " + (this.colorFor(row, j, 'label')) + "'>\n  " + this.options.labels[j] + ":\n  " + (this.yLabelFormat(y)) + "\n</div>";
            }
            if (typeof this.options.hoverCallback === 'function') {
                content = this.options.hoverCallback(index, this.options, content, row.src);
            }
            return [content, row._x, row._ymax];
        };

        line.prototype.generatePaths = function() {
            var c, coords, i, r, smooth;
            return this.paths = (function() {
                var _i, ref, ref1, results;
                results = [];
                for (i = _i = 0, ref = this.options.ykeys.length; 0 <= ref ? _i < ref : _i > ref; i = 0 <= ref ? ++_i : --_i) {
                    smooth = typeof this.options.smooth === "boolean" ? this.options.smooth : (ref1 = this.options.ykeys[i], indexOf.call(this.options.smooth, ref1) >= 0);
                    coords = (function() {
                        var j, len, ref2, results1;
                        ref2 = this.data;
                        results1 = [];
                        for (j = 0, len = ref2.length; j < len; j++) {
                            r = ref2[j];
                            if (r._y[i] !== void 0) {
                                results1.push({
                                    x: r._x,
                                    y: r._y[i]
                                });
                            }
                        }
                        return results1;
                    }).call(this);
                    if (this.options.continuousLine) {
                        coords = (function() {
                            var j, len, results1;
                            results1 = [];
                            for (j = 0, len = coords.length; j < len; j++) {
                                c = coords[j];
                                if (c.y !== null) {
                                    results1.push(c);
                                }
                            }
                            return results1;
                        })();
                    }
                    if (coords.length > 1) {
                        results.push(morris.Line.createPath(coords, smooth, this.bottom));
                    } else {
                        results.push(null);
                    }
                }
                return results;
            }).call(this);
        };

        line.prototype.draw = function() {
            var ref;
            if ((ref = this.options.axes) === true || ref === 'both' || ref === 'x') {
                this.drawXAxis();
            }
            this.drawSeries();
            if (this.options.hideHover === false) {
                return this.displayHoverForRow(this.data.length - 1);
            }
        };

        line.prototype.drawXAxis = function() {
            var drawLabel,
                l,
                labels,
                prevAngleMargin,
                prevLabelMargin,
                row,
                ypos,
                i,
                len,
                results,
                _this = this;
            ypos = this.bottom + this.options.padding / 2;
            prevLabelMargin = null;
            prevAngleMargin = null;
            drawLabel = function(labelText, xpos) {
                var label, labelBox, margin, offset, textBox;
                label = _this.drawXAxisLabel(_this.transX(xpos), ypos, labelText);
                textBox = label.getBBox();
                label.transform("r" + (-_this.options.xLabelAngle));
                labelBox = label.getBBox();
                label.transform("t0," + (labelBox.height / 2) + "...");
                if (_this.options.xLabelAngle !== 0) {
                    offset = -0.5 * textBox.width * Math.cos(_this.options.xLabelAngle * Math.PI / 180.0);
                    label.transform("t" + offset + ",0...");
                }
                labelBox = label.getBBox();
                if (((prevLabelMargin == null) || prevLabelMargin >= labelBox.x + labelBox.width || (prevAngleMargin != null) && prevAngleMargin >= labelBox.x) && labelBox.x >= 0 && (labelBox.x + labelBox.width) < _this.el.width()) {
                    if (_this.options.xLabelAngle !== 0) {
                        margin = 1.25 * _this.options.gridTextSize / Math.sin(_this.options.xLabelAngle * Math.PI / 180.0);
                        prevAngleMargin = labelBox.x - margin;
                    }
                    return prevLabelMargin = labelBox.x - _this.options.xLabelMargin;
                } else {
                    return label.remove();
                }
            };
            if (this.options.parseTime) {
                if (this.data.length === 1 && this.options.xLabels === 'auto') {
                    labels = [[this.data[0].label, this.data[0].x]];
                } else {
                    labels = morris.labelSeries(this.xmin, this.xmax, this.width, this.options.xLabels, this.options.xLabelFormat);
                }
            } else {
                labels = (function() {
                    var i, len, ref, results;
                    ref = this.data;
                    results = [];
                    for (i = 0, len = ref.length; i < len; i++) {
                        row = ref[i];
                        results.push([row.label, row.x]);
                    }
                    return results;
                }).call(this);
            }
            labels.reverse();
            results = [];
            for (i = 0, len = labels.length; i < len; i++) {
                l = labels[i];
                results.push(drawLabel(l[0], l[1]));
            }
            return results;
        };

        line.prototype.drawSeries = function() {
            var i, _i, j, ref, ref1, results;
            this.seriesPoints = [];
            for (i = _i = ref = this.options.ykeys.length - 1; ref <= 0 ? _i <= 0 : _i >= 0; i = ref <= 0 ? ++_i : --_i) {
                this._drawLineFor(i);
            }
            results = [];
            for (i = j = ref1 = this.options.ykeys.length - 1; ref1 <= 0 ? j <= 0 : j >= 0; i = ref1 <= 0 ? ++j : --j) {
                results.push(this._drawPointFor(i));
            }
            return results;
        };

        line.prototype._drawPointFor = function(index) {
            var circle, row, i, len, ref, results;
            this.seriesPoints[index] = [];
            ref = this.data;
            results = [];
            for (i = 0, len = ref.length; i < len; i++) {
                row = ref[i];
                circle = null;
                if (row._y[index] != null) {
                    circle = this.drawLinePoint(row._x, row._y[index], this.colorFor(row, index, 'point'), index);
                }
                results.push(this.seriesPoints[index].push(circle));
            }
            return results;
        };

        line.prototype._drawLineFor = function(index) {
            var path;
            path = this.paths[index];
            if (path !== null) {
                return this.drawLinePath(path, this.colorFor(null, index, 'line'), index);
            }
        };

        line.createPath = function(coords, smooth, bottom) {
            var coord, g, grads, i, ix, lg, path, prevCoord, x1, x2, y1, y2, _i, len;
            path = "";
            if (smooth) {
                grads = morris.Line.gradients(coords);
            }
            prevCoord = {
                y: null
            };
            for (i = _i = 0, len = coords.length; _i < len; i = ++_i) {
                coord = coords[i];
                if (coord.y != null) {
                    if (prevCoord.y != null) {
                        if (smooth) {
                            g = grads[i];
                            lg = grads[i - 1];
                            ix = (coord.x - prevCoord.x) / 4;
                            x1 = prevCoord.x + ix;
                            y1 = Math.min(bottom, prevCoord.y + ix * lg);
                            x2 = coord.x - ix;
                            y2 = Math.min(bottom, coord.y - ix * g);
                            path += "C" + x1 + "," + y1 + "," + x2 + "," + y2 + "," + coord.x + "," + coord.y;
                        } else {
                            path += "L" + coord.x + "," + coord.y;
                        }
                    } else {
                        if (!smooth || (grads[i] != null)) {
                            path += "M" + coord.x + "," + coord.y;
                        }
                    }
                }
                prevCoord = coord;
            }
            return path;
        };

        line.gradients = function(coords) {
            var coord, grad, i, nextCoord, prevCoord, _i, len, results;
            grad = function(a, b) {
                return (a.y - b.y) / (a.x - b.x);
            };
            results = [];
            for (i = _i = 0, len = coords.length; _i < len; i = ++_i) {
                coord = coords[i];
                if (coord.y != null) {
                    nextCoord = coords[i + 1] || {
                        y: null
                    };
                    prevCoord = coords[i - 1] || {
                        y: null
                    };
                    if ((prevCoord.y != null) && (nextCoord.y != null)) {
                        results.push(grad(prevCoord, nextCoord));
                    } else if (prevCoord.y != null) {
                        results.push(grad(prevCoord, coord));
                    } else if (nextCoord.y != null) {
                        results.push(grad(coord, nextCoord));
                    } else {
                        results.push(null);
                    }
                } else {
                    results.push(null);
                }
            }
            return results;
        };

        line.prototype.hilight = function(index) {
            var i, _i, j, ref, ref1;
            if (this.prevHilight !== null && this.prevHilight !== index) {
                for (i = _i = 0, ref = this.seriesPoints.length - 1; 0 <= ref ? _i <= ref : _i >= ref; i = 0 <= ref ? ++_i : --_i) {
                    if (this.seriesPoints[i][this.prevHilight]) {
                        this.seriesPoints[i][this.prevHilight].animate(this.pointShrinkSeries(i));
                    }
                }
            }
            if (index !== null && this.prevHilight !== index) {
                for (i = j = 0, ref1 = this.seriesPoints.length - 1; 0 <= ref1 ? j <= ref1 : j >= ref1; i = 0 <= ref1 ? ++j : --j) {
                    if (this.seriesPoints[i][index]) {
                        this.seriesPoints[i][index].animate(this.pointGrowSeries(i));
                    }
                }
            }
            return this.prevHilight = index;
        };

        line.prototype.colorFor = function(row, sidx, type) {
            if (typeof this.options.lineColors === 'function') {
                return this.options.lineColors.call(this, row, sidx, type);
            } else if (type === 'point') {
                return this.options.pointFillColors[sidx % this.options.pointFillColors.length] || this.options.lineColors[sidx % this.options.lineColors.length];
            } else {
                return this.options.lineColors[sidx % this.options.lineColors.length];
            }
        };

        line.prototype.drawXAxisLabel = function(xPos, yPos, text) {
            return this.raphael.text(xPos, yPos, text).attr('font-size', this.options.gridTextSize).attr('font-family', this.options.gridTextFamily).attr('font-weight', this.options.gridTextWeight).attr('fill', this.options.gridTextColor);
        };

        line.prototype.drawLinePath = function(path, lineColor, lineIndex) {
            return this.raphael.path(path).attr('stroke', lineColor).attr('stroke-width', this.lineWidthForSeries(lineIndex));
        };

        line.prototype.drawLinePoint = function(xPos, yPos, pointColor, lineIndex) {
            return this.raphael.circle(xPos, yPos, this.pointSizeForSeries(lineIndex)).attr('fill', pointColor).attr('stroke-width', this.pointStrokeWidthForSeries(lineIndex)).attr('stroke', this.pointStrokeColorForSeries(lineIndex));
        };

        line.prototype.pointStrokeWidthForSeries = function(index) {
            return this.options.pointStrokeWidths[index % this.options.pointStrokeWidths.length];
        };

        line.prototype.pointStrokeColorForSeries = function(index) {
            return this.options.pointStrokeColors[index % this.options.pointStrokeColors.length];
        };

        line.prototype.lineWidthForSeries = function(index) {
            if (this.options.lineWidth instanceof Array) {
                return this.options.lineWidth[index % this.options.lineWidth.length];
            } else {
                return this.options.lineWidth;
            }
        };

        line.prototype.pointSizeForSeries = function(index) {
            if (this.options.pointSize instanceof Array) {
                return this.options.pointSize[index % this.options.pointSize.length];
            } else {
                return this.options.pointSize;
            }
        };

        line.prototype.pointGrowSeries = function(index) {
            return Raphael.animation({
                r: this.pointSizeForSeries(index) + 3
            }, 25, 'linear');
        };

        line.prototype.pointShrinkSeries = function(index) {
            return Raphael.animation({
                r: this.pointSizeForSeries(index)
            }, 25, 'linear');
        };

        return line;

    })(morris.Grid);

    morris.labelSeries = function(dmin, dmax, pxwidth, specName, xLabelFormat) {
        var d, d0, ddensity, name, ret, s, spec, t, i, len, ref;
        ddensity = 200 * (dmax - dmin) / pxwidth;
        d0 = new Date(dmin);
        spec = morris.LABEL_SPECS[specName];
        if (spec === void 0) {
            ref = morris.AUTO_LABEL_ORDER;
            for (i = 0, len = ref.length; i < len; i++) {
                name = ref[i];
                s = morris.LABEL_SPECS[name];
                if (ddensity >= s.span) {
                    spec = s;
                    break;
                }
            }
        }
        if (spec === void 0) {
            spec = morris.LABEL_SPECS["second"];
        }
        if (xLabelFormat) {
            spec = $.extend({}, spec, {
                fmt: xLabelFormat
            });
        }
        d = spec.start(d0);
        ret = [];
        while ((t = d.getTime()) <= dmax) {
            if (t >= dmin) {
                ret.push([spec.fmt(d), t]);
            }
            spec.incr(d);
        }
        return ret;
    };

    minutesSpecHelper = function(interval) {
        return {
            span: interval * 60 * 1000,
            start: function(d) {
                return new Date(d.getFullYear(), d.getMonth(), d.getDate(), d.getHours());
            },
            fmt: function(d) {
                return "" + (morris.pad2(d.getHours())) + ":" + (morris.pad2(d.getMinutes()));
            },
            incr: function(d) {
                return d.setUTCMinutes(d.getUTCMinutes() + interval);
            }
        };
    };

    secondsSpecHelper = function(interval) {
        return {
            span: interval * 1000,
            start: function(d) {
                return new Date(d.getFullYear(), d.getMonth(), d.getDate(), d.getHours(), d.getMinutes());
            },
            fmt: function(d) {
                return "" + (morris.pad2(d.getHours())) + ":" + (morris.pad2(d.getMinutes())) + ":" + (morris.pad2(d.getSeconds()));
            },
            incr: function(d) {
                return d.setUTCSeconds(d.getUTCSeconds() + interval);
            }
        };
    };

    morris.LABEL_SPECS = {
        "decade": {
            span: 172800000000,
            start: function(d) {
                return new Date(d.getFullYear() - d.getFullYear() % 10, 0, 1);
            },
            fmt: function(d) {
                return "" + (d.getFullYear());
            },
            incr: function(d) {
                return d.setFullYear(d.getFullYear() + 10);
            }
        },
        "year": {
            span: 17280000000,
            start: function(d) {
                return new Date(d.getFullYear(), 0, 1);
            },
            fmt: function(d) {
                return "" + (d.getFullYear());
            },
            incr: function(d) {
                return d.setFullYear(d.getFullYear() + 1);
            }
        },
        "month": {
            span: 2419200000,
            start: function(d) {
                return new Date(d.getFullYear(), d.getMonth(), 1);
            },
            fmt: function(d) {
                return "" + (d.getFullYear()) + "-" + (morris.pad2(d.getMonth() + 1));
            },
            incr: function(d) {
                return d.setMonth(d.getMonth() + 1);
            }
        },
        "week": {
            span: 604800000,
            start: function(d) {
                return new Date(d.getFullYear(), d.getMonth(), d.getDate());
            },
            fmt: function(d) {
                return "" + (d.getFullYear()) + "-" + (morris.pad2(d.getMonth() + 1)) + "-" + (morris.pad2(d.getDate()));
            },
            incr: function(d) {
                return d.setDate(d.getDate() + 7);
            }
        },
        "day": {
            span: 86400000,
            start: function(d) {
                return new Date(d.getFullYear(), d.getMonth(), d.getDate());
            },
            fmt: function(d) {
                return "" + (d.getFullYear()) + "-" + (morris.pad2(d.getMonth() + 1)) + "-" + (morris.pad2(d.getDate()));
            },
            incr: function(d) {
                return d.setDate(d.getDate() + 1);
            }
        },
        "hour": minutesSpecHelper(60),
        "30min": minutesSpecHelper(30),
        "15min": minutesSpecHelper(15),
        "10min": minutesSpecHelper(10),
        "5min": minutesSpecHelper(5),
        "minute": minutesSpecHelper(1),
        "30sec": secondsSpecHelper(30),
        "15sec": secondsSpecHelper(15),
        "10sec": secondsSpecHelper(10),
        "5sec": secondsSpecHelper(5),
        "second": secondsSpecHelper(1)
    };

    morris.AUTO_LABEL_ORDER = ["decade", "year", "month", "week", "day", "hour", "30min", "15min", "10min", "5min", "minute", "30sec", "15sec", "10sec", "5sec", "second"];

    morris.Area = (function(_super) {
        var areaDefaults;

        __extends(area, _super);

        areaDefaults = {
            fillOpacity: 'auto',
            behaveLikeLine: false
        };

        function area(options) {
            var areaOptions;
            if (!(this instanceof morris.Area)) {
                return new morris.Area(options);
            }
            areaOptions = $.extend({}, areaDefaults, options);
            this.cumulative = !areaOptions.behaveLikeLine;
            if (areaOptions.fillOpacity === 'auto') {
                areaOptions.fillOpacity = areaOptions.behaveLikeLine ? .8 : 1;
            }
            area.__super__.constructor.call(this, areaOptions);
        }

        area.prototype.calcPoints = function() {
            var row, total, y, i, len, ref, results;
            ref = this.data;
            results = [];
            for (i = 0, len = ref.length; i < len; i++) {
                row = ref[i];
                row._x = this.transX(row.x);
                total = 0;
                row._y = (function() {
                    var j, len1, ref1, results1;
                    ref1 = row.y;
                    results1 = [];
                    for (j = 0, len1 = ref1.length; j < len1; j++) {
                        y = ref1[j];
                        if (this.options.behaveLikeLine) {
                            results1.push(this.transY(y));
                        } else {
                            total += y || 0;
                            results1.push(this.transY(total));
                        }
                    }
                    return results1;
                }).call(this);
                results.push(row._ymax = Math.max.apply(Math, row._y));
            }
            return results;
        };

        area.prototype.drawSeries = function() {
            var i, range, _i, j, k, len, ref, ref1, results, results1, results2;
            this.seriesPoints = [];
            if (this.options.behaveLikeLine) {
                range = (function() {
                    results = [];
                    for (var _i = 0, ref = this.options.ykeys.length - 1; 0 <= ref ? _i <= ref : _i >= ref; 0 <= ref ? _i++ : _i--) {
                        results.push(_i);
                    }
                    return results;
                }).apply(this);
            } else {
                range = (function() {
                    results1 = [];
                    for (var j = ref1 = this.options.ykeys.length - 1; ref1 <= 0 ? j <= 0 : j >= 0; ref1 <= 0 ? j++ : j--) {
                        results1.push(j);
                    }
                    return results1;
                }).apply(this);
            }
            results2 = [];
            for (k = 0, len = range.length; k < len; k++) {
                i = range[k];
                this._drawFillFor(i);
                this._drawLineFor(i);
                results2.push(this._drawPointFor(i));
            }
            return results2;
        };

        area.prototype._drawFillFor = function(index) {
            var path;
            path = this.paths[index];
            if (path !== null) {
                path = path + ("L" + (this.transX(this.xmax)) + "," + this.bottom + "L" + (this.transX(this.xmin)) + "," + this.bottom + "Z");
                return this.drawFilledPath(path, this.fillForSeries(index));
            }
        };

        area.prototype.fillForSeries = function(i) {
            var color;
            color = Raphael.rgb2hsl(this.colorFor(this.data[i], i, 'line'));
            return Raphael.hsl(color.h, this.options.behaveLikeLine ? color.s * 0.9 : color.s * 0.75, Math.min(0.98, this.options.behaveLikeLine ? color.l * 1.2 : color.l * 1.25));
        };

        area.prototype.drawFilledPath = function(path, fill) {
            return this.raphael.path(path).attr('fill', fill).attr('fill-opacity', this.options.fillOpacity).attr('stroke', 'none');
        };

        return area;

    })(morris.Line);

    morris.Bar = (function(_super) {
        __extends(bar, _super);

        function bar(options) {
            this.onHoverOut = bind(this.onHoverOut, this);
            this.onHoverMove = bind(this.onHoverMove, this);
            this.onGridClick = bind(this.onGridClick, this);
            if (!(this instanceof morris.Bar)) {
                return new morris.Bar(options);
            }
            bar.__super__.constructor.call(this, $.extend({}, options, {
                parseTime: false
            }));
        }

        bar.prototype.init = function() {
            this.cumulative = this.options.stacked;
            if (this.options.hideHover !== 'always') {
                this.hover = new morris.Hover({
                    parent: this.el
                });
                this.on('hovermove', this.onHoverMove);
                this.on('hoverout', this.onHoverOut);
                return this.on('gridclick', this.onGridClick);
            }
        };

        bar.prototype.defaults = {
            barSizeRatio: 0.75,
            barGap: 3,
            barColors: ['#0b62a4', '#7a92a3', '#4da74d', '#afd8f8', '#edc240', '#cb4b4b', '#9440ed'],
            barOpacity: 1.0,
            barRadius: [0, 0, 0, 0],
            xLabelMargin: 50
        };

        bar.prototype.calc = function() {
            var ref;
            this.calcBars();
            if (this.options.hideHover === false) {
                return (ref = this.hover).update.apply(ref, this.hoverContentForRow(this.data.length - 1));
            }
        };

        bar.prototype.calcBars = function() {
            var idx, row, y, i, len, ref, results;
            ref = this.data;
            results = [];
            for (idx = i = 0, len = ref.length; i < len; idx = ++i) {
                row = ref[idx];
                row._x = this.left + this.width * (idx + 0.5) / this.data.length;
                results.push(row._y = (function() {
                    var j, len1, ref1, results1;
                    ref1 = row.y;
                    results1 = [];
                    for (j = 0, len1 = ref1.length; j < len1; j++) {
                        y = ref1[j];
                        if (y != null) {
                            results1.push(this.transY(y));
                        } else {
                            results1.push(null);
                        }
                    }
                    return results1;
                }).call(this));
            }
            return results;
        };

        bar.prototype.draw = function() {
            var ref;
            if ((ref = this.options.axes) === true || ref === 'both' || ref === 'x') {
                this.drawXAxis();
            }
            return this.drawSeries();
        };

        bar.prototype.drawXAxis = function() {
            var i, label, labelBox, margin, offset, prevAngleMargin, prevLabelMargin, row, textBox, ypos, _i, ref, results;
            ypos = this.bottom + (this.options.xAxisLabelTopPadding || this.options.padding / 2);
            prevLabelMargin = null;
            prevAngleMargin = null;
            results = [];
            for (i = _i = 0, ref = this.data.length; 0 <= ref ? _i < ref : _i > ref; i = 0 <= ref ? ++_i : --_i) {
                row = this.data[this.data.length - 1 - i];
                label = this.drawXAxisLabel(row._x, ypos, row.label);
                textBox = label.getBBox();
                label.transform("r" + (-this.options.xLabelAngle));
                labelBox = label.getBBox();
                label.transform("t0," + (labelBox.height / 2) + "...");
                if (this.options.xLabelAngle !== 0) {
                    offset = -0.5 * textBox.width * Math.cos(this.options.xLabelAngle * Math.PI / 180.0);
                    label.transform("t" + offset + ",0...");
                }
                if (((prevLabelMargin == null) || prevLabelMargin >= labelBox.x + labelBox.width || (prevAngleMargin != null) && prevAngleMargin >= labelBox.x) && labelBox.x >= 0 && (labelBox.x + labelBox.width) < this.el.width()) {
                    if (this.options.xLabelAngle !== 0) {
                        margin = 1.25 * this.options.gridTextSize / Math.sin(this.options.xLabelAngle * Math.PI / 180.0);
                        prevAngleMargin = labelBox.x - margin;
                    }
                    results.push(prevLabelMargin = labelBox.x - this.options.xLabelMargin);
                } else {
                    results.push(label.remove());
                }
            }
            return results;
        };

        bar.prototype.drawSeries = function() {
            var barWidth, bottom, groupWidth, idx, lastTop, left, leftPadding, numBars, row, sidx, size, top, ypos, zeroPos;
            groupWidth = this.width / this.options.data.length;
            numBars = this.options.stacked != null ? 1 : this.options.ykeys.length;
            barWidth = (groupWidth * this.options.barSizeRatio - this.options.barGap * (numBars - 1)) / numBars;
            leftPadding = groupWidth * (1 - this.options.barSizeRatio) / 2;
            zeroPos = this.ymin <= 0 && this.ymax >= 0 ? this.transY(0) : null;
            return this.bars = (function() {
                var i, len, ref, results;
                ref = this.data;
                results = [];
                for (idx = i = 0, len = ref.length; i < len; idx = ++i) {
                    row = ref[idx];
                    lastTop = 0;
                    results.push((function() {
                        var j, len1, ref1, results1;
                        ref1 = row._y;
                        results1 = [];
                        for (sidx = j = 0, len1 = ref1.length; j < len1; sidx = ++j) {
                            ypos = ref1[sidx];
                            if (ypos !== null) {
                                if (zeroPos) {
                                    top = Math.min(ypos, zeroPos);
                                    bottom = Math.max(ypos, zeroPos);
                                } else {
                                    top = ypos;
                                    bottom = this.bottom;
                                }
                                left = this.left + idx * groupWidth + leftPadding;
                                if (!this.options.stacked) {
                                    left += sidx * (barWidth + this.options.barGap);
                                }
                                size = bottom - top;
                                if (this.options.stacked) {
                                    top -= lastTop;
                                }
                                this.drawBar(left, top, barWidth, size, this.colorFor(row, sidx, 'bar'), this.options.barOpacity, this.options.barRadius);
                                results1.push(lastTop += size);
                            } else {
                                results1.push(null);
                            }
                        }
                        return results1;
                    }).call(this));
                }
                return results;
            }).call(this);
        };

        bar.prototype.colorFor = function(row, sidx, type) {
            var r, s;
            if (typeof this.options.barColors === 'function') {
                r = {
                    x: row.x,
                    y: row.y[sidx],
                    label: row.label
                };
                s = {
                    index: sidx,
                    key: this.options.ykeys[sidx],
                    label: this.options.labels[sidx]
                };
                return this.options.barColors.call(this, r, s, type);
            } else {
                return this.options.barColors[sidx % this.options.barColors.length];
            }
        };

        bar.prototype.hitTest = function(x) {
            if (this.data.length === 0) {
                return null;
            }
            x = Math.max(Math.min(x, this.right), this.left);
            return Math.min(this.data.length - 1, Math.floor((x - this.left) / (this.width / this.data.length)));
        };

        bar.prototype.onGridClick = function(x, y) {
            var index;
            index = this.hitTest(x);
            return this.fire('click', index, this.data[index].src, x, y);
        };

        bar.prototype.onHoverMove = function(x, y) {
            var index, ref;
            index = this.hitTest(x);
            return (ref = this.hover).update.apply(ref, this.hoverContentForRow(index));
        };

        bar.prototype.onHoverOut = function() {
            if (this.options.hideHover !== false) {
                return this.hover.hide();
            }
        };

        bar.prototype.hoverContentForRow = function(index) {
            var content, j, row, x, y, i, len, ref;
            row = this.data[index];
            content = "<div class='morris-hover-row-label'>" + row.label + "</div>";
            ref = row.y;
            for (j = i = 0, len = ref.length; i < len; j = ++i) {
                y = ref[j];
                content += "<div class='morris-hover-point' style='color: " + (this.colorFor(row, j, 'label')) + "'>\n  " + this.options.labels[j] + ":\n  " + (this.yLabelFormat(y)) + "\n</div>";
            }
            if (typeof this.options.hoverCallback === 'function') {
                content = this.options.hoverCallback(index, this.options, content, row.src);
            }
            x = this.left + (index + 0.5) * this.width / this.data.length;
            return [content, x];
        };

        bar.prototype.drawXAxisLabel = function(xPos, yPos, text) {
            var label;
            return label = this.raphael.text(xPos, yPos, text).attr('font-size', this.options.gridTextSize).attr('font-family', this.options.gridTextFamily).attr('font-weight', this.options.gridTextWeight).attr('fill', this.options.gridTextColor);
        };

        bar.prototype.drawBar = function(xPos, yPos, width, height, barColor, opacity, radiusArray) {
            var maxRadius, path;
            maxRadius = Math.max.apply(Math, radiusArray);
            if (maxRadius === 0 || maxRadius > height) {
                path = this.raphael.rect(xPos, yPos, width, height);
            } else {
                path = this.raphael.path(this.roundedRect(xPos, yPos, width, height, radiusArray));
            }
            return path.attr('fill', barColor).attr('fill-opacity', opacity).attr('stroke', 'none');
        };

        bar.prototype.roundedRect = function(x, y, w, h, r) {
            if (r == null) {
                r = [0, 0, 0, 0];
            }
            return ["M", x, r[0] + y, "Q", x, y, x + r[0], y, "L", x + w - r[1], y, "Q", x + w, y, x + w, y + r[1], "L", x + w, y + h - r[2], "Q", x + w, y + h, x + w - r[2], y + h, "L", x + r[3], y + h, "Q", x, y + h, x, y + h - r[3], "Z"];
        };

        return bar;

    })(morris.Grid);

    morris.Donut = (function(_super) {
        __extends(donut, _super);

        donut.prototype.defaults = {
            colors: ['#0B62A4', '#3980B5', '#679DC6', '#95BBD7', '#B0CCE1', '#095791', '#095085', '#083E67', '#052C48', '#042135'],
            backgroundColor: '#FFFFFF',
            labelColor: '#000000',
            formatter: morris.commas,
            resize: false
        };

        function donut(options) {
            this.resizeHandler = bind(this.resizeHandler, this);
            this.select = bind(this.select, this);
            this.click = bind(this.click, this);
            var _this = this;
            if (!(this instanceof morris.Donut)) {
                return new morris.Donut(options);
            }
            this.options = $.extend({}, this.defaults, options);
            if (typeof options.element === 'string') {
                this.el = $(document.getElementById(options.element));
            } else {
                this.el = $(options.element);
            }
            if (this.el === null || this.el.length === 0) {
                throw new Error("Graph placeholder not found.");
            }
            if (options.data === void 0 || options.data.length === 0) {
                return;
            }
            this.raphael = new Raphael(this.el[0]);
            if (this.options.resize) {
                $(window).bind('resize', function(evt) {
                    if (_this.timeoutId != null) {
                        window.clearTimeout(_this.timeoutId);
                    }
                    return _this.timeoutId = window.setTimeout(_this.resizeHandler, 100);
                });
            }
            this.setData(options.data);
        }

        donut.prototype.redraw = function() {
            var c, cx, cy, i, idx, last, maxValue, min, next, seg, total, value, w, _i, j, k, len, len1, len2, ref, ref1, ref2, results;
            this.raphael.clear();
            cx = this.el.width() / 2;
            cy = this.el.height() / 2;
            w = (Math.min(cx, cy) - 10) / 3;
            total = 0;
            ref = this.values;
            for (_i = 0, len = ref.length; _i < len; _i++) {
                value = ref[_i];
                total += value;
            }
            min = 5 / (2 * w);
            c = 1.9999 * Math.PI - min * this.data.length;
            last = 0;
            idx = 0;
            this.segments = [];
            ref1 = this.values;
            for (i = j = 0, len1 = ref1.length; j < len1; i = ++j) {
                value = ref1[i];
                next = last + min + c * (value / total);
                seg = new morris.DonutSegment(cx, cy, w * 2, w, last, next, this.data[i].color || this.options.colors[idx % this.options.colors.length], this.options.backgroundColor, idx, this.raphael);
                seg.render();
                this.segments.push(seg);
                seg.on('hover', this.select);
                seg.on('click', this.click);
                last = next;
                idx += 1;
            }
            this.text1 = this.drawEmptyDonutLabel(cx, cy - 10, this.options.labelColor, 15, 800);
            this.text2 = this.drawEmptyDonutLabel(cx, cy + 10, this.options.labelColor, 14);
            maxValue = Math.max.apply(Math, this.values);
            idx = 0;
            ref2 = this.values;
            results = [];
            for (k = 0, len2 = ref2.length; k < len2; k++) {
                value = ref2[k];
                if (value === maxValue) {
                    this.select(idx);
                    break;
                }
                results.push(idx += 1);
            }
            return results;
        };

        donut.prototype.setData = function(data) {
            var row;
            this.data = data;
            this.values = (function() {
                var i, len, ref, results;
                ref = this.data;
                results = [];
                for (i = 0, len = ref.length; i < len; i++) {
                    row = ref[i];
                    results.push(parseFloat(row.value));
                }
                return results;
            }).call(this);
            return this.redraw();
        };

        donut.prototype.click = function(idx) {
            return this.fire('click', idx, this.data[idx]);
        };

        donut.prototype.select = function(idx) {
            var row, s, segment, i, len, ref;
            ref = this.segments;
            for (i = 0, len = ref.length; i < len; i++) {
                s = ref[i];
                s.deselect();
            }
            segment = this.segments[idx];
            segment.select();
            row = this.data[idx];
            return this.setLabels(row.label, this.options.formatter(row.value, row));
        };

        donut.prototype.setLabels = function(label1, label2) {
            var inner, maxHeightBottom, maxHeightTop, maxWidth, text1Bbox, text1Scale, text2Bbox, text2Scale;
            inner = (Math.min(this.el.width() / 2, this.el.height() / 2) - 10) * 2 / 3;
            maxWidth = 1.8 * inner;
            maxHeightTop = inner / 2;
            maxHeightBottom = inner / 3;
            this.text1.attr({
                text: label1,
                transform: ''
            });
            text1Bbox = this.text1.getBBox();
            text1Scale = Math.min(maxWidth / text1Bbox.width, maxHeightTop / text1Bbox.height);
            this.text1.attr({
                transform: "S" + text1Scale + "," + text1Scale + "," + (text1Bbox.x + text1Bbox.width / 2) + "," + (text1Bbox.y + text1Bbox.height)
            });
            this.text2.attr({
                text: label2,
                transform: ''
            });
            text2Bbox = this.text2.getBBox();
            text2Scale = Math.min(maxWidth / text2Bbox.width, maxHeightBottom / text2Bbox.height);
            return this.text2.attr({
                transform: "S" + text2Scale + "," + text2Scale + "," + (text2Bbox.x + text2Bbox.width / 2) + "," + text2Bbox.y
            });
        };

        donut.prototype.drawEmptyDonutLabel = function(xPos, yPos, color, fontSize, fontWeight) {
            var text;
            text = this.raphael.text(xPos, yPos, '').attr('font-size', fontSize).attr('fill', color);
            if (fontWeight != null) {
                text.attr('font-weight', fontWeight);
            }
            return text;
        };

        donut.prototype.resizeHandler = function() {
            this.timeoutId = null;
            this.raphael.setSize(this.el.width(), this.el.height());
            return this.redraw();
        };

        return donut;

    })(morris.EventEmitter);

    morris.DonutSegment = (function(_super) {
        __extends(donutSegment, _super);

        function donutSegment(cx, cy, inner, outer, p0, p1, color, backgroundColor, index, raphael) {
            this.cx = cx;
            this.cy = cy;
            this.inner = inner;
            this.outer = outer;
            this.color = color;
            this.backgroundColor = backgroundColor;
            this.index = index;
            this.raphael = raphael;
            this.deselect = bind(this.deselect, this);
            this.select = bind(this.select, this);
            this.sin_p0 = Math.sin(p0);
            this.cos_p0 = Math.cos(p0);
            this.sin_p1 = Math.sin(p1);
            this.cos_p1 = Math.cos(p1);
            this.is_long = (p1 - p0) > Math.PI ? 1 : 0;
            this.path = this.calcSegment(this.inner + 3, this.inner + this.outer - 5);
            this.selectedPath = this.calcSegment(this.inner + 3, this.inner + this.outer);
            this.hilight = this.calcArc(this.inner);
        }

        donutSegment.prototype.calcArcPoints = function(r) {
            return [this.cx + r * this.sin_p0, this.cy + r * this.cos_p0, this.cx + r * this.sin_p1, this.cy + r * this.cos_p1];
        };

        donutSegment.prototype.calcSegment = function(r1, r2) {
            var ix0, ix1, iy0, iy1, ox0, ox1, oy0, oy1, ref, ref1;
            ref = this.calcArcPoints(r1), ix0 = ref[0], iy0 = ref[1], ix1 = ref[2], iy1 = ref[3];
            ref1 = this.calcArcPoints(r2), ox0 = ref1[0], oy0 = ref1[1], ox1 = ref1[2], oy1 = ref1[3];
            return ("M" + ix0 + "," + iy0) + ("A" + r1 + "," + r1 + ",0," + this.is_long + ",0," + ix1 + "," + iy1) + ("L" + ox1 + "," + oy1) + ("A" + r2 + "," + r2 + ",0," + this.is_long + ",1," + ox0 + "," + oy0) + "Z";
        };

        donutSegment.prototype.calcArc = function(r) {
            var ix0, ix1, iy0, iy1, ref;
            ref = this.calcArcPoints(r), ix0 = ref[0], iy0 = ref[1], ix1 = ref[2], iy1 = ref[3];
            return ("M" + ix0 + "," + iy0) + ("A" + r + "," + r + ",0," + this.is_long + ",0," + ix1 + "," + iy1);
        };

        donutSegment.prototype.render = function() {
            var _this = this;
            this.arc = this.drawDonutArc(this.hilight, this.color);
            return this.seg = this.drawDonutSegment(this.path, this.color, this.backgroundColor, function() {
                return _this.fire('hover', _this.index);
            }, function() {
                return _this.fire('click', _this.index);
            });
        };

        donutSegment.prototype.drawDonutArc = function(path, color) {
            return this.raphael.path(path).attr({
                stroke: color,
                'stroke-width': 2,
                opacity: 0
            });
        };

        donutSegment.prototype.drawDonutSegment = function(path, fillColor, strokeColor, hoverFunction, clickFunction) {
            return this.raphael.path(path).attr({
                fill: fillColor,
                stroke: strokeColor,
                'stroke-width': 3
            }).hover(hoverFunction).click(clickFunction);
        };

        donutSegment.prototype.select = function() {
            if (!this.selected) {
                this.seg.animate({
                    path: this.selectedPath
                }, 150, '<>');
                this.arc.animate({
                    opacity: 1
                }, 150, '<>');
                return this.selected = true;
            }
        };

        donutSegment.prototype.deselect = function() {
            if (this.selected) {
                this.seg.animate({
                    path: this.path
                }, 150, '<>');
                this.arc.animate({
                    opacity: 0
                }, 150, '<>');
                return this.selected = false;
            }
        };

        return donutSegment;

    })(morris.EventEmitter);

}).call(this);