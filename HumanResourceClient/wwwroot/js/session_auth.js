window.onload = function () {
    if (!localStorage.getItem('token')) {
        window.location.href = "/login";
    }

    setTimeout(showPage, 200);

    function showPage() {
        document.getElementById("loading-screen").style.display = "none";
    }
};

function Logout() {
    window.localStorage.clear();
    window.location.assign("https://localhost:44396/login");
}