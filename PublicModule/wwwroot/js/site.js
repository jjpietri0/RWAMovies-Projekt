// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function logout() {
    sessionStorage.removeItem("AccessToken");
    sessionStorage.removeItem("Username");
    sessionStorage.removeItem("ResponseId");
    window.location.href = "/";
}