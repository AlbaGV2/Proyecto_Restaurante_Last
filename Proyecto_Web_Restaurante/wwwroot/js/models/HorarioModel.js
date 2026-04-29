// ============================================================
// MODEL: HorarioModel.js
// Define los horarios de apertura y los slots de reserva.
// ============================================================

export const horarioRestaurante = {
  diasSemana: [
    { dias: 'Mar - Vie', horas: '13:00 - 23:00' },
    { dias: 'Sáb - Dom', horas: '13:00 - 00:00' },
    { dias: 'Lunes', horas: null } // null = cerrado
  ]
};

// Slots de tiempo para el turno de comida
export const slotsComida = [
  { hora: '13:00', disponible: true },
  { hora: '13:15', disponible: true },
  { hora: '13:30', disponible: false },
  { hora: '13:45', disponible: true },
  { hora: '14:00', disponible: true },
  { hora: '14:15', disponible: true },
  { hora: '14:30', disponible: true },
  { hora: '14:45', disponible: false },
  { hora: '15:00', disponible: true },
  { hora: '15:15', disponible: true },
  { hora: '15:30', disponible: true },
  { hora: '15:45', disponible: true }
];

// Slots de tiempo para el turno de cena
export const slotsCena = [
  { hora: '20:00', disponible: true },
  { hora: '20:15', disponible: true },
  { hora: '20:30', disponible: false },
  { hora: '20:45', disponible: true },
  { hora: '21:00', disponible: true },
  { hora: '21:15', disponible: true },
  { hora: '21:30', disponible: true },
  { hora: '21:45', disponible: true },
  { hora: '22:00', disponible: true },
  { hora: '22:15', disponible: false },
  { hora: '22:30', disponible: true },
  { hora: '22:45', disponible: true }
];
