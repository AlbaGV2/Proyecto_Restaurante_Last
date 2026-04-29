// ============================================================
// CONTROLLER: AuthController.js
// Lógica compartida de Auth//Auth/Login y Auth/Registro.html
// ============================================================

import { initScrollAnimations } from '../../lib/Utils.js';

document.addEventListener('DOMContentLoaded', () => {
  initScrollAnimations();

  // --- Lógica Login ---
  const btnLogin = document.getElementById('btn-login');
  if (btnLogin) {
    btnLogin.addEventListener('click', () => {
      const user = document.getElementById('username')?.value?.trim();
      const pass = document.getElementById('password')?.value?.trim();
      if (!user || !pass) {
        alert('Por favor introduce tu usuario y contraseña.');
        return;
      }
      alert('Sesión iniciada. Su reserva ha sido completada.');
      window.location.href = '/Home/Index';
    });
  }

  // --- Lógica Registro ---
  const btnRegistro = document.getElementById('btn-registro');
  if (btnRegistro) {
    btnRegistro.addEventListener('click', () => {
      const email   = document.getElementById('email')?.value?.trim();
      const user    = document.getElementById('username')?.value?.trim();
      const pass    = document.getElementById('password')?.value?.trim();
      const confirm = document.getElementById('confirm_password')?.value?.trim();
      const terms   = document.getElementById('terms')?.checked;

      if (!email || !user || !pass || !confirm) {
        alert('Por favor rellena todos los campos.');
        return;
      }
      if (pass !== confirm) {
        alert('Las contraseñas no coinciden.');
        return;
      }
      if (!terms) {
        alert('Debes aceptar la política de privacidad.');
        return;
      }
      alert('Cuenta creada exitosamente.');
      window.location.href = '/Auth/Login';
    });
  }

  // --- Modal Privacidad ---
  const btnPrivacy    = document.getElementById('btn-privacy');
  const modalPrivacy  = document.getElementById('modal-privacy');
  const btnCloseModal = document.getElementById('btn-close-modal');
  const btnAceptarModal = document.getElementById('btn-aceptar-modal');

  if (btnPrivacy && modalPrivacy) {
    btnPrivacy.addEventListener('click', () => modalPrivacy.classList.remove('hidden'));
  }
  if (btnCloseModal) {
    btnCloseModal.addEventListener('click', () => modalPrivacy.classList.add('hidden'));
  }
  if (btnAceptarModal) {
    btnAceptarModal.addEventListener('click', () => {
      modalPrivacy.classList.add('hidden');
      const terms = document.getElementById('terms');
      if (terms) terms.checked = true;
    });
  }
  // Cerrar modal clicando fuera
  if (modalPrivacy) {
    modalPrivacy.addEventListener('click', (e) => {
      if (e.target === modalPrivacy) modalPrivacy.classList.add('hidden');
    });
  }
});


