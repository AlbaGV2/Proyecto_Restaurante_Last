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

  // --- Botón Reservar ---
  if (btnReservar) {
    btnReservar.addEventListener('click', () => {
      if (ReservaModel.esValida()) {
        ReservaModel.guardar();
        window.location.href = '../Auth//Auth/Login';
      }
    });
  }
});


