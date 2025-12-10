function hasClass(obj, cls) {
    return obj && obj.className.match(new RegExp('(\\s|^)' + cls + '(\\s|$)'));
}

function addClass(obj, cls) {
    if (obj && !this.hasClass(obj, cls)) obj.className += ' ' + cls;
}

function removeClass(obj, cls) {
    if (obj && hasClass(obj, cls)) {
        var reg = new RegExp('(\\s|^)' + cls + '(\\s|$)');
        obj.className = obj.className.replace(reg, ' ');
    }
}

function toggleClass(obj, cls) {
    if (hasClass(obj, cls)) {
        removeClass(obj, cls);
    } else {
        addClass(obj, cls);
    }
}

function initHamburgerNav() {
    var hamburgerNavBtn = document.querySelector('.hamburger-nav-btn');
    var hamburgerNavEle = document.querySelector('.narrow-screen.site-nav');
    new c1.sample.MultiLevelMenu(hamburgerNavEle, hamburgerNavBtn);
}

function initTaskNav() {
    var taskNavBtn = document.querySelector('.task-bar>.semi-bold.narrow-screen');
    var taskNavEle = document.querySelector('.narrow-screen.task-bar-nav');
    new c1.sample.MultiLevelMenu(taskNavEle, taskNavBtn);
}

function initBreadcrumNav() {
    var controlNameEle = document.querySelector('.breadcrumb li.even a'),
        controlsNav = document.getElementById('controlsNav'),
        hideControlsNavTimer = null,
        showControlsGroupNav = function () {
            if (hideControlsNavTimer != null) {
                clearTimeout(hideControlsNavTimer);
                hideControlsNavTimer = null;
            }

            controlsNav.classList.remove('hide');
            wijmo.showPopup(controlsNav, controlNameEle);
        }, hideControlsGroupNav = function () {
            if (hideControlsNavTimer != null) {
                return;
            }

            hideControlsNavTimer = setTimeout(function () {
                wijmo.hidePopup(controlsNav);
                hideControlsNavTimer = null;
            }, 200);
        };

    if (controlNameEle) {
        controlNameEle.addEventListener('mouseover', showControlsGroupNav);
        controlNameEle.addEventListener('mouseout', hideControlsGroupNav);
    }

    if (controlsNav) {
        controlsNav.addEventListener('mouseover', showControlsGroupNav);
        controlsNav.addEventListener('mouseout', hideControlsGroupNav);
    }
}

c1.documentReady(function () {
    initHamburgerNav();
    initTaskNav();
    initBreadcrumNav();
});
