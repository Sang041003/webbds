
const signupButton = document.getElementById("signup-button");
const loginButton = document.getElementById("login-button");
const userForms = document.getElementById("user_options-forms");


// Event listener cho nút "Đăng ký"
signupButton.addEventListener(
    "click",
    () => {
        userForms.classList.remove("bounceRight");
        userForms.classList.add("bounceLeft");
    },
    false
);

// Event listener cho nút "Đăng nhập"
loginButton.addEventListener(
    "click",
    () => {
        userForms.classList.remove("bounceLeft");
        userForms.classList.add("bounceRight");
    },
    false
);


