// ============================================================
// CONTROLLER: ReservasController.js
// Gestiona la lógica de la página Reservas/Index.html
// Usa ReservaModel para persistir la selección.
// ============================================================

import { ReservaModel } from '../models/ReservaModel.js';
import { slotsComida, slotsCena } from '../models/HorarioModel.js';
import { initScrollAnimations, getTodayFormatted } from '../../lib/Utils.js';

document.addEventListener('DOMContentLoaded', () => {
  initScrollAnimations();

  // --- Fecha inicial ---
  const dateInput = document.getElementById('fecha');
  if (dateInput) {
    const today = getTodayFormatted();
    dateInput.value = today;
    dateInput.min   = today;
    ReservaModel.actualizar('fecha', today);
    dateInput.addEventListener('change', () => ReservaModel.actualizar('fecha', dateInput.value));
  }

  // --- Personas ---
  const personasSelect = document.getElementById('personas');
  if (personasSelect) {
    personasSelect.addEventListener('change', () =>
      ReservaModel.actualizar('personas', personasSelect.value)
    );
  }

  // --- Renderizar slots de horas ---
  function renderSlots(container, slots) {
    container.innerHTML = slots.map(({ hora, disponible }) => {
      if (!disponible) {
        return `<div class="bg-gray-100 text-gray-400 border border-gray-100 rounded-full py-4 px-4 text-center text-base font-serif cursor-not-allowed opacity-50">
                  <span class="relative z-10">${hora}</span>
                </div>`;
      }
      return `<div data-hora="${hora}" class="slot-hora border border-primary text-primary hover:bg-primary hover:text-white rounded-full py-4 px-4 text-center text-base font-serif cursor-pointer transition-colors duration-300">${hora}</div>`;
    }).join('');
  }

  const gridComidas = document.getElementById('grid-comidas');
  const gridCenas   = document.getElementById('grid-cenas');
  if (gridComidas) renderSlots(gridComidas, slotsComida);
  if (gridCenas)   renderSlots(gridCenas, slotsCena);

  // --- Turno Comida / Cena ---
  const btnComida  = document.getElementById('btn-comida');
  const btnCena    = document.getElementById('btn-cena');
  const btnReservar = document.getElementById('btn-reservar');
  let selectedTime = null;

  const resetSelection = () => {
    selectedTime = null;
    document.querySelectorAll('.slot-hora').forEach(s => {
      s.classList.remove('bg-primary', 'text-white', 'scale-105', 'shadow-md');
      s.classList.add('border-primary', 'text-primary');
    });
    ReservaModel.actualizar('hora', null);
    if (btnReservar) {
      btnReservar.dataset.active = 'false';
      btnReservar.innerHTML = 'Reservar';
    }
  };

  if (btnComida) {
    btnComida.addEventListener('click', () => {
      btnComida.classList.add('bg-dark', 'text-white');
      btnComida.classList.remove('text-gray-500');
      btnCena.classList.remove('bg-dark', 'text-white');
      btnCena.classList.add('text-gray-500');
      gridComidas.classList.remove('hidden');
      gridCenas.classList.add('hidden');
      ReservaModel.actualizar('turno', 'Comida');
      resetSelection();
    });
  }

  if (btnCena) {
    btnCena.addEventListener('click', () => {
      btnCena.classList.add('bg-dark', 'text-white');
      btnCena.classList.remove('text-gray-500');
      btnComida.classList.remove('bg-dark', 'text-white');
      btnComida.classList.add('text-gray-500');
      gridCenas.classList.remove('hidden');
      gridComidas.classList.add('hidden');
      ReservaModel.actualizar('turno', 'Cena');
      resetSelection();
    });
  }

  // --- Selección de hora por delegación ---
  const hoursContainer = document.getElementById('hours-container');
  if (hoursContainer) {
    hoursContainer.addEventListener('click', (e) => {
      const slot = e.target.closest('.slot-hora');
      if (!slot) return;

      if (selectedTime === slot.dataset.hora) {
        resetSelection();
        return;
      }
      resetSelection();
      slot.classList.remove('border-primary', 'text-primary');
      slot.classList.add('bg-primary', 'text-white', 'scale-105', 'shadow-md');
      selectedTime = slot.dataset.hora;
      ReservaModel.actualizar('hora', selectedTime);

      if (btnReservar) {
        btnReservar.dataset.active = 'true';
      }
    });
  }

  // --- Nombre ---
  const nombreInput = document.getElementById('nombre');
  const errorNombre = document.getElementById('error-nombre');
  if (nombreInput) {
    nombreInput.addEventListener('input', () => {
      ReservaModel.actualizar('nombre', nombreInput.value);
      actualizarBoton();
    });
  }

  // --- Email ---
  const emailInput = document.getElementById('email');
  const errorEmail = document.getElementById('error-email');
  if (emailInput) {
    emailInput.addEventListener('input', () => {
      ReservaModel.actualizar('email', emailInput.value);
      actualizarBoton();
    });
  }

  // Activa/desactiva el botón Reservar según validez del modelo
  function actualizarBoton() {
    if (!btnReservar) return;
    if (ReservaModel.esValida()) {
      btnReservar.dataset.active = 'true';
    } else {
      btnReservar.dataset.active = 'false';
    }
  }

  // --- Botón Reservar ---
  const mensajeDiv = document.getElementById('mensaje-reserva');

  function mostrarMensaje(texto, esExito) {
    if (!mensajeDiv) return;
    mensajeDiv.textContent = texto;
    mensajeDiv.className = esExito
      ? 'text-center mb-8 px-4 py-3 rounded-2xl text-sm font-medium bg-green-50 text-green-700 border border-green-200'
      : 'text-center mb-8 px-4 py-3 rounded-2xl text-sm font-medium bg-red-50 text-red-700 border border-red-200';
  }

  function limpiarErrores() {
    if (errorNombre) { errorNombre.textContent = ''; errorNombre.classList.add('hidden'); }
    if (errorEmail)  { errorEmail.textContent  = ''; errorEmail.classList.add('hidden'); }
    if (mensajeDiv)  { mensajeDiv.className = 'hidden'; mensajeDiv.textContent = ''; }
  }

  if (btnReservar) {
    btnReservar.addEventListener('click', async () => {
      if (!ReservaModel.esValida()) return;

      limpiarErrores();
      btnReservar.textContent = 'Enviando...';
      btnReservar.disabled = true;

      const { personas, fecha, turno, hora, nombre, email } = ReservaModel.estado;

      // Obtener el token CSRF generado por ASP.NET Core
      const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value ?? '';

      try {
        const respuesta = await fetch('/Reservas/Crear', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': token
          },
          body: JSON.stringify({
            nombre,
            email,
            fecha,
            personas: parseInt(personas, 10),
            turno,
            hora
          })
        });

        const datos = await respuesta.json();

        if (respuesta.ok && datos.success) {
          // Éxito: mostrar mensaje de confirmación y limpiar formulario
          mostrarMensaje(datos.mensaje, true);
          ReservaModel.limpiar();
          if (nombreInput) nombreInput.value = '';
          if (emailInput)  emailInput.value  = '';
          btnReservar.dataset.active = 'false';
        } else {
          // Error del servidor: mostrar errores por campo
          mostrarMensaje('Por favor, revisa los datos del formulario.', false);

          if (datos.errores) {
            if (datos.errores.Nombre && errorNombre) {
              errorNombre.textContent = datos.errores.Nombre[0];
              errorNombre.classList.remove('hidden');
            }
            if (datos.errores.Email && errorEmail) {
              errorEmail.textContent = datos.errores.Email[0];
              errorEmail.classList.remove('hidden');
            }
            // Otros errores (Fecha, Hora, etc.) se muestran en el mensaje general
            const otrosErrores = Object.entries(datos.errores)
              .filter(([k]) => k !== 'Nombre' && k !== 'Email')
              .map(([, v]) => v[0])
              .join(' ');
            if (otrosErrores) mostrarMensaje(otrosErrores, false);
          }
        }
      } catch (err) {
        mostrarMensaje('Error de conexión. Por favor, inténtalo de nuevo.', false);
      } finally {
        btnReservar.textContent = 'Reservar';
        btnReservar.disabled = false;
      }
    });
  }
});
