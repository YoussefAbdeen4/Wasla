/**
 * Merchant Dashboard - Full Working Version with Real Excel (XLSX)
 * يدعم: تحميل قالب Excel حقيقي، رفع Excel، PDF، الطلبات، المالية، الترجمة
 */

(function() {
    // ======================== Mock Database ========================
    window.MockDB = (function() {
        let orders = [
            { id: 1001, customer: 'محمد عبدالله', phone: '01001234567', address: '36 شارع النيل', city: 'القاهرة', total: '450', status: 'تم التوصيل', date: '2025-05-20', merchantId: 'MERCH-1' },
            { id: 1002, customer: 'نورا أحمد', phone: '01123456789', address: '12 شارع البحر', city: 'الإسكندرية', total: '1200', status: 'قيد المعالجة', date: '2025-05-21', merchantId: 'MERCH-1' },
            { id: 1003, customer: 'يوسف سامي', phone: '01234567890', address: '7 شارع المطار', city: 'الجيزة', total: '320', status: 'قيد الانتظار', date: '2025-05-22', merchantId: 'MERCH-1' },
            { id: 1004, customer: 'فاطمة إبراهيم', phone: '01567890123', address: '22 شارع الجمهورية', city: 'بورسعيد', total: '780', status: 'مرتجع', date: '2025-05-18', merchantId: 'MERCH-1' }
        ];
        let transactions = [
            { id: 'TRX-1001', type: 'إيرادات الطلب #ORD-1001', amount: '+450', status: 'مكتمل', date: '2025-05-20' },
            { id: 'TRX-1002', type: 'سحب بنكي', amount: '-10000', status: 'قيد المعالجة', date: '2025-05-19' },
            { id: 'TRX-1003', type: 'إيرادات الطلب #ORD-1004', amount: '+780', status: 'مكتمل', date: '2025-05-18' }
        ];
        let nextId = 1005;
        return {
            getAll: (col) => col === 'orders' ? orders : (col === 'transactions' ? transactions : []),
            getById: (col, id) => (col === 'orders' ? orders.find(o => o.id == id) : null),
            create: (col, item) => {
                if (col === 'orders') {
                    item.id = nextId++;
                    item.date = new Date().toISOString().split('T')[0];
                    item.status = 'قيد الانتظار';
                    orders.push(item);
                    return item;
                }
            },
            bulkCreate: (col, items) => {
                if (col === 'orders') {
                    items.forEach(item => {
                        item.id = nextId++;
                        item.date = new Date().toISOString().split('T')[0];
                        item.status = 'قيد الانتظار';
                        orders.push(item);
                    });
                    return items.length;
                }
                return 0;
            },
            getTransactions: () => [...transactions],
            getProfile: () => ({ storeName: 'متجر النيل للأزياء', email: 'info@nilefashion.com', phone: '+201012345678', address: '36 شارع النيل، الجيزة' }),
            updateProfile: (data) => { console.log('Profile updated', data); alert('تم حفظ الملف الشخصي'); },
            getSettings: () => ({ apiKey: 'wasla_live_123', webhook: '', emailNotif: true, smsNotif: true }),
            updateSettings: (data) => { console.log('Settings saved', data); alert('تم حفظ الإعدادات'); }
        };
    })();

    // ======================== الترجمة ========================
    const translations = {
        ar: {
            nav_dashboard: 'الرئيسية', nav_orders: 'الطلبات', nav_add_order: 'إضافة طلب',
            nav_wallet: 'المالية', nav_profile: 'الملف الشخصي', nav_settings: 'الإعدادات',
            logout: 'تسجيل خروج', total_orders: 'إجمالي الطلبات', delivered: 'تم التوصيل',
            rejected: 'مرفوض / مرتجع', under_review: 'قيد المراجعة', view_all: 'عرض الكل',
            add_order_btn: 'إضافة طلب جديد', back: 'رجوع', save: 'حفظ', cancel: 'إلغاء',
            available_balance: 'الرصيد المتاح', pending_clearance: 'معلق للتحصيل',
            total_withdrawn: 'إجمالي المسحوب', total_revenue: 'إجمالي الإيرادات',
            export_pdf: 'تصدير PDF', recent_transactions: 'أحدث المعاملات',
            col_order_id: 'رقم الطلب', col_customer: 'العميل', col_amount: 'المبلغ',
            col_status: 'الحالة', col_date: 'التاريخ', col_actions: 'إجراءات',
            view_details: 'تفاصيل', notifications: 'الإشعارات', mark_all_read: 'تحديد الكل كمقروء',
            api_integrations: 'API والإضافات', api_key: 'مفتاح API', copy: 'نسخ',
            webhook_url: 'رابط Webhook', notification_settings: 'إعدادات الإشعارات',
            email_notifications: 'إشعارات البريد الإلكتروني', sms_notifications: 'إشعارات SMS',
            save_config: 'حفظ الإعدادات', welcome: 'مرحباً بعودتك، أحمد!',
            dashboard_desc: 'ملخص أداء متجرك في السوق المصري', merchant_name: 'أحمد حسني',
            store_name: 'متجر النيل للأزياء'
        },
        en: {
            nav_dashboard: 'Dashboard', nav_orders: 'Orders', nav_add_order: 'Add Order',
            nav_wallet: 'Financials', nav_profile: 'Profile', nav_settings: 'Settings',
            logout: 'Logout', total_orders: 'Total Orders', delivered: 'Delivered',
            rejected: 'Rejected / Returned', under_review: 'Under Review', view_all: 'View All',
            add_order_btn: 'Add New Order', back: 'Back', save: 'Save', cancel: 'Cancel',
            available_balance: 'Available Balance', pending_clearance: 'Pending Clearance',
            total_withdrawn: 'Total Withdrawn', total_revenue: 'Total Revenue',
            export_pdf: 'Export PDF', recent_transactions: 'Recent Transactions',
            col_order_id: 'Order ID', col_customer: 'Customer', col_amount: 'Amount',
            col_status: 'Status', col_date: 'Date', col_actions: 'Actions',
            view_details: 'View', notifications: 'Notifications', mark_all_read: 'Mark all as read',
            api_integrations: 'API & Integrations', api_key: 'API Key', copy: 'Copy',
            webhook_url: 'Webhook URL', notification_settings: 'Notification Settings',
            email_notifications: 'Email notifications', sms_notifications: 'SMS notifications',
            save_config: 'Save Configuration', welcome: 'Welcome Back, Ahmed!',
            dashboard_desc: "Here's a summary of your store's performance",
            merchant_name: 'Ahmed Husni', store_name: 'Nile Fashion Store'
        }
    };

    let currentLang = localStorage.getItem('lang') || 'ar';

    function translatePage() {
        document.querySelectorAll('[data-i18n]').forEach(el => {
            const key = el.getAttribute('data-i18n');
            if (translations[currentLang] && translations[currentLang][key]) {
                if (el.tagName === 'INPUT' || el.tagName === 'TEXTAREA') {
                    el.placeholder = translations[currentLang][key];
                } else {
                    el.innerHTML = translations[currentLang][key];
                }
            }
        });
        document.documentElement.setAttribute('dir', currentLang === 'ar' ? 'rtl' : 'ltr');
        document.documentElement.lang = currentLang;
    }

    // ======================== دوال مساعدة ========================
    function formatCurrency(amount) { return `${amount} E.G`; }

    function getStatusBadgeClass(status) {
        switch(status) {
            case 'تم التوصيل': return 'merchant-badge-success';
            case 'قيد المعالجة': return 'merchant-badge-primary';
            case 'قيد الانتظار': return 'merchant-badge-warning';
            case 'مرتجع': return 'merchant-badge-danger';
            default: return 'merchant-badge-warning';
        }
    }

    // ======================== تحميل قالب Excel حقيقي (.xlsx) ========================
    function downloadExcelTemplate() {
        // بيانات القالب: الأعمدة + صفين نموذجيين
        const templateData = [
            ['Customer Name', 'Phone', 'Address', 'City', 'Total Amount (EGP)', 'Notes'],
            ['محمد علي', '01001234567', 'شارع النيل', 'القاهرة', '500', ''],
            ['نورا حسين', '01123456789', 'شارع البحر', 'الإسكندرية', '750', 'توصيل سريع']
        ];
        // إنشاء ورقة عمل
        const ws = XLSX.utils.aoa_to_sheet(templateData);
        // ضبط عرض الأعمدة
        ws['!cols'] = [{wch:20}, {wch:15}, {wch:30}, {wch:15}, {wch:18}, {wch:25}];
        // إنشاء مصنف
        const wb = XLSX.utils.book_new();
        XLSX.utils.book_append_sheet(wb, ws, 'Orders_Template');
        // تصدير الملف
        XLSX.writeFile(wb, 'order_template.xlsx');
    }

    // ======================== رفع ملف Excel واستيراد الطلبات ========================
    function setupExcelUpload() {
        // إنشاء input مخفي لرفع الملف
        let fileInput = document.getElementById('excel-file-input');
        if (!fileInput) {
            fileInput = document.createElement('input');
            fileInput.type = 'file';
            fileInput.id = 'excel-file-input';
            fileInput.accept = '.xlsx, .xls, .csv';
            fileInput.style.display = 'none';
            document.body.appendChild(fileInput);
        }
        // ربط حدث رفع الملف
        fileInput.onchange = function(e) {
            const file = e.target.files[0];
            if (!file) return;
            const reader = new FileReader();
            reader.onload = function(loadEvent) {
                const data = new Uint8Array(loadEvent.target.result);
                const workbook = XLSX.read(data, { type: 'array' });
                const firstSheet = workbook.Sheets[workbook.SheetNames[0]];
                const rows = XLSX.utils.sheet_to_json(firstSheet, { header: 1, defval: "" });
                if (!rows || rows.length < 2) {
                    alert('الملف لا يحتوي على بيانات صالحة');
                    return;
                }
                // توقع وجود رأس الأعمدة في الصف الأول
                const headers = rows[0];
                const nameIndex = headers.findIndex(h => h && (h.includes('Customer') || h.includes('Name') || h.includes('العميل')));
                const phoneIndex = headers.findIndex(h => h && (h.includes('Phone') || h.includes('هاتف')));
                const addressIndex = headers.findIndex(h => h && (h.includes('Address') || h.includes('عنوان')));
                const cityIndex = headers.findIndex(h => h && (h.includes('City') || h.includes('مدينة')));
                const totalIndex = headers.findIndex(h => h && (h.includes('Total') || h.includes('Amount') || h.includes('المبلغ')));
                
                if (nameIndex === -1 || phoneIndex === -1 || totalIndex === -1) {
                    alert('تنسيق الملف غير صحيح. تأكد من وجود أعمدة: الاسم، الهاتف، المبلغ');
                    return;
                }
                const newOrders = [];
                for (let i = 1; i < rows.length; i++) {
                    const row = rows[i];
                    if (!row || row.length === 0) continue;
                    const customer = row[nameIndex] ? row[nameIndex].toString().trim() : '';
                    const phone = row[phoneIndex] ? row[phoneIndex].toString().trim() : '';
                    const address = (addressIndex !== -1 && row[addressIndex]) ? row[addressIndex].toString().trim() : '';
                    const city = (cityIndex !== -1 && row[cityIndex]) ? row[cityIndex].toString().trim() : 'القاهرة';
                    let total = (totalIndex !== -1 && row[totalIndex]) ? row[totalIndex].toString().trim() : '';
                    total = total.replace(/[^0-9]/g, '');
                    if (customer && phone && total) {
                        newOrders.push({
                            customer, phone, address, city, total,
                            merchantId: 'MERCH-1'
                        });
                    }
                }
                if (newOrders.length === 0) {
                    alert('لم يتم العثور على طلبات صالحة في الملف');
                    return;
                }
                const count = window.MockDB.bulkCreate('orders', newOrders);
                alert(`تم استيراد ${count} طلب بنجاح`);
                // تحديث الصفحة الحالية (إذا كانت الرئيسية أو الطلبات)
                const path = window.location.pathname;
                if (path.includes('index.html') || path.endsWith('/merchant/')) initDashboard();
                else if (path.includes('orders.html')) initOrdersPage();
                // إعادة تعيين input ليمكن رفع نفس الملف مرة أخرى
                fileInput.value = '';
            };
            reader.readAsArrayBuffer(file);
        };
        // ربط النقر على بطاقة رفع الملف
        const uploadCard = document.getElementById('excel-upload-card');
        if (uploadCard) {
            uploadCard.onclick = (e) => {
                // منع التنبيه القديم
                if (e.target && e.target.closest('[data-browse-btn]')) {
                    e.stopPropagation();
                }
                fileInput.click();
            };
        }
        // ربط زر التصفح الموجود داخل البطاقة
        const browseBtn = document.querySelector('[data-browse-btn]');
        if (browseBtn) {
            browseBtn.onclick = (e) => {
                e.stopPropagation();
                fileInput.click();
            };
        }
    }

    // ======================== تصدير PDF (يعمل حقيقياً) ========================
    async function exportToPDF(tableId, title) {
        const table = document.getElementById(tableId);
        if (!table) {
            alert('لا يوجد جدول لتصديره');
            return;
        }
        if (typeof html2canvas === 'undefined' || typeof window.jspdf === 'undefined') {
            alert('مكتبات PDF غير محملة. تأكد من اتصال الإنترنت وأعد تحميل الصفحة.');
            return;
        }
        const btn = document.getElementById('export-financial-btn');
        const originalText = btn ? btn.innerHTML : '';
        if (btn) {
            btn.innerHTML = '<i class="fas fa-spinner fa-spin"></i> جاري التحميل...';
            btn.disabled = true;
        }
        try {
            const originalContainer = table.closest('.merchant-table-container') || table.parentElement;
            const cloneContainer = originalContainer.cloneNode(true);
            cloneContainer.style.width = '100%';
            cloneContainer.style.background = 'white';
            cloneContainer.style.padding = '20px';
            cloneContainer.style.borderRadius = '8px';
            
            const titleDiv = document.createElement('div');
            titleDiv.style.textAlign = currentLang === 'ar' ? 'right' : 'left';
            titleDiv.style.marginBottom = '20px';
            titleDiv.style.fontFamily = 'Cairo, Inter, sans-serif';
            titleDiv.innerHTML = `
                <h2 style="color:#0061ff; margin:0;">${title}</h2>
                <p style="color:#64748b; margin:5px 0 0;">تاريخ التقرير: ${new Date().toLocaleDateString(currentLang === 'ar' ? 'ar-EG' : 'en-EG')}</p>
            `;
            cloneContainer.prepend(titleDiv);
            
            cloneContainer.style.position = 'absolute';
            cloneContainer.style.top = '-9999px';
            cloneContainer.style.left = '-9999px';
            document.body.appendChild(cloneContainer);
            
            const canvas = await html2canvas(cloneContainer, { scale: 2, backgroundColor: '#ffffff', logging: false });
            document.body.removeChild(cloneContainer);
            
            const imgData = canvas.toDataURL('image/png');
            const { jsPDF } = window.jspdf;
            const imgWidth = 210;
            const imgHeight = (canvas.height * imgWidth) / canvas.width;
            const pdf = new jsPDF({ orientation: 'portrait', unit: 'mm', format: 'a4' });
            pdf.addImage(imgData, 'PNG', 0, 0, imgWidth, imgHeight);
            pdf.save('financial_report.pdf');
            alert('تم حفظ التقرير بنجاح');
        } catch (err) {
            console.error('PDF error:', err);
            alert('حدث خطأ في إنشاء PDF: ' + err.message);
        } finally {
            if (btn) {
                btn.innerHTML = originalText;
                btn.disabled = false;
            }
        }
    }

    // ======================== تهيئة الصفحات ========================
    function initDashboard() {
        const orders = window.MockDB.getAll('orders').filter(o => o.merchantId === 'MERCH-1');
        const statTotal = document.getElementById('stat-total');
        const statDelivered = document.getElementById('stat-delivered');
        const statRejected = document.getElementById('stat-rejected');
        const statReview = document.getElementById('stat-review');
        if (statTotal) statTotal.innerText = orders.length;
        if (statDelivered) statDelivered.innerText = orders.filter(o => o.status === 'تم التوصيل').length;
        if (statRejected) statRejected.innerText = orders.filter(o => o.status === 'مرتجع').length;
        if (statReview) statReview.innerText = orders.filter(o => o.status === 'قيد الانتظار' || o.status === 'قيد المعالجة').length;
        
        const tbody = document.getElementById('recent-orders-tbody');
        if (tbody) {
            tbody.innerHTML = '';
            orders.slice(-5).reverse().forEach(order => {
                tbody.innerHTML += `
                    <tr>
                        <td><strong>#ORD-${order.id}</strong></td>
                        <td>${order.customer}<br><span style="color:var(--merchant-text-secondary); font-size:0.75rem;">${order.city}، مصر</span></td>
                        <td>${formatCurrency(order.total)}</span></td>
                        <td><span class="merchant-badge ${getStatusBadgeClass(order.status)}">${order.status}</span></td>
                        <td>${order.date}</td>
                    </tr>
                `;
            });
        }
        const addOrderBtn = document.getElementById('add-order-redirect');
        if (addOrderBtn) addOrderBtn.onclick = () => window.location.href = 'add-order.html';
    }

    function initOrdersPage() {
        const orders = window.MockDB.getAll('orders').filter(o => o.merchantId === 'MERCH-1');
        const tbody = document.querySelector('#orders-table tbody');
        if (tbody) {
            tbody.innerHTML = '';
            orders.forEach(order => {
                tbody.innerHTML += `
                    <tr>
                        <td><strong>#ORD-${order.id}</strong></td>
                        <td>${order.customer}<br><span style="color:var(--merchant-text-secondary); font-size:0.75rem;">${order.city}، مصر</span></td>
                        <td>${formatCurrency(order.total)}</span></td>
                        <td><span class="merchant-badge ${getStatusBadgeClass(order.status)}">${order.status}</span></td>
                        <td>${order.date}</td>
                        <td><a href="order-details.html?id=${order.id}" class="merchant-btn merchant-btn-secondary">تفاصيل</a></td>
                    </tr>
                `;
            });
        }
        const newOrderBtn = document.getElementById('new-order-btn');
        if (newOrderBtn) newOrderBtn.onclick = () => window.location.href = 'add-order.html';
        
        const searchInput = document.getElementById('search-orders');
        if (searchInput) {
            searchInput.oninput = function(e) {
                const term = e.target.value.toLowerCase();
                const rows = document.querySelectorAll('#orders-table tbody tr');
                rows.forEach(row => {
                    const text = row.innerText.toLowerCase();
                    row.style.display = text.includes(term) ? '' : 'none';
                });
            };
        }
    }

    function initOrderDetails() {
        const id = new URLSearchParams(window.location.search).get('id');
        if (id) {
            const order = window.MockDB.getById('orders', parseInt(id));
            if (order) {
                if (document.getElementById('order-id-display')) document.getElementById('order-id-display').innerText = `#ORD-${order.id}`;
                if (document.getElementById('detail-customer')) document.getElementById('detail-customer').innerText = order.customer;
                if (document.getElementById('detail-phone')) document.getElementById('detail-phone').innerText = order.phone;
                if (document.getElementById('detail-address')) document.getElementById('detail-address').innerText = `${order.address}، ${order.city}`;
                if (document.getElementById('detail-total')) document.getElementById('detail-total').innerHTML = formatCurrency(order.total);
                if (document.getElementById('detail-date')) document.getElementById('detail-date').innerText = order.date;
                const statusSpan = document.getElementById('detail-status');
                if (statusSpan) {
                    statusSpan.innerText = order.status;
                    statusSpan.className = `merchant-badge ${getStatusBadgeClass(order.status)}`;
                }
            }
        }
        const backBtn = document.getElementById('back-to-orders');
        if (backBtn) backBtn.onclick = () => window.location.href = 'orders.html';
    }

    function initAddOrderForm() {
        const form = document.getElementById('add-order-form');
        if (form) {
            form.onsubmit = (e) => {
                e.preventDefault();
                const customer = document.getElementById('customer-name')?.value;
                const phone = document.getElementById('customer-phone')?.value;
                const address = document.getElementById('customer-address')?.value;
                const city = document.getElementById('customer-city')?.value;
                const total = document.getElementById('order-amount')?.value;
                if (!customer || !phone || !address || !city || !total) {
                    alert('الرجاء ملء جميع الحقول');
                    return;
                }
                window.MockDB.create('orders', { customer, phone, address, city, total, merchantId: 'MERCH-1' });
                alert('تم إنشاء الطلب بنجاح');
                window.location.href = 'orders.html';
            };
        }
        const backBtn = document.getElementById('back-to-orders');
        if (backBtn) backBtn.onclick = () => window.location.href = 'orders.html';
        const cancelBtn = document.getElementById('cancel-order');
        if (cancelBtn) cancelBtn.onclick = () => window.location.href = 'orders.html';
    }

    function initWalletPage() {
        const transactions = window.MockDB.getTransactions();
        const totalWithdrawn = transactions.filter(t => t.amount.startsWith('-')).reduce((s, t) => s + parseInt(t.amount.substring(1)), 0);
        const totalRevenue = transactions.filter(t => t.amount.startsWith('+')).reduce((s, t) => s + parseInt(t.amount.substring(1)), 0);
        
        const avail = document.getElementById('available-balance');
        const pending = document.getElementById('pending-clearance');
        const withdrawn = document.getElementById('total-withdrawn');
        const revenue = document.getElementById('total-revenue');
        if (avail) avail.innerHTML = `38,750 <span>E.G</span>`;
        if (pending) pending.innerHTML = `12,400 <span>E.G</span>`;
        if (withdrawn) withdrawn.innerHTML = `${totalWithdrawn} <span>E.G</span>`;
        if (revenue) revenue.innerHTML = `${totalRevenue} <span>E.G</span>`;
        
        const tbody = document.querySelector('#transactions-table tbody');
        if (tbody) {
            tbody.innerHTML = '';
            transactions.forEach(t => {
                const amountClass = t.amount.startsWith('+') ? 'merchant-success' : 'merchant-danger';
                tbody.innerHTML += `
                    <tr>
                        <td><strong>${t.id}</strong></td>
                        <td>${t.type}</td>
                        <td class="${amountClass}">${t.amount} E.G</td>
                        <td><span class="merchant-badge merchant-badge-success">${t.status}</span></td>
                        <td>${t.date}</td>
                    </tr>
                `;
            });
        }
        const exportBtn = document.getElementById('export-financial-btn');
        if (exportBtn) {
            exportBtn.onclick = () => exportToPDF('transactions-table', currentLang === 'ar' ? 'التقرير المالي' : 'Financial Report');
        }
    }

    function initProfilePage() {
        const profile = window.MockDB.getProfile();
        const form = document.getElementById('profile-form');
        if (form) {
            if (form.storeName) form.storeName.value = profile.storeName;
            if (form.email) form.email.value = profile.email;
            if (form.phone) form.phone.value = profile.phone;
            if (form.address) form.address.value = profile.address;
            form.onsubmit = (e) => {
                e.preventDefault();
                window.MockDB.updateProfile({
                    storeName: form.storeName.value,
                    email: form.email.value,
                    phone: form.phone.value,
                    address: form.address.value
                });
            };
        }
    }

    function initSettingsPage() {
        const settings = window.MockDB.getSettings();
        const webhook = document.getElementById('webhook-url');
        const emailChk = document.getElementById('email-notifications');
        const smsChk = document.getElementById('sms-notifications');
        if (webhook) webhook.value = settings.webhook || '';
        if (emailChk) emailChk.checked = settings.emailNotif;
        if (smsChk) smsChk.checked = settings.smsNotif;
        
        const saveBtn = document.getElementById('save-settings');
        if (saveBtn) {
            saveBtn.onclick = () => {
                window.MockDB.updateSettings({
                    webhook: webhook ? webhook.value : '',
                    emailNotif: emailChk ? emailChk.checked : true,
                    smsNotif: smsChk ? smsChk.checked : true
                });
            };
        }
        const copyBtn = document.getElementById('copy-api-key');
        if (copyBtn) {
            copyBtn.onclick = () => {
                navigator.clipboard.writeText(settings.apiKey);
                alert('تم نسخ المفتاح');
            };
        }
    }

    function initNotificationsPage() {
        const notifications = window.MockDB.getNotifications();
        const container = document.getElementById('notifications-container');
        if (container) {
            container.innerHTML = '';
            notifications.forEach(n => {
                const div = document.createElement('div');
                div.style.cssText = 'padding:1.5rem; border-bottom:1px solid var(--merchant-border); display:flex; gap:1.5rem; align-items:flex-start;';
                if (!n.read) div.style.backgroundColor = 'var(--merchant-primary-light)';
                div.innerHTML = `
                    <div style="width:12px; height:12px; border-radius:50%; background:${!n.read ? 'var(--merchant-primary)' : 'transparent'}; margin-top:6px;"></div>
                    <div>
                        <h4 style="margin:0 0 0.25rem 0;"><i class="fas fa-bell"></i> ${n.title}</h4>
                        <p style="margin:0 0 0.5rem 0;">${n.message}</p>
                        <span style="font-size:0.75rem;">${n.time}</span>
                    </div>
                `;
                container.appendChild(div);
            });
        }
        const markAll = document.getElementById('mark-all-read');
        if (markAll) {
            markAll.onclick = () => {
                window.MockDB.getNotifications().forEach(n => window.MockDB.markNotificationRead(n.id));
                initNotificationsPage();
                alert('تم تحديث الإشعارات');
            };
        }
    }

    // ======================== تشغيل التطبيق ========================
    function initLanguageSwitcher() {
        const btns = document.querySelectorAll('.lang-btn');
        const setLang = (lang) => {
            localStorage.setItem('lang', lang);
            currentLang = lang;
            translatePage();
            btns.forEach(b => b.classList.remove('active'));
            const active = document.querySelector(`.lang-btn[data-lang="${lang}"]`);
            if (active) active.classList.add('active');
        };
        btns.forEach(btn => {
            btn.onclick = () => setLang(btn.dataset.lang);
        });
        setLang(currentLang);
    }

    function initSidebarToggle() {
        const toggle = document.getElementById('merchant-sidebar-toggle');
        const sidebar = document.querySelector('.merchant-sidebar');
        if (toggle && sidebar) {
            toggle.onclick = (e) => { e.stopPropagation(); sidebar.classList.toggle('active'); };
            document.addEventListener('click', (e) => {
                if (window.innerWidth <= 768 && sidebar && !sidebar.contains(e.target) && !toggle.contains(e.target))
                    sidebar.classList.remove('active');
            });
        }
    }

    function initGlobalNavigation() {
        const profileDiv = document.querySelector('[data-profile-link], .merchant-user-profile');
        if (profileDiv && !profileDiv.hasAttribute('data-listener')) {
            profileDiv.onclick = () => window.location.href = 'profile.html';
            profileDiv.setAttribute('data-listener', 'true');
        }
        // زر تحميل القالب (Excel) في الصفحة الرئيسية
        const templateBtn = document.getElementById('download-template-btn');
        if (templateBtn && !templateBtn.hasAttribute('data-listener')) {
            templateBtn.onclick = downloadExcelTemplate;
            templateBtn.setAttribute('data-listener', 'true');
        }
    }

    document.addEventListener('DOMContentLoaded', () => {
        initSidebarToggle();
        initLanguageSwitcher();
        setupExcelUpload(); // تفعيل رفع Excel
        initGlobalNavigation();
        const path = window.location.pathname;
        if (path.includes('index.html') || path.endsWith('/merchant/') || path.endsWith('/merchant')) initDashboard();
        else if (path.includes('orders.html')) initOrdersPage();
        else if (path.includes('add-order.html')) initAddOrderForm();
        else if (path.includes('order-details.html')) initOrderDetails();
        else if (path.includes('wallet.html')) initWalletPage();
        else if (path.includes('profile.html')) initProfilePage();
        else if (path.includes('settings.html')) initSettingsPage();
        else if (path.includes('notifications.html')) initNotificationsPage();
    });
})();