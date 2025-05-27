const OpenModalRegister = document.querySelector('.btn-registro'); 
const modal = document.querySelector('.modal-signup');              
const CloseModals = document.querySelectorAll('.Modal-close');

/*Var para login*/
const OpenModalLogin = document.querySelector('.btn-login');
const ModalLogin = document.querySelector('.modal-login');    
const CloseModalsLogin = document.querySelectorAll('.Modal-close');

/*Var para admin login*/
const OpenModalAdmin = document.querySelector('.btn-login-admin');
const ModalLoginAdmin = document.querySelector('.modal-admin');    
const CloseModalsLoginAdmin = document.querySelectorAll('.Modal-close');


OpenModalRegister.addEventListener('click', (e)=> {
    e.preventDefault();
    modal.classList.add('modal-signup--show');
});


CloseModals.forEach(btn => {
    btn.addEventListener('click', (e) => {
        e.preventDefault();
        modal.classList.remove('modal-signup--show');
    });
});

/*Para login*/

OpenModalLogin.addEventListener('click', (e)=> {
    e.preventDefault();
    ModalLogin.classList.add('modal-login--show');
});

CloseModalsLogin.forEach(btn => {
    btn.addEventListener('click', (e) => {
        e.preventDefault();
        ModalLogin.classList.remove('modal-login--show');
    });
});

/*Para admin login*/

OpenModalAdmin.addEventListener('click', (e)=> {
    e.preventDefault();
    ModalLoginAdmin.classList.add('modal-admin--show');
});

CloseModalsLogin.forEach(btn => {
    btn.addEventListener('click', (e) => {
        e.preventDefault();
        ModalLoginAdmin.classList.remove('modal-admin--show');
    });
});