// Auth Logic
document.addEventListener('DOMContentLoaded', () => {

    // 1. Password Show/Hide Toggle
    const togglePasswordButtons = document.querySelectorAll('.toggle-password');
    togglePasswordButtons.forEach(btn => {
        btn.addEventListener('click', (e) => {
            e.preventDefault();
            const input = btn.parentElement.querySelector('input');
            const icon = btn.querySelector('i');

            if (input.type === 'password') {
                input.type = 'text';
                icon.classList.remove('fa-eye');
                icon.classList.add('fa-eye-slash');
            } else {
                input.type = 'password';
                icon.classList.remove('fa-eye-slash');
                icon.classList.add('fa-eye');
            }
        });
    });

    // 2. Role Card Selection (UI only)
    const roleCards = document.querySelectorAll('.role-card');
    roleCards.forEach(card => {
        card.addEventListener('click', () => {
            roleCards.forEach(c => c.classList.remove('selected'));
            card.classList.add('selected');

            const radio = card.querySelector('input[type="radio"]');
            if (radio) radio.checked = true;
        });
    });

    // 3. BASIC CLIENT VALIDATION (UI ONLY - NO PREVENT SUBMIT)
    const forms = document.querySelectorAll('form');

    forms.forEach(form => {

        form.addEventListener('submit', () => {

            // Required fields check
            const requiredInputs = form.querySelectorAll('input[required], select[required]');
            let isValid = true;

            requiredInputs.forEach(input => {
                if (!input.value.trim()) {
                    isValid = false;
                    input.style.borderColor = 'var(--error)';
                } else {
                    input.style.borderColor = 'var(--border)';
                }
            });

            // Email validation
            const emailInputs = form.querySelectorAll('input[type="email"]');
            emailInputs.forEach(input => {
                if (input.value && !/^\S+@\S+\.\S+$/.test(input.value)) {
                    isValid = false;
                    input.style.borderColor = 'var(--error)';
                }
            });

            // Password match
            const password = form.querySelector('input[name="Password"]');
            const confirmPassword = form.querySelector('input[name="ConfirmPassword"]');

            if (password && confirmPassword) {
                if (password.value !== confirmPassword.value) {
                    isValid = false;
                    confirmPassword.style.borderColor = 'var(--error)';
                    alert(document.documentElement.lang === 'ar'
                        ? 'كلمات المرور غير متطابقة'
                        : 'Passwords do not match');
                }
            }

            if (!isValid) {
                alert(document.documentElement.lang === 'ar'
                    ? 'يرجى التأكد من ملء جميع الحقول المطلوبة بشكل صحيح'
                    : 'Please check required fields and format');
            }

        });

    });

});

const categorySelect = document.getElementById("categorySelect");
const otherCategoryDiv = document.getElementById("otherCategoryDiv");
const otherCategory = document.getElementById("otherCategory");

if (categorySelect) {

    categorySelect.addEventListener("change", function () {

        if (this.value === "Other") {
            otherCategoryDiv.style.display = "block";
            otherCategory.required = true;
        }
        else {
            otherCategoryDiv.style.display = "none";
            otherCategory.required = false;
            otherCategory.value = "";
        }

    });

}
// Auth Logic
//document.addEventListener('DOMContentLoaded', () => {
//    // 1. Password Show/Hide Toggle
//    const togglePasswordButtons = document.querySelectorAll('.toggle-password');
//    togglePasswordButtons.forEach(btn => {
//        btn.addEventListener('click', (e) => {
//            e.preventDefault();
//            const input = btn.parentElement.querySelector('input');
//            const icon = btn.querySelector('i');
            
//            if (input.type === 'password') {
//                input.type = 'text';
//                icon.classList.remove('fa-eye');
//                icon.classList.add('fa-eye-slash');
//            } else {
//                input.type = 'password';
//                icon.classList.remove('fa-eye-slash');
//                icon.classList.add('fa-eye');
//            }
//        });
//    });

//    // 2. Role Card Selection (for signup-role.html)
//    const roleCards = document.querySelectorAll('.role-card');
//    roleCards.forEach(card => {
//        card.addEventListener('click', () => {
//            roleCards.forEach(c => c.classList.remove('selected'));
//            card.classList.add('selected');
//            const radio = card.querySelector('input[type="radio"]');
//            if (radio) radio.checked = true;
//        });
//    });

//    // 3. Form Validations & Submissions
//    const forms = document.querySelectorAll('form');
//    forms.forEach(form => {
//        form.addEventListener('submit', (e) => {
//            e.preventDefault();

//            // Generic required field check
//            let isValid = true;
//            const requiredInputs = form.querySelectorAll('input[required], select[required]');
//            requiredInputs.forEach(input => {
//                if (!input.value.trim()) {
//                    isValid = false;
//                    input.style.borderColor = 'var(--error)';
//                } else {
//                    input.style.borderColor = 'var(--border)';
//                }
//            });

//            // Email format check
//            const emailInputs = form.querySelectorAll('input[type="email"]');
//            emailInputs.forEach(input => {
//                if (input.value && !/^\S+@\S+\.\S+$/.test(input.value)) {
//                    isValid = false;
//                    input.style.borderColor = 'var(--error)';
//                }
//            });

//            // Password match check
//            const password = form.querySelector('input[name="Password"]');
//            const confirmPassword = form.querySelector('input[name="ConfirmPassword"]');
//            if (password && confirmPassword) {
//                if (password.value !== confirmPassword.value) {
//                    isValid = false;
//                    confirmPassword.style.borderColor = 'var(--error)';
//                    alert(document.documentElement.lang === 'ar' ? 'كلمات المرور غير متطابقة' : 'Passwords do not match');
//                }
//            }

//            if (!isValid) {
//                alert(document.documentElement.lang === 'ar' ? 'يرجى التأكد من ملء جميع الحقول المطلوبة بشكل صحيح' : 'Please check required fields and format');
//                return;
//            }

//            // Collect Data
//            const formData = new FormData(form);
//            const dataObj = Object.fromEntries(formData.entries());

//            // Specific Form Handlers
//            if (form.id === 'signup-role-form') {
//                // Save basic info to sessionStorage
//                sessionStorage.setItem('signupBasic', JSON.stringify(dataObj));
//                // Redirect based on role
//                if (dataObj.UserType === 'Merchant') {
//                    window.location.href = '/Auth/RegisterMerchant';
//                } else if (dataObj.UserType === 'Company') {
//                    window.location.href = '/Auth/RegisterCompany';
//                } else {
//                    alert('Please select a role');
//                }
//            } 
//            else if (form.id === 'signup-merchant-form' || form.id === 'signup-company-form') {
//                // Combine with sessionStorage data
//                const basicData = JSON.parse(sessionStorage.getItem('signupBasic') || '{}');
//                const finalData = { ...basicData, ...dataObj };
//                console.log('Final Registration Data:', finalData);
                
//                alert(document.documentElement.lang === 'ar' ? 'تم إنشاء الحساب - سيتم ربط الواجهة الخلفية لاحقاً' : 'Account created – backend will be connected');
//                sessionStorage.removeItem('signupBasic');
//            }
//            else if (form.id === 'login-form') {
//                console.log('Login Data:', dataObj);
//                alert(document.documentElement.lang === 'ar' ? 'تم تسجيل الدخول - سيتم ربط الواجهة الخلفية لاحقاً' : 'Logged in – backend will be connected');
//            }
//            else if (form.id === 'forgot-password-form') {
//                console.log('Forgot Password Email:', dataObj.email);
//                alert(document.documentElement.lang === 'ar' ? 'تم إرسال رابط إعادة التعيين' : 'Reset link sent');
//            }
//            else if (form.id === 'reset-password-form') {
//                console.log('New Password Data:', dataObj);
//                alert(document.documentElement.lang === 'ar' ? 'تم تغيير كلمة المرور - سيتم ربط الواجهة الخلفية لاحقاً' : 'Password reset – backend will be connected');
//            }
//        });
//    });
//});
