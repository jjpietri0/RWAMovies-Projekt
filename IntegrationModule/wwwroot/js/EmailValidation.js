var url = window.location.href;
var username = url.split('?')[1].split('&')[0].split('=')[1];
var b64Token = url.split('?')[1].split('&')[1].split('=')[1];
document.getElementById("username").value = username;
document.getElementById("b64Token").value = b64Token;

var submitButton = document.getElementById("submitBtn");

submitButton.addEventListener("click", function () {
    fetch("/api/Users/ValidateEmail", {
method: "POST",
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            "Username": username,
            "B64Token": b64Token
        })
    }).then(function (response) {
        if (response.ok) {
            alert("Email successfully validated. You can safely close this window.");
        }
        else {
            alert("Email validation failed");
        }
    }).catch(function (error) {
        console.log(error);
    });

});
