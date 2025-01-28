function nameInput() {
    document.getElementById('nameInput').addEventListener('keydown', function (e) {
        // Allow only English letters and space
        if (!/^[A-Za-z 0-9\s]$/.test(e.key) && e.key !== 'Backspace' && e.key !== 'ArrowLeft' && e.key !== 'ArrowRight') {
            e.preventDefault();
        }
    });
}
function authorsInput() {
    document.getElementById('authorsInput').addEventListener('keydown', function (e) {
        // Allow only English letters and space
        if (!/^[0-9]$/.test(e.key) && e.key !== 'Backspace' && e.key !== 'ArrowLeft' && e.key !== 'ArrowRight') {
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
function familyNameInput() {
    document.getElementById('familyNameInput').addEventListener('keydown', function (e) {
        // Allow only English letters and space
        if (!/^[A-Za-z 0-9\s]$/.test(e.key) && e.key !== 'Backspace' && e.key !== 'ArrowLeft' && e.key !== 'ArrowRight') {
            e.preventDefault();
        }
    });
}
function specialityInput() {
    document.getElementById('specialityInput').addEventListener('keydown', function (e) {
        // Allow only English letters and space
        if (!/^[A-Za-z 0-9\s]$/.test(e.key) && e.key !== 'Backspace' && e.key !== 'ArrowLeft' && e.key !== 'ArrowRight') {
            e.preventDefault();
        }
    });
}
function journalInput() {
    document.getElementById('journalInput').addEventListener('keydown', function (e) {
        // Allow only English letters and space
        if (!/^[A-Za-z\s]$/.test(e.key) && e.key !== 'Backspace' && e.key !== 'ArrowLeft' && e.key !== 'ArrowRight') {
            e.preventDefault();
        }
    });
}
function titleInput() {
    document.getElementById('titleInput').addEventListener('keydown', function (e) {
        // Allow only English letters and space
        if (!/^[A-Za-z 0-9#\.\s]$/.test(e.key) && e.key !== 'Backspace' && e.key !== 'ArrowLeft' && e.key !== 'ArrowRight') {
            e.preventDefault();
        }
    });
}
function articalInput() {
    document.getElementById('articalInput').addEventListener('keydown', function (e) {
        // Allow only English letters and space
        if (!/^[A-Za-z 0-9#@\+\.\s]$/.test(e.key) && e.key !== 'Backspace' && e.key !== 'ArrowLeft' && e.key !== 'ArrowRight') {
            e.preventDefault();
        }
    });
}
function journalNameInput() {
    document.getElementById('journalNameInput').addEventListener('keydown', function (e) {
        // Allow only English letters and space
        if (!/^[A-Za-z\s]$/.test(e.key) && e.key !== 'Backspace' && e.key !== 'ArrowLeft' && e.key !== 'ArrowRight') {
            e.preventDefault();
        }
    });
}
function languageInput() {
    document.getElementById('languageInput').addEventListener('keydown', function (e) {
        // Allow only English letters and space
        if (!/^[A-Za-z 0-9#\.\s]$/.test(e.key) && e.key !== 'Backspace' && e.key !== 'ArrowLeft' && e.key !== 'ArrowRight') {
            e.preventDefault();
        }
    });
}
function issueInput() {
    document.getElementById('issueInput').addEventListener('keydown', function (e) {
        // Allow only English letters and space
        if (!/^[0-9\-]$/.test(e.key) && e.key !== 'Backspace' && e.key !== 'ArrowLeft' && e.key !== 'ArrowRight') {
            e.preventDefault();
        }
    });
}
function doiInput() {
    document.getElementById('doiInput').addEventListener('keydown', function (e) {
        // Allow only English letters and space
        if (!/^[A-Za-z 0-9/:#\.\s]$/.test(e.key) && e.key !== 'Backspace' && e.key !== 'ArrowLeft' && e.key !== 'ArrowRight') {
            e.preventDefault();
        }
    });
}
function pageInput() {
    document.getElementById('pageInput').addEventListener('keydown', function (e) {
        // Allow only English letters and space
        if (!/^[0-9\-]$/.test(e.key) && e.key !== 'Backspace' && e.key !== 'ArrowLeft' && e.key !== 'ArrowRight') {
            e.preventDefault();
        }
    });
}