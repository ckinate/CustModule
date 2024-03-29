function initializeInactivityTimer(dotnetHelper, duration) {
    var timer;
    document.onmousemove = resetTimer;
    document.onkeypress = resetTimer;

    function resetTimer() {
        clearTimeout(timer);
        timer = setTimeout(logout, duration);
    }

    function logout() {
        dotnetHelper.invokeMethodAsync("Logout");
    }
}