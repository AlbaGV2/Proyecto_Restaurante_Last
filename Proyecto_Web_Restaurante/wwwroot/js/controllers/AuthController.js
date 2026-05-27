// ============================================================
// CONTROLLER: AuthController.js
// Lógica compartida de Auth//Auth/Login y Auth/Registro.html
// ============================================================

import { initScrollAnimations } from '../../lib/Utils.js';

const swalConfig = {
  confirmButtonColor: '#1A1A1A',
  cancelButtonColor: '#f3f4f6',
  customClass: {
    popup: 'rounded-[30px] border-none shadow-2xl font-sans',
    confirmButton: 'rounded-xl px-8 py-3 text-[10px] font-bold uppercase tracking-widest',
    cancelButton: 'rounded-xl px-8 py-3 text-[10px] font-bold uppercase tracking-widest text-gray-500'
  }
};

document.addEventListener('DOMContentLoaded', () => {
  initScrollAnimations();

  // --- Lógica Login ---
  const btnLogin = document.getElementById('btn-login');
  if (btnLogin) {
    btnLogin.addEventListener('click', async () => {
      const user = document.getElementById('username')?.value?.trim();
      const pass = document.getElementById('password')?.value?.trim();
      if (!user || !pass) {
        Swal.fire({
          ...swalConfig,
          title: 'Campos Incompletos',
          text: 'Por favor introduce tu usuario y contraseña.',
          icon: 'warning'
        });
        return;
      }
      
      btnLogin.textContent = 'Iniciando sesión...';
      btnLogin.disabled = true;
      
      try {
        const respuesta = await fetch('/Auth/Login', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify({ Username: user, Password: pass })
        });
        
        const resultado = await respuesta.json();
        if (resultado.success) {
          const key = 'ultimo_acceso_' + user.toLowerCase().trim();
          const ultimoAcceso = localStorage.getItem(key);
          const mensajeUltimoAcceso = ultimoAcceso 
            ? `Último acceso: ${ultimoAcceso}`
            : 'Primer acceso registrado.';
            
          // Formatear fecha y hora actual en un formato español muy elegante
          const ahora = new Date();
          const dia = String(ahora.getDate()).padStart(2, '0');
          const mes = String(ahora.getMonth() + 1).padStart(2, '0');
          const año = ahora.getFullYear();
          const horas = String(ahora.getHours()).padStart(2, '0');
          const minutos = String(ahora.getMinutes()).padStart(2, '0');
          const segundos = String(ahora.getSeconds()).padStart(2, '0');
          const fechaFormateada = `${dia}/${mes}/${año} a las ${horas}:${minutos}:${segundos}`;
          
          localStorage.setItem(key, fechaFormateada);
          
          // Guardar flags en sessionStorage para mostrarlas en el gestor de reservas
          sessionStorage.setItem('mostrar_login_exito', 'true');
          sessionStorage.setItem('login_ultimo_acceso', mensajeUltimoAcceso);

          window.location.href = resultado.redirectUrl || '/Reservas/Gestion';
        } else {
          Swal.fire({
            ...swalConfig,
            title: 'Acceso Denegado',
            text: resultado.message || 'Usuario o contraseña incorrectos.',
            icon: 'error'
          });
        }
      } catch (err) {
        console.error('Error de login:', err);
        Swal.fire({
          ...swalConfig,
          title: 'Error de Conexión',
          text: 'Error de conexión. Inténtalo de nuevo.',
          icon: 'error'
        });
      } finally {
        btnLogin.textContent = 'Entrar y Confirmar';
        btnLogin.disabled = false;
      }
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
        Swal.fire({
          ...swalConfig,
          title: 'Campos Incompletos',
          text: 'Por favor rellena todos los campos.',
          icon: 'warning'
        });
        return;
      }
      if (pass !== confirm) {
        Swal.fire({
          ...swalConfig,
          title: 'Contraseñas Diferentes',
          text: 'Las contraseñas no coinciden.',
          icon: 'warning'
        });
        return;
      }
      if (!terms) {
        Swal.fire({
          ...swalConfig,
          title: 'Privacidad Requerida',
          text: 'Debes aceptar la política de privacidad.',
          icon: 'warning'
        });
        return;
      }
      Swal.fire({
        ...swalConfig,
        title: 'Registro Exitoso',
        text: 'Cuenta creada exitosamente.',
        icon: 'success'
      }).then(() => {
        window.location.href = '/Auth/Login';
      });
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


