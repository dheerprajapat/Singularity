
window.makeItPressable = (el, dotnet) => {
    el.addEventListener('long-press', function (e) {
        dotnet.invokeMethodAsync("longPress");
    });
};



