using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Planificacion.Objetos;

namespace Planificacion
{
    public class Program
    {
        static void Main(string[] args)
        {
            Objetos.Lista_recursos recursos = new Objetos.Lista_recursos();
            recursos.lista = recursos.obtener_lista_recursos(@"/home/ivan/Documentos/GIT/Planificacion/Datos/Recursos.csv");

            string ios_id = "iOS";
            string android_id = "Android";
            
            Objetos.Proyecto ios = obtener_proyecto(@"/home/ivan/Documentos/GIT/Planificacion/Datos/iOS.csv", ios_id);
            Objetos.Proyecto android = obtener_proyecto(@"/home/ivan/Documentos/GIT/Planificacion/Datos/Android.csv", android_id);


            //Estimar proyecto ios
            int dia_proyecto = 0;

            //Ordenar lista de recursos por seniority senior, ssr, jr
            recursos.lista = recursos.lista.OrderByDescending(o=>o.seniority_ios).ToList();
            //Ordenar lista de recursos por seniority senior, ssr, jr
            recursos.lista = recursos.lista.OrderByDescending(o=>o.seniority_android).ToList();
            
            
/*
            Console.WriteLine(ios_id);
            while (true)
            {
                //Crear dia
                dia_proyecto += 1;
                Console.WriteLine("Dia {0}", dia_proyecto);
                inicializar_tiempo_recursos(recursos);
                inicializar_tareas(ios.tareas);
                //Ver que tareas se pueden hacer hoy
                List<Objetos.Tarea> tareas_hoy = new List<Objetos.Tarea>();
                tareas_hoy = obtener_tareas_hoy(ios.tareas);
                if (tareas_hoy.Count == 0)
                {
                    break;
                }
                //Asignar tareas
                //Buscar un recurso que tenga hs disponibles
                for (int i = 0; i < tareas_hoy.Count; i++)
                {
                    for (int j = 0; j < recursos.lista.Count; j++)
                    {
                        if (recursos.lista[j].tiempo_disponlible > 0)
                        {
                            tareas_hoy[i].asignar(recursos.lista[j], ios_id, dia_proyecto);
                        }
                        if (tareas_hoy[i].fecha_fin != 0 || tareas_hoy[i].asignados.lista.Count > 1)   //Verifica que se haya terminado la tarea para pasar a la siguiente
                        {
                            break;
                        }
                    }
                    //Reemplazar las tareas que se procesaron hoy
                    var indice = ios.tareas.IndexOf(ios.tareas.Where(w => w.id == tareas_hoy[i].id).First());
                    if (indice != -1)
                    {
                        ios.tareas[indice] = tareas_hoy[i];
                    }
                }
            }

            /* Android */
            //Estimar proyecto 
            dia_proyecto = 0;

            Console.WriteLine(android_id);
            while (true)
            {
                //Crear dia
                dia_proyecto += 1;
                Console.WriteLine("Dia {0}", dia_proyecto);
                inicializar_tiempo_recursos(recursos);
                inicializar_tareas(android.tareas);
                //Ver que tareas se pueden hacer hoy
                List<Objetos.Tarea> tareas_hoy = new List<Objetos.Tarea>();
                tareas_hoy = obtener_tareas_hoy(android.tareas);
                if (tareas_hoy.Count == 0)
                {
                    break;
                }
                //Asignar tareas
                //Buscar un recurso que tenga hs disponibles
                for (int i = 0; i < tareas_hoy.Count; i++)
                {
                    for (int j = 0; j < recursos.lista.Count; j++)
                    {
                        if (recursos.lista[j].tiempo_disponlible > 0)
                        {
                            tareas_hoy[i].asignar(recursos.lista[j], android_id, dia_proyecto);
                        }
                        if (tareas_hoy[i].fecha_fin != 0 || tareas_hoy[i].asignados.lista.Count > 1)   //Verifica que se haya terminado la tarea para pasar a la siguiente
                        {
                            break;
                        }
                    }
                    //Reemplazar las tareas que se procesaron hoy
                    var indice = android.tareas.IndexOf(android.tareas.Where(w => w.id == tareas_hoy[i].id).First());
                    if (indice != -1)
                    {
                        android.tareas[indice] = tareas_hoy[i];
                    }
                }
            }

            /*-------------------------------------------------------- */
            /*AMBOS */
            //Estimar proyecto 
            dia_proyecto = 0;

            //Ordenar lista de recursos por seniority senior, ssr, jr
            recursos.lista = recursos.lista.OrderByDescending(o=>o.seniority_android).ThenBy(o=>o.seniority_ios).ToList();
            Console.WriteLine("/*-------------AMBOS-------------*/");
            while (true)
            {
                //Crear dia
                dia_proyecto += 1;
                Console.WriteLine("Dia {0}", dia_proyecto);
                inicializar_tiempo_recursos(recursos);
                inicializar_tareas(android.tareas);
                //Ver que tareas se pueden hacer hoy
                List<Objetos.Tarea> tareas_hoy = new List<Objetos.Tarea>();
                tareas_hoy = obtener_tareas_hoy(android.tareas);
                tareas_hoy.AddRange(obtener_tareas_hoy(ios.tareas));
                if (tareas_hoy.Count == 0)
                {
                    break;
                }
                //Asignar tareas
                //Buscar un recurso que tenga hs disponibles
                for (int i = 0; i < tareas_hoy.Count; i++)
                {
                    for (int j = 0; j < recursos.lista.Count; j++)
                    {
                        if (recursos.lista[j].tiempo_disponlible > 0)
                        {
                            tareas_hoy[i].asignar(recursos.lista[j], "ambos", dia_proyecto);
                        }
                        if (tareas_hoy[i].fecha_fin != 0 || tareas_hoy[i].asignados.lista.Count > 1)   //Verifica que se haya terminado la tarea para pasar a la siguiente
                        {
                            break;
                        }
                    }
                    //Reemplazar las tareas que se procesaron hoy
                    var indice = android.tareas.IndexOf(android.tareas.Where(w => w.id == tareas_hoy[i].id).Where(w => w.duracion == tareas_hoy[i].duracion).Where(w => w.precedencias == tareas_hoy[i].precedencias).FirstOrDefault());
                    if (indice != -1)
                    {
                        android.tareas[indice] = tareas_hoy[i];
                    }
                    //Reemplazar las tareas que se procesaron hoy
                    indice = ios.tareas.IndexOf(ios.tareas.Where(w => w.id == tareas_hoy[i].id).Where(w => w.duracion == tareas_hoy[i].duracion).Where(w => w.precedencias == tareas_hoy[i].precedencias).FirstOrDefault());
                    if (indice != -1)
                    {
                        ios.tareas[indice] = tareas_hoy[i];
                    }
                }
            }
            /*-------------------------------------------------------- */
            //Grabar CSV
            /*
            string linea = "";
            foreach (var tarea in ios.tareas)
            {
                linea = tarea.id +";"+ tarea.duracion +";"+ tarea.fecha_ini +";"+ tarea.fecha_fin;
                foreach (var persona in tarea.asignados.lista)
                {
                    linea = linea +";"+ persona.nombre;
                }
                Console.WriteLine(linea);
            }
            */
        }

        private static void inicializar_tareas(List<Tarea> tareas)
        {
            for (int i = 0; i < tareas.Count; i++)
            {
                tareas[i].asignados.lista = new List<Recurso>();
            }
        }

        private static void inicializar_tiempo_recursos(Objetos.Lista_recursos recursos)
        {
            for (int i = 0; i < recursos.lista.Count; i++)
            {
                if (recursos.lista[i].nombre == "Daniela")
                {
                    recursos.lista[i].tiempo_disponlible = 6;
                }
                else
                {
                    recursos.lista[i].tiempo_disponlible = 8;
                }
            }
        }

        /*/public static List<Objetos.Recurso> obtener_recursos(string ruta_archivo)
        {
            List<Objetos.Recurso> recursos = new List<Objetos.Recurso>();
            using(var reader = new StreamReader(ruta_archivo))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    line = line.Replace("\"", "");
                    var values = line.Split(';');

                    Objetos.Recurso recurso = new Objetos.Recurso();
                    recurso.nombre = values[0];
                    recurso.tipo = values[1];
                    recurso.seniority_ios = values[2];
                    recurso.seniority_android = values[3];
                    string costo = values[4].Replace('$', ' ').Trim();
                    Int32.TryParse(costo, out recurso.costo);
                    
                    if (recurso.nombre == "Daniela")
                    {
                        recurso.tiempo_disponlible = 6;
                    }
                    else
                    {
                        recurso.tiempo_disponlible = 8;
                    }

                    recursos.Add(recurso);
                }
            }
            return recursos;
        }*/
        /* public static List<Objetos.estimacion> obtener_estimacion(string ruta_archivo)
        {
            List<Objetos.estimacion> estimaciones = new List<Objetos.estimacion>();
            using (var reader = new StreamReader(ruta_archivo))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    line = line.Replace("\"", "");
                    var values = line.Split(';');

                    Objetos.estimacion estimacion = new Objetos.estimacion();
                    estimacion.tarea = values[0];
                    estimacion.descripcion = values[1];
                    Int32.TryParse(values[2], out estimacion.horas);

                    estimacion.precedencias = values[3].Split(',');
                    for (int i = 0; i < estimacion.precedencias.Length; i++)
                    {
                        estimacion.precedencias[i] = estimacion.precedencias[i].Trim();
                    }

                    estimaciones.Add(estimacion);
                }
            }
            return estimaciones;
        }*/
        public static Objetos.Proyecto obtener_proyecto(string ruta_archivo, string id)
        {
            Objetos.Proyecto proyecto = new Objetos.Proyecto();
            proyecto.tareas = new List<Objetos.Tarea>();
            proyecto.id = id;
            using(var reader = new StreamReader(ruta_archivo))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    line = line.Replace("\"", "");
                    var values = line.Split(';');

                    Objetos.Tarea tarea = new Objetos.Tarea();
                    tarea.id = values[0];
                    tarea.descripcion = values[1];
                    int duracion;
                    Int32.TryParse(values[2], out duracion);
                    tarea.duracion = duracion;
                    tarea.hs_rest = duracion;
                    tarea.precedencias = values[3].Split(',');
                    for (int i = 0; i < tarea.precedencias.Length; i++)
                    {
                        tarea.precedencias[i] = tarea.precedencias[i].Trim();
                    }
                    
                    proyecto.tareas.Add(tarea);
                }
            }
            return proyecto;
        }
        //Devolver tareas que se pueden hacer hoy
        public static List<Objetos.Tarea> obtener_tareas_hoy(List<Objetos.Tarea> lista_tareas)
        {
            List<Objetos.Tarea> resultado = new List<Objetos.Tarea>();            
            for (int i = 0; i < lista_tareas.Count; i++)
            {
                var precedencias = Precedencias_faltantes(lista_tareas, lista_tareas[i]);
                if (lista_tareas[i].hs_rest > 0 && precedencias == 0)
                {
                    resultado.Add(lista_tareas[i]);
                    //Console.WriteLine("Tareas para hoy: {0}", lista_tareas[i].id);
                }
            }
            return resultado;
        }
        private static int Precedencias_faltantes(List<Objetos.Tarea> lista_tareas, Objetos.Tarea tarea)
        {
            int cantidad_precedencias = tarea.precedencias.Count();
            for (int i = 0; i < tarea.precedencias.Length; i++)
            {
                //Buscar pre en la lista de tareas
                Objetos.Tarea tarea_faltante = lista_tareas.Find(x => x.id == tarea.precedencias[i]);
                
                //if (tarea_faltante == null || tarea_faltante.hs_rest <= 0)
                if (tarea_faltante != null)
                {
                    if (tarea_faltante.hs_rest <= 0)
                    {
                        cantidad_precedencias -= 1;            
                    }
                }
                else
                {
                    cantidad_precedencias -= 1;
                }
            
                
            }
            return cantidad_precedencias;
        }
 
    }


namespace Objetos
{
    public class Dia_proyecto
    {
        public int id;
        public List<Tarea> lista_tareas = new List<Tarea>();
    }
    public class Recurso
    {
        public string nombre;
        public string tipo;
        public string seniority_ios;
        public string seniority_android;
        public int costo;
        public int tiempo_disponlible;
    }
    public class Lista_recursos
    {
        public List<Recurso> lista = new List<Recurso>();
        public List<Objetos.Recurso> obtener_lista_recursos(string ruta_archivo)
        {
            List<Objetos.Recurso> lista = new List<Objetos.Recurso>();
            using(var reader = new StreamReader(ruta_archivo))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    line = line.Replace("\"", "");
                    var values = line.Split(';');

                    Objetos.Recurso recurso = new Objetos.Recurso();
                    recurso.nombre = values[0];
                    recurso.tipo = values[1];
                    recurso.seniority_ios = values[2];
                    recurso.seniority_android = values[3];
                    string costo = values[4].Replace('$', ' ').Trim();
                    Int32.TryParse(costo, out recurso.costo);
                    
                    if (recurso.nombre == "Daniela")
                    {
                        recurso.tiempo_disponlible = 6;
                    }
                    else
                    {
                        recurso.tiempo_disponlible = 8;
                    }

                    lista.Add(recurso);
                }
            }
            return lista;
        }
        public void inicializar_tiempo_disponible()
        {
            for (int i = 0; i < lista.Count; i++)
            {
                if (lista[i].nombre == "Daniela")
                {
                    lista[i].tiempo_disponlible = 6;
                }
                else
                {
                    lista[i].tiempo_disponlible = 8;
                }
            }
        }
    }
    
    public class Proyecto{
        public string id;
        public List<Tarea> tareas;
    }

    public class Tarea
    {
        public string id ;
        public int duracion;
        public int hs_rest;
        public int fecha_ini;
        public int fecha_fin;
        public string descripcion;
        public string[] precedencias;
        public Lista_recursos asignados = new Lista_recursos();
        //public Lista_recursos Asignados { set {asignados = new Lista_recursos();}}

        public int asignar(Recurso persona, string proyecto_id, int dia_proyecto)
        {
            int tiempo_disponlible = 0;
            int tiempo_asignado = 0;
            if (hs_rest == duracion)
            {
                fecha_ini = dia_proyecto;
            }
            if (proyecto_id == "iOS")
            {
                if (persona.seniority_ios == "Senior")
                {
                    while (persona.tiempo_disponlible > 0 && hs_rest > 0)
                    {
                        hs_rest = hs_rest - 1 * 2;
                        persona.tiempo_disponlible = persona.tiempo_disponlible - 1;
                        tiempo_asignado += 1;
                    }
                }
                if (persona.seniority_ios == "SemiSenior")
                {
                    while (persona.tiempo_disponlible > 0 && hs_rest > 0)
                    {
                        hs_rest = hs_rest - 1;
                        persona.tiempo_disponlible = persona.tiempo_disponlible - 1;
                        tiempo_asignado += 1;
                    }
                }
                if (persona.seniority_ios == "Junior")
                {
                    while (persona.tiempo_disponlible > 0 && hs_rest > 0)
                    {
                        hs_rest = hs_rest - 1 / 2;
                        persona.tiempo_disponlible = persona.tiempo_disponlible - 1;
                        tiempo_asignado += 1;
                    }
                }
            }
            else
            {
                if (persona.seniority_android == "Senior")
                {
                    while (persona.tiempo_disponlible > 0 && hs_rest > 0)
                    {
                        hs_rest = hs_rest - 1 * 2;
                        persona.tiempo_disponlible = persona.tiempo_disponlible - 1;
                        tiempo_asignado += 1;
                    }
                }
                if (persona.seniority_android == "SemiSenior")
                {
                    while (persona.tiempo_disponlible > 0 && hs_rest > 0)
                    {
                        hs_rest = hs_rest - 1;
                        persona.tiempo_disponlible = persona.tiempo_disponlible - 1;
                        tiempo_asignado += 1;
                    }
                }
                if (persona.seniority_android == "Junior")
                {
                    while (persona.tiempo_disponlible > 0 && hs_rest > 0)
                    {
                        hs_rest = hs_rest - 1 / 2;
                        persona.tiempo_disponlible = persona.tiempo_disponlible - 1;
                        tiempo_asignado += 1;
                    }
                }
            }

            if (hs_rest <= 0)   //Si termine la tarea seteo la fecha de fin
            {
                fecha_fin = dia_proyecto;
            }
            Console.WriteLine("Asignado {1}, tarea {2}, hs {3}", proyecto_id, persona.nombre, id, tiempo_asignado);
            asignados.lista.Add(persona);
            return tiempo_disponlible;
        }

    }
    }
}