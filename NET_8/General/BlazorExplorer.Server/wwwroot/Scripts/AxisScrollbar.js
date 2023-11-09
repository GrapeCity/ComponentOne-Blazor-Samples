var wijmo;
(function (wijmo) {
    (function (_chart) {
        (function (interaction) {
            'use strict';

            /**
            * The @see:AxisScrollbar control displays a scrollbar that allows the user to
            * choose the range of Axis.
            
            */
            var AxisScrollbar = (function () {
                /**
                * Initializes a new instance of the @see:AxisScrollbar control.
                *
                * @param axis The axis that displays scrollbar.
                * @param options A JavaScript object containing initialization data for the control.
                */
                function AxisScrollbar(axis, options) {
                    // fields
                    this._isVisible = true;
                    this._min = null;
                    this._max = null;
                    this._buttonsVisible = true;
                    this._minScale = 0.01;
                    // host
                    this._chart = null;
                    this._axis = null;
                    this._rangeSlider = null;
                    // elements
                    this._slbarContainer = null;
                    this._isXAxis = true;
                    this._chartRefreshDelay = null;
                    if (!chart) {
                        wijmo.assert(false, 'The Axis cannot be null.');
                    }

                    this._axis = axis;
                    this._chart = axis._chart;
                    this._isXAxis = this._axis.axisType === 0 /* X */;
                    wijmo.copy(this, options);
                    this._createScrollbar();
                }
                Object.defineProperty(AxisScrollbar.prototype, "buttonsVisible", {
                    /**
                    * Gets or sets the decrease button and increase button is visible or not.
                    */
                    get: function () {
                        return this._buttonsVisible;
                    },
                    set: function (value) {
                        if (value !== this._buttonsVisible) {
                            this._buttonsVisible = wijmo.asBoolean(value);
                            if (!this._rangeSlider) {
                                return;
                            }
                            this._rangeSlider.buttonsVisible = this._buttonsVisible;
                        }
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(AxisScrollbar.prototype, "isVisible", {
                    /**
                    * Gets or sets the visibility of the Axis scrollbar.
                    */
                    get: function () {
                        return this._isVisible;
                    },
                    set: function (value) {
                        if (value != this._isVisible) {
                            this._isVisible = wijmo.asBoolean(value);
                            if (!this._rangeSlider) {
                                return;
                            }
                            this._rangeSlider.isVisible = value;
                        }
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(AxisScrollbar.prototype, "minPos", {
                    set: function (value) {
                        if (value < 0 && value > 1 && value > this._rangeSlider._maxPos) {
                            return;
                        }
                        this._rangeSlider._minPos = value;
                        this._updateAxisRange();
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(AxisScrollbar.prototype, "maxPos", {
                    set: function (value) {
                        if (value < 0 && value > 1 && value < this._rangeSlider._minPos) {
                            return;
                        }
                        this._rangeSlider._maxPos = value;
                        this._updateAxisRange();
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(AxisScrollbar.prototype, "minScale", {
                    /**
                    * Gets or sets the minimum range scale of the Axis scrollbar.
                    * The minimum range scale should be greater than zero.
                    */
                    get: function () {
                        return this._minScale;
                    },
                    set: function (value) {
                        if (value > 0 && value != this._minScale) {
                            this._minScale = wijmo.asNumber(value);
                            if (!this._rangeSlider) {
                                return;
                            }
                            this._rangeSlider.minScale = value;
                        }
                    },
                    enumerable: true,
                    configurable: true
                });

                /**
                * Removes the range selector from the chart.
                */
                AxisScrollbar.prototype.remove = function () {
                    if (this._slbarContainer) {
                        this._chart.hostElement.removeChild(this._slbarContainer);
                        this._switchEvent(false);
                        this._slbarContainer = null;
                    }
                };

                AxisScrollbar.prototype._createScrollbar = function () {
                    var chart = this._chart, chartHostEle = chart.hostElement;

                    this._slbarContainer = wijmo.createElement('<div class="wj-axis-scrollbar-container"></div>');
                    this._rangeSlider = new interaction._RangeSlider(this._slbarContainer, true, true, {
                        buttonsVisible: this._buttonsVisible,
                        isHorizontal: this._isXAxis,
                        isVisible: this._isVisible,
                        minScale: this._minScale
                    });
                    chartHostEle.appendChild(this._slbarContainer);
                    this._switchEvent(true);
                };

                AxisScrollbar.prototype._switchEvent = function (isOn) {
                    var eventListener = isOn ? 'addEventListener' : 'removeEventListener', eventHandler = isOn ? 'addHandler' : 'removeHandler';

                    if (this._chart.hostElement) {
                        this._chart.rendered[eventHandler](this._refresh, this);
                        this._rangeSlider.rangeChanged[eventHandler](this._updateAxisRange, this);
                        this._rangeSlider.rangeChanging[eventHandler](this._updatingAxisRange, this);
                    }
                };

                AxisScrollbar.prototype._refresh = function () {
                    var chartHostEle = this._chart.hostElement, rangeSlider = this._rangeSlider, pa, pOffset, plotBox, axisRect = this._axis._axrect, axisEle, axisOffset, isBottom, isLeft, rsWidth, rOffset = wijmo.getElementRect(this._slbarContainer);

                    if (rangeSlider._isSliding) {
                        return;
                    }

                    //init the scrollbar range
                    if (this._min === null) {
                        this._min = wijmo.isDate(this._axis.actualMin) ? this._axis.actualMin.valueOf() : this._axis.actualMin;
                    }
                    if (this._max === null) {
                        this._max = wijmo.isDate(this._axis.actualMax) ? this._axis.actualMax.valueOf() : this._axis.actualMax;
                    }

                    pa = chartHostEle.querySelector('.' + _chart.FlexChart._CSS_PLOT_AREA);
                    pOffset = wijmo.getElementRect(pa);
                    plotBox = pa.getBBox(pa);

                    //TODO: get the axis element when has multiple axes
                    axisEle = chartHostEle.querySelector(this._isXAxis ? '.wj-axis-x' : '.wj-axis-y');
                    axisOffset = wijmo.getElementRect(axisEle);
                    if (axisOffset.height === 0) {
                        return;
                    }

                    if (this._isXAxis) {
                        isBottom = this._axis.position === 4 /* Bottom */;

                        rangeSlider._refresh({
                            left: plotBox.x,
                            top: isBottom ? axisOffset.top + axisOffset.height - rOffset.top : axisOffset.top - rOffset.top - axisOffset.height,
                            width: plotBox.width
                        });
                    } else {
                        isLeft = this._axis.position === 1 /* Left */;
                        rsWidth = rangeSlider._handleWidth;

                        rangeSlider._refresh({
                            left: isLeft ? axisOffset.left - rOffset.left - rsWidth : axisOffset.left - rOffset.left + pOffset.width + this._axis._axrect.width,
                            top: pOffset.top - rOffset.top,
                            height: plotBox.height
                        });
                    }
                };

                AxisScrollbar.prototype._updateAxisRange = function () {
                    var chart, axis, range, rangeSlider = this._rangeSlider;

                    chart = this._chart;
                    axis = this._axis;
                    range = this._max - this._min;

                    axis.min = this._min + rangeSlider._minPos * range;
                    axis.max = this._min + rangeSlider._maxPos * range;

                    chart.invalidate();
                };

                AxisScrollbar.prototype._updatingAxisRange = function () {
                    var self = this;
                    this._chartRefreshDelay = window.setTimeout(function () {
                        if (self._chartRefreshDelay) {
                            clearTimeout(self._chartRefreshDelay);
                            self._chartRefreshDelay = null;
                        }
                        self._updateAxisRange();
                    }, 200);
                };
                return AxisScrollbar;
            })();
            interaction.AxisScrollbar = AxisScrollbar;
        })(_chart.interaction || (_chart.interaction = {}));
        var interaction = _chart.interaction;
    })(wijmo.chart || (wijmo.chart = {}));
    var chart = wijmo.chart;
})(wijmo || (wijmo = {}));
//# sourceMappingURL=AxisScrollbar.js.map
