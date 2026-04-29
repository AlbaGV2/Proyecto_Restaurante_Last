// ============================================================
// CONTROLLER: MenuController.js
// Renderiza el menú dinámicamente desde MenuModel.js
// ============================================================

import { menuData } from '../models/MenuModel.js';
import { initScrollAnimations } from '../../lib/Utils.js';

document.addEventListener('DOMContentLoaded', () => {

  // --- Renderizar Menú del Día ---
  const contenedorMenuDia = document.getElementById('menu-dia');
  if (contenedorMenuDia) {
    const { titulo, precio, secciones } = menuData.menuDelDia;

    const buildPlatos = (platos) =>
      platos.map(p => `
        <div class="mb-5">
          <span class="font-semibold text-dark text-sm">${p.nombre}</span>
          <p class="text-gray-500 text-xs font-light mt-1.5 italic leading-relaxed">${p.ingredientes}</p>
        </div>
      `).join('');

    contenedorMenuDia.innerHTML = `
      <div class="flex justify-between items-end border-b-2 border-primary/20 pb-4 mb-8">
        <h3 class="font-serif text-2xl font-bold text-dark uppercase tracking-widest">${titulo}</h3>
        <span class="text-primary font-bold text-xl font-serif">${precio}</span>
      </div>
      <div class="grid grid-cols-1 md:grid-cols-2 gap-x-12 gap-y-8">
        <div class="flex flex-col">
          <h4 class="font-bold text-dark text-base tracking-wide uppercase mb-3">Primeros</h4>
          ${buildPlatos(secciones.primeros)}
        </div>
        <div class="flex flex-col">
          <h4 class="font-bold text-dark text-base tracking-wide uppercase mb-3">Segundos</h4>
          ${buildPlatos(secciones.segundos)}
        </div>
      </div>
    `;
  }

  // --- Renderizar secciones con precio por ítem ---
  function renderSeccionConPrecios(containerId, seccion) {
    const container = document.getElementById(containerId);
    if (!container) return;

    const itemsHTML = seccion.items.map(item => `
      <div class="flex justify-between items-start gap-4">
        <div>
          <span class="font-semibold text-dark text-sm uppercase tracking-wide">${item.nombre}</span>
          <p class="text-gray-500 text-xs font-light mt-1.5 italic leading-relaxed">${item.ingredientes}</p>
        </div>
        <span class="text-primary font-bold text-base font-serif whitespace-nowrap">${item.precio}</span>
      </div>
    `).join('');

    container.innerHTML = `
      <div class="flex justify-between items-end border-b-2 border-primary/20 pb-4 mb-8">
        <h3 class="font-serif text-2xl font-bold text-dark uppercase tracking-widest">${seccion.titulo}</h3>
      </div>
      <div class="grid grid-cols-1 md:grid-cols-2 gap-x-12 gap-y-8">
        ${itemsHTML}
      </div>
    `;
  }

  renderSeccionConPrecios('menu-clasicos', menuData.clasicosInformales);
  renderSeccionConPrecios('menu-carta', menuData.alaCarta);

  initScrollAnimations();
});


