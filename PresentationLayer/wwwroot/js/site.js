// Ask before submitting any form marked with data-confirm (deletes, unenrollments).
document.addEventListener("submit", (event) => {
    const message = event.target.dataset?.confirm;
    if (message && !window.confirm(message)) {
        event.preventDefault();
    }
});

// Light/dark theme toggle, remembered per browser.
(() => {
    const button = document.getElementById("themeToggle");
    if (!button) return;

    const icon = button.querySelector("i");
    const sync = () => {
        const dark = document.documentElement.getAttribute("data-bs-theme") === "dark";
        icon.className = dark ? "bi bi-sun" : "bi bi-moon-stars";
    };

    button.addEventListener("click", () => {
        const next = document.documentElement.getAttribute("data-bs-theme") === "dark" ? "light" : "dark";
        document.documentElement.setAttribute("data-bs-theme", next);
        try { localStorage.setItem("theme", next); } catch { /* storage unavailable */ }
        sync();
        document.dispatchEvent(new Event("themechange"));
    });
    sync();
})();
