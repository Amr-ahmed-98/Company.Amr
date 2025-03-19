// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var SearchInput = document.getElementById("SearchInput");
SearchInput.addEventListener("keyup", () => {

    let xhr = new XMLHttpRequest();

    let url = 'https://localhost:44368/Employee?SearchInput=${SearchInput.value}';

    xhr.open("GET", url, true);

    xhr.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            console.log(this.responseText);
        }
    }

    xhr.send();
}
});