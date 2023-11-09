/* * By: James Tran - GC, 11/2019 * */
(function () {
    var attachEvent = document.attachEvent,
        resizeObserver = window.ResizeObserver;
    
    function _onresize(e) {
        var view = e.target || e.srcElement || e[0].target, 
            rsz = view.__resize__,
            elm = rsz.element;
        if (elm.offsetWidth != rsz.offsetWidth || elm.offsetHeight != rsz.offsetHeight) {
            rsz.offsetWidth = elm.offsetWidth;
            rsz.offsetHeight = elm.offsetHeight;
            rsz.listeners.forEach(function (fn) {
                fn.call(e, rsz);
            });
        }
    }

    window.addResizeObserver = function (element, fn) {
        var rsz = element.__resize__;
        if (!rsz) {
            rsz = element.__resize__ = { listeners: [], trigger: element, element: element, offsetWidth:0, offsetHeight:0 };
            if (attachEvent) {
                element.attachEvent('onresize', _onresize);
            } else if (resizeObserver) {
                (rsz.trigger = new resizeObserver(_onresize)).observe(element);
            } else {
                var obj = rsz.trigger = document.createElement('object');
                obj.setAttribute('style', 'visibility: hidden; display: block; position: absolute; left: 0; top: 0; width: 100%; height: 100%; overflow: hidden; pointer-events: none; z-index: -1;');
                obj.type = 'text/html';
                obj.onload = function () {
                    var view = this.contentDocument.defaultView;
                    view.__resize__ = rsz;
                    view.addEventListener('resize', _onresize);
                }
                obj.data = 'about:blank';
                element.appendChild(obj);
            }
        }
        rsz.listeners.push(fn);
    };

    window.removeResizeObserver = function (element, fn) {
        var rsz = element.__resize__;
        if (rsz) {
            rsz.listeners.splice(rsz.listeners.indexOf(fn), 1);
            if (rsz.listeners.length == 0) {
                if (attachEvent) {
                    element.detachEvent('onresize', _onresize);
                } else if (resizeObserver) {
                    rsz.trigger.unobserve(element);
                } else {
                    rsz.trigger.contentDocument.defaultView.removeEventListener('resize', _onresize);
                    element.removeChild(rsz.trigger);
                }
                element.__resize__ = null;
            }
        }
    }
})();