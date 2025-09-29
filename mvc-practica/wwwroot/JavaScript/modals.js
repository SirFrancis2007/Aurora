// Variables para los modales
const OpenModalRegister = document.querySelector('.btn-registro'); 
const modalRegister = document.querySelector('#modal-signup-empresa');              
const CloseModals = document.querySelectorAll('.modal__close');

/* Variables para login empresa */
const OpenModalLogin = document.querySelector('.btn-login');
const modalLogin = document.querySelector('#modal-login-empresa');    

/* Variables para login admin */
const OpenModalAdmin = document.querySelector('.btn-login-admin');
const modalAdmin = document.querySelector('#modal-login-admin');    

// Función para cerrar todos los modales
function closeAllModals() {
    const modals = document.querySelectorAll('.modal');
    modals.forEach(modal => {
        modal.classList.remove('modal--active');
    });
    // Prevenir scroll del body
    document.body.style.overflow = 'auto';
}

// Función para abrir modal específico
function openModal(modal) {
    closeAllModals();
    modal.classList.add('modal--active');
    // Prevenir scroll del body cuando el modal está abierto
    document.body.style.overflow = 'hidden';
}

// Evento para registro
if (OpenModalRegister && modalRegister) {
    OpenModalRegister.addEventListener('click', (e) => {
        e.preventDefault();
        openModal(modalRegister);
    });
}

// Evento para login empresa
if (OpenModalLogin && modalLogin) {
    OpenModalLogin.addEventListener('click', (e) => {
        e.preventDefault();
        openModal(modalLogin);
    });
}

// Evento para login admin
if (OpenModalAdmin && modalAdmin) {
    OpenModalAdmin.addEventListener('click', (e) => {
        e.preventDefault();
        openModal(modalAdmin);
    });
}

// Eventos para cerrar modales
CloseModals.forEach(btn => {
    btn.addEventListener('click', (e) => {
        e.preventDefault();
        closeAllModals();
    });
});

// Cerrar modal al hacer click fuera del contenido
document.addEventListener('click', (e) => {
    if (e.target.classList.contains('modal')) {
        closeAllModals();
    }
});

// Cerrar modal con tecla ESC
document.addEventListener('keydown', (e) => {
    if (e.key === 'Escape') {
        closeAllModals();
    }
});

// Prevenir que el click dentro del contenedor cierre el modal
document.querySelectorAll('.modal__container').forEach(container => {
    container.addEventListener('click', (e) => {
        e.stopPropagation();
    });
});