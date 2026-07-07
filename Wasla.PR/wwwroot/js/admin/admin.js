/**
 * Wasla Admin Dashboard – Complete JavaScript (Translations + Logic + UI)
 * Version: 3.0 – All-in-One
 */

// ======================== 1. TRANSLATIONS OBJECT (built-in) ========================
window.translations = {
    en: {
        admin_platform_name: "Wasla Admin",
        nav_dashboard: "Dashboard",
        nav_companies: "Companies",
        nav_merchants: "Merchants",
        nav_orders: "Orders",
        nav_delegates: "Delegates",
        nav_pricing: "Pricing Packages",
        nav_settings: "Settings",
        admin_name: "Ahmed Mohammed",
        admin_role: "System Admin",
        stat_companies: "Total Companies",
        stat_merchants: "Total Merchants",
        stat_orders: "Total Orders",
        stat_delegates: "Total Delegates",
        col_order_id: "Order ID",
        col_customer: "Customer",
        col_date: "Date",
        col_status: "Status",
        col_actions: "Actions",
        col_name: "Name",
        col_phone: "Phone",
        view_details: "View",
        status_active: "Active",
        status_available: "Available",
        status_processing: "Processing",
        status_shipped: "Shipped",
        status_delivered: "Delivered",
        status_cancelled: "Cancelled",
        logout: "Logout",
        back: "Back",
        btn_save: "Save Changes",
        view_all: "View All",
        recent_orders: "Recent Orders",
        registered_companies: "Registered Companies",
        settings_title: "General Configuration",
        set_platform_name: "Platform Name",
        set_email: "Support Email",
        set_phone: "Support Phone",
        order_timeline: "Delivery Stages",
        info_customer: "Customer Information",
        info_merchant: "Merchant Information",
        info_payment: "Payment Details",
        full_name: "Full Name",
        phone_number: "Phone Number",
        business_address: "Address",
        store_name: "Store Name",
        col_amount: "Total Amount",
        col_payment_method: "Payment Method",
        currency: "SAR",
        stage_merchant: "Merchant",
        stage_company: "Shipping Company",
        stage_delegate: "Delivery Rep",
        stage_customer: "Customer"
    },
    ar: {
        admin_platform_name: "وصلة - لوحة التحكم",
        nav_dashboard: "الرئيسية",
        nav_companies: "شركات الشحن",
        nav_merchants: "التجار",
        nav_orders: "الطلبات",
        nav_delegates: "مندوبي التوصيل",
        nav_pricing: "باقات الأسعار",
        nav_settings: "الإعدادات",
        admin_name: "أحمد محمد",
        admin_role: "مدير النظام",
        stat_companies: "إجمالي الشركات",
        stat_merchants: "إجمالي التجار",
        stat_orders: "إجمالي الطلبات",
        stat_delegates: "إجمالي المندوبين",
        col_order_id: "رقم الطلب",
        col_customer: "العميل",
        col_date: "التاريخ",
        col_status: "الحالة",
        col_actions: "إجراءات",
        col_name: "الاسم",
        col_phone: "الهاتف",
        view_details: "عرض",
        status_active: "نشط",
        status_available: "متاح",
        status_processing: "قيد المعالجة",
        status_shipped: "تم الشحن",
        status_delivered: "تم التوصيل",
        status_cancelled: "ملغي",
        logout: "تسجيل الخروج",
        back: "رجوع",
        btn_save: "حفظ التغييرات",
        view_all: "عرض الكل",
        recent_orders: "أحدث الطلبات",
        registered_companies: "الشركات المسجلة",
        settings_title: "الإعدادات العامة",
        set_platform_name: "اسم المنصة",
        set_email: "البريد الإلكتروني للدعم",
        set_phone: "هاتف الدعم",
        order_timeline: "مراحل التوصيل",
        info_customer: "معلومات العميل",
        info_merchant: "معلومات التاجر",
        info_payment: "تفاصيل الدفع",
        full_name: "الاسم الكامل",
        phone_number: "رقم الهاتف",
        business_address: "العنوان",
        store_name: "اسم المتجر",
        col_amount: "المبلغ الإجمالي",
        col_payment_method: "طريقة الدفع",
        currency: "ريال",
        stage_merchant: "التاجر",
        stage_company: "شركة الشحن",
        stage_delegate: "مندوب التوصيل",
        stage_customer: "العميل"
    }
};

// ======================== 2. LANGUAGE & DIRECTION HANDLER ========================
window.setLanguage = function(lang) {
    localStorage.setItem('lang', lang);
    document.documentElement.setAttribute('lang', lang);
    document.documentElement.dir = lang === 'ar' ? 'rtl' : 'ltr';
    
    // Update static elements with data-i18n
    document.querySelectorAll('[data-i18n]').forEach(el => {
        const key = el.getAttribute('data-i18n');
        if (window.translations[lang] && window.translations[lang][key]) {
            // If element contains only text or has no children
            if (el.children.length === 0 || el.hasAttribute('data-i18n-raw')) {
                el.textContent = window.translations[lang][key];
            } else {
                // For elements with inner HTML (badges etc) – replace text content only
                const originalHtml = el.innerHTML;
                const tempDiv = document.createElement('div');
                tempDiv.innerHTML = originalHtml;
                // Simple replacement for direct text nodes? safer: just set text
                el.textContent = window.translations[lang][key];
            }
        }
    });
    
    // Update dynamic tables (if any)
    if (typeof window.updateDynamicTranslations === 'function') {
        window.updateDynamicTranslations();
    }
    
    // Update active class on language buttons
    document.querySelectorAll('.lang-btn').forEach(btn => {
        const btnLang = btn.getAttribute('data-lang');
        if (btnLang === lang) {
            btn.classList.add('active');
        } else {
            btn.classList.remove('active');
        }
    });
};

// Helper: translate dynamic rows (called after table population)
window.updateDynamicTranslations = function() {
    const currentLang = document.documentElement.getAttribute('lang') || localStorage.getItem('lang') || 'en';
    document.querySelectorAll('[data-i18n]').forEach(el => {
        const key = el.getAttribute('data-i18n');
        if (window.translations[currentLang] && window.translations[currentLang][key]) {
            el.textContent = window.translations[currentLang][key];
        }
    });
};

// ======================== 3. HELPER FUNCTIONS ========================
function escapeHtml(str) {
    if (!str) return '';
    return str.replace(/[&<>]/g, function(m) {
        if (m === '&') return '&amp;';
        if (m === '<') return '&lt;';
        if (m === '>') return '&gt;';
        return m;
    });
}

window.showToast = function(message, type = 'success') {
    const toast = document.createElement('div');
    toast.className = `admin-toast admin-toast-${type}`;
    toast.textContent = message;
    toast.style.cssText = 'position:fixed; bottom:20px; right:20px; background:#10b981; color:white; padding:0.75rem 1.5rem; border-radius:40px; font-weight:500; z-index:9999; box-shadow:0 10px 15px -3px rgba(0,0,0,0.1); opacity:0; transform:translateY(20px); transition:opacity 0.2s, transform 0.2s;';
    if (type === 'error') toast.style.backgroundColor = '#ef4444';
    document.body.appendChild(toast);
    setTimeout(() => { toast.style.opacity = '1'; toast.style.transform = 'translateY(0)'; }, 10);
    setTimeout(() => {
        toast.style.opacity = '0';
        toast.style.transform = 'translateY(20px)';
        setTimeout(() => toast.remove(), 300);
    }, 3000);
};

// ======================== 4. DASHBOARD LOGIC ========================
function updateAdminStats() {
    const countOrdersEl = document.getElementById('count-orders');
    const countDelegatesEl = document.getElementById('count-delegates');
    const countCompaniesEl = document.getElementById('count-companies');
    const countMerchantsEl = document.getElementById('count-merchants');
    if (countOrdersEl && window.DB) countOrdersEl.textContent = window.DB.getOrders().length;
    if (countDelegatesEl && window.DB) countDelegatesEl.textContent = window.DB.getUsers('delegate').length;
    if (countCompaniesEl && window.DB) countCompaniesEl.textContent = window.DB.getUsers('company').length;
    if (countMerchantsEl && window.DB) countMerchantsEl.textContent = window.DB.getUsers('merchant').length;
}

function populateAdminTables() {
    if (!window.DB) return;

    // Merchants
    const merchantsList = document.getElementById('merchants-list');
    if (merchantsList) {
        const merchants = window.DB.getUsers('merchant');
        merchantsList.innerHTML = merchants.map(m => `
            <tr>
                <td><strong>${escapeHtml(m.name)}</strong></td>
                <td>${escapeHtml(m.phone || '-')}</td>
                <td>${m.ordersCount || 0}</td>
                <td><span class="admin-badge admin-badge-success" data-i18n="status_active">Active</span></td>
                <td><a href="merchant-details.html?id=${m.id}" class="admin-btn admin-btn-outline" data-i18n="view_details">View</a></td>
            </tr>
        `).join('');
    }

    // Companies
    const companiesList = document.getElementById('companies-list');
    if (companiesList) {
        const companies = window.DB.getUsers('company');
        companiesList.innerHTML = companies.map(c => `
            <tr>
                <td><strong>${escapeHtml(c.name)}</strong></td>
                <td>${escapeHtml(c.phone || '-')}</td>
                <td>${c.ordersCount || 0}</td>
                <td><span class="admin-badge admin-badge-success" data-i18n="status_active">Active</span></td>
                <td><a href="company-details.html?id=${c.id}" class="admin-btn admin-btn-outline" data-i18n="view_details">View</a></td>
            </tr>
        `).join('');
    }

    // Delegates
    const delegatesList = document.getElementById('delegates-list');
    if (delegatesList) {
        const delegates = window.DB.getUsers('delegate');
        delegatesList.innerHTML = delegates.map(d => `
            <tr>
                <td><strong>${escapeHtml(d.name)}</strong></td>
                <td>${escapeHtml(d.phone || '-')}</td>
                <td><span class="admin-badge admin-badge-success" data-i18n="status_available">Available</span></td>
                <td><a href="delegate-details.html?id=${d.id}" class="admin-btn admin-btn-outline" data-i18n="view_details">View</a></td>
            </tr>
        `).join('');
    }

    // Orders
    const ordersList = document.getElementById('orders-list');
    if (ordersList) {
        const orders = window.DB.getOrders();
        ordersList.innerHTML = orders.map(o => {
            let badgeClass = 'admin-badge-warning';
            let statusKey = `status_${o.status.toLowerCase()}`;
            if (o.status === 'processing') badgeClass = 'admin-badge-primary';
            if (o.status === 'shipped') badgeClass = 'admin-badge-primary';
            if (o.status === 'delivered') badgeClass = 'admin-badge-success';
            if (o.status === 'cancelled') badgeClass = 'admin-badge-danger';
            return `
                <tr>
                    <td><strong>${escapeHtml(o.id)}</strong></td>
                    <td>${escapeHtml(o.customerName)}</td>
                    <td>${escapeHtml(o.date)}</td>
                    <td><span class="admin-badge ${badgeClass}" data-i18n="${statusKey}">${o.status}</span></td>
                    <td><a href="order-details.html?id=${o.id}" class="admin-btn admin-btn-outline" data-i18n="view_details">View</a></td>
                </tr>
            `;
        }).join('');
    }

    // Re-apply translation to dynamic content
    window.updateDynamicTranslations();
}

// ======================== 5. DETAIL PAGE SAVE HANDLER (toast + redirect) ========================
function enhanceDetailForms() {
    const editForm = document.getElementById('edit-form');
    if (editForm && !editForm.hasAttribute('data-enhanced')) {
        editForm.setAttribute('data-enhanced', 'true');
        editForm.addEventListener('submit', (e) => {
            // Do not prevent default – let original inline script handle saving
            setTimeout(() => {
                const lang = localStorage.getItem('lang') || 'en';
                const msg = lang === 'ar' ? 'تم الحفظ بنجاح!' : 'Saved successfully!';
                if (window.showToast) window.showToast(msg, 'success');
            }, 150);
        });
    }
}

// ======================== 6. ORDER DETAILS PAGE (Timeline & Actions) ========================
function renderOrderDetails() {
    if (!document.getElementById('tracking-timeline')) return;
    const urlParams = new URLSearchParams(window.location.search);
    const orderId = urlParams.get('id') || 'ORD-1024';
    const role = (window.DB && window.DB.getCurrentRole) ? window.DB.getCurrentRole() : 'admin';
    const orders = window.DB ? window.DB.getOrders() : [];
    const order = orders.find(o => o.id === orderId) || orders[0] || { id: 'N/A', date: 'N/A', customer: 'N/A', merchant: 'N/A', amount: 0, status: 'pending', history: ['pending'] };

    document.getElementById('order-title').textContent = `${window.translations[document.documentElement.lang]?.col_order_id || 'Order ID'} #${order.id}`;
    const dateEl = document.getElementById('order-date');
    if (dateEl) dateEl.textContent = `${window.translations[document.documentElement.lang]?.col_date || 'Date'}: ${order.date}`;
    const customerNameSpan = document.getElementById('customer-name');
    if (customerNameSpan) customerNameSpan.textContent = order.customer;
    const merchantNameSpan = document.getElementById('merchant-name');
    if (merchantNameSpan) merchantNameSpan.textContent = order.merchant;
    const amountSpan = document.getElementById('order-amount');
    if (amountSpan) amountSpan.textContent = order.amount;

    const badge = document.getElementById('order-status-badge');
    if (badge) {
        let badgeClass = 'admin-badge-warning';
        if (order.status === 'processing') badgeClass = 'admin-badge-primary';
        if (order.status === 'shipped') badgeClass = 'admin-badge-primary';
        if (order.status === 'delivered') badgeClass = 'admin-badge-success';
        if (order.status === 'cancelled') badgeClass = 'admin-badge-danger';
        badge.className = `admin-badge ${badgeClass}`;
        badge.textContent = window.translations[document.documentElement.lang]?.[`status_${order.status}`] || order.status;
    }

    // Timeline
    const timelineContainer = document.getElementById('tracking-timeline');
    if (timelineContainer) {
        const stages = [
            { id: 'pending', key: 'stage_merchant', icon: '<path d="M19 3H5c-1.1 0-2 .9-2 2v14c0 1.1.9 2 2 2h14c1.1 0 2-.9 2-2V5c0-1.1-.9-2-2-2zM9 17H7v-7h2v7zm4 0h-2V7h2v10zm4 0h-2v-4h2v4z"/>' },
            { id: 'processing', key: 'stage_company', icon: '<path d="M20 8h-3V4H3c-1.1 0-2 .9-2 2v11h2c0 1.66 1.34 3 3 3s3-1.34 3-3h6c0 1.66 1.34 3 3 3s3-1.34 3-3h2v-5l-3-4zM6 18.5c-.83 0-1.5-.67-1.5-1.5s.67-1.5 1.5-1.5 1.5.67 1.5 1.5-.67 1.5-1.5 1.5zm13.5-9l1.96 2.5H17V9.5h2.5zm-1.5 9c-.83 0-1.5-.67-1.5-1.5s.67-1.5 1.5-1.5 1.5.67 1.5 1.5-.67 1.5-1.5 1.5z"/>' },
            { id: 'shipped', key: 'stage_delegate', icon: '<path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/>' },
            { id: 'delivered', key: 'stage_customer', icon: '<path d="M9 16.17L4.83 12l-1.42 1.41L9 19 21 7l-1.41-1.41z"/>' }
        ];
        timelineContainer.innerHTML = '';
        stages.forEach(stage => {
            const step = document.createElement('div');
            step.className = 'tracking-step';
            if (order.history && order.history.includes(stage.id)) {
                step.classList.add(order.status === stage.id ? 'active' : 'completed');
            }
            step.innerHTML = `
                <div class="tracking-icon">
                    <svg viewBox="0 0 24 24" width="24" height="24" fill="currentColor">${stage.icon}</svg>
                </div>
                <div class="tracking-label">${window.translations[document.documentElement.lang]?.[stage.key] || stage.key}</div>
            `;
            timelineContainer.appendChild(step);
        });
    }

    // Action panel (for non-admin roles)
    const actionPanel = document.getElementById('action-panel');
    if (actionPanel && role !== 'admin') {
        actionPanel.style.display = 'block';
        const actionBtns = document.getElementById('action-buttons');
        if (actionBtns) {
            actionBtns.innerHTML = '';
            const actions = ['processing', 'shipped', 'delivered', 'cancelled'];
            actions.forEach(act => {
                if (order.status !== act) {
                    const btn = document.createElement('button');
                    btn.className = 'admin-btn admin-btn-primary';
                    btn.textContent = window.translations[document.documentElement.lang]?.[`status_${act}`] || act;
                    btn.onclick = () => { if (window.DB) window.DB.updateOrderStatus(order.id, act); renderOrderDetails(); };
                    actionBtns.appendChild(btn);
                }
            });
        }
    } else if (actionPanel) {
        actionPanel.style.display = 'none';
    }
}

// ======================== 7. SIDEBAR TOGGLE & INITIALIZATION ========================
document.addEventListener('DOMContentLoaded', () => {
    // Set language from localStorage or default
    const savedLang = localStorage.getItem('lang') || 'en';
    window.setLanguage(savedLang);

    // Sidebar toggle
    const toggleBtn = document.getElementById('admin-sidebar-toggle');
    const adminApp = document.querySelector('.admin-app');
    if (toggleBtn && adminApp) {
        toggleBtn.addEventListener('click', (e) => {
            e.preventDefault();
            adminApp.classList.toggle('sidebar-collapsed');
            localStorage.setItem('admin_sidebar_collapsed', adminApp.classList.contains('sidebar-collapsed'));
        });
        const savedState = localStorage.getItem('admin_sidebar_collapsed') === 'true';
        if (savedState) adminApp.classList.add('sidebar-collapsed');
    }

    // Attach language switcher events
    document.querySelectorAll('.lang-btn').forEach(btn => {
        btn.addEventListener('click', () => {
            const lang = btn.getAttribute('data-lang');
            if (lang) window.setLanguage(lang);
        });
    });

    // If we are on detail page (has edit-form), enhance it
    if (document.getElementById('edit-form')) {
        enhanceDetailForms();
    }

    // If we are on order-details page, render timeline & actions
    if (document.getElementById('tracking-timeline')) {
        // wait for DB
        const waitForDB = setInterval(() => {
            if (window.DB && typeof window.DB.getOrders === 'function') {
                clearInterval(waitForDB);
                renderOrderDetails();
            }
        }, 100);
    }

    // For dashboard & list pages: wait for DB then populate
    function isDBReady() { return window.DB && typeof window.DB.getUsers === 'function' && typeof window.DB.getOrders === 'function'; }
    function initDashboard() {
        if (!isDBReady()) {
            setTimeout(initDashboard, 200);
            return;
        }
        updateAdminStats();
        populateAdminTables();
    }
    if (document.querySelector('.admin-stats-grid') || document.getElementById('merchants-list') || document.getElementById('orders-list')) {
        initDashboard();
    }
});