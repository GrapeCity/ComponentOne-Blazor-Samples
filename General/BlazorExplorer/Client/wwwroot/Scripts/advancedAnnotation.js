var axisXScrollbar, chart, al, data;

c1.documentReady(function () {
    if (!chart) {
        chart = wijmo.Control.getControl("#anChart");
    }

    if (!al) {
        al = getAnnotationLayer(chart);
    }

    if (!data) {
        data = chart.itemsSource.sourceCollection;
    }

    if (!axisXScrollbar) {
        axisXScrollbar = new wijmo.chart.interaction.AxisScrollbar(chart.axes[0]);
        chart.refresh();
        window.setTimeout(function () {
            axisXScrollbar.maxPos = 0.5;
            axisXScrollbar.minScale = 0.02;
        }, 20);
    }

    chart.hostElement['ontouchmove'] = function (e) {
        setQuoteDetailInfo(e);
    };
    chart.hostElement['onmousemove'] = function (e) {
        setQuoteDetailInfo(e);
    };
    chart.hostElement['onmouseleave'] = function (e) {
        clearDetail();
    };
});

function getAnnotationLayer(chart) {
    return c1.getExtender(chart, "AnnotationLayer");
}

function setQuoteDetailInfo(e) {
    if (al == null) return;
    var series = chart.series[0], hitTest, itmSource,
        point = e instanceof MouseEvent ?
    new wijmo.Point(e.pageX, e.pageY) :
    new wijmo.Point(e.changedTouches[0].pageX, e.changedTouches[0].pageY);
    if (!series) {
        return;
    }
    hitTest = series.hitTest(new wijmo.Point(point.x, NaN));
    if (hitTest == null || hitTest.x == null || hitTest.y == null) {
        return;
    }
    itmSource = data[hitTest.pointIndex];

    al.getItem('detailContainer').isVisible = true;
    al.getItem('detailLow').text = 'Low: ' + itmSource.Low;
    al.getItem('detailHigh').text = 'High: ' + itmSource.High;
    al.getItem('detailOpen').text = 'Open: ' + itmSource.Open;
    al.getItem('detailClose').text = 'Close: ' + itmSource.Close;
    al.getItem('detailVolume').text = 'Volume: ' + itmSource.Volume;
}

function clearDetail() {
    if (al == null) return;

    al.getItem('detailContainer').isVisible = false;
    al.getItem('detailLow').text = '';
    al.getItem('detailHigh').text = '';
    al.getItem('detailOpen').text = '';
    al.getItem('detailClose').text = '';
    al.getItem('detailVolume').text = '';
}

function updateLastPoint() {
    var content, maxItm, maxLineItm, len = data.length,
        maxDate = Math.ceil(chart.axisX.max);
    maxDate = fromOADate(maxDate);

    if (al && al.items) {
        maxItm = al.getItem('endPrice');
        maxLineItm = al.getItem('endPriceLine');
        if (!maxItm || !maxLineItm) {
            return;
        }
        for (var i = 0; i < len; i++) {
            if (i === len - 1 || data[i].date.getTime() === maxDate.getTime()) {
                content = data[i].close;
                break;
            } else if (i < len - 1 && maxDate.getTime() > data[i].date.getTime() &&
                maxDate.getTime() < data[i + 1].date.getTime()) {
                content = data[i + 1].close;
                break;
            }
        }
        if (!content) {
            maxItm.isVisible = false;
            maxLineItm.isVisible = false;
        } else {
            maxItm.isVisible = true;
            maxItm.content = content;
            maxItm.point = { x: maxDate, y: content };
            maxLineItm.isVisible = true;
            maxLineItm.start = { x: new Date(2014, 3, 10, 0, 0, 0), y: content };
            maxLineItm.end = { x: maxDate, y: content };
        }
    }
}

function fromOADate(val) {
    var dec = val - Math.floor(val);
    if (val < 0 && dec) {
        val = Math.floor(val) - dec;
    }
    return new Date(val * 86400000 + new Date(1899, 11, 30).getTime());
}
