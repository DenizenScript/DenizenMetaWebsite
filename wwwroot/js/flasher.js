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
