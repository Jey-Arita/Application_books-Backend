using Application_books.Database.Entities;
using Application_books.Database.Entitties;
using Application_books.Dtos.Autor;
using Application_books.Dtos.Calificacion;
using Application_books.Dtos.Comentarios;
using Application_books.Dtos.Dashboard;
using Application_books.Dtos.Generos;
using Application_books.Dtos.Libros;
using Application_books.Dtos.ListaFavoritos;
using Application_books.Dtos.Membresia;
using Application_books.Dtos.Usuarios;
using AutoMapper;

namespace Application_books.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            MapsForLibros(); 
            MapsForUsuario();
            MapsForAutor();
            MapsForCalificacion();
            MapsForMembresia();
            MapsForListaFavorito();
            MapsForComentario();
            MapsForGeneros();
            MapsDashboard();
        }

        private void MapsDashboard()
        {
            CreateMap<DashboardSummaryDto, DashboardSummaryDto>();
        }
        private void MapsForGeneros()
        {
            CreateMap<GeneroEntity, GenerosDto>()
            .ReverseMap();
        }

        private void MapsForLibros()
        {
        CreateMap<LibroEntity, LibroDto>()
        .ForMember(dest => dest.Promedio, opt => opt.MapFrom(src =>
            src.Calificaciones.Any() ? Math.Round(src.Calificaciones.Average(c => c.Puntuacion), 1) : 0))
        .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
            CreateMap<LibroEditDto, LibroEntity>();
            CreateMap<LibroCreateDto, LibroEntity>();

        }

        private void MapsForUsuario()
        {
            CreateMap<UserEntity, UsuarioDto>();
            CreateMap<UsuarioCreateDto, UserEntity>();
            CreateMap<UsuarioEditDto, UserEntity>();
        }
        private void MapsForAutor()
        {
            CreateMap<AutorEntity, AutorDto>();
            CreateMap<AutorCreateDto, AutorEntity>();
            CreateMap<AutorEditDto, AutorEntity>();
        }
        private void MapsForCalificacion()
        {
            CreateMap<CalificacionEntity, CalificacionDto>(); 
            CreateMap<CalificacionCreateDto, CalificacionEntity>();
            CreateMap<CalificacionEditDto, CalificacionEntity>();
        }
        private void MapsForMembresia()
        {
            // Mapear MembresiaEntity a MembresiaDto
            CreateMap<MembresiaEntity, MembresiaDto>()
                .ForMember(dest => dest.ActivaMembresia, opt =>
                    opt.MapFrom(src => src.FechaFin.HasValue && src.FechaFin.Value > DateTime.Now))
                .ReverseMap();

            // Mapear MembresiaCreateDto a MembresiaEntity
            CreateMap<MembresiaCreateDto, MembresiaEntity>()
                .ForMember(dest => dest.FechaInicio, opt => opt.MapFrom(_ => DateTime.Now))
                .ForMember(dest => dest.FechaFin, opt => opt.Ignore()) // Será calculado en el servicio
                .ForMember(dest => dest.DiasRestantes, opt => opt.Ignore()); // Calculado en lógica de negocio

            // Mapear MembresiaEditDto a MembresiaEntity
            CreateMap<MembresiaEditDto, MembresiaEntity>()
                .ForMember(dest => dest.TipoMembresia, opt => opt.Condition(src => src.TipoMembresia != null));
        }
        private void MapsForListaFavorito()
        {
            CreateMap<ListaFavoritoEntity, ListaFavoritoDto>();
            CreateMap<ListaFavoritoCreateDto, ListaFavoritoEntity>();
            CreateMap<ListaFavoritoEditDto, ListaFavoritoEntity>();
        }

        private void MapsForComentario()
        {
            CreateMap<ComentarioEntity, ComentarioDto>()
            .ForMember(dest => dest.NombreUsuario, opt => opt.MapFrom(src => $"{src.Usuario.FirstName} {src.Usuario.LastName}"))
            .ForMember(dest => dest.Respuestas, opt => opt.MapFrom(src => src.Respuestas));
            CreateMap<ComentarioCreateDto, ComentarioEntity>();
            CreateMap<ComentarioEditDto, ComentarioEntity>();
            CreateMap<ComentarioDto, ComentarioEntity>();
        }
    }
}
