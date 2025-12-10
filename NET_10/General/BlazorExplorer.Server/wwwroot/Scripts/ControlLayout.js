function toggleTabPanelVisibility(tabs, panels, i) {
    var activeID = 0, j;
    for (j = 0; j < tabs.length; j++) {
        if (tabs[j].parentElement.className.indexOf('active') >= 0) {
            activeID = j;
            break;
        }
    }
    if (activeID != i) {
        removeClass(tabs[activeID].parentElement, 'active');
        removeClass(panels[activeID], 'active');
        addClass(tabs[i].parentElement, 'active');
        addClass(panels[i], 'active');
    }
}

function addEventListenerForTab(tabs, panels, i) {
    tabs[i].addEventListener('click', function (event) {
        toggleTabPanelVisibility(tabs, panels, i);

        if (tabs[i].id !== 'docs') {
            event.preventDefault();
        }
    });
}

function createTabs(tabId, panelId) {
    var tabCon = document.getElementById(tabId),
        tabs, panel, panels, i, className;

    if (tabCon == null) {
        return;
    }

    tabs = tabCon.getElementsByTagName('a');
    panel = document.getElementById(panelId);
    panels = new Array();

    for (i = 0; i < panel.children.length; i++) {
        className = panel.children[i].className;
        if (className && className.indexOf('tab-pane') >= 0) {
            panels.push(panel.children[i]);
        }
    }
    addClass(tabs[0].parentElement, 'active');
    addClass(panels[0], 'active');
    for (i = 0; i < tabs.length; i++) {
        addEventListenerForTab(tabs, panels, i);
    }
}

function createCollapseStruct(titleEle, toggleClassName) {
    titleEle.addEventListener('click', function (event) {
        var list = event.currentTarget.nextElementSibling;
        if (list) {
            toggleClass(list, toggleClassName);
            event.preventDefault();
        }
    });
}

function initTabs() {
  

  SyntaxHighlighter.all();
  
    SyntaxHighlighter.defaults['toolbar'] = false;
    SyntaxHighlighter.defaults['quick-code'] = false;
    //var titles = document.getElementsByClassName('collapse-title'),
        //sampleNavBtn = document.getElementById('sampleNavBtn');
  
}

function intFeaturesNav() {
  
    var featuresNavBtn = document.querySelector('.features-bar');
	var featuresNav = featuresNavBtn.querySelector('ul');
  
    new c1.sample.MultiLevelMenu(featuresNav, featuresNavBtn);
}

function findPage(list, actionName) {
    if (list) {
        for (var i = 0; i < list.length; i++) {
            var page = list[i];
            var lActionName = page.Name.toLowerCase();
            if (lActionName == actionName.toLowerCase()) {
                return page;
            }

            if (page.SubPages) {
                var subPage = findPage(page.SubPages, actionName);
                if (subPage != null) {
                    return subPage;
                }
            }
        }
    }
}

function gotoDoc(url, event) {
    window.open(url, '_blank');
    event.preventDefault();
    event.stopPropagation();
    return false;
}

function fillSummary() {
    var element = document.querySelector('.description');
    if (!element || element.textContent) return;
    var demoDes = document.querySelector('.demo-description>div');
    if (!demoDes) return;
    var demoText = demoDes.textContent || '';
    var findEnd = function (text) {
        var index = demoText.indexOf(text);
        if (index == -1) return false;
        element.textContent = demoText.substring(0, index + 1);
        return true;
    };
    if (findEnd('. ')) return;
    if (findEnd('.\r')) return;
    if (findEnd('.\n')) return;
    if (findEnd('。 ')) return;
    if (findEnd('。\r')) return;
    if (findEnd('。\n')) return;
}

InitControlLayout = function () {
    fillSummary();
    initTabs();
    intFeaturesNav();
};

function initControlsNav() {
    var controlsNav = document.querySelector('.controls-nav');
    for (var i = 0; i < controlsNav.children.length; i++) {
	    var li = controlsNav.children[i];	
	    if (!li.tagName || li.tagName.toLowerCase() !== 'li') {
	        continue;
	    }	

        var title = li.querySelector('h3'),
            content = li.querySelector('ul');
	        //downIcon = document.createElement('span');
	    //downIcon.className = 'arrow-down';
	    title.style.cursor = 'pointer';
	    title.innerHTML += ' ';
	    //title.appendChild(downIcon);
        
        var setStatus = function (uli, expand, collapseId) {
            collapseId = collapseId || 0;
            var toggleClass = ["collapse-none", "collapse-slidev", "expand-slidev"];
            var currentId = 0;
            for (var currentId = 0; currentId < 3; currentId++) {
                if (uli.classList.contains(toggleClass[currentId]))
                    break;
            }
            if (expand != true && expand != false) {
                expand = (currentId < 2 || collapseId == 2);
            }
            if (currentId == 3) { //not found
                uli.classList.add(toggleClass[collapseId]);
                currentId = collapseId;
            }
            uli.classList.replace(toggleClass[currentId], toggleClass[expand ? 2 : collapseId]);
            var downIcon = uli.parentNode.querySelector("span");
            if (downIcon) downIcon.className = expand ? "arrow-up" : "arrow-down";

            return expand;
        }
        
	    title.addEventListener('click', function () {
            var uli = this.parentElement.querySelector('ul');
            if (setStatus(uli, null, 1)) { //toggle => expand
                var uls = controlsNav.querySelectorAll('ul');
                uls.forEach(function (uli1) {
                    if (uli1 !== uli)
                        setStatus(uli1, false);
                });
            }
        }, false);

        setStatus(content, null, i==0 ? 2 : 0);
    }    
}

highlightSource = function (source, brush, sourceContentId) {
    //SyntaxHighlighter.defaults['toolbar'] = false;
    //SyntaxHighlighter.defaults['quick-code'] = false;
    //var titles = document.getElementsByClassName('collapse-title'),
    //sampleNavBtn = document.getElementById('sampleNavBtn');
    var elm = document.getElementById(sourceContentId);
    if (elm) {
    var pre = document.createElement("pre");
    pre.className = brush;
    pre.textContent = source;    
    if (elm.firstChild)
        elm.replaceChild(pre, elm.firstChild)
    else elm.appendChild(pre);    
	SyntaxHighlighter.highlight();
    }
}

imgOnerror = function (img, alts, path) {
    if (alts.trim() == "") return;
    path = path || "";
    var a = alts.split(",");
    var src = path + "/" + a.pop();
    if (alts.length == 1) {
        img.onerror = null;
    } else {
        img.onerror = 'imgOnerror(this,"' + a.join(',') + '","' + path + '");';
    }
    img.src = src;
    //onerror = "this.onerror=null;this.src='@_App.IconPath/Unknown.png';"
}
