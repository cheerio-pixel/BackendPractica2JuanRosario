

using backend.DTO;
using backend.Models;
using backend.Repositories.Interfaces;

namespace backend.Repositories
{
    internal class JsonClientRepository(string jsonPath, int fetchSize)
    : JsonObjectsRepository<Client>(jsonPath), IClientRepository, IProductNameCheck,
      // Okay, is not my fault that I *have* to put products inside of the user.
      IProductRepository
    {
        public ResultValue<Guid> Create(ClientProfileDTO client)
        {
            try
            {
                Acquire();

                List<string> errors = new();
                List<Client> clients = Load();
                clients.ForEach(c =>
                    {
                        if (c.Email == client.Email)
                        {
                            errors.Add("Ya existe un usuario con ese email.");
                        }

                        if (c.Name == client.Name && c.Surname == client.Surname)
                        {
                            errors.Add("Ya existe un usuario con ese nombre y apellido.");
                        }
                    });
                if (errors.Count == 0)
                {
                    Guid id = Guid.NewGuid();
                    clients.Add(new Client(
                        id, client.Name, client.Surname, client.Telefono, client.Address, client.Email,
                        client.Password, new List<Producto>()
                    ));
                    Save(clients);
                    return new ResultValue<Guid>(id);
                }
                return new ResultValue<Guid>(errors, null);
            }
            finally
            {
                Release();
            }
        }

        public ResultValue<Guid> Create(Guid clientId, Producto product)
        {
            List<string> errors = new();
            if (ProductNameExists(product.Name))
            {
                errors.Add("El nombre del producto ya existe.");
                return new(errors, null);
            }
            try
            {
                Acquire();

                List<Client> clients = Load();
                int clientIndex = clients.FindIndex(c => c.Id == clientId);
                if (clientIndex == -1)
                {
                    errors.Add("El cliente no existe.");
                    return new(errors, null);
                }
                Client client = clients[clientIndex];
                if (client.Productos == null)
                {
                    client.Productos = new List<Producto>() { product };
                    // Most probably is not necessary, since we are keeping
                    // a reference to client and not the value, so it
                    // should get reflected. But it doesn't kill.
                    clients[clientIndex] = client;
                    return new(product.Id);
                }
                if (clients[clientIndex].Productos.FirstOrDefault(p => p.Id == product.Id) != null)
                {
                    errors.Add("El producto ya existe");
                    return new(errors, null);
                }
                // Right now I'm considering moving to a list, but don't
                // know where IEnumerable is applicable or the other
                client.Productos.Add(product);
                Save(clients);

                return new(product.Id);
            }
            finally
            {
                Release();
            }
        }

        public bool Delete(Guid id)
        {
            return RemoveByPredicate(c => c.Id == id);
        }

        public void Delete(Guid clientId, Guid productId)
        {
            try
            {
                Acquire();
                List<Client> clients = Load();
                int index = clients.FindIndex(c => c.Id == clientId);
                if (index == -1)
                {
                    // Is it even necessary to notify non-existant
                    // resource? Maybe
                    return;
                }
                // Okay, next time I'm reading the documentation of what
                // interface is better for each context. Because enumerable
                // is not for having collections of items.
                clients[index].Productos = clients[index].Productos.Where(p => p.Id != productId).ToList();
                Save(clients);
            }
            finally
            {
                Release();
            }

        }

        public Client? GetClient(Guid id)
        {
            try
            {
                Acquire();
                return Load().Find(c => c.Id == id);
            }
            finally
            {
                Release();
            }
        }

        public bool ProductNameExists(string productName)
        {
            try
            {
                Acquire();
                return Load().SelectMany(c => c.Productos).Any(p => p.Name == productName);
            }
            finally
            {
                Release();
            }
        }

        public bool Save(Guid id, ClientProfileDTO client)
        {
            try
            {
                Acquire();
                List<Client> elements = Load();
                int clientIndex = elements.FindIndex(c => c.Id == id);
                if (clientIndex != -1)
                {
                    Client clientFound = elements[clientIndex];
                    clientFound.Name = client.Name;
                    clientFound.Surname = client.Surname;
                    clientFound.Telefono = client.Telefono;
                    clientFound.Address = client.Address;
                    clientFound.Email = client.Email;
                    if (!(string.IsNullOrWhiteSpace(client.Password) || string.IsNullOrEmpty(client.Password)))
                    {
                        clientFound.Password = client.Password;
                    }

                    // Maybe this line is actually unnecesary
                    elements[clientIndex] = clientFound;
                    Save(elements);
                    return true;
                }
                return false;
            }
            finally
            {
                Release();
            }
        }

        public ResultValue<bool> Save(Guid clientId, Producto product)
        {
            try
            {
                Acquire();

                List<string> errors = new();
                List<Client> clients = Load();
                int index = clients.FindIndex(c => c.Id == clientId);
                if (index == -1)
                {
                    errors.Add("El cliente no existe.");
                    return new(errors, false);
                }
                if (clients[index].Productos == null)
                {
                    errors.Add("La lista de productos no existe.");
                    return new(errors, false);
                }
                Producto? producto = clients[index].Productos.FirstOrDefault(p => p.Id == product.Id);
                if (producto == null)
                {
                    errors.Add("El producto no existe.");
                    return new(errors, false);
                }
                // Indeed, absurd
                clients[index].Productos.Remove(producto);
                clients[index].Productos.Add(product);
                Save(clients);
                return new(true);
            }
            finally
            {
                Release();
            }
        }

        public IEnumerable<Client> Search(string? name)
        {
            try
            {
                Acquire();
                if (string.IsNullOrEmpty(name))
                {
                    return Load().Take(fetchSize);
                }
                return Load().Where(c => c.FullName.Contains(name)).Take(fetchSize);
            }
            finally
            {
                Release();
            }
        }

        public Result<Producto> Show(Guid clientId, Guid productId)
        {
            try
            {
                Acquire();
                List<string> errors = new();
                List<Client> clients = Load();
                Client? client = clients.Find(c => c.Id == clientId);
                if (client == null)
                {
                    errors.Add("No se pudo encontrar dicho cliente.");
                    return new(errors, null);
                }
                if (client.Productos == null)
                {
                    errors.Add("La lista de productos no existe.");
                    return new(errors, null);
                }
                Producto? producto = client.Productos.FirstOrDefault(p => p.Id == productId);
                if (producto == null)
                {
                    errors.Add("El producto no existe");
                    return new(errors, null);
                }
                return new(producto);
            }
            finally
            {
                Release();
            }
        }
    }
}