function flashElement(elementName) {
    var element = document.getElementById(elementName);
    element.classList.add("flash");
    setTimeout(function() {
        element.classList.remove("flash");
        if (element.id == "flasher_tool_generated") {
            element.id = "flash_dead";
            element.parentNode.removeChild(element);
        }
    }, 2100);
}
function doFlashFor(element) {
    var flashable = document.getElementById(element);
    if (!flashable) {
        return;
    }
    if (!document.getElementById("flasher_tool_generated")) {
        var flasher = document.createElement("SPAN");
        flasher.id = "flasher_tool_generated";
        flasher.classList.add("flasher_tool");
        flashable.appendChild(flasher);
        flashElement("flasher_tool_generated");
    }
    setTimeout(function() {
        window.scroll(0, flashable.getBoundingClientRect().top + window.pageYOffset - window.innerHeight / 2);
    }, 10);
}
function autoflash() {
    var target = window.location.hash.substring(1);
    if (!target) {
        return;
    }
    doFlashFor(target);
}
function flashElement(elementName) {
    var element = document.getElementById(elementName);
    element.classList.add("flash");
    setTimeout(function() { element.classList.remove("flash"); }, 2100);
}
function generateFlasher(element) {
    var flasher = document.createElement("SPAN");
    flasher.classList.add("flasher_tool");
    element.appendChild(flasher);
    flasher.classList.add("flash");
    setTimeout(function() { flasher.remove(); }, 2100);
}
function autoflash() {
    var target = window.location.hash.substring(1);
    if (!target) {
        return;
    }
    var flashable = document.getElementById(target);
    if (!flashable) {
        return;
    }
    generateFlasher(flashable);
    window.scroll(0, flashable.getBoundingClientRect().top + window.pageYOffset - window.innerHeight / 2);
}
$(function() {
    $('a[href*="#"]:not([href="#"])').click(function() {
        if (location.pathname.replace(/^\//, '') == this.pathname.replace(/^\//, '') && location.hostname == this.hostname) {
            var target = document.getElementById(this.hash.slice(1));
            if (target) {
                generateFlasher(target);
                var newY = target.getBoundingClientRect().top + window.pageYOffset - window.innerHeight / 2;
                $('html,body').animate({ scrollTop: newY }, 500);
                return true;
            }
        }
    });
});
