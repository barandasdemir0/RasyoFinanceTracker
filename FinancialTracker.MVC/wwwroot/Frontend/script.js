document.addEventListener('DOMContentLoaded', () => {
    
    // ==========================================
    // UI INTERACTIONS
    // ==========================================
    const refreshBtn = document.getElementById('refreshBtn');
    refreshBtn.addEventListener('click', () => {
        const originalText = refreshBtn.innerText;
        refreshBtn.innerText = "Syncing...";
        refreshBtn.disabled = true;
        setTimeout(() => {
            refreshBtn.innerText = "Prices Synced!";
            refreshBtn.style.background = "var(--profit-green)";
            setTimeout(() => {
                refreshBtn.innerText = originalText;
                refreshBtn.style.background = "var(--accent-blue)";
                refreshBtn.disabled = false;
            }, 2000);
        }, 1500);
    });

    const modal = document.getElementById('addModal');
    const openModalBtn = document.getElementById('openModalBtn');
    const closeModalBtn = document.getElementById('closeModalBtn');

    openModalBtn.addEventListener('click', () => modal.classList.remove('hidden'));
    closeModalBtn.addEventListener('click', () => modal.classList.add('hidden'));
    modal.addEventListener('click', (e) => {
        if (e.target === modal) modal.classList.add('hidden');
    });

    // ==========================================
    // CHART.JS INIT (SAFE FALLBACK)
    // ==========================================
    // Not: Chart.js kütüphanesi en yeni 'oklch()' renk formatını henüz tam çözemediği için 
    // grafiğin ekrana çizilmesini engelliyordu. Bu yüzden grafik için güvenli HEX renkleri kullanıyoruz.
    const primaryColor = '#4facfe'; 
    const gridColor = 'rgba(255, 255, 255, 0.08)';
    const textColor = '#8b9bb4';

    const canvas = document.getElementById('portfolioChart');
    if(canvas) {
        const ctx = canvas.getContext('2d');
        
        // Fake Data for the last 7 days
        const labels = ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'];
        const dataPoints = [11200, 11450, 11300, 11800, 11950, 12100, 12450];

        new Chart(ctx, {
            type: 'line',
            data: {
                labels: labels,
                datasets: [{
                    label: 'Portfolio Value ($)',
                    data: dataPoints,
                    borderColor: primaryColor,
                    backgroundColor: 'rgba(79, 172, 254, 0.1)', // Hafif mavi dolgu
                    borderWidth: 3,
                    tension: 0.4, 
                    pointBackgroundColor: primaryColor,
                    pointBorderColor: '#fff',
                    pointRadius: 4,
                    fill: true
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: { display: false } 
                },
                scales: {
                    x: {
                        grid: { color: 'transparent' }, 
                        ticks: { color: textColor }
                    },
                    y: {
                        grid: { color: gridColor }, 
                        ticks: { 
                            color: textColor,
                            callback: function(value) { return '$' + value; }
                        }
                    }
                }
            }
        });
    }

});
