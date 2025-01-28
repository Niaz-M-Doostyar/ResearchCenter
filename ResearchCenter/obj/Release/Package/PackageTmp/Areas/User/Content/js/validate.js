function nameInput() {
    document.getElementById('nameInput').addEventListener('keydown', function (e) {
        // Allow only English letters and space
        if (!/^[A-Za-z 0-9\s]$/.test(e.key) && e.key !== 'Backspace' && e.key !== 'ArrowLeft' && e.key !== 'ArrowRight') {
            e.preventDefault();
        }
    });
}
function phoneInput() {
    document.getElementById('phoneInput').addEventListener('keydown', function (e) {
        // Allow only English letters and space
        if (!/^[0-9+\s]$/.test(e.key) && e.key !== 'Backspace' && e.key !== 'ArrowLeft' && e.key !== 'ArrowRight') {
            e.preventDefault();
        }
    });
}
function deptInput() {
    document.getElementById('deptInput').addEventListener('keydown', function (e) {
        // Allow only English letters and space
        if (!/^[A-Za-z\s]$/.test(e.key) && e.key !== 'Backspace' && e.key !== 'ArrowLeft' && e.key !== 'ArrowRight') {
            e.preventDefault();
        }
    });
}
function emailInput() {
    document.getElementById('emailInput').addEventListener('keydown', function (e) {
        // Allow only English letters and space
        if (!/^[A-Za-z 0-9@\.]$/.test(e.key) && e.key !== 'Backspace' && e.key !== 'ArrowLeft' && e.key !== 'ArrowRight') {
            e.preventDefault();
        }
    });
}
function passInput() {
    document.getElementById('passInput').addEventListener('keydown', function (e) {
        // Allow only English letters and space
        if (!/^[A-Za-z 0-9@#\$%+\.\s]$/.test(e.key) && e.key !== 'Backspace' && e.key !== 'ArrowLeft' && e.key !== 'ArrowRight') {
            e.preventDefault();
        }
    });
}