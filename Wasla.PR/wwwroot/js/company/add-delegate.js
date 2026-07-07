// add-delegate.js - إضافة مندوب جديد
(function() {
    const form = document.getElementById('addDelegateForm');
    const cancelBtn = document.getElementById('cancelAddDelegateBtn');
    if (cancelBtn) {
        cancelBtn.addEventListener('click', () => window.location.href = 'delegates.html');
    }
    if (form) {
        form.addEventListener('submit', (e) => {
            e.preventDefault();
            const name = document.getElementById('delegateName').value.trim();
            const phone = document.getElementById('delegatePhone').value.trim();
            const email = document.getElementById('delegateEmail').value.trim();
            const citySelect = document.getElementById('delegateCity');
            const city = citySelect.options[citySelect.selectedIndex]?.textContent || '';
            const vehicleSelect = document.getElementById('delegateVehicleType');
            const vehicleType = vehicleSelect.options[vehicleSelect.selectedIndex]?.textContent || '';
            const plate = document.getElementById('delegatePlate').value.trim();
            if (!name || !phone) {
                showToast('الرجاء إدخال الاسم ورقم الهاتف', 'error');
                return;
            }
            const newDelegate = {
                id: 'del_' + Date.now(),
                name, phone, email, city, vehicleType, plateNumber: plate,
                status: 'active', cashInHand: 0, cashDeliveredToCompany: 0, avatar: ''
            };
            const delegates = getDelegates();
            delegates.push(newDelegate);
            saveDelegates(delegates);
            showToast('تمت إضافة المندوب بنجاح', 'success');
            window.location.href = 'delegates.html';
        });
    }
})();