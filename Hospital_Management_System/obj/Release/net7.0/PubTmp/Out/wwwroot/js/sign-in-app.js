const form = document.getElementById('form');
const userName = document.getElementById('userName');
const pattern = /^[a-zA-Z0-9_-]{3,16}$/;

userName.addEventListener('input', () => {
    const inputValue = userName.value.trim(); // Trim to remove leading/trailing spaces

    // If the input field is empty, delete classes
    if (inputValue === '') {
        form.classList.remove('invalid');
        form.classList.remove('valid');
    } else {
        // Evaluates if it matches the pattern values
        if (inputValue.match(pattern)) {
            form.classList.add('valid');
            form.classList.remove('invalid');
        } else {
            form.classList.add('invalid');
            form.classList.remove('valid');
        }
    }
});
