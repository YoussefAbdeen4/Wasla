// financials.js - إدارة المالية (المتوقع تحصيله، المسلم للشركة، الرصيد، جدول التحصيل)
(function() {
    function updateFinancialStats() {
        const expected = getCompanyExpectedCash();
        const submitted = getCompanySubmittedCash();
        const balance = expected - submitted;
        const expectedEl = document.getElementById('expectedCashValue');
        const submittedEl = document.getElementById('submittedCashValue');
        const balanceEl = document.getElementById('balanceValue');
        if (expectedEl) expectedEl.innerHTML = expected.toFixed(2) + ' ج.م';
        if (submittedEl) submittedEl.innerHTML = submitted.toFixed(2) + ' ج.م';
        if (balanceEl) balanceEl.innerHTML = balance.toFixed(2) + ' ج.م';
    }

    function loadCollectionTable() {
        const transactions = getTransactions();
        const delegates = getDelegates();
        const delegateMap = {};
        delegates.forEach(d => { delegateMap[d.id] = d.name; });
        const tbody = document.getElementById('collectionTableBody');
        if (!tbody) return;
        if (transactions.length === 0) {
            tbody.innerHTML = '<tr><td colspan="3" style="text-align:center;">لا توجد عمليات تحصيل بعد<\/td></tr>';
            return;
        }
        tbody.innerHTML = transactions.map(t => `
            <tr>
                <td>${delegateMap[t.delegateId] || 'مندوب غير معروف'}<\/td>
                <td>${t.amount.toFixed(2)} ج.م<\/td>
                <td>${new Date(t.date).toLocaleString()}<\/td>
            </tr>
        `).join('');
    }

    document.addEventListener('DOMContentLoaded', () => {
        updateFinancialStats();
        loadCollectionTable();
        window.addEventListener('storage', (e) => {
            if (e.key === STORAGE_KEYS.ORDERS || e.key === STORAGE_KEYS.AGENTS || e.key === STORAGE_KEYS.TRANSACTIONS) {
                updateFinancialStats();
                loadCollectionTable();
            }
        });
    });
})();