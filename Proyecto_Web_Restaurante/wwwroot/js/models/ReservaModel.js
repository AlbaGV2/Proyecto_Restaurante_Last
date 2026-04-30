// ============================================================
// MODEL: ReservaModel.js
// Gestiona el estado de una reserva en curso.
// Persiste los datos en sessionStorage.
// ============================================================

const STORAGE_KEY = 'reserva_temp';

export const ReservaModel = {

  /** Estado interno de la reserva en curso */
  estado: {
    personas: '2',
    fecha: '',
    turno: 'Comida', // 'Comida' | 'Cena'
    hora: null,
    nombre: '',
    email: ''
  },

  /** Guarda el estado actual en sessionStorage */
  guardar() {
    sessionStorage.setItem(STORAGE_KEY, JSON.stringify(this.estado));
  },

  /** Carga los datos guardados de sessionStorage (si existen) */
  cargar() {
    const raw = sessionStorage.getItem(STORAGE_KEY);
    if (raw) {
      try {
        this.estado = JSON.parse(raw);
      } catch {
        sessionStorage.removeItem(STORAGE_KEY);
      }
    }
    return this.estado;
  },

  /** Borra los datos de reserva del sessionStorage */
  limpiar() {
    sessionStorage.removeItem(STORAGE_KEY);
    this.estado = { personas: '2', fecha: '', turno: 'Comida', hora: null, nombre: '', email: '' };
  },

  /** Actualiza un campo del estado */
  actualizar(campo, valor) {
    this.estado[campo] = valor;
  },

  /** Devuelve true si la reserva tiene todos los datos necesarios para confirmar */
  esValida() {
    return (
      this.estado.personas &&
      this.estado.fecha &&
      this.estado.turno &&
      this.estado.hora !== null &&
      this.estado.nombre.trim() !== '' &&
      this.estado.email.trim() !== ''
    );
  }
};
