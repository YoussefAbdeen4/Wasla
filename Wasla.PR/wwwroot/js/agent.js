// agent.js - Connected strictly to C# Backend Database
(function () {
    // ========== كائن الترجمة ==========
    const translations = {
        ar: {
            dashboard: "لوحة التحكم", my_orders: "طلباتي", my_financials: "مالياتي",
            profile: "بروفايلي", logout: "تسجيل خروج", role_agent: "مندوب توصيل",
            total_orders: "إجمالي الطلبات", delivered: "تم التوصيل", returned: "مرتجع",
            cash_in_hand: "النقدية معي", cash_delivered: "سلمت للشركة", balance: "الرصيد المتبقي",
            recent_orders: "أحدث الطلبات", order_no: "رقم الطلب", customer: "العميل",
            amount: "المبلغ", status: "الحالة", all_orders: "جميع طلباتي", actions: "الإجراءات",
            pick_up: "استلام", return_order: "إرجاع", deliver: "تسليم", no_actions: "لا إجراءات",
            submit_cash: "تسليم نقدية للشركة", amount_egp: "المبلغ (E.G)", submit: "تسليم",
            update_avatar: "تحديث الصورة", upload_image: "رفع صورة", image_url: "أو رابط صورة",
            update: "تحديث الصورة", full_name: "الاسم بالكامل", phone: "رقم الهاتف", vehicle_type: "نوع المركبة",
            cash_submit_success: "✅ تم تسليم المبلغ بنجاح", cash_submit_error: "❌ حدث خطأ، تأكد من إدخال مبلغ صحيح",
            order_pickup_success: "✅ تم تحديث حالة الطلب بنجاح",
            avatar_update_success: "🖼️ تم تحديث الصورة بنجاح", avatar_update_error: "⚠️ يرجى اختيار ملف صالح",
            confirm_pickup: "هل أنت متأكد من استلام الطلب؟",
            confirm_deliver: "هل تم توصيل الطلب بنجاح؟", confirm_return: "هل تريد إرجاع الطلب؟",
            no_data: "لا توجد بيانات", dashboard_title: "لوحة تحكم المندوب",
            my_financials_title: "مالياتي - مندوب", my_orders_title: "طلباتي - مندوب",
            profile_title: "بروفايلي - مندوب"
        },
        en: {
            dashboard: "Dashboard", my_orders: "My Orders", my_financials: "My Financials",
            profile: "My Profile", logout: "Logout", role_agent: "Delivery Agent",
            total_orders: "Total Orders", delivered: "Delivered", returned: "Returned",
            cash_in_hand: "Cash in Hand", cash_delivered: "Submitted to Company", balance: "Remaining Balance",
            recent_orders: "Recent Orders", order_no: "Order No.", customer: "Customer",
            amount: "Amount", status: "Status", all_orders: "All My Orders", actions: "Actions",
            pick_up: "Pick Up", return_order: "Return", deliver: "Deliver", no_actions: "No Actions",
            submit_cash: "Submit Cash to Company", amount_egp: "Amount (E.G)", submit: "Submit",
            update_avatar: "Update Avatar", upload_image: "Upload Image", image_url: "Or Image URL",
            update: "Update Avatar", full_name: "Full Name", phone: "Phone Number", vehicle_type: "Vehicle Type",
            cash_submit_success: "✅ Cash submitted successfully", cash_submit_error: "❌ Error submitting cash",
            order_pickup_success: "✅ Order status updated",
            avatar_update_success: "🖼️ Avatar updated successfully", avatar_update_error: "⚠️ Please select a valid file",
            confirm_pickup: "Are you sure you want to pick up this order?",
            confirm_deliver: "Has the order been delivered successfully?", confirm_return: "Do you want to return this order?",
            no_data: "No data", dashboard_title: "Agent Dashboard",
            my_financials_title: "My Financials - Agent", my_orders_title: "My Orders - Agent",
            profile_title: "My Profile - Agent"
        }
    };

    let currentLang = localStorage.getItem('agent_lang') || 'ar';

    function translatePage() {
        document.querySelectorAll('[data-i18n]').forEach(el => {
            const key = el.getAttribute('data-i18n');
            if (translations[currentLang] && translations[currentLang][key]) {
                if (el.tagName === 'INPUT' || el.tagName === 'TEXTAREA') {
                    if (el.hasAttribute('data-i18n-placeholder')) el.placeholder = translations[currentLang][key];
                    else if (el.type !== 'submit') el.value = translations[currentLang][key];
                    else el.placeholder = translations[currentLang][key];
                } else if (el.tagName === 'BUTTON') {
                    const icon = el.querySelector('i');
                    if (icon) {
                        const textNode = Array.from(el.childNodes).find(n => n.nodeType === 3 && n.textContent.trim());
                        if (textNode) textNode.textContent = ' ' + translations[currentLang][key];
                        else el.appendChild(document.createTextNode(' ' + translations[currentLang][key]));
                    } else el.textContent = translations[currentLang][key];
                } else el.textContent = translations[currentLang][key];
            }
        });
        document.documentElement.setAttribute('dir', currentLang === 'ar' ? 'rtl' : 'ltr');
        document.documentElement.lang = currentLang;

        const path = window.location.pathname.toLowerCase();
        let titleKey = 'dashboard_title';
        if (path.includes('/financials')) titleKey = 'my_financials_title';
        else if (path.includes('/orders')) titleKey = 'my_orders_title';
        else if (path.includes('/profile')) titleKey = 'profile_title';
        document.title = translations[currentLang][titleKey];
    }

    function setLanguage(lang) {
        if (!translations[lang]) return;
        currentLang = lang;
        localStorage.setItem('agent_lang', lang);
        translatePage();

        document.querySelectorAll('.agent-lang-btn').forEach(btn => {
            if (btn.dataset.lang === lang) btn.classList.add('active');
            else btn.classList.remove('active');
        });
    }

    function initLanguageSwitcher() {
        const btns = document.querySelectorAll('.agent-lang-btn');
        btns.forEach(btn => {
            btn.classList.remove('active');
            btn.addEventListener('click', (e) => setLanguage(e.currentTarget.dataset.lang));
        });
        const activeBtn = document.querySelector(`.agent-lang-btn[data-lang="${currentLang}"]`);
        if (activeBtn) activeBtn.classList.add('active');
        translatePage();
    }

    function showToast(message, type = 'info') {
        let container = document.querySelector('.agent-toast-container');
        if (!container) {
            container = document.createElement('div');
            container.className = 'agent-toast-container';
            document.body.appendChild(container);
        }
        const toast = document.createElement('div');
        toast.className = `agent-toast agent-toast-${type}`;
        const icon = type === 'success' ? 'fa-circle-check' : (type === 'error' ? 'fa-circle-exclamation' : 'fa-info-circle');
        toast.innerHTML = `<i class="fa-solid ${icon}"></i> <span>${message}</span>`;
        container.appendChild(toast);
        setTimeout(() => toast.remove(), 3000);
    }

    // ========== ربط أحداث الأزرار بـ C# BACKEND ==========

    // دالة مسؤولة عن تحديث حالة الطلب
    async function handleOrderAction(btn, status, confirmMsg) {
        if (!confirm(confirmMsg)) return;

        const orderId = btn.getAttribute('data-order-id');
        if (!orderId) return;

        try {
            const body = new URLSearchParams();
            body.append('orderId', orderId);
            body.append('status', status);

            const res = await fetch('/Agent/UpdateOrderStatus', {
                method: 'POST',
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                body: body.toString()
            });

            const json = await res.json();
            if (json && json.success) {
                showToast(translations[currentLang].order_pickup_success, 'success');
                // تحديث الصفحة بعد ثانية لرؤية التغييرات الجديدة من قاعدة البيانات
                setTimeout(() => window.location.reload(), 1000);
            } else {
                showToast((json && json.message) || 'Error', 'error');
            }
        } catch (err) {
            showToast('Network error', 'error');
        }
    }

    // تهيئة أزرار صفحة الطلبات
    function initOrders() {
        document.querySelectorAll('.agent-pickup').forEach(btn => {
            btn.addEventListener('click', (e) => handleOrderAction(e.currentTarget, 'PickedUp', translations[currentLang].confirm_pickup));
        });
        document.querySelectorAll('.agent-deliver').forEach(btn => {
            btn.addEventListener('click', (e) => handleOrderAction(e.currentTarget, 'Delivered', translations[currentLang].confirm_deliver));
        });
        document.querySelectorAll('.agent-return').forEach(btn => {
            btn.addEventListener('click', (e) => handleOrderAction(e.currentTarget, 'Returned', translations[currentLang].confirm_return));
        });
    }

    // تهيئة زر تسليم الأموال
    function initFinancials() {
        const submitBtn = document.getElementById('submitCashBtn');
        const amountInput = document.getElementById('submitAmount');
        if (submitBtn && amountInput) {
            submitBtn.onclick = async () => {
                const amount = parseFloat(amountInput.value);
                if (!amount || amount <= 0) {
                    showToast(translations[currentLang].cash_submit_error, 'error');
                    return;
                }
                try {
                    const body = new URLSearchParams();
                    body.append('amount', amount);
                    const res = await fetch('/Agent/SubmitCash', {
                        method: 'POST',
                        headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                        body: body.toString()
                    });
                    const json = await res.json();
                    if (json && json.success) {
                        showToast(translations[currentLang].cash_submit_success, 'success');
                        setTimeout(() => window.location.reload(), 1000);
                    } else {
                        showToast(translations[currentLang].cash_submit_error, 'error');
                    }
                } catch (err) {
                    showToast('Error', 'error');
                }
            };
        }
    }

    // تهيئة زر تحديث البروفايل
    function initProfile() {
        const updateBtn = document.getElementById('updateAvatarBtn');
        if (updateBtn) {
            updateBtn.onclick = async () => {
                const avatarUpload = document.getElementById('avatarUpload');
                const file = avatarUpload ? avatarUpload.files[0] : null;

                if (!file) {
                    showToast(translations[currentLang].avatar_update_error, 'error');
                    return;
                }

                const formData = new FormData();
                formData.append('avatar', file);

                try {
                    const res = await fetch('/Agent/UpdateAvatar', {
                        method: 'POST',
                        body: formData
                    });
                    const json = await res.json();
                    if (json && json.success) {
                        showToast(translations[currentLang].avatar_update_success, 'success');
                        setTimeout(() => window.location.reload(), 1000);
                    } else {
                        showToast('Error updating avatar', 'error');
                    }
                } catch (err) {
                    showToast('Network error', 'error');
                }
            };
        }
    }

    // ========== بدء التشغيل حسب الصفحة الحالية ==========
    document.addEventListener('DOMContentLoaded', () => {
        initLanguageSwitcher();

        const path = window.location.pathname.toLowerCase();

        if (path.includes('/orders')) {
            initOrders();
        } else if (path.includes('/financials')) {
            initFinancials();
        } else if (path.includes('/profile')) {
            initProfile();
        }
    });
})();