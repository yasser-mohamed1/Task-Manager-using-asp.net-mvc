document.addEventListener('DOMContentLoaded', function () {
    // Auto-hide alerts after 5 seconds
    const alerts = document.querySelectorAll('.alert');
    alerts.forEach(function (alert) {
        setTimeout(function () {
            if (alert && alert.parentNode) {
                alert.classList.add('fade');
                setTimeout(function () {
                    alert.remove();
                }, 150);
            }
        }, 5000);
    });

    // Confirm delete actions
    const deleteButtons = document.querySelectorAll('[data-confirm-delete]');
    deleteButtons.forEach(function (button) {
        button.addEventListener('click', function (e) {
            if (!confirm('Are you sure you want to delete this item?')) {
                e.preventDefault();
            }
        });
    });

    // Task completion animation
    const toggleButtons = document.querySelectorAll('[data-task-toggle]');
    toggleButtons.forEach(function (button) {
        button.addEventListener('click', function () {
            const card = button.closest('.card');
            if (card) {
                card.style.opacity = '0.6';
                card.style.transition = 'opacity 0.3s ease';
            }
        });
    });

    // Enhanced form validation feedback
    const forms = document.querySelectorAll('form');
    forms.forEach(function (form) {
        form.addEventListener('submit', function () {
            const submitBtn = form.querySelector('button[type="submit"]');
            if (submitBtn && form.checkValidity()) {
                submitBtn.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span>Processing...';
                submitBtn.disabled = true;
            }
        });
    });

    // Priority color coding
    const prioritySelects = document.querySelectorAll('select[name="Priority"]');
    prioritySelects.forEach(function (select) {
        select.addEventListener('change', function () {
            updatePriorityColor(select);
        });
        updatePriorityColor(select);
    });

    function updatePriorityColor(select) {
        const value = select.value;
        select.className = select.className.replace(/priority-\w+/g, '');

        switch (value) {
            case '1': // Low
                select.classList.add('priority-low');
                break;
            case '2': // Medium
                select.classList.add('priority-medium');
                break;
            case '3': // High
                select.classList.add('priority-high');
                break;
            case '4': // Critical
                select.classList.add('priority-critical');
                break;
        }
    }

    // Real-time search functionality (if implemented)
    const searchInput = document.querySelector('#taskSearch');
    if (searchInput) {
        searchInput.addEventListener('input', debounce(function () {
            filterTasks(this.value);
        }, 300));
    }

    function filterTasks(searchTerm) {
        const taskCards = document.querySelectorAll('.task-card');
        taskCards.forEach(function (card) {
            const title = card.querySelector('.card-title').textContent.toLowerCase();
            const description = card.querySelector('.card-text')?.textContent.toLowerCase() || '';

            if (title.includes(searchTerm.toLowerCase()) || description.includes(searchTerm.toLowerCase())) {
                card.style.display = 'block';
            } else {
                card.style.display = 'none';
            }
        });
    }

    // Utility function for debouncing
    function debounce(func, wait) {
        let timeout;
        return function executedFunction(...args) {
            const later = () => {
                clearTimeout(timeout);
                func(...args);
            };
            clearTimeout(timeout);
            timeout = setTimeout(later, wait);
        };
    }

    // Dashboard statistics animation
    const statNumbers = document.querySelectorAll('[data-stat-number]');
    statNumbers.forEach(function (element) {
        animateValue(element, 0, parseInt(element.textContent), 1000);
    });

    function animateValue(element, start, end, duration) {
        let startTimestamp = null;
        const step = (timestamp) => {
            if (!startTimestamp) startTimestamp = timestamp;
            const progress = Math.min((timestamp - startTimestamp) / duration, 1);
            element.textContent = Math.floor(progress * (end - start) + start);
            if (progress < 1) {
                window.requestAnimationFrame(step);
            }
        };
        window.requestAnimationFrame(step);
    }
});