// ── MOBILE MENU ──
function toggleMenu() {
    document.getElementById('mobileMenu').classList.toggle('open');
}

// ── FADE UP ANIMATION ──
const observer = new IntersectionObserver((entries) => {
    entries.forEach(entry => {
        if (entry.isIntersecting)
            entry.target.classList.add('visible');
    });
}, { threshold: 0.1 });

document.querySelectorAll('.fade-up').forEach(el => observer.observe(el));

// ── SLOTS DE AGENDAMENTO ──
function selectSlot(el, slotId) {
    document.querySelectorAll('.slot').forEach(s => s.classList.remove('selected'));
    el.classList.add('selected');
    document.getElementById('selectedSlot').value = slotId;
}

// ── NAVBAR SCROLL ──
window.addEventListener('scroll', () => {
    const nav = document.querySelector('nav');
    if (window.scrollY > 50) {
        nav.style.padding = '12px 48px';
    } else {
        nav.style.padding = '20px 48px';
    }
});