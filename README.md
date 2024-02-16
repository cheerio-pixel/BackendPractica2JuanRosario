# BackendPractica2JuanRosario

# Descripcion
El backend de la pratica 2 de juan Rosario.

El objetivo es crear una gestion ficticia de la gestion de un banco. Se puede guardar informacion de clientes y manejar productos bancarios a su nombre. La funcionalidad esta segregada en dos roles, el Agente de Servicio y el Gerente. La unica diferencia entre los dos esta en que el gerente puede habilitar y deshabilitar la creacion de nuevos productos.

**Importante**
Este proyecto fue creado y publicado unicamente con el proposito de ser educativo, y de ser practica y ejemplo.

Este proyecto implementa

1. Patron factory y singleton.
2. Inyeccion de dependencias y Inversion de control.
3. Autenticacion por Json Web Token.
4. Autorization basada en permisos. (no recomiendo mi implementacion)
5. Segregacion de funcionalidad en tres capas.

# Instalacion

> $ dotnet restore

# Ejecucion

> $ dotnet run --launch-profile http

Notar: No es necesario que sea http, pero lo probe y desarrolle asi.

# Configuracion

En el archivo `appsettings.json` estan los nombres de los archivos json y la configuracion del JWT.

Otro apartado es que en el momento de construir el proyecto, si este no encuentra una carpeta `res/` con json del mismo nombre que estan dentro de `Resources/MockData/*.json`, este los copiara en esa carpeta, pueden añadir datos de prueba en esa carpeta para poder extenderlo y ademas poder borrar los datos con seguridad de que se re-estableceran.

# Contribuciones

Gracias pero no, haz un fork.
