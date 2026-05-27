// ============================================================
// MODEL: MenuModel.js
// Contiene los datos del menú del restaurante.
// Fuente única de verdad para MenuController.js
// ============================================================

export const menuData = {

  menuDelDia: {
    titulo: 'Menú del Día',
    precio: '25€',
    secciones: {
      primeros: [
        {
          nombre: 'Crema de Calabaza Asada',
          ingredientes: 'Calabaza, pipas tostadas, aceite de trufa blanca, picatostes artesanales.'
        },
        {
          nombre: 'Ensalada de Queso de Cabra',
          ingredientes: 'Mezclum de lechugas, queso de cabra caramelizado, nueces, vinagreta de miel y mostaza.'
        }
      ],
      segundos: [
        {
          nombre: 'Entrecot a la Parrilla',
          ingredientes: 'Lomo de ternera, patatas rústicas, pimientos de Padrón, sal en escamas.'
        },
        {
          nombre: 'Salmón al Horno',
          ingredientes: 'Lomo de salmón fresco, espárragos trigueros, salsa de eneldo y limón.'
        }
      ]
    }
  },

  clasicosInformales: {
    titulo: 'Clásicos Informales',
    items: [
      {
        nombre: 'Hamburguesa Elegance',
        ingredientes: 'Carne de Wagyu 200g, queso rulo de cabra, cebolla caramelizada, rúcula, pan brioche tostado.',
        precio: '18€'
      },
      {
        nombre: 'Sándwich Club Premium',
        ingredientes: 'Pollo a la parrilla, bacon ahumado, huevo frito, lechuga viva, tomate ibérico, mayonesa trufada.',
        precio: '15€'
      },
      {
        nombre: 'Burger Vegana de la Huerta',
        ingredientes: 'Burger de lentejas y quinoa, aguacate, tomate seco, espinacas baby, pan de masa madre.',
        precio: '16€'
      },
      {
        nombre: 'Sándwich de Pastrami',
        ingredientes: 'Pastrami ahumado artesanal, col fermentada, mostaza antigua, pepinillos, pan de centeno.',
        precio: '17€'
      }
    ]
  },

  alaCarta: {
    titulo: 'A la Carta',
    items: [
      {
        nombre: 'Tartar de Atún Rojo',
        ingredientes: 'Atún rojo salvaje, aguacate, sésamo negro, soja, aceite de sésamo, perlas de wasabi.',
        precio: '22€'
      },
      {
        nombre: 'Risotto de Setas Silvestres',
        ingredientes: 'Arroz carnaroli, boletus edulis, queso parmesano curado, mantequilla, crujiente de parmesano.',
        precio: '19€'
      },
      {
        nombre: 'Solomillo Wellington',
        ingredientes: 'Solomillo de ternera, duxelles de champiñones, jamón ibérico, hojaldre casero, salsa demi-glace.',
        precio: '28€'
      },
      {
        nombre: 'Pulpo a la Brasa',
        ingredientes: 'Pata de pulpo, puré de patata trufado, pimentón de la Vera, aceite de oliva virgen extra, escamas de sal negra.',
        precio: '24€'
      }
    ]
  }
};
