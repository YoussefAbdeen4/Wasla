// agent.js - النسخة النهائية المستقرة (تعرض بيانات البروفايل وتحدث الصورة)
(function() {
    // ========== كائن الترجمة (مختصر للوضوح) ==========
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
            cash_submit_success: "✅ تم تسليم المبلغ بنجاح", cash_submit_error: "❌ المبلغ غير صالح أو يتجاوز الرصيد",
            order_pickup_success: "✅ تم استلام الطلب بنجاح", order_deliver_success: "✅ تم توصيل الطلب، بانتظار موافقة الشركة",
            order_return_success: "🔄 تم إرجاع الطلب", avatar_update_success: "🖼️ تم تحديث الصورة بنجاح",
            avatar_update_error: "⚠️ يرجى اختيار ملف أو إدخال رابط",
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
            cash_submit_success: "✅ Cash submitted successfully", cash_submit_error: "❌ Invalid amount or exceeds balance",
            order_pickup_success: "✅ Order picked up successfully", order_deliver_success: "✅ Order delivered, pending company approval",
            order_return_success: "🔄 Order returned", avatar_update_success: "🖼️ Avatar updated successfully",
            avatar_update_error: "⚠️ Please select a file or enter a URL",
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
        const path = window.location.pathname;
        let titleKey = 'dashboard_title';
        if (path.includes('my-financials.html')) titleKey = 'my_financials_title';
        else if (path.includes('my-orders.html')) titleKey = 'my_orders_title';
        else if (path.includes('profile.html')) titleKey = 'profile_title';
        document.title = translations[currentLang][titleKey];
    }

    function setLanguage(lang) {
        if (!translations[lang]) return;
        currentLang = lang;
        localStorage.setItem('agent_lang', lang);
        translatePage();
        const path = window.location.pathname;
        if (path.includes('dashboard.html') && typeof refreshDashboard === 'function') refreshDashboard();
        else if (path.includes('my-orders.html') && typeof renderOrders === 'function') renderOrders();
        else if (path.includes('my-financials.html') && typeof refreshFinancials === 'function') refreshFinancials();
        else if (path.includes('profile.html') && typeof loadProfile === 'function') loadProfile();
        document.querySelectorAll('.agent-lang-btn').forEach(btn => {
            if (btn.dataset.lang === lang) btn.classList.add('active');
            else btn.classList.remove('active');
        });
    }

    function initLanguageSwitcher() {
        const btns = document.querySelectorAll('.agent-lang-btn');
        btns.forEach(btn => {
            btn.removeEventListener('click', langHandler);
            btn.addEventListener('click', langHandler);
        });
        function langHandler(e) { const lang = e.currentTarget.dataset.lang; if (lang) setLanguage(lang); }
        const activeBtn = document.querySelector(`.agent-lang-btn[data-lang="${currentLang}"]`);
        if (activeBtn) activeBtn.classList.add('active');
        translatePage();
    }

    function showToast(message, type = 'info') {
        let container = document.querySelector('.agent-toast-container');
        if (!container) { container = document.createElement('div'); container.className = 'agent-toast-container'; document.body.appendChild(container); }
        const toast = document.createElement('div'); toast.className = `agent-toast agent-toast-${type}`;
        const icon = type === 'success' ? 'fa-circle-check' : (type === 'error' ? 'fa-circle-exclamation' : 'fa-info-circle');
        toast.innerHTML = `<i class="fa-solid ${icon}"></i> <span>${message}</span>`;
        container.appendChild(toast);
        setTimeout(() => toast.remove(), 3000);
    }

    // ========== بيانات ثابتة للمندوب (لضمان ظهورها) ==========
    // نتحقق من localStorage وننشئ بيانات افتراضية إذا لم توجد
    function initializeAgentData() {
        let currentUser = JSON.parse(localStorage.getItem('currentUser') || '{}');
        let agents = JSON.parse(localStorage.getItem('app_agents') || '[]');
        
        // إذا لم يكن هناك agentId في currentUser، ننشئ واحداً
        if (!currentUser.agentId) {
            currentUser = { role: 'agent', agentId: 'agent_123' };
            localStorage.setItem('currentUser', JSON.stringify(currentUser));
            console.log('تم إنشاء currentUser جديد');
        }
        
        // إذا لم يكن هناك مندوب بهذا الـ id، ننشئ مندوباً افتراضياً
        const agentId = currentUser.agentId;
        const existingAgent = agents.find(a => a.id === agentId);
        if (!existingAgent) {
            const newAgent = {
                id: agentId,
                name: 'زياد أحمد',
                phone: '01001234567',
                vehicleType: 'سيارة سيدان',
                avatar: null,
                status: 'active'
            };
            agents.push(newAgent);
            localStorage.setItem('app_agents', JSON.stringify(agents));
            console.log('تم إنشاء مندوب افتراضي:', newAgent);
        }
        
        // التأكد من وجود بعض الطلبات التجريبية إذا كانت فارغة
        let orders = JSON.parse(localStorage.getItem('app_orders') || '[]');
        if (orders.length === 0) {
            orders = [
                { id: 'ord1', orderNo: '#1001', customer: 'محمد علي', netAmount: 250, status: 'assigned', delegateId: agentId, createdAt: Date.now() },
                { id: 'ord2', orderNo: '#1002', customer: 'نورا حسين', netAmount: 180, status: 'picked_up', delegateId: agentId, createdAt: Date.now() - 86400000 }
            ];
            localStorage.setItem('app_orders', JSON.stringify(orders));
        }
        
        let transactions = JSON.parse(localStorage.getItem('app_transactions') || '[]');
        if (transactions.length === 0) {
            localStorage.setItem('app_transactions', JSON.stringify([]));
        }
        
        return { currentUser, agents };
    }

    const { currentUser, agents } = initializeAgentData();
    const AGENT_ID = currentUser.agentId;
    console.log('AGENT_ID المستخدم:', AGENT_ID);

    function getAgentOrders() {
        if (!AGENT_ID) return [];
        const orders = JSON.parse(localStorage.getItem('app_orders') || '[]');
        return orders.filter(o => o.delegateId === AGENT_ID);
    }

    function getAgentById() {
        if (!AGENT_ID) return null;
        const agents = JSON.parse(localStorage.getItem('app_agents') || '[]');
        const agent = agents.find(a => a.id === AGENT_ID);
        console.log('getAgentById يعيد:', agent);
        return agent;
    }

    function getAgentFinancials() {
        if (!AGENT_ID) return { cashInHand: 0, cashDeliveredToCompany: 0, balance: 0 };
        const orders = getAgentOrders();
        const cashInHand = orders.reduce((sum, order) => {
            if (order.status === 'delivered_pending_company_approval' || order.status === 'approved_by_company') {
                return sum + (order.netAmount || 0);
            }
            return sum;
        }, 0);
        const transactions = JSON.parse(localStorage.getItem('app_transactions') || '[]');
        const cashDeliveredToCompany = transactions.filter(t => t.agentId === AGENT_ID).reduce((sum, t) => sum + (t.amount || 0), 0);
        const balance = cashInHand - cashDeliveredToCompany;
        return { cashInHand, cashDeliveredToCompany, balance };
    }

    function updateOrderStatus(orderId, newStatus, extra = {}) {
        if (!AGENT_ID) return false;
        const orders = JSON.parse(localStorage.getItem('app_orders') || '[]');
        const idx = orders.findIndex(o => o.id === orderId);
        if (idx === -1) return false;
        orders[idx].status = newStatus;
        orders[idx].statusHistory = orders[idx].statusHistory || [];
        orders[idx].statusHistory.push({ status: newStatus, timestamp: Date.now(), note: extra.note || '', agentId: AGENT_ID });
        localStorage.setItem('app_orders', JSON.stringify(orders));
        return true;
    }

    function submitAgentCash(amount) {
        if (!AGENT_ID) { showToast(translations[currentLang].cash_submit_error, 'error'); return false; }
        const { balance } = getAgentFinancials();
        if (amount <= 0 || amount > balance) { showToast(translations[currentLang].cash_submit_error, 'error'); return false; }
        const transactions = JSON.parse(localStorage.getItem('app_transactions') || '[]');
        transactions.push({ id: 'tx_' + Date.now(), agentId: AGENT_ID, amount: amount, date: Date.now(), type: 'cash_submission' });
        localStorage.setItem('app_transactions', JSON.stringify(transactions));
        showToast(translations[currentLang].cash_submit_success, 'success');
        return true;
    }

    // ========== تحديث الصورة ==========
    function updateAvatar(fileOrUrl, isFile) {
        const finalize = (imageData) => {
            const agents = JSON.parse(localStorage.getItem('app_agents') || '[]');
            const idx = agents.findIndex(a => a.id === AGENT_ID);
            if (idx !== -1) {
                agents[idx].avatar = imageData;
                localStorage.setItem('app_agents', JSON.stringify(agents));
                loadProfile();      // تحديث البروفايل فوراً
                refreshDashboard(); // تحديث الهيدر في الصفحات الأخرى
                showToast(translations[currentLang].avatar_update_success, 'success');
            } else {
                showToast('لم يتم العثور على المندوب', 'error');
            }
        };

        if (isFile) {
            const reader = new FileReader();
            reader.onload = function(e) {
                const img = new Image();
                img.onload = function() {
                    const canvas = document.createElement('canvas');
                    let width = img.width, height = img.height;
                    const maxSize = 300;
                    if (width > maxSize) { height = (height * maxSize) / width; width = maxSize; }
                    canvas.width = width; canvas.height = height;
                    const ctx = canvas.getContext('2d');
                    ctx.drawImage(img, 0, 0, width, height);
                    const compressed = canvas.toDataURL('image/jpeg', 0.8);
                    finalize(compressed);
                };
                img.src = e.target.result;
            };
            reader.readAsDataURL(fileOrUrl);
        } else {
            finalize(fileOrUrl);
        }
    }

    // ========== دوال عرض البيانات (Dashboard) ==========
    function refreshDashboard() {
        if (!AGENT_ID) return;
        const orders = getAgentOrders();
        const total = orders.length;
        const delivered = orders.filter(o => o.status === 'approved_by_company').length;
        const returned = orders.filter(o => o.status === 'returned').length;
        const { cashInHand, cashDeliveredToCompany, balance } = getAgentFinancials();

        document.getElementById('statTotal').innerText = total;
        document.getElementById('statDelivered').innerText = delivered;
        document.getElementById('statReturned').innerText = returned;
        document.getElementById('statCashInHand').innerHTML = cashInHand.toFixed(2) + ' E.G';
        document.getElementById('statCashDelivered').innerHTML = cashDeliveredToCompany.toFixed(2) + ' E.G';
        document.getElementById('statBalance').innerHTML = balance.toFixed(2) + ' E.G';

        const recent = orders.slice(-5).reverse();
        const tbody = document.getElementById('recentOrdersBody');
        if (tbody) {
            if (recent.length === 0) tbody.innerHTML = `<tr><td colspan="4" style="text-align:center;">${translations[currentLang].no_data}</td></tr>`;
            else tbody.innerHTML = recent.map(o => `<tr><td><strong>${o.orderNo || o.id}</strong></td><td>${o.customer}</td><td>${o.netAmount || 0} E.G</td><td><span class="agent-badge">${o.status}</span></td></tr>`).join('');
        }

        const agent = getAgentById();
        if (agent) {
            const nameSpan = document.getElementById('agentName');
            if (nameSpan) nameSpan.innerText = agent.name;
            const avatarDiv = document.getElementById('agentAvatar');
            if (avatarDiv) {
                if (agent.avatar) { avatarDiv.style.backgroundImage = `url(${agent.avatar})`; avatarDiv.style.backgroundSize = 'cover'; avatarDiv.innerText = ''; }
                else { avatarDiv.style.backgroundImage = ''; avatarDiv.innerText = agent.name.charAt(0) || 'م'; }
            }
        }
    }

    function initDashboard() { refreshDashboard(); setInterval(refreshDashboard, 5000); window.addEventListener('storageSync', refreshDashboard); }

    // ========== صفحة الطلبات ==========
    function renderOrders() {
        if (!AGENT_ID) { const tbody = document.getElementById('agentOrdersBody'); if (tbody) tbody.innerHTML = `<td><td colspan="5">${translations[currentLang].no_data}<\/td></td>`; return; }
        const orders = getAgentOrders();
        const tbody = document.getElementById('agentOrdersBody');
        if (!tbody) return;
        if (orders.length === 0) { tbody.innerHTML = `<td><td colspan="5">${translations[currentLang].no_data}<\/td></tr>`; return; }
        tbody.innerHTML = orders.map(order => {
            let actions = '';
            if (order.status === 'assigned') actions = `<button class="agent-btn agent-btn-primary" data-action="pickup" data-id="${order.id}">${translations[currentLang].pick_up}</button> <button class="agent-btn agent-btn-danger" data-action="return" data-id="${order.id}">${translations[currentLang].return_order}</button>`;
            else if (order.status === 'picked_up') actions = `<button class="agent-btn agent-btn-success" data-action="deliver" data-id="${order.id}">${translations[currentLang].deliver}</button> <button class="agent-btn agent-btn-danger" data-action="return" data-id="${order.id}">${translations[currentLang].return_order}</button>`;
            else actions = `<span class="agent-badge">${translations[currentLang].no_actions}</span>`;
            return `<tr><td><strong>${order.orderNo || order.id}</strong></td><td>${order.customer}</td><td>${order.netAmount || 0} E.G</td><td>${order.status}</td><td class="order-actions">${actions}</td></tr>`;
        }).join('');
        document.querySelectorAll('#agentOrdersBody [data-action]').forEach(btn => { btn.removeEventListener('click', handleOrderAction); btn.addEventListener('click', handleOrderAction); });
    }

    async function handleOrderAction(e) {
        const btn = e.currentTarget, action = btn.getAttribute('data-action'), orderId = btn.getAttribute('data-id');
        const order = getAgentOrders().find(o => o.id === orderId);
        if (!order) return;
        if (action === 'pickup') { if (confirm(translations[currentLang].confirm_pickup)) { updateOrderStatus(orderId, 'picked_up'); renderOrders(); showToast(translations[currentLang].order_pickup_success, 'success'); } }
        else if (action === 'deliver') { if (confirm(translations[currentLang].confirm_deliver)) { updateOrderStatus(orderId, 'delivered_pending_company_approval'); renderOrders(); showToast(translations[currentLang].order_deliver_success, 'success'); } }
        else if (action === 'return') { if (confirm(translations[currentLang].confirm_return)) { updateOrderStatus(orderId, 'returned'); renderOrders(); showToast(translations[currentLang].order_return_success, 'info'); } }
    }

    function updateAgentHeader() {
        const agent = getAgentById();
        if (agent) {
            const nameSpan = document.getElementById('agentName');
            if (nameSpan) nameSpan.innerText = agent.name;
            const avatarDiv = document.getElementById('agentAvatar');
            if (avatarDiv) {
                if (agent.avatar) { avatarDiv.style.backgroundImage = `url(${agent.avatar})`; avatarDiv.style.backgroundSize = 'cover'; avatarDiv.innerText = ''; }
                else { avatarDiv.style.backgroundImage = ''; avatarDiv.innerText = agent.name.charAt(0) || 'م'; }
            }
        }
    }

    function initOrders() { renderOrders(); updateAgentHeader(); setInterval(() => { renderOrders(); updateAgentHeader(); }, 5000); window.addEventListener('storageSync', () => { renderOrders(); updateAgentHeader(); }); }

    // ========== صفحة الماليات ==========
    function refreshFinancials() {
        if (!AGENT_ID) return;
        const { cashInHand, cashDeliveredToCompany, balance } = getAgentFinancials();
        document.getElementById('cashInHand').innerHTML = cashInHand.toFixed(2) + ' E.G';
        document.getElementById('cashDelivered').innerHTML = cashDeliveredToCompany.toFixed(2) + ' E.G';
        document.getElementById('balance').innerHTML = balance.toFixed(2) + ' E.G';
    }

    function initFinancials() {
        refreshFinancials();
        const submitBtn = document.getElementById('submitCashBtn');
        const amountInput = document.getElementById('submitAmount');
        if (submitBtn && amountInput) submitBtn.onclick = () => { const amount = parseFloat(amountInput.value); if (submitAgentCash(amount)) { refreshFinancials(); amountInput.value = ''; } };
        setInterval(refreshFinancials, 5000);
        window.addEventListener('storageSync', refreshFinancials);
    }

    // ========== صفحة البروفايل (مع عرض البيانات وتحديث الصورة) ==========
    function loadProfile() {
        console.log('loadProfile تم استدعاؤها');
        if (!AGENT_ID) { console.warn('AGENT_ID غير موجود'); return; }
        const agent = getAgentById();
        if (!agent) { console.warn('لم يتم العثور على المندوب في localStorage'); return; }
        console.log('المندوب:', agent);

        // تعبئة الحقول المعطلة
        const nameInput = document.getElementById('agentName');
        const phoneInput = document.getElementById('agentPhone');
        const vehicleInput = document.getElementById('agentVehicle');
        const fullNameSpan = document.getElementById('agentFullName');
        if (nameInput) nameInput.value = agent.name;
        if (phoneInput) phoneInput.value = agent.phone;
        if (vehicleInput) vehicleInput.value = agent.vehicleType;
        if (fullNameSpan) fullNameSpan.innerText = agent.name;

        // معاينة الصورة الكبيرة
        const avatarPreview = document.getElementById('avatarPreview');
        if (avatarPreview) {
            if (agent.avatar) {
                avatarPreview.style.backgroundImage = `url(${agent.avatar})`;
                avatarPreview.style.backgroundSize = 'cover';
                avatarPreview.innerHTML = '';
            } else {
                avatarPreview.style.backgroundImage = '';
                avatarPreview.innerHTML = agent.name.charAt(0) || 'م';
            }
        }

        // تحديث الهيدر
        const headerName = document.getElementById('agentNameHeader');
        const headerAvatar = document.getElementById('agentAvatarHeader');
        if (headerName) headerName.innerText = agent.name;
        if (headerAvatar) {
            if (agent.avatar) {
                headerAvatar.style.backgroundImage = `url(${agent.avatar})`;
                headerAvatar.style.backgroundSize = 'cover';
                headerAvatar.innerText = '';
            } else {
                headerAvatar.style.backgroundImage = '';
                headerAvatar.innerText = agent.name.charAt(0) || 'م';
            }
        }

        // تحديث الصورة الجانبية في القائمة (إذا وجدت)
        const sideAvatar = document.getElementById('agentAvatar');
        if (sideAvatar) {
            if (agent.avatar) {
                sideAvatar.style.backgroundImage = `url(${agent.avatar})`;
                sideAvatar.style.backgroundSize = 'cover';
                sideAvatar.innerText = '';
            } else {
                sideAvatar.style.backgroundImage = '';
                sideAvatar.innerText = agent.name.charAt(0) || 'م';
            }
        }
    }

    function initProfile() {
        loadProfile();
        const updateBtn = document.getElementById('updateAvatarBtn');
        if (updateBtn) {
            updateBtn.onclick = () => {
                if (!AGENT_ID) { showToast(translations[currentLang].no_agent_found, 'error'); return; }
                const avatarUpload = document.getElementById('avatarUpload');
                const avatarUrl = document.getElementById('avatarUrl');
                const file = avatarUpload ? avatarUpload.files[0] : null;
                const url = avatarUrl ? avatarUrl.value.trim() : null;
                if (file) { updateAvatar(file, true); avatarUpload.value = ''; }
                else if (url) { updateAvatar(url, false); avatarUrl.value = ''; }
                else { showToast(translations[currentLang].avatar_update_error, 'error'); }
            };
        }
        window.addEventListener('storageSync', loadProfile);
    }

    // ========== بدء التشغيل ==========
    document.addEventListener('DOMContentLoaded', () => {
        initLanguageSwitcher();
        const path = window.location.pathname;
        if (path.includes('dashboard.html')) initDashboard();
        else if (path.includes('my-orders.html')) initOrders();
        else if (path.includes('my-financials.html')) initFinancials();
        else if (path.includes('profile.html')) initProfile();
        else initDashboard();
    });
})();