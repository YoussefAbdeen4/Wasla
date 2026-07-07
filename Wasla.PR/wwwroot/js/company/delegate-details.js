// delegate-details.js - عرض بيانات مندوب وتعديله وحذفه
(function() {
    const urlParams = new URLSearchParams(window.location.search);
    const delegateId = urlParams.get('id');
    if (!delegateId) return;

    let currentDelegate = null;

    function loadDelegateData() {
        const delegates = getDelegates();
        currentDelegate = delegates.find(d => d.id === delegateId);
        if (!currentDelegate) {
            document.querySelector('.company-content').innerHTML = '<div class="company-card"><p style="color:red;">لم يتم العثور على المندوب</p></div>';
            return;
        }
        document.getElementById('delegateName').innerText = currentDelegate.name;
        document.getElementById('delegatePhone').innerText = currentDelegate.phone;
        document.getElementById('delegateCity').innerText = currentDelegate.city || 'غير محدد';
        document.getElementById('delegateVehicle').innerText = currentDelegate.vehicleType;
        document.getElementById('delegatePlate').innerText = currentDelegate.plateNumber || 'غير محدد';
        const statusSpan = document.getElementById('delegateStatus');
        if (statusSpan) {
            const isActive = currentDelegate.status === 'active';
            statusSpan.innerHTML = `<i class="fa-solid fa-circle"></i> ${isActive ? 'نشط' : 'غير نشط'}`;
            statusSpan.className = `company-badge ${isActive ? 'company-badge-success' : 'company-badge-danger'}`;
        }
        const avatarImg = document.getElementById('delegateAvatar');
        if (avatarImg) {
            avatarImg.src = currentDelegate.avatar || `https://ui-avatars.com/api/?name=${encodeURIComponent(currentDelegate.name)}&background=f0f7ff&color=0061ff`;
        }
        const orders = getOrders().filter(o => o.delegateId === delegateId);
        const pending = orders.filter(o => o.status === 'assigned' || o.status === 'picked_up').length;
        const completed = orders.filter(o => o.status === 'approved_by_company').length;
        const returned = orders.filter(o => o.status === 'returned').length;
        document.getElementById('delegatePendingCount').innerText = pending;
        document.getElementById('delegateCompletedCount').innerText = completed;
        document.getElementById('delegateReturnedCount').innerText = returned;
    }

    // Modal edit logic
    const modal = document.getElementById('editDelegateModal');
    const editBtn = document.getElementById('editDelegateBtn');
    const deleteBtn = document.getElementById('deleteDelegateBtn');
    const saveBtn = document.getElementById('saveModalBtn');
    const closeBtns = modal ? modal.querySelectorAll('[data-modal-close]') : [];

    function openModal() {
        if (!currentDelegate) return;
        document.getElementById('editDelegateName').value = currentDelegate.name;
        document.getElementById('editDelegatePhone').value = currentDelegate.phone;
        document.getElementById('editDelegateEmail').value = currentDelegate.email || '';
        document.getElementById('editDelegateCity').value = currentDelegate.city || 'القاهرة';
        document.getElementById('editDelegateVehicle').value = currentDelegate.vehicleType;
        document.getElementById('editDelegatePlate').value = currentDelegate.plateNumber || '';
        document.getElementById('editDelegateStatus').value = currentDelegate.status === 'active' ? 'active' : 'inactive';
        modal.classList.add('show');
    }

    function closeModal() { modal.classList.remove('show'); }

    function saveDelegate() {
        const updatedDelegate = {
            ...currentDelegate,
            name: document.getElementById('editDelegateName').value,
            phone: document.getElementById('editDelegatePhone').value,
            email: document.getElementById('editDelegateEmail').value,
            city: document.getElementById('editDelegateCity').value,
            vehicleType: document.getElementById('editDelegateVehicle').value,
            plateNumber: document.getElementById('editDelegatePlate').value,
            status: document.getElementById('editDelegateStatus').value
        };
        const delegates = getDelegates();
        const idx = delegates.findIndex(d => d.id === delegateId);
        if (idx !== -1) {
            delegates[idx] = updatedDelegate;
            saveDelegates(delegates);
            showToast('تم تحديث بيانات المندوب بنجاح', 'success');
            loadDelegateData();
            closeModal();
        } else {
            showToast('حدث خطأ', 'error');
        }
    }

    function deleteDelegate() {
        window.showConfirmModal('هل أنت متأكد من حذف هذا المندوب؟', 'تأكيد الحذف').then(confirmed => {
            if (!confirmed) return;
            const balance = (currentDelegate.cashInHand || 0) - (currentDelegate.cashDeliveredToCompany || 0);
            const activeOrders = getOrders().filter(o => o.delegateId === delegateId && (o.status === 'assigned' || o.status === 'picked_up')).length;
            if (balance !== 0 || activeOrders > 0) {
                showToast('لا يمكن حذف المندوب: لديه رصيد غير صفري أو طلبات نشطة', 'error');
                return;
            }
            const newDelegates = getDelegates().filter(d => d.id !== delegateId);
            saveDelegates(newDelegates);
            showToast('تم حذف المندوب بنجاح', 'success');
            window.location.href = 'delegates.html';
        });
    }

    if (editBtn) editBtn.addEventListener('click', openModal);
    if (deleteBtn) deleteBtn.addEventListener('click', deleteDelegate);
    if (saveBtn) saveBtn.addEventListener('click', saveDelegate);
    closeBtns.forEach(btn => btn.addEventListener('click', closeModal));
    if (modal) modal.addEventListener('click', (e) => { if (e.target === modal) closeModal(); });

    loadDelegateData();
})();