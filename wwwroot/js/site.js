(function ($) {
    "use strict";

    document.querySelectorAll("[data-password-toggle]").forEach(function (button) {
        button.addEventListener("click", function () {
            var input = button.parentElement.querySelector("input");
            var visible = input.type === "password";
            input.type = visible ? "text" : "password";
            button.textContent = visible ? "Жасыру" : "Көрсету";
            button.setAttribute("aria-label", visible ? "Құпиясөзді жасыру" : "Құпиясөзді көрсету");
        });
    });
})(window.jQuery);
