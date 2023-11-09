var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var c1;
(function (c1) {
    var sample;
    (function (sample) {
        function _eventFrom(event, element) {
            return element && (element.contains(event.target) || event.target === element);
        }
        function _getAutomaticHeight(ele) {
            var name = '__AutomaticHeight';
            if (!ele[name]) {
                var html = ele.outerHTML;
                var copy = $(html);
                copy.width(0);
                copy.css('height', 'auto');
                copy.css('display', '');
                copy.appendTo(document.body);
                ele[name] = copy.height() + 'px';
                copy.remove();
            }
            return ele[name];
        }
        function _getDocumentWidth() {
            return $(document.body).width() + 'px';
        }
        function _slideDown(ele, refer) {
            ele.style.height = '0px';
            wijmo.showPopup(ele, refer);
            $(ele).animate({ height: _getAutomaticHeight(ele) });
        }
        function _slideUp(ele) {
            $(ele).animate({ height: '0px' }, undefined, function () {
                wijmo.hidePopup(ele);
                ele.style.height = 'auto';
            });
        }
        function _showFromLeft(ele, refer) {
            ele.style.visibility = 'hidden';
            wijmo.showPopup(ele, refer);
            ele.style.left = '-' + _getDocumentWidth();
            ele.style.visibility = '';
            $(ele).animate({ left: '0px' });
        }
        function _hideToLeft(ele) {
            var left = '-' + _getDocumentWidth();
            $(ele).animate({ left: left }, undefined, function () {
                wijmo.hidePopup(ele);
                ele.style.left = '0px';
            });
        }
        function _showFromRight(ele, refer) {
            ele.style.visibility = 'hidden';
            var docWidth = _getDocumentWidth();
            wijmo.showPopup(ele, refer);
            ele.style.left = docWidth;
            ele.style.visibility = '';
            $(ele).animate({ left: '0px' });
        }
        function _hideToRight(ele) {
            var docWidth = _getDocumentWidth();
            $(ele).animate({ left: docWidth }, undefined, function () {
                wijmo.hidePopup(ele);
                ele.style.left = '0px';
            });
        }
        var MultiLevelMenu = (function (_super) {
            __extends(MultiLevelMenu, _super);
            function MultiLevelMenu(ele, parent, trigger) {
                _super.call(this, ele);
                this._level = 0;
                this._childMenus = [];
                if (parent instanceof String) {
                    parent = document.querySelector(parent);
                }
                if (parent != null) {
                    this._parent = parent;
                    var control = wijmo.Control.getControl(parent);
                    if (control instanceof MultiLevelMenu) {
                        this._parentMenu = control;
                        this._refer = this._parentMenu.refer;
                        this._level = this._parentMenu.level + 1;
                    }
                    else {
                        this._refer = parent;
                    }
                }
                if (trigger instanceof String) {
                    trigger = document.querySelector(trigger);
                }
                this._trigger = trigger || this._refer;
                this._init();
            }
            Object.defineProperty(MultiLevelMenu.prototype, "refer", {
                get: function () {
                    return this._refer;
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(MultiLevelMenu.prototype, "level", {
                get: function () {
                    return this._level;
                },
                enumerable: true,
                configurable: true
            });
            MultiLevelMenu.prototype._init = function () {
                var _this = this;
                this.hostElement.style.display = 'none';
                document.addEventListener('click', this._documentClick.bind(this));
                if (this._trigger) {
                    this._trigger.style.cursor = 'pointer';
                    this._trigger.addEventListener('click', function () {
                        if (_this._parentMenu) {
                            _this.show(_this._parentMenu.level);
                        }
                        else if (_this.isShown) {
                            _this.hide();
                        }
                        else {
                            _this.show();
                        }
                    });
                }
                if (this.hostElement.tagName.toLowerCase() == 'ul') {
                    this._root = this.hostElement;
                }
                else {
                    this._root = this.hostElement.querySelector('ul');
                }
                if (this._parentMenu) {
                    var back = document.createElement('li');
                    back.innerHTML = '&lt; Back';
                    if (this._root.children[0]) {
                        this._root.insertBefore(back, this._root.children[0]);
                    }
                    else {
                        this._root.appendChild(back);
                    }
                    back.addEventListener('click', function (event) {
                        event['targetMenu'] = _this;
                        _this.hide(_this.level);
                    });
                }
                var children = this._root.childNodes || [];
                for (var i = 0, l = children.length; i < l; i++) {
                    var child = children[i];
                    if (!child || !child.tagName || child.tagName.toLowerCase() != 'li') {
                        continue;
                    }
                    var ul = child.querySelector('ul');
                    if (!ul) {
                        continue;
                    }
                    ul.className += ' ' + this._root.className;
                    var lis = ul.querySelectorAll('li');
                    if (!lis || !lis.length) {
                        continue;
                    }
                    if (child.children && child.children.length > 1) {
                        var title = child.children[0];
                        if (title && title.innerHTML) {
                            title.innerHTML += ' &gt;';
                        }
                    }
                    this._childMenus.push(new MultiLevelMenu(ul, this.hostElement, child));
                }
            };
            MultiLevelMenu.prototype._documentClick = function (event) {
                if (_eventFrom(event, this.hostElement) || _eventFrom(event, this._trigger)) {
                    return;
                }
                var menu = event['targetMenu'];
                if (menu && menu._parentMenu === this && _eventFrom(event, menu.hostElement)) {
                    return;
                }
                this.hide();
            };
            Object.defineProperty(MultiLevelMenu.prototype, "isShown", {
                get: function () {
                    var display = this.hostElement.style.display || '';
                    if (display.toLowerCase() != 'none') {
                        return true;
                    }
                    for (var i = 0, l = this._childMenus.length; i < l; i++) {
                        if (this._childMenus[i].isShown) {
                            return true;
                        }
                    }
                    return false;
                },
                enumerable: true,
                configurable: true
            });
            MultiLevelMenu.prototype.show = function (fromLevel) {
                if (this.isShown && fromLevel == null) {
                    return;
                }
                if (fromLevel != null) {
                    if (this._parentMenu && fromLevel <= this.level) {
                        this._parentMenu.hide(this.level);
                        _showFromRight(this.hostElement, this.refer);
                        return;
                    }
                    if (fromLevel > this.level) {
                        _showFromLeft(this.hostElement, this.refer);
                        return;
                    }
                }
                _slideDown(this.hostElement, this.refer);
            };
            MultiLevelMenu.prototype.hide = function (fromLevel) {
                if (!this.isShown && fromLevel != null) {
                    return;
                }
                if (fromLevel != null) {
                    if (this._parentMenu && fromLevel <= this.level) {
                        _hideToRight(this.hostElement);
                        this._parentMenu.show(this.level);
                        return;
                    }
                    if (fromLevel > this.level) {
                        _hideToLeft(this.hostElement);
                        return;
                    }
                }
                _slideUp(this.hostElement);
            };
            return MultiLevelMenu;
        }(wijmo.Control));
        sample.MultiLevelMenu = MultiLevelMenu;
    })(sample = c1.sample || (c1.sample = {}));
})(c1 || (c1 = {}));
//# sourceMappingURL=MultiLevelMenu.js.map