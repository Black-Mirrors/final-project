using System;

namespace biblioteca
{
    class Biblioteca
    {
        static void Main(string[] args)
        {
            Biblioteca biblioteca = new Biblioteca();
            biblioteca.Iniciar();

            while (true)
            {
                Console.WriteLine("Menú:");
                Console.WriteLine("1. Prestar artículo");
                Console.WriteLine("2. Renovar artículo");
                Console.WriteLine("3. Devolver artículo");
                Console.WriteLine("4. Salir");
                Console.Write("Elija una opción: ");
                int opcion = int.Parse(Console.ReadLine());

                switch (opcion)
                {
                    case 1:
                        Console.Write("Ingrese su número de miembro: ");
                        int idMiembro = int.Parse(Console.ReadLine());
                        Console.Write("Ingrese el tipo de artículo (Libro/Película/Juego): ");
                        string tipoArticulo = Console.ReadLine();
                        biblioteca.PrestarArticulo(idMiembro, tipoArticulo);
                        break;

                    case 2:
                        Console.Write("Ingrese su número de miembro: ");
                        int idMiembroRenovar = int.Parse(Console.ReadLine());
                        Console.Write("Ingrese el título del artículo a renovar: ");
                        string tituloArticuloRenovar = Console.ReadLine();
                        biblioteca.RenovarArticulo(idMiembroRenovar, tituloArticuloRenovar);
                        break;

                    case 3:
                        Console.Write("Ingrese su número de miembro: ");
                        int idMiembroDevolver = int.Parse(Console.ReadLine());
                        Console.Write("Ingrese el título del artículo a devolver: ");
                        string tituloArticuloDevolver = Console.ReadLine();
                        biblioteca.DevolverArticulo(idMiembroDevolver, tituloArticuloDevolver);
                        break;

                    case 4:
                        Console.WriteLine("Saliendo del programa.");
                        return;

                    default:
                        Console.WriteLine("Opción no válida. Intente nuevamente.");
                        break;
                }
            }
        }

        private Dictionary<int, Miembro> miembros;
        private List<Articulo> articulos;

        public void Iniciar()
        {
            miembros = new Dictionary<int, Miembro>();
            articulos = new List<Articulo>();

            miembros.Add(1, new Miembro(1, "Usuario1"));
            miembros.Add(2, new Miembro(2, "Usuario2"));
            articulos.Add(new Libro("Libro1", "Autor1"));
            articulos.Add(new Pelicula("Película1", "Director1"));
            articulos.Add(new Juego("Juego1", "Desarrollador1"));
        }

        public void PrestarArticulo(int idMiembro, string tipoArticulo)
        {
            if (miembros.ContainsKey(idMiembro) && articulos.Exists(articulo => articulo.Titulo == tipoArticulo))
            {
                Miembro miembro = miembros[idMiembro];
                Articulo articulo = articulos.Find(art => art.Titulo == tipoArticulo);

                if (miembro.ArticulosPrestados.Count < 4 && articulo.Disponible)
                {
                    if (articulo.Prestar(miembro))
                    {
                        Console.WriteLine($"{miembro.Nombre} ha prestado '{articulo.Titulo}'.");
                    }
                    else
                    {
                        Console.WriteLine($"No se pudo prestar '{articulo.Titulo}'.");
                    }
                }
                else
                {
                    Console.WriteLine("No puede tomar prestados más artículos en este momento.");
                }
            }
            else
            {
                Console.WriteLine("Miembro o artículo no encontrado.");
            }
        }

        public void RenovarArticulo(int idMiembro, string tituloArticulo)
        {
            if (miembros.ContainsKey(idMiembro) && articulos.Exists(articulo => articulo.Titulo == tituloArticulo))
            {
                Miembro miembro = miembros[idMiembro];
                Articulo articulo = articulos.Find(art => art.Titulo == tituloArticulo);

                if (miembro.ArticulosPrestados.Contains(articulo) && articulo.PuedeRenovar())
                {
                    articulo.Renovar();
                    Console.WriteLine($"'{tituloArticulo}' ha sido renovado por {miembro.Nombre}.");
                }
                else
                {
                    Console.WriteLine("No se pudo renovar el artículo.");
                }
            }
            else
            {
                Console.WriteLine("Miembro o artículo no encontrado.");
            }
        }

        public void DevolverArticulo(int idMiembro, string tituloArticulo)
        {
            if (miembros.ContainsKey(idMiembro) && articulos.Exists(articulo => articulo.Titulo == tituloArticulo))
            {
                Miembro miembro = miembros[idMiembro];
                Articulo articulo = articulos.Find(art => art.Titulo == tituloArticulo);

                if (miembro.ArticulosPrestados.Contains(articulo))
                {
                    articulo.Devolver(miembro);
                    Console.WriteLine($"{miembro.Nombre} ha devuelto '{tituloArticulo}'.");
                }
                else
                {
                    Console.WriteLine("No puede devolver este artículo.");
                }
            }
            else
            {
                Console.WriteLine("Miembro o artículo no encontrado.");
            }
        }
    }

    class Miembro
    {
        public int IdMiembro { get; }
        public string Nombre { get; }
        public List<Articulo> ArticulosPrestados { get; }

        public Miembro(int idMiembro, string nombre)
        {
            IdMiembro = idMiembro;
            Nombre = nombre;
            ArticulosPrestados = new List<Articulo>();
        }
    }

    abstract class Articulo
    {
        public string Titulo { get; }
        public bool Disponible { get; set; }

        public Articulo(string titulo)
        {
            Titulo = titulo;
            Disponible = true;
        }

        public bool Prestar(Miembro miembro)
        {
            if (Disponible)
            {
                PrestadoPor(miembro);
                return true;
            }
            return false;
        }

        public void Devolver(Miembro miembro)
        {
            DevueltoPor(miembro);
        }

        public abstract bool PuedeRenovar();
        public abstract void Renovar();

        protected abstract void PrestadoPor(Miembro miembro);
        protected abstract void DevueltoPor(Miembro miembro);
    }

    class Libro : Articulo
    {
        public string Autor { get; }

        public Libro(string titulo, string autor) : base(titulo)
        {
            Autor = autor;
        }

        public override bool PuedeRenovar()
        {
            // Puede renovar si no está vencido
            return true;
        }

        public override void Renovar()
        {
            // Implementar lógica de renovación
        }

        protected override void PrestadoPor(Miembro miembro)
        {
            Disponible = false;
            miembro.ArticulosPrestados.Add(this);
        }

        protected override void DevueltoPor(Miembro miembro)
        {
            Disponible = true;
            miembro.ArticulosPrestados.Remove(this);
        }
    }

    class Pelicula : Articulo
    {
        public string Director { get; }

        public Pelicula(string titulo, string director) : base(titulo)
        {
            Director = director;
        }

        public override bool PuedeRenovar()
        {
            // Puede renovar si no está vencida
            return true;
        }

        public override void Renovar()
        {
            // Implementar lógica de renovación
        }

        protected override void PrestadoPor(Miembro miembro)
        {
            Disponible = false;
            miembro.ArticulosPrestados.Add(this);
        }

        protected override void DevueltoPor(Miembro miembro)
        {
            Disponible = true;
            miembro.ArticulosPrestados.Remove(this);
        }
    }

    class Juego : Articulo
    {
        public string Desarrollador { get; }

        public Juego(string titulo, string desarrollador) : base(titulo)
        {
            Desarrollador = desarrollador;
        }

        public override bool PuedeRenovar()
        {
            // Implementar lógica para renovar un juego
            return true; // Cambiar según sea necesario
        }

        public override void Renovar()
        {
            // Implementar lógica para renovar un juego
        }

        protected override void PrestadoPor(Miembro miembro)
        {
            Disponible = false;
            miembro.ArticulosPrestados.Add(this);
        }

        protected override void DevueltoPor(Miembro miembro)
        {
            Disponible = true;
            miembro.ArticulosPrestados.Remove(this);
        }
    }
}
