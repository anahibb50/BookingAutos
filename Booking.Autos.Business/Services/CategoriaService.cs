using Booking.Autos.Business.DTOs.Catalogos.Categoria;
using Booking.Autos.Business.Exceptions;
using Booking.Autos.Business.Interfaces;
using Booking.Autos.Business.Mappers;
using Booking.Autos.DataManagement.Interfaces;
using Booking.Autos.DataManagement.Models.Categorias;

namespace Booking.Autos.Business.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaDataService _categoriaDataService;

        public CategoriaService(ICategoriaDataService categoriaDataService)
        {
            _categoriaDataService = categoriaDataService;
        }

        // =========================
        // CREAR
        // =========================
        public async Task<CategoriaResponse> CrearAsync(
            CrearCategoriaRequest request,
            CancellationToken cancellationToken = default)
        {
            // 🔥 VALIDACIÓN BÁSICA
            if (string.IsNullOrWhiteSpace(request.Nombre))
                throw new ValidationException(new List<string> { "El nombre es obligatorio." });

            // 🔍 VALIDAR DUPLICADO
            var existe = await _categoriaDataService
                .ExistsByNombreAsync(request.Nombre, cancellationToken);

            if (existe)
                throw new ValidationException(new List<string> { "Ya existe una categoría con ese nombre." });

            // 🔥 MAPEAR
            var dataModel = CategoriaBusinessMapper.ToDataModel(request);

            var creado = await _categoriaDataService
                .CreateAsync(dataModel, cancellationToken);

            return CategoriaBusinessMapper.ToResponse(creado);
        }

        // =========================
        // ACTUALIZAR
        // =========================
        public async Task<CategoriaResponse> ActualizarAsync(
            ActualizarCategoriaRequest request,
            CancellationToken cancellationToken = default)
        {
            if (request.Id <= 0)
                throw new ValidationException(new List<string> { "Id inválido." });

            if (string.IsNullOrWhiteSpace(request.Nombre))
                throw new ValidationException(new List<string> { "El nombre es obligatorio." });

            // 🔍 EXISTENTE
            var existente = await _categoriaDataService
                .GetByIdAsync(request.Id, cancellationToken);

            if (existente is null)
                throw new NotFoundException("Categoria", request.Id);

            // 🔍 VALIDAR DUPLICADO
            var existeNombre = await _categoriaDataService
                .ExistsByNombreAsync(request.Nombre, cancellationToken);

            if (existeNombre && existente.Nombre != request.Nombre)
                throw new ValidationException(new List<string> { "Ya existe otra categoría con ese nombre." });

            // 🔥 MAPEAR
            var dataModel = CategoriaBusinessMapper.ToDataModel(request);

            // 🔥 CONSERVAR DATOS IMPORTANTES
            dataModel.Guid = existente.Guid;
            dataModel.FechaCreacion = existente.FechaCreacion;
            dataModel.EsEliminado = existente.EsEliminado;

            var actualizado = await _categoriaDataService
                .UpdateAsync(dataModel, cancellationToken);

            return CategoriaBusinessMapper.ToResponse(actualizado);
        }

        // =========================
        // ELIMINACIÓN LÓGICA
        // =========================
        public async Task EliminarLogicoAsync(
            int id,
            string usuario,
            CancellationToken cancellationToken = default)
        {
            var existente = await _categoriaDataService
                .GetByIdAsync(id, cancellationToken);

            if (existente is null)
                throw new NotFoundException("Categoria", id);

            existente.EsEliminado = true;
            existente.FechaEliminacion = DateTime.UtcNow;

            await _categoriaDataService.UpdateAsync(existente, cancellationToken);
        }

        // =========================
        // OBTENER POR ID
        // =========================
        public async Task<CategoriaResponse> ObtenerPorIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var categoria = await _categoriaDataService
                .GetByIdAsync(id, cancellationToken);

            if (categoria is null)
                throw new NotFoundException("Categoria", id);

            return CategoriaBusinessMapper.ToResponse(categoria);
        }

        // =========================
        // LISTAR
        // =========================
        public async Task<IReadOnlyList<CategoriaResponse>> ListarAsync(
            CancellationToken cancellationToken = default)
        {
            var categorias = await _categoriaDataService
                .GetAllAsync(cancellationToken);

            return categorias
                .Select(CategoriaBusinessMapper.ToResponse)
                .ToList();
        }

        // =========================
        // VALIDACIÓN
        // =========================
        public async Task<bool> ExistePorNombreAsync(
            string nombre,
            CancellationToken cancellationToken = default)
        {
            return await _categoriaDataService
                .ExistsByNombreAsync(nombre, cancellationToken);
        }
    }
}